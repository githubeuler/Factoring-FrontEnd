﻿using Microsoft.AspNetCore.Authorization;
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
            _fondeadorProxy= fondeadorProxy;
            _catalogoProxy = catalogoProxy;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
           // var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 103, Valor = "0" });
            
            ViewBag.ListInversionista = await _fondeadorProxy.GetAllListFondeadoreslista();
            var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 114, Valor = "0" });
            var _Motivos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 115,Valor = "0" });
            var _Acciones = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 123, Valor = "0" });
            ViewBag.Estados = _Estados.Data.ToList();
            ViewBag.Motivos = _Motivos.Data.ToList();
            ViewBag.Acciones = _Acciones.Data.ToList();
            return View();
        }

        public async Task<JsonResult> GetOperacionAllList(OperacionesFacturaRequestDataTableDto model)
        {
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
                var data = await _facturaOperacionesProxy.GetBandejaFacturas(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].totalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTotal, recordsFiltered = recordsTotal });
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

        public async Task<IActionResult> ObtenerAsignaciones(List<int> IdFacturas)
        {
            try
            {
                var result = await _facturaOperacionesProxy.ObtenerAsignaciones(new OperacionesFacturaValidaAsignacion
                {
                    nLstIdFacturas = IdFacturas
                });
                if (result != null)
                {
                    if (result.Data != null)
                    {
                        if (result.Data[0].cantidadInversionistas > 1)
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
    }
}