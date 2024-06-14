using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Factoring.Model;
using Factoring.Model.Models.Auth;
//using Factoring.Model.Models.Catalogo;
//using Factoring.Model.Models.EvaluacionOperacion;
//using Factoring.Model.Models.EvaluacionOperacionesComentarios;
//using Factoring.Model.Models.Operaciones;
//using Factoring.Model.Models.OperacionesFactura;
//using Factoring.Model.Models.ProcesoMasivo;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using Factoring.WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Factoring.Model.Models.Operaciones;
using Factoring.Model.Models.EvaluacionOperacion;
using Factoring.Model.Models.OperacionesFactura;
using Factoring.Model.Models.Adquiriente;
using Factoring.Model.Models.AdquirienteUbicacion;
using Factoring.Model.Models.Girador;
using Factoring.Model.Models.GiradorUbicacion;
using Factoring.Model.Models.ReporteGiradorOperaciones;
using System.Xml.XPath;
using System.Xml;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class OperacionController : Controller
    {
        private static Random random = new Random();

        private readonly ILogger<OperacionController> _logger;
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOperacionProxy _operacionProxy;
        private readonly IGiradorProxy _giradorProxy;
        private readonly IAdquirienteProxy _adquirienteProxy;
        //private readonly IAdquirienteUbicacionProxy _adquirienteUbicacion;
        //private readonly IGiradorUbicacionProxy _giradorUbicacionProxy;
        private readonly IFacturaOperacionesProxy _facturaOperacionesProxy;
        //private readonly IEvaluacionOperacionesProxy
        //private readonly IDivisoExternProxy _divisoExternProxy;
        private readonly IFilesProxy _filesProxy;
        private readonly IEvaluacionOperacionesProxy _evaluacionOperacionesProxy;
        //private readonly IComentariosProxy _comentariosProxy;
        private readonly IConfiguration _configuration;
        public const string Operaciones = "Operaciones";
        public const string TipoDocumento = "TipoDocumento";
        public const string Facturas = "Facturas";
        private readonly IWebHostEnvironment _env;
        //private readonly IEvaluacionOperacionesComentariosProxy _evaluacionOperacionesComentariosProxy;
        //private readonly IProcesoMasivoLoteFacturaProxy _procesoMasivoLoteFacturaProxy;
        public OperacionController(
            ILogger<OperacionController> logger,
            ICatalogoProxy catalogoProxy,
            IHttpContextAccessor httpContextAccessor,
            IOperacionProxy operacionProxy,
            IGiradorProxy giradorProxy,
            IAdquirienteProxy adquirienteProxy,
            //IAdquirienteUbicacionProxy adquirienteUbicacionProxy,
            //IGiradorUbicacionProxy giradorUbicacionProxy,
            IFacturaOperacionesProxy facturaOperacionesProxy,
            //IDivisoExternProxy divisoExternProxy,
            IFilesProxy filesProxy,
            IEvaluacionOperacionesProxy evaluacionOperacionesProxy,
            //IComentariosProxy comentariosProxy,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment
        //IEvaluacionOperacionesComentariosProxy evaluacionOperacionesComentariosProxy,
        //IProcesoMasivoLoteFacturaProxy procesoMasivoLoteFacturaProxy
        )
        {
            _logger = logger;
            _catalogoProxy = catalogoProxy;
            _httpContextAccessor = httpContextAccessor;
            _operacionProxy = operacionProxy;
            _giradorProxy = giradorProxy;
            _adquirienteProxy = adquirienteProxy;
            //_adquirienteUbicacion = adquirienteUbicacionProxy;
            //_giradorUbicacionProxy = giradorUbicacionProxy;
            _facturaOperacionesProxy = facturaOperacionesProxy;
            //_divisoExternProxy = divisoExternProxy;
            _filesProxy = filesProxy;
            _evaluacionOperacionesProxy = evaluacionOperacionesProxy;
            //_comentariosProxy = comentariosProxy;
            _configuration = configuration;
            _env = webHostEnvironment;
            //_evaluacionOperacionesComentariosProxy = evaluacionOperacionesComentariosProxy;
            //_procesoMasivoLoteFacturaProxy = procesoMasivoLoteFacturaProxy;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }

            var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 103, Valor = "0" });
            ViewBag.Estados = _Estados.Data.ToList();
            var _EstadosAprobacion = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 3, Codigo = 103, Valor = "0" });
            ViewBag.EstadoEvaluacion = _EstadosAprobacion.Data.ToList();
            return View();
        }

        public async Task<JsonResult> GetOperacionAllList(OperacionViewModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                var requestData = new OperacionesRequestDataTableDto();
                requestData.Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault()));
                requestData.PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault()));
                requestData.Sorting = "nIdOperaciones";
                requestData.SortOrder = "desc";
                requestData.FilterNroOperacion = model.NroOperacion;
                requestData.FilterRazonGirador = model.RazonGirador;
                requestData.FilterRazonAdquiriente = model.RazonAdquiriente;
                requestData.FilterFecCrea = model.FechaCreacion;
                requestData.Estado = model.Estado;
                requestData.Usuario = userName;

                var data = await _operacionProxy.GetAllListOperaciones(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].TotalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> DescargarRegistroOperacionArchivo(string operacion, string girador, string adquiriente, string fecha, string estado)
        {
            var response = await _operacionProxy.GetReporteRegistroOperacionDonwload(new OperacionesRequestDataTableDto
            {
                Estado = estado,
                FilterFecCrea = fecha,
                FilterNroOperacion = operacion,
                FilterRazonAdquiriente = adquiriente,
                FilterRazonGirador = girador
            });

            string base64data = response.Data;
            string fileName = DateTime.Now.ToString() + "_reporte_registro_operacion.xlsx";
            byte[] bytes = Convert.FromBase64String(base64data);
            return File(bytes, "application/octet-stream", fileName);
        }


        public async Task<IActionResult> DeleteOperacion(int nIdOperacion)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //foreach (int id in selectedOperacion)
            //{
            await _operacionProxy.Delete(nIdOperacion, userName);
            //}
            return Json(new { succeeded = true, message = "Registros eliminados correctamente..." });
        }

        public async Task<IActionResult> GetListDireccionByGirador(int idGirador)
        {
            //var listaDireccionGirador = await _giradorUbicacionProxy.GetAllListDireccionGirador(idGirador);
            return Json(null);
        }

        public async Task<IActionResult> GetListDireccionByAdquiriente(int idAdquiriente)
        {
            //var listaDireccionAdquiriente = await _adquirienteUbicacion.GetAllListDireccionAdquiriente(idAdquiriente);
            return Json(null);
        }

        public async Task<IActionResult> GetListCategoriaByGirador(int idGirador)
        {
            //CatalogoListDto obj = new CatalogoListDto { Codigo = idGirador };
            //var listaCategoriaGirador = await _catalogoProxy.GetGategoriaGirador(obj);
            return Json(null);
        }
        public async Task<IActionResult> Registro(int? operacionId)
        {
            ViewBag.Title = "Registro Operación: " + ((operacionId == null) ? "Nueva Operación" : "Editar Operación");
            ViewBag.IsEdit = operacionId != null;
            ViewBag.ListGirador = await _giradorProxy.GetAllListGiradorlista();
            ViewBag.ListAdquiriente = await _adquirienteProxy.GetAllListAdquirientelista();
            if (operacionId == null)
            {
                ViewBag.DireccionGirador = "";
                ViewBag.DireccionAdquiriente = "";
                ViewBag.Categoria = "";
                return View();
            }
            else
            {
                var operacionDetalle = await _operacionProxy.GetOperaciones((int)operacionId);
                if (operacionDetalle.Succeeded == false)
                {
                    return Redirect("~/Operacion/Index");
                }

                var _Categoria = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 102, Valor = "0" });
                ViewBag.Categoria = _Categoria.Data;

                OperacionCreateModel operacionData = new();
                if (ModelState.IsValid)
                {
                    operacionData.nNroOperacion = operacionDetalle.Data.nNroOperacion;
                    operacionData.IdOperacion = operacionDetalle.Data.nIdOperaciones;
                    operacionData.IdGirador = operacionDetalle.Data.nIdGirador;
                    operacionData.IdAdquiriente = operacionDetalle.Data.nIdAdquiriente;
                    operacionData.TEM = operacionDetalle.Data.nTEM;
                    operacionData.PorcentajeFinanciamiento = operacionDetalle.Data.nPorcentajeFinanciamiento;
                    operacionData.MontoOperacion = operacionDetalle.Data.nMontoOperacion;
                    operacionData.DescCobranza = operacionDetalle.Data.nDescCobranza;
                    operacionData.IdTipoMoneda = operacionDetalle.Data.nIdTipoMoneda;
                    operacionData.Estado = operacionDetalle.Data.nEstado;
                    operacionData.NombreEstado = operacionDetalle.Data.NombreEstado;
                    operacionData.InteresMoratorio = operacionDetalle.Data.InteresMoratorio;
                    operacionData.IdCategoria = operacionDetalle.Data.IdCategoria;
                    operacionData.IdGiradorCod = operacionDetalle.Data.nIdGirador;
                    operacionData.IdAdquirienteCod = operacionDetalle.Data.nIdAdquiriente;
                    operacionData.DescFactura = operacionDetalle.Data.nDescFactura;
                    operacionData.DescContrato = operacionDetalle.Data.nDescContrato;
                    ViewBag.IdGiradorCod = operacionDetalle.Data.nIdGirador;
                    ViewBag.IdAdquirienteCod = operacionDetalle.Data.nIdAdquiriente;
                }
                else
                {
                    ViewBag.IdGiradorCod = "";
                    ViewBag.IdAdquirienteCod = "";
                }
                return View(operacionData);
            }
        }

        public async Task<IActionResult> Detalle(int? operacionId)
        {
            ViewBag.Title = "Registro Operación: " + ((operacionId == null) ? "" : "Detalle Operación");
            if (operacionId == null)
            {
                return View();
            }
            else
            {
                var operacionDetalle = await _operacionProxy.GetOperaciones((int)operacionId);
                if (operacionDetalle.Succeeded == false)
                {
                    return Redirect("~/Operacion/Index");
                }

                OperacionSingleViewModel operacionData = new();
                if (ModelState.IsValid)
                {

                    var _Categoria = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 102, Valor = "0" });//await _catalogoProxy.GetGategoriaGirador(new Model.Models.Catalogo.CatalogoListDto { Codigo = operacionDetalle.Data.nIdGirador });
                    ViewBag.Categoria = _Categoria.Data;
                    operacionData.nNroOperacion = operacionDetalle.Data.nNroOperacion;
                    operacionData.IdOperacion = operacionDetalle.Data.nIdOperaciones;
                    operacionData.IdGirador = operacionDetalle.Data.nIdGirador;
                    operacionData.IdAdquiriente = operacionDetalle.Data.nIdAdquiriente;
                    operacionData.IdGiradorDireccion = operacionDetalle.Data.nIdGiradorDireccion;
                    operacionData.IdAdquirienteDireccion = operacionDetalle.Data.nIdAdquirienteDireccion;
                    operacionData.TEM = operacionDetalle.Data.nTEM;
                    operacionData.PorcentajeFinanciamiento = operacionDetalle.Data.nPorcentajeFinanciamiento;
                    operacionData.MontoOperacion = operacionDetalle.Data.nMontoOperacion;
                    operacionData.DescContrato = operacionDetalle.Data.nDescContrato;
                    operacionData.DescFactura = operacionDetalle.Data.nDescFactura;
                    operacionData.DescCobranza = operacionDetalle.Data.nDescCobranza;
                    operacionData.IdTipoMoneda = operacionDetalle.Data.nIdTipoMoneda;
                    operacionData.Moneda = operacionDetalle.Data.Moneda;
                    operacionData.PorcentajeRetencion = operacionDetalle.Data.nPorcentajeRetencion;
                    operacionData.RazonSocialGirador = operacionDetalle.Data.cRazonSocialGirador;
                    operacionData.RazonSocialAdquiriente = operacionDetalle.Data.cRazonSocialAdquiriente;
                    operacionData.RegUnicoEmpresaAdquiriente = operacionDetalle.Data.cRegUnicoEmpresaAdquiriente;
                    operacionData.RegUnicoEmpresaGirador = operacionDetalle.Data.cRegUnicoEmpresaGirador;
                    operacionData.DireccionAdquiriente = operacionDetalle.Data.DireccionAdquiriente;
                    operacionData.DireccionGirador = operacionDetalle.Data.DireccionGirador;
                    operacionData.NombreEstado = operacionDetalle.Data.NombreEstado;
                    operacionData.InteresMoratorio = operacionDetalle.Data.InteresMoratorio;
                    operacionData.Categoria = operacionDetalle.Data.IdCategoria == 0 ? string.Empty : _Categoria.Data.FirstOrDefault(x => x.nId == operacionDetalle.Data.IdCategoria).cNombre;
                    operacionData.MotivoTransaccion = operacionDetalle.Data.MotivoTransaccion;
                    operacionData.SustentoComercial = operacionDetalle.Data.SustentoComercial;
                    operacionData.Plazo = operacionDetalle.Data.Plazo;
                }
                return View(operacionData);
            }
        }

        public async Task<IActionResult> GetPartial(int paramId)
        {
            //var operacionDetalle = await _operacionProxy.GetOperaciones((int)paramId);
            OperacionSingleViewModel operacionData = new();
            //if (operacionDetalle.Succeeded == false)
            //{
            //    ViewBag.ErrorMsg = "No se encontro información para mostrar.";
            //}
            //else
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var _TipoDocumento = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 118 });
            //        ViewBag.TipoDocumento = _TipoDocumento.Data.ToList();
            //        ViewBag.nEstadoDetalle = operacionDetalle.Data.nEstado;
            //        operacionData.IdOperacion = operacionDetalle.Data.nIdOperaciones;
            //        operacionData.IdGirador = operacionDetalle.Data.nIdGirador;
            //        operacionData.IdAdquiriente = operacionDetalle.Data.nIdAdquiriente;
            //        //operacionData.IdInversionista = operacionDetalle.Data.nIdInversionista;
            //        //operacionData.NombreInversionista = operacionDetalle.Data.cNombreInversionista;
            //        operacionData.IdGiradorDireccion = operacionDetalle.Data.nIdGiradorDireccion;
            //        operacionData.IdAdquirienteDireccion = operacionDetalle.Data.nIdAdquirienteDireccion;
            //        operacionData.TEM = operacionDetalle.Data.nTEM;
            //        operacionData.PorcentajeFinanciamiento = operacionDetalle.Data.nPorcentajeFinanciamiento;
            //        operacionData.MontoOperacion = operacionDetalle.Data.nMontoOperacion;
            //        operacionData.DescContrato = operacionDetalle.Data.nDescContrato;
            //        operacionData.DescFactura = operacionDetalle.Data.nDescFactura;
            //        operacionData.DescCobranza = operacionDetalle.Data.nDescCobranza;
            //        operacionData.IdTipoMoneda = operacionDetalle.Data.nIdTipoMoneda;
            //        operacionData.Moneda = operacionDetalle.Data.Moneda;
            //        operacionData.PorcentajeRetencion = operacionDetalle.Data.nPorcentajeRetencion;
            //        operacionData.RazonSocialGirador = operacionDetalle.Data.cRazonSocialGirador;
            //        operacionData.RazonSocialAdquiriente = operacionDetalle.Data.cRazonSocialAdquiriente;
            //        operacionData.RegUnicoEmpresaAdquiriente = operacionDetalle.Data.cRegUnicoEmpresaAdquiriente;
            //        operacionData.RegUnicoEmpresaGirador = operacionDetalle.Data.cRegUnicoEmpresaGirador;
            //        operacionData.DireccionAdquiriente = operacionDetalle.Data.DireccionAdquiriente;
            //        operacionData.DireccionGirador = operacionDetalle.Data.DireccionGirador;
            //        operacionData.NombreEstado = operacionDetalle.Data.NombreEstado;
            //        operacionData.InteresMoratorio = operacionDetalle.Data.InteresMoratorio;
            //        operacionData.cDesCategoria = operacionDetalle.Data.cDesCategoria;
            //        operacionData.MotivoTransaccion = operacionDetalle.Data.MotivoTransaccion;
            //        operacionData.SustentoComercial = operacionDetalle.Data.SustentoComercial;
            //        operacionData.Plazo = operacionDetalle.Data.Plazo;
            //        operacionData.IdCategoria = operacionDetalle.Data.IdCategoria;
            //    }
            //}
            return PartialView("_PartialDetalle", operacionData);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroProceso(int operacionId, string operacionFlat, OperacionCreateModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool IsOperacionExist = false;
            model.IdAdquiriente = model.IdAdquirienteCod;
            model.IdGirador = model.IdGiradorCod;
            var operacionDetalle = await _operacionProxy.GetOperaciones(operacionId);
            if (operacionDetalle.Succeeded == true)
            {
                IsOperacionExist = true;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (IsOperacionExist)
                    {
                        var result = await _operacionProxy.Update(new OperacionesUpdateDto
                        {
                            IdOperaciones = operacionId,
                            IdGirador = model.IdGiradorCod,
                            IdAdquiriente = model.IdAdquirienteCod,
                            //IdGiradorDireccion = model.IdGiradorDireccion,
                            //IdAdquirienteDireccion = model.IdAdquirienteDireccion,
                            TEM = model.TEM,
                            PorcentajeFinanciamiento = model.PorcentajeFinanciamiento,
                            MontoOperacion = model.MontoOperacion,
                            DescContrato = model.DescContrato,
                            DescFactura = model.DescFactura,
                            DescCobranza = model.DescCobranza,
                            IdTipoMoneda = model.IdTipoMoneda,
                            UsuarioActualizacion = userName,
                            InteresMoratorio = model.InteresMoratorio,
                            IdCategoria = model.IdCategoria

                        });
                        return Json(result);
                    }
                    else
                    {
                        var result = await _operacionProxy.Create(new OperacionesInsertDto
                        {
                            IdGirador = model.IdGiradorCod,
                            IdAdquiriente = model.IdAdquirienteCod,
                            TEM = model.TEM,
                            PorcentajeFinanciamiento = model.PorcentajeFinanciamiento,
                            MontoOperacion = model.MontoOperacion,
                            DescContrato = 0,
                            DescFactura = 0,
                            DescCobranza = model.DescCobranza,
                            IdTipoMoneda = model.IdTipoMoneda,
                            UsuarioCreador = userName,
                            InteresMoratorio = model.InteresMoratorio,
                            IdCategoria = model.IdCategoria
                        });
                        return Json(result);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Redirect("~/Operacion/Index");
        }


        public async Task<IActionResult> ResultadoEvaluacion(OperacionViewModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 4, Codigo = 103, Valor = "0" });
            var nCant = _Estados.Data.Where(n => n.nId == model.nIdEstadoEvaluacion).ToList();
            bool pRegistro=false;
           
            var _estadoOperaciones = await _evaluacionOperacionesProxy.Create(new EvaluacionOperacionesInsertDto
            {
                IdOperaciones = model.nIdOperacionEval,
                IdCatalogoEstado = model.nIdEstadoEvaluacion,
                UsuarioCreador = userName,
                Comentario = model.cComentario,
                bRegistro=pRegistro

            });

            if (nCant.Count > 0 && model.nIdEstadoEvaluacion != 11)
            {
                pRegistro = true;
            }

            if (_estadoOperaciones.Succeeded && nCant.Count > 0)
            {
                await _evaluacionOperacionesProxy.CreateEstadoFactura(new EvaluacionOperacionesEstadoInsertDto
                {
                    IdOperaciones = model.nIdOperacionEval,
                    IdCatalogoEstado = model.nIdEstadoEvaluacion,
                    UsuarioCreador = userName,
                    Comentario = model.cComentario,
                    bRegistro = pRegistro
                });

                if (model.nIdEstadoEvaluacion == 10 || model.nIdEstadoEvaluacion == 11)
                {
                    var oFactura = await _facturaOperacionesProxy.GetAllListFacturaByIdOperaciones(model.nIdOperacionEval);
                    if (oFactura.Data.Count > 0)
                    {
                        foreach (var item in oFactura.Data)
                        {
                            await _evaluacionOperacionesProxy.UpdateCalculoFactura(new EvaluacionOperacionesCalculoInsertDto
                            {
                                IdOperaciones = model.nIdOperacionEval,
                                IdOperacionesFactura= item.nIdOperacionesFacturas,
                                IdCatalogoEstado = model.nIdEstadoEvaluacion,
                                UsuarioCreador = userName,
                                cFecha = item.dFechaRegistro.ToString()
                            });
                        }
                    }
                }
                //if (model.nIdEstadoEvaluacion == 11)
                //{
                //    await _evaluacionOperacionesProxy.UpdateCalculoFactura(new EvaluacionOperacionesCalculoInsertDto
                //    {
                //        IdOperaciones = model.nIdOperacionEval,
                //        IdOperacionesFactura = 0,
                //        IdCatalogoEstado = model.nIdEstadoEvaluacion,
                //        UsuarioCreador = userName,
                //    });
                //}
            }

            return Json(_estadoOperaciones);
        }

        public async Task<IActionResult> ActualizarMontoFactura(OperacionViewModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        
            var _estadoOperaciones = await _facturaOperacionesProxy.EditarMonto(new OperacionesFacturaEditMontoDto
            {
                nIdOperaciones = model.nIdOperaciones.Value,
                nIdOperacionesFacturas = model.nIdOperacionesFacturas.Value,
                cUsuarioActualizacion = userName,
                nMonto = model.nMonto.Value
            });

            return Json(_estadoOperaciones);
        }

        //return Json(new { succeeded = true, message = "Registros eliminados correctamente..." });
        public async Task<IActionResult> AnularOperacion(int operacionId)
        {
            //var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var _estadoAnulacion = await _evaluacionOperacionesProxy.Create(new EvaluacionOperacionesInsertDto
            //{
            //    IdOperaciones = operacionId,
            //    IdCatalogoEstado = 0,
            //    UsuarioCreador = userName
            //});
            return Json(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LevantarObservacionAsync(OperacionCreateModel model)
        {
            //var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var _estadoOperacion = await _evaluacionOperacionesProxy.Create(new EvaluacionOperacionesInsertDto
            //{
            //    IdOperaciones = model.IdOperacion,
            //    IdCatalogoEstado = 18,
            //    UsuarioCreador = userName
            //});
            //if (_estadoOperacion.Succeeded)
            //{
            //    await _evaluacionOperacionesProxy.CreateComment(new EvaluacionOperacionesComentariosInsertDto
            //    {
            //        IdEvaluacionOperaciones = _estadoOperacion.Data,
            //        Comentario = model.ComentarioOperaciones,
            //        IdCatalogoTipoComentario = 1,
            //        UsuarioCreador = userName
            //    });
            //}
            return Json(null);
        }
        public async Task<IActionResult> GetAllFacturas(int operacionId)
        {
            return Json(await _facturaOperacionesProxy.GetAllListFacturaByIdOperaciones(operacionId));
        }

        public async Task<IActionResult> GetAllCavali(int operacionId)
        {
            return Json(null);
        }


        public async Task<IActionResult> GetAllDocumentoSolicitud(int operacionId)
        {
            return Json(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarFacturas(AgregarFactura model)
        {
            string nroOperacion = string.Empty;
            List<ReportesGiradorOperacionesResponse> lstAuxiliar = new List<ReportesGiradorOperacionesResponse>();
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //int nIdGirador = 0;
            if (ModelState.IsValid)
            {
                var operacionDetalle = await _operacionProxy.GetOperaciones(model.IdOperacionCabeceraFacturas);
                if (operacionDetalle != null)
                {
                    nroOperacion = operacionDetalle.Data.nNroOperacion;
                }

                byte[] data;
                using (var br = new BinaryReader(model.fileXml.OpenReadStream()))
                    data = br.ReadBytes((int)model.fileXml.OpenReadStream().Length);

                string path1 = $"{_env.WebRootPath}\\assets\\upload";
                if (!Directory.Exists(path1))
                    Directory.CreateDirectory(path1);
                string sFileName = $"{path1}\\{model.fileXml.FileName}";
                System.IO.File.WriteAllBytes(sFileName, data);
                string cFactura = string.Empty;
                string cGirador = string.Empty;
                string cGiradorRUT = string.Empty;
                string cGiradorDireccion = string.Empty;
                string cGiradorUbigeo = string.Empty;

                string cAdquiriente = string.Empty;
                string cAdquirienteRUT = string.Empty;
                string cAdquirienteDireccion = string.Empty;
                string cAdquirienteUbigeo = string.Empty;

                string cMoneda = string.Empty;
                string cFieldName = string.Empty;
                DateTime dFechaEmision = DateTime.Today;
                DateTime dFechaVencimiento = DateTime.Today;
                decimal nMonto = 0;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(sFileName);

                #region Ensobrado

                XPathNavigator navNs = xmlDocument.CreateNavigator();
                navNs.MoveToFollowing(XPathNodeType.Element);
                IDictionary<string, string> nsDictList = navNs.GetNamespacesInScope(XmlNamespaceScope.All);

                XmlNamespaceManager ns = new XmlNamespaceManager(xmlDocument.NameTable);

                foreach (var nsItem in nsDictList)
                {
                    if (string.IsNullOrEmpty(nsItem.Key))
                        ns.AddNamespace("sig", nsItem.Value);
                    else
                        ns.AddNamespace(nsItem.Key, nsItem.Value);
                }
                ns.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

                var nav = xmlDocument.CreateNavigator();

                #endregion

                string fecha = nav.SelectSingleNode("/descendant::*[local-name() = 'IssueDate'][1]", ns)?.Value ?? "1900-01-01";
                dFechaEmision = Convert.ToDateTime(fecha);
                fecha = nav.SelectSingleNode("/descendant::*[local-name() = 'PaymentDueDate'][1]", ns)?.Value ?? "1900-01-01";
                dFechaVencimiento = Convert.ToDateTime(fecha);
                cFactura = nav.SelectSingleNode("//*[local-name() = 'Invoice']/*[local-name() = 'ID']", ns)?.Value ?? "0000000";
                cMoneda = nav.SelectSingleNode("//*[local-name() = 'DocumentCurrencyCode']", ns)?.Value ?? "";
                cGiradorRUT = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyIdentification']/*[local-name() = 'ID']", ns)?.Value ?? "";
                cAdquirienteRUT = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingCustomerParty']/*[local-name() = 'Party']/*[local-name() = 'PartyIdentification']/*[local-name() = 'ID']", ns)?.Value ?? "";
                cGirador = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingSupplierParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationName']", ns)?.Value ?? "";
                cAdquiriente = nav.SelectSingleNode("/*[local-name() = 'Invoice']/*[local-name() = 'AccountingCustomerParty']/*[local-name() = 'Party']/*[local-name() = 'PartyLegalEntity']/*[local-name() = 'RegistrationName']", ns)?.Value ?? "";

                foreach (XPathNavigator node in nav.Select("//cac:PaymentTerms", ns))
                {
                    bool bValidNode = false;
                    foreach (XPathNavigator child in node.SelectChildren(XPathNodeType.All))
                    {
                        if (child.Value.ToUpper() == "CREDITO")
                            bValidNode = true;
                        if (child.Name == "cbc:Amount" && bValidNode)
                            nMonto = Convert.ToDecimal(child.Value);
                    }
                }
                string mensajeErrorG = "{0} {1} tiene que ser igual que la operación";

                var oGirador = GetGirador(cGiradorRUT).Result;
                if (oGirador == null)
                {
                    //throw new Exception($"Girador {cGiradorRUT} tiene que ser igual que la operación.");
                    return Json(new { succeeded = false, message = string.Format(mensajeErrorG,"Girador", cGirador) });

                }

                var oAdquiriente = GetAdquiriente(cAdquirienteRUT).Result;
                if (oAdquiriente == null)
                {
                    return Json(new { succeeded = false, message = string.Format(mensajeErrorG, "Aceptante", cAdquiriente) });
                }

                if (oGirador.nIdGirador != model.nIdGiradorFact)
                {
                    return Json(new { succeeded = false, message = string.Format(mensajeErrorG, "Girador", cGirador) });
                }
                if (oAdquiriente.nIdAdquiriente != model.nIdAdquirenteFact)
                {
                    return Json(new { succeeded = false, message = string.Format(mensajeErrorG, "Aceptante", cAdquiriente) });
                }
                if (operacionDetalle.Data.nIdTipoMoneda != (cMoneda == "PEN" ? 1 : 2))
                {
                    return Json(new { succeeded = false, message = string.Format(mensajeErrorG, "Moneda", cMoneda) });
                }


                string[] docnum = cFactura.Split('-');
                var NroDocumento = new { Serie = docnum[0].ToString(), Numero = docnum[1].ToString() };
                var jsonFactura = System.Text.Json.JsonSerializer.Serialize(NroDocumento).ToString();
                var oFactura = GetFactura(model.nIdGiradorFact, model.nIdAdquirenteFact, jsonFactura).Result;
                if (oFactura.Succeeded)
                    if (oFactura.Data.nEstado == 1)
                        throw new Exception($"Factura Nro. {cFactura}  ya se encuentra registrada.");

                var oRecord = new ReportesGiradorOperacionesResponse
                {
                    nNroOperacion = 0,
                    IdGirador = model.nIdGiradorFact,
                    IdAdquiriente = model.nIdAdquirenteFact,
                    IdTipoMoneda = (cMoneda == "PEN" ? 1 : 2),
                    NroFactura = cFactura,
                    cMoneda = cMoneda,
                    ImporteNetoFactura = nMonto,
                    dFechaEmision = dFechaEmision,
                    dFechaVencimiento = dFechaVencimiento,
                    dFechaPagoNegociado = dFechaVencimiento,
                    NombreDocumentoXML = model.fileXml.FileName,
                    Estado = "OK"
                };
                lstAuxiliar.Add(oRecord);
                string randon = RandomString(10);
                var path = _configuration[$"PathDocumentos:{Operaciones}"].ToString() + "\\" + nroOperacion + _configuration[$"PathDocumentos:{Facturas}"].ToString();
                var archivoXML = await _filesProxy.UploadFile(model.fileXml, randon + "_" + model.fileXml.FileName, path);
                if (archivoXML.Succeeded)
                {
                    var result = await _facturaOperacionesProxy.Create(new OperacionesFacturaInsertDto
                    {
                        IdOperaciones = model.IdOperacionCabeceraFacturas,
                        NroDocumento = System.Text.Json.JsonSerializer.Serialize(NroDocumento).ToString(),
                        Monto = lstAuxiliar[0].ImporteNetoFactura,
                        FechaEmision = (DateTime)lstAuxiliar[0].dFechaEmision,
                        FechaVencimiento = (DateTime)lstAuxiliar[0].dFechaVencimiento,
                        NombreDocumentoXML = model.fileXml.FileName,
                        RutaDocumentoXML = Path.Combine(path, randon + "_" + model.fileXml.FileName),
                        NombreDocumentoPDF = "",
                        RutaDocumentoPDF = "",
                        UsuarioCreador = userName,
                        FechaPagoNegociado = (DateTime)lstAuxiliar[0].dFechaPagoNegociado

                    });
                    return Json(result);
                }
                else
                {
                    return Json(new { succeeded = false, message = "El archivo no se cargo, intente nuevamente..." });
                }
            }
            else
            {
                return Json(new { succeeded = false, message = "Ocurrió un error, intente nuevamente..." });
            }
        }
        private async Task<AdquirienteResponseDatatableDto> GetAdquiriente(string sRUC)
        {
            AdquirienteResponseDatatableDto oRecord = null;
            try
            {
                var requestData = new AdquirienteRequestDatatableDto();
                requestData.Pageno = 0;
                requestData.PageSize = 5;
                requestData.Sorting = "nIdAdquiriente";
                requestData.SortOrder = "asc";
                requestData.FilterRuc = sRUC;
                var data = await _adquirienteProxy.GetAllListAdquiriente(requestData);
                if (data.Data.Count > 0)
                    oRecord = data.Data[0];
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }
        private async Task<GiradorResponseDatatableDto> GetGirador(string sRUC)
        {
            GiradorResponseDatatableDto oRecord = null;
            try
            {
                var requestData = new GiradorRequestDatatableDto();
                requestData.Pageno = 0;
                requestData.PageSize = 5;
                requestData.Sorting = "nIdGirador";
                requestData.SortOrder = "asc";
                requestData.FilterRuc = sRUC;
                var data = await _giradorProxy.GetAllListGirador(requestData);

                if (data.Data.Count > 0)
                    oRecord = data.Data[0];
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }


        private async Task<ResponseData<OperacionesFacturaListDto>> GetFactura(int IdGirador, int IdAdquiriente, string NroFactura)
        {
            ResponseData<OperacionesFacturaListDto> oRecord;
            try
            {
                oRecord = await _facturaOperacionesProxy.GetInvoiceByNumber(IdGirador, IdAdquiriente, NroFactura);
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarFactura(EliminarFactura model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var archivoXML = await _filesProxy.DeleteFiles(model.filePath);
            if (archivoXML.Succeeded)
            {
                var result = await _facturaOperacionesProxy.Delete(model.operacionFacturaId, userName);
                return Json(result);
            }
            else
            {
                return Json(new { succeeded = false, message = "El archivo no se elimino, intente nuevamente..." });
            }

        }


        public async Task<IActionResult> GetAllComentariosOperacion(int operacionId)
        {
            //var comentarios = await _comentariosProxy.GetAllListComentarios(3, operacionId);
            return Json(null);
        }

        public async Task<IActionResult> GetAllCondiciones(int operacionId)
        {
            //var comentarios = await _comentariosProxy.GetAllListCondiciones(operacionId);
            /*f()*/
            return Json(null);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string nIdOperacionFactura)
        {
            string filename = string.Empty;
            ResponseData<List<OperacionesFacturaListDto>> lst = new ResponseData<List<OperacionesFacturaListDto>>();
            lst = await _facturaOperacionesProxy.GetAllListFacturaByIdOperacionesFactura(Convert.ToInt32(nIdOperacionFactura));
            if (lst.Data != null)
            {
                if (lst.Data.Count > 0)
                    filename = lst.Data[0].cRutaDocumentoXML;

            }
            //filename = _configuration[$"PathDocumentos:{Operaciones}"].ToString() + "\\" + filename;
            var bytesFile = await _filesProxy.DownloadFile(filename);
            string[] words = filename.Split(@"\");
            for (int i = 1; i < words.Length; i++)
            {
                if (i == words.Length - 1)
                {
                    filename = words[i].ToString();
                }
            }
            return File(bytesFile, "application/octet-stream", filename);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private DataSet SetAsDataSet(IExcelDataReader reader)
        {
            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true,
                }
            });
            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFactura(EditarFactura model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _facturaOperacionesProxy.Editar(new OperacionesFacturaEditDto()
            {
                FechaPagoNegociado = model.dFechaPagoNegociado,
                UsuarioActualizacion = userName,
                IdOperacionesFacturas = model.nIdOperacionesFacturas,
                Estado = 0      //  <OAV - 30/01/2023>
            });
            return Json(result.Succeeded);


        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFileSolicitud(string nIdSolicitudEvaluacion)
        {
            //string filename = string.Empty;
            //ResponseData<List<DocumentoSolicitudOperacionListIdDto>> lst = new ResponseData<List<DocumentoSolicitudOperacionListIdDto>>();
            //lst = await _facturaOperacionesProxy.GetAllDocumentoSolicitudByOperaciones(Convert.ToInt32(nIdSolicitudEvaluacion));
            //if (lst.Data != null)
            //{
            //    if (lst.Data.Count > 0)
            //        filename = lst.Data[0].cRutaDocumento;

            //}
            //var bytesFile = await _filesProxy.DownloadFile(filename);
            //string[] words = filename.Split(@"\");
            //for (int i = 1; i < words.Length; i++)
            //{
            //    if (i == words.Length - 1)
            //    {
            //        filename = words[i].ToString();
            //    }
            //}
            return File("", "application/octet-stream", "");
        }

    }
}