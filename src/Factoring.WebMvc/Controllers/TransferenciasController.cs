using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Factoring.Model;
using Factoring.Model.Models.Auth;
using Factoring.Model.Models.Cavali;
using Factoring.Model.Models.Externos;
using Factoring.Model.Models.OperacionesFactura;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO.Pipes;
using Factoring.Model.Models.Usuario;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class TransferenciasController : Controller
    {
        private readonly ILogger<TransferenciasController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOperacionProxy _operacionProxy;
        private readonly IFacturaOperacionesProxy _facturaOperacionesProxy;
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IFondeadorProxy _fondeadorProxy;
        public TransferenciasController(
            ILogger<TransferenciasController> logger,
            IHttpContextAccessor httpContextAccessor,
            IOperacionProxy operacionProxy,
            IFacturaOperacionesProxy facturaOperacionesProxy,
            //IDivisoExternProxy divisoExternProxy,
            IFondeadorProxy fondeadorProxy,
            ICatalogoProxy catalogoProxy
        )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _operacionProxy = operacionProxy;
            _facturaOperacionesProxy = facturaOperacionesProxy;
            //_divisoExternProxy = divisoExternProxy;
            _fondeadorProxy = fondeadorProxy;
            _catalogoProxy = catalogoProxy;
        }
        public async Task<IActionResult> IndexAsync()
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            // var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 103, Valor = "0" });
            // HttpContext.Session.SetObjectAsJson("nIdAccionMenuOpeCavl", accionOperacionCavl);

            var objOpcionRol = HttpContext.Session.GetObjectFromJson<AccionRol>("nIdAccionMenuOpeCavl");
            //if (objOpcionRol!=null)
            ViewBag.ListInversionista = await _fondeadorProxy.GetAllListFondeadoreslista();
            var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 114, Valor = "0" });
            var _Motivos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 115, Valor = "0" });
            var _Acciones = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = objOpcionRol != null ? objOpcionRol.nOpcion : 0, Codigo = 123, Valor = "0" });
            ViewBag.Estados = _Estados.Data.ToList();
            ViewBag.Motivos = _Motivos.Data.ToList();
            ViewBag.Acciones = _Acciones.Data.ToList();
            return View();
        }
        public async Task<JsonResult> GetOperacionAllList(OperacionesFacturaRequestDataTableDto model)
        {
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                var requestData = new OperacionesFacturaRequestDataTableDto();
                requestData.Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault()));
                requestData.PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault()));
                requestData.Sorting = "nIdOperacionesFacturas";
                requestData.SortOrder = "desc";

                requestData.Estado = model.Estado;
                requestData.FechaCreacion = model.FechaCreacion;
                requestData.FilterNroOperacion = model.NroOperacion;
                requestData.Usuario = userName;
                var data = await _facturaOperacionesProxy.GetBandejaFacturas(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].totalRecords : 0;
                return Json(new { data = data.Data, recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> Registro(List<int> IdFacturas)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var nNumeroProcesados = 0;
                var MensajeRetorno = new ResponseData<ResponseCavaliInvoice4012>();
                var nValidarEstado = 0;
                foreach (int factura in IdFacturas)
                {
                    var _validarEstado = await _facturaOperacionesProxy.ValidarEstadoFactura(new OperacionesFacturaListDto
                    {
                        nIdOperacionesFacturas = factura,
                        nEstado = 1
                    });

                    if (!_validarEstado.Succeeded)
                        nValidarEstado++;

                }
                if (nValidarEstado == 0)
                {

                    foreach (int factura in IdFacturas)
                    {
                        var result = await _facturaOperacionesProxy.OperacionCavaliInvoicesSend4012(new OperacionesFacturaLoteCavali
                        {
                            FlagRegisterProcess = 1,
                            FlagAcvProcess = 0,
                            FlagTransferProcess = 0,
                            CodParticipante = 0,
                            UsuarioCreador = userName,
                            InvoicesFactura = factura,
                            Invoices = IdFacturas
                        });
                        nNumeroProcesados++;
                        MensajeRetorno = result;
                    }
                    if (MensajeRetorno != null)
                        return Json(MensajeRetorno.Data);
                    else
                    {
                        return Json("No se procesaron las facturas.");
                    }
                }
                else
                {
                    return Json("No se procesaron las facturas, ya que existen estados no permitidos.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> RegistroCavali(List<int> IdFacturasAccion, int InversionistaAccionRemover, int InversionistaAccion)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                var nNumeroProcesados = 0;
                if (InversionistaAccion == 1)
                {
                    //var MensajeRetorno = new ResponseData<ResponseCavaliInvoice4012>();
                    var MensajeRetorno = new ResponseData<ResponseCavaliInvoice4012>
                    {
                        Message = "No se procesaron las facturas.",
                        Succeeded = false,
                        Data = new ResponseCavaliInvoice4012
                        {
                            Mensaje = "No se procesaron las facturas.",
                            Error = true
                        }
                    };


                    foreach (int factura in IdFacturasAccion)
                    {
                        var result = await _facturaOperacionesProxy.OperacionCavaliInvoicesSend4012(new OperacionesFacturaLoteCavali
                        {
                            FlagRegisterProcess = 1,
                            FlagAcvProcess = 0,
                            FlagTransferProcess = 0,
                            CodParticipante = 0,
                            UsuarioCreador = userName,
                            InvoicesFactura = factura,
                            Invoices = IdFacturasAccion
                        });
                        nNumeroProcesados++;
                        if (result != null)
                        {
                            MensajeRetorno = result;
                        }
                    }
                    return Json(MensajeRetorno);
                    //if (MensajeRetorno != null)
                    //    return Json(MensajeRetorno);
                    //else
                    //{
                    //    ResponseCavaliInvoice4012 responseCavaliInvoice4012 = new()
                    //    {
                    //        Mensaje = "No se procesaron las facturas.",
                    //        Error = true
                    //    };

                    //    MensajeRetorno.Message = "No se procesaron las facturas.";
                    //    MensajeRetorno.Succeeded = false;
                    //    MensajeRetorno.Data = responseCavaliInvoice4012;
                    //    return Json(MensajeRetorno);
                    //}
                }
                else if (InversionistaAccion == 2)
                {
                    var MensajeRetorno = new ResponseData<ResponseCavaliInvoice4012>();
                    int nIdFondeador = 0;
                    int nCategoriaFondeador = 0, nCantidadAsignaciones = 0, nIdGiradorPlus = 0;
                    bool bFondeadorPlus = false, bSegundoFlagDiferente = false;
                    var resultEval = await _facturaOperacionesProxy.ObtenerValidacionAsignaciones(new RequestOperacionesFacturaValidacion
                    {
                        nLstIdFacturas = IdFacturasAccion,
                        nTipo = InversionistaAccion
                    });
                    List<FacturasGetRegistro> listaFacturas;
                    if (resultEval.Data.listaFacturas.Count > 0)
                    {
                        OperacionesFacturaLoteCavali objTramaOperacion = new();
                        foreach (var item in IdFacturasAccion)
                        {
                            listaFacturas = new List<FacturasGetRegistro>();
                            listaFacturas = resultEval.Data.listaFacturas.Where(x => x.nIdFactura == item).ToList();
                            if (listaFacturas.Count > 0)
                            {
                                int cantidad = listaFacturas.Count;
                                if (cantidad == 1)
                                {
                                    int nCantidadConfechaNew = listaFacturas.Count(x => x.dFechacAsignacion != "" && (x.nEstadoFactura == 8 || x.nEstadoFactura == 10 || x.nEstadoFactura == 16));
                                    if (nCantidadConfechaNew > 0)
                                    {
                                        nIdFondeador = listaFacturas[0].nIdFondeador;
                                        nCategoriaFondeador = listaFacturas[0].nIdCategoria;
                                        nCantidadAsignaciones = 1;
                                        nIdGiradorPlus = listaFacturas[0].nCodFondeadorPlus;
                                        bFondeadorPlus = listaFacturas[0].bFondeadorPlus;
                                        bSegundoFlagDiferente = false;
                                        objTramaOperacion = new()
                                        {
                                            FlagRegisterProcess = 0,
                                            FlagAcvProcess = 0,
                                            FlagTransferProcess = 1,
                                            CodParticipante = nIdFondeador,
                                            UsuarioCreador = userName,
                                            InvoicesFactura = item,
                                            Invoices = IdFacturasAccion,
                                            nCategoriaFondeador = nCategoriaFondeador,
                                            nCantidadAsignacion = nCantidadAsignaciones,
                                            nIdGiradorPlus = nIdGiradorPlus,
                                            bFondeadorPlus = bFondeadorPlus,
                                            bSegundoFlagDiferente = bSegundoFlagDiferente
                                        };
                                        var resultado = await EnviarTransferencia(objTramaOperacion);
                                        MensajeRetorno = resultado;
                                    }
                                    else
                                    {
                                        ResponseCavaliInvoice4012 responseCavaliInvoice4012 = new()
                                        {
                                            Mensaje = "El girador no tiene fecha de Desembolso.",
                                            Error = true
                                        };
                                        MensajeRetorno.Message = "El girador no tiene fecha de Desembolso.";
                                        MensajeRetorno.Succeeded = false;
                                        MensajeRetorno.Data = responseCavaliInvoice4012;
                                        return Json(MensajeRetorno);
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(listaFacturas[0].dFechaDesembolsoFondeador) && listaFacturas[0].nNumeroAsignaciones == 1 && !string.IsNullOrWhiteSpace(listaFacturas[1].dFechacAsignacion) && (listaFacturas[1].nEstadoFactura == 13 || listaFacturas[1].nEstadoFactura == 10 || listaFacturas[1].nEstadoFactura == 16))
                                    {
                                        nIdFondeador = listaFacturas[1].nIdFondeador;
                                        nCategoriaFondeador = listaFacturas[1].nIdCategoria;
                                        nIdGiradorPlus = listaFacturas[1].nCodFondeadorPlus;
                                        nCantidadAsignaciones = 2;
                                        bFondeadorPlus = listaFacturas[0].bFondeadorPlus;
                                        if (listaFacturas[0].nIdCategoria != listaFacturas[1].nIdCategoria)
                                        {
                                            bSegundoFlagDiferente = true;
                                            objTramaOperacion = new()
                                            {
                                                FlagRegisterProcess = 0,
                                                FlagAcvProcess = 0,
                                                FlagTransferProcess = 1,
                                                CodParticipante = nIdFondeador,
                                                UsuarioCreador = userName,
                                                InvoicesFactura = item,
                                                Invoices = IdFacturasAccion,
                                                nCategoriaFondeador = nCategoriaFondeador,
                                                nCantidadAsignacion = nCantidadAsignaciones,
                                                nIdGiradorPlus = nIdGiradorPlus,
                                                bFondeadorPlus = bFondeadorPlus,
                                                bSegundoFlagDiferente = bSegundoFlagDiferente
                                            };
                                            var resultado = await EnviarTransferencia(objTramaOperacion);
                                            MensajeRetorno = resultado;
                                        }
                                        else
                                        {
                                            ResponseCavaliInvoice4012 responseCavaliInvoice4012 = new()
                                            {
                                                Mensaje = "El segundo fondeador es igual al primer transferido.",
                                                Error = true
                                            };
                                            MensajeRetorno.Message = "El segundo fondeador es igual al primer transferido.";
                                            MensajeRetorno.Succeeded = false;
                                            MensajeRetorno.Data = responseCavaliInvoice4012;
                                            return Json(MensajeRetorno);
                                        }
                                    }
                                    else
                                    {
                                        ResponseCavaliInvoice4012 responseCavaliInvoice4012 = new()
                                        {
                                            Mensaje = "El fondeador no tiene fecha de Desembolso.",
                                            Error = true
                                        };
                                        MensajeRetorno.Message = "El fondeador no tiene fecha de Desembolso.";
                                        MensajeRetorno.Succeeded = false;
                                        MensajeRetorno.Data = responseCavaliInvoice4012;
                                        return Json(MensajeRetorno);

                                    }
                                }
                            }
                            else
                            {
                                ResponseCavaliInvoice4012 responseCavaliInvoice4012 = new()
                                {
                                    Mensaje = "No hay facturas en estado desembolsado.",
                                    Error = true
                                };
                                MensajeRetorno.Message = "No hay facturas en estado desembolsado.";
                                MensajeRetorno.Succeeded = false;
                                MensajeRetorno.Data = responseCavaliInvoice4012;
                                return Json(MensajeRetorno);
                            }
                        }
                        //return Json(MensajeRetorno);
                    }
                    else
                    {
                        ResponseCavaliInvoice4012 responseCavaliInvoice4012 = new()
                        {
                            Mensaje = "Las facturas deben estar en estado 'Anotado en cuenta', 'Desembolsado' o 'Pendiente de transferencia'.",
                            Error=true
                        };
                        MensajeRetorno.Message = "Las facturas deben estar en estado 'Anotado en cuenta', 'Desembolsado' o 'Pendiente de transferencia'.";
                        MensajeRetorno.Succeeded = false;
                        MensajeRetorno.Data = responseCavaliInvoice4012;
                        //MensajeRetorno.Errors=
                    }
                    return Json(MensajeRetorno);
                }
                else
                {
                    var MensajeRetorno = new ResponseData<ResponseCavaliRemove4008>();
                    foreach (int factura in IdFacturasAccion)
                    {
                        var result = await _facturaOperacionesProxy.OperacionCavaliRemove4008(new OperacionesFacturaRemoveCavali
                        {
                            IdMotivo = InversionistaAccionRemover,
                            UsuarioCreador = userName,
                            InvoicesFactura = factura,
                            Invoices = IdFacturasAccion
                        });
                        nNumeroProcesados++;
                        MensajeRetorno = result;
                    }
                    if (MensajeRetorno != null)
                    {
                        if (MensajeRetorno.Data != null)
                        {
                            //MensajeRetorno.Data.Valores.body.processId = 1;
                            return Json(MensajeRetorno);
                        }

                        return Json(MensajeRetorno.Message);
                    }
                    else
                    {
                        ResponseCavaliRemove4008 responseCavaliInvoice4008 = new()
                        {
                            Mensaje = "No se procesaron las facturas.",
                        };
                        MensajeRetorno.Succeeded = false;
                        MensajeRetorno.Message = "No se procesaron las facturas.";
                        MensajeRetorno.Data = responseCavaliInvoice4008;
                        return Json(MensajeRetorno);
                    }

                }
                //return Json(MensajeRetorno);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> Transferir(List<int> IdFacturas, int Inversionista)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var nNumeroProcesados = 0;
                var MensajeRetorno = new ResponseData<ResponseCavaliInvoice4012>();

                var nValidarEstado = 0;
                foreach (int factura in IdFacturas)
                {
                    var _validarEstado = await _facturaOperacionesProxy.ValidarEstadoFactura(new OperacionesFacturaListDto
                    {
                        nIdOperacionesFacturas = factura,
                        nEstado = 2
                    });

                    if (!_validarEstado.Succeeded)
                        nValidarEstado++;

                }
                if (nValidarEstado == 0)
                {
                    foreach (int factura in IdFacturas)
                    {
                        var result = await _facturaOperacionesProxy.OperacionCavaliInvoicesSend4012(new OperacionesFacturaLoteCavali
                        {
                            FlagRegisterProcess = 0,
                            FlagAcvProcess = 0,
                            FlagTransferProcess = 1,
                            CodParticipante = Inversionista,
                            UsuarioCreador = userName,
                            InvoicesFactura = factura,
                            Invoices = IdFacturas
                        });
                        nNumeroProcesados++;
                        MensajeRetorno = result;
                    }

                    if (MensajeRetorno != null)
                    {
                        if (MensajeRetorno.Data.Valores != null)
                            return Json(MensajeRetorno.Data.Valores.body);
                        else
                            return Json(MensajeRetorno.Data.Mensaje);
                    }
                    else
                        return Json("No se procesaron las facturas.");
                }
                else return Json("No se procesaron las facturas, ya que existen estados no permitidos.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> Traspasar(List<int> IdFacturas, int Inversionista)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var nNumeroProcesados = 0;
                var MensajeRetorno = new ResponseData<ResponseCavaliInvoice4012>();
                var nValidarEstado = 0;
                var _validarEstado = new ResponseData<int>();
                foreach (int factura in IdFacturas)
                {
                    _validarEstado = await _facturaOperacionesProxy.ValidarEstadoFactura(new OperacionesFacturaListDto
                    {
                        nIdOperacionesFacturas = factura,
                        nEstado = 3
                    });

                    if (!_validarEstado.Succeeded)
                        nValidarEstado++;

                }

                if (nValidarEstado == 0)
                {
                    foreach (int factura in IdFacturas)
                    {
                        var result = await _facturaOperacionesProxy.OperacionCavaliInvoicesSend4012(new OperacionesFacturaLoteCavali
                        {
                            FlagRegisterProcess = 0,
                            FlagAcvProcess = 0,
                            FlagTransferProcess = 1,
                            CodParticipante = Inversionista,
                            UsuarioCreador = userName,
                            InvoicesFactura = factura,
                            Invoices = IdFacturas
                        });
                        nNumeroProcesados++;
                        MensajeRetorno = result;
                    }

                    if (MensajeRetorno != null)
                    {
                        if (MensajeRetorno.Data != null)
                            return Json(MensajeRetorno.Data.Valores.body);
                        return Json(MensajeRetorno.Message);
                    }
                    else
                        return Json("No se procesaron las facturas.");
                }
                else
                {
                    if (_validarEstado.Message != null)
                        return Json(_validarEstado.Message);
                    else
                        return Json("No se procesaron las facturas, ya que existen estados no permitidos.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> Remover(List<int> IdFacturas, int Inversionista)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var nNumeroProcesados = 0;
                var MensajeRetorno = new ResponseData<ResponseCavaliRemove4008>();
                var nValidarEstado = 0;
                foreach (int factura in IdFacturas)
                {
                    var _validarEstado = await _facturaOperacionesProxy.ValidarEstadoFactura(new OperacionesFacturaListDto
                    {
                        nIdOperacionesFacturas = factura,
                        nEstado = 4
                    });

                    if (!_validarEstado.Succeeded)
                        nValidarEstado++;

                }

                if (nValidarEstado == 0)
                {
                    foreach (int factura in IdFacturas)
                    {
                        var result = await _facturaOperacionesProxy.OperacionCavaliRemove4008(new OperacionesFacturaRemoveCavali
                        {
                            IdMotivo = Inversionista,
                            UsuarioCreador = userName,
                            InvoicesFactura = factura,
                            Invoices = IdFacturas
                        });
                        nNumeroProcesados++;
                        MensajeRetorno = result;
                    }
                    if (MensajeRetorno != null)
                    {
                        if (MensajeRetorno.Data != null)
                        {
                            MensajeRetorno.Data.Valores.body.processId = 1;
                            return Json(MensajeRetorno.Data.Valores.body);
                        }

                        return Json(MensajeRetorno.Message);
                    }
                    else
                        return Json("No se procesaron las facturas.");
                }
                else
                    return Json("No se procesaron las facturas, ya que existen estados no permitidos.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> ObtenerAsignaciones(List<int> IdFacturas, int nIdOpcionOperacion)
        {
            try
            {
                var result = await _facturaOperacionesProxy.ObtenerAsignaciones(new OperacionesFacturaValidaAsignacion
                {
                    nLstIdFacturas = IdFacturas,
                    nIdOpcionOperacion = nIdOpcionOperacion
                });
                if (result != null)
                {
                    if (result.Data.listaFondeador != null)
                    {
                        if (result.Data.listaFondeador[0].cantidadInversionistas > 1)
                            return Json("Las facturas seleccionadas tienen mas de UN INVERSIONISTA asociado.");
                        else
                            return Json(result);
                    }
                }

                return Json("No se procesaron las facturas.");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IActionResult> ObtenerAsignacionesCavali(List<int> IdFacturas, int nIdOpcionOperacion)
        {
            try
            {
                var result = await _facturaOperacionesProxy.ObtenerValidacionAsignaciones(new RequestOperacionesFacturaValidacion
                {
                    nLstIdFacturas = IdFacturas,
                    nTipo = nIdOpcionOperacion
                });
                return Json(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<ResponseData<ResponseCavaliInvoice4012>> EnviarTransferencia(OperacionesFacturaLoteCavali request)
        {
            var MensajeRetorno = new ResponseData<ResponseCavaliInvoice4012>();
            var result = await _facturaOperacionesProxy.OperacionCavaliInvoicesSend4012(request);
            if (result != null)
            {
                MensajeRetorno = result;
                //if (result.Data.Valores != null)
                //    MensajeRetorno.Data.Valores = result.Data.Valores;
                //else
                //    MensajeRetorno.Data.Mensaje = result.Data.Mensaje;
            }
            else
            {
                ResponseCavaliInvoice4012 responseCavaliInvoice4012 = new()
                {
                    Mensaje = "No se procesaron las facturas.",
                };
                MensajeRetorno.Succeeded = false;
                MensajeRetorno.Message = "No se procesaron las facturas.";
                MensajeRetorno.Data = responseCavaliInvoice4012;
            }
            return MensajeRetorno;

        }

    }
}