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
        //private readonly IDivisoExternProxy _divisoExternProxy;
        //private readonly IFilesProxy _filesProxy;
        private readonly IEvaluacionOperacionesProxy _evaluacionOperacionesProxy;
        //private readonly IComentariosProxy _comentariosProxy;
        private readonly IConfiguration _configuration;
        public const string Operaciones = "Operaciones";
        public const string TipoDocumento = "TipoDocumento";
        public const string Facturas = "Facturas";
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
            //IFilesProxy filesProxy,
            IEvaluacionOperacionesProxy evaluacionOperacionesProxy,
            //IComentariosProxy comentariosProxy,
            IConfiguration configuration
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
            //_filesProxy = filesProxy;
            _evaluacionOperacionesProxy = evaluacionOperacionesProxy;
            //_comentariosProxy = comentariosProxy;
            _configuration = configuration;
            //_evaluacionOperacionesComentariosProxy = evaluacionOperacionesComentariosProxy;
            //_procesoMasivoLoteFacturaProxy = procesoMasivoLoteFacturaProxy;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }

            var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 101, Valor = "0" });
            ViewBag.Estados = _Estados.Data.ToList();
            return View();
        }

        public async Task<JsonResult> GetOperacionAllList(OperacionViewModel model)
        {
            try
            {
                var requestData = new OperacionesRequestDataTableDto();
                requestData.Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault()));
                requestData.PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault()));
                requestData.Sorting = "nIdOperaciones";
                requestData.SortOrder = "asc";
                requestData.FilterNroOperacion = model.NroOperacion;
                requestData.FilterRazonGirador = model.RazonGirador;
                requestData.FilterRazonAdquiriente = model.RazonAdquiriente;
                requestData.FilterFecCrea = model.FechaCreacion;
                requestData.Estado = model.Estado;

                var data = await _operacionProxy.GetAllListOperaciones(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].TotalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IActionResult> DeleteOperacion(int[] selectedOperacion)
        {
            //var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //foreach (int id in selectedOperacion)
            //{
            //    await _operacionProxy.Delete(id, userName);
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

                var _Categoria = await _catalogoProxy.GetGategoriaGirador(new Model.Models.Catalogo.CatalogoListDto { Codigo = operacionDetalle.Data.nIdGirador });
                ViewBag.Categoria = _Categoria.Data;

                OperacionCreateModel operacionData = new();
                if (ModelState.IsValid)
                {
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
                    operacionData.PorcentajeRetencion = operacionDetalle.Data.nPorcentajeRetencion;
                    operacionData.Estado = operacionDetalle.Data.nEstado;
                    operacionData.NombreEstado = operacionDetalle.Data.NombreEstado;
                    operacionData.InteresMoratorio = operacionDetalle.Data.InteresMoratorio;
                    operacionData.MotivoTransaccion = operacionDetalle.Data.MotivoTransaccion;
                    operacionData.SustentoComercial = operacionDetalle.Data.SustentoComercial;
                    operacionData.IdCategoria = operacionDetalle.Data.IdCategoria;
                    operacionData.Plazo = operacionDetalle.Data.Plazo;

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
                //var operacionDetalle = await _operacionProxy.GetOperaciones((int)operacionId);
                //if (operacionDetalle.Succeeded == false)
                //{
                //    return Redirect("~/Operacion/Index");
                //}

                OperacionSingleViewModel operacionData = new();
                //if (ModelState.IsValid)
                //{

                //    var _Categoria = await _catalogoProxy.GetGategoriaGirador(new Model.Models.Catalogo.CatalogoListDto { Codigo = operacionDetalle.Data.nIdGirador });

                //    ViewBag.Categoria = _Categoria.Data;

                //    operacionData.IdOperacion = operacionDetalle.Data.nIdOperaciones;
                //    operacionData.IdGirador = operacionDetalle.Data.nIdGirador;
                //    operacionData.IdAdquiriente = operacionDetalle.Data.nIdAdquiriente;
                //    //operacionData.IdInversionista = operacionDetalle.Data.nIdInversionista;
                //    //operacionData.NombreInversionista = operacionDetalle.Data.cNombreInversionista;
                //    operacionData.IdGiradorDireccion = operacionDetalle.Data.nIdGiradorDireccion;
                //    operacionData.IdAdquirienteDireccion = operacionDetalle.Data.nIdAdquirienteDireccion;
                //    operacionData.TEM = operacionDetalle.Data.nTEM;
                //    operacionData.PorcentajeFinanciamiento = operacionDetalle.Data.nPorcentajeFinanciamiento;
                //    operacionData.MontoOperacion = operacionDetalle.Data.nMontoOperacion;
                //    operacionData.DescContrato = operacionDetalle.Data.nDescContrato;
                //    operacionData.DescFactura = operacionDetalle.Data.nDescFactura;
                //    operacionData.DescCobranza = operacionDetalle.Data.nDescCobranza;
                //    operacionData.IdTipoMoneda = operacionDetalle.Data.nIdTipoMoneda;
                //    operacionData.Moneda = operacionDetalle.Data.Moneda;
                //    operacionData.PorcentajeRetencion = operacionDetalle.Data.nPorcentajeRetencion;
                //    operacionData.RazonSocialGirador = operacionDetalle.Data.cRazonSocialGirador;
                //    operacionData.RazonSocialAdquiriente = operacionDetalle.Data.cRazonSocialAdquiriente;
                //    operacionData.RegUnicoEmpresaAdquiriente = operacionDetalle.Data.cRegUnicoEmpresaAdquiriente;
                //    operacionData.RegUnicoEmpresaGirador = operacionDetalle.Data.cRegUnicoEmpresaGirador;
                //    operacionData.DireccionAdquiriente = operacionDetalle.Data.DireccionAdquiriente;
                //    operacionData.DireccionGirador = operacionDetalle.Data.DireccionGirador;
                //    operacionData.NombreEstado = operacionDetalle.Data.NombreEstado;
                //    operacionData.InteresMoratorio = operacionDetalle.Data.InteresMoratorio;
                //    operacionData.Categoria = operacionDetalle.Data.IdCategoria == 0 ? string.Empty : _Categoria.Data.FirstOrDefault(x => x.nId == operacionDetalle.Data.IdCategoria).cNombre;
                //    operacionData.MotivoTransaccion = operacionDetalle.Data.MotivoTransaccion;
                //    operacionData.SustentoComercial = operacionDetalle.Data.SustentoComercial;
                //    operacionData.Plazo = operacionDetalle.Data.Plazo;
                //}
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
                            IdGirador = model.IdGirador,
                            IdAdquiriente = model.IdAdquiriente,
                            IdGiradorDireccion = model.IdGiradorDireccion,
                            IdAdquirienteDireccion = model.IdAdquirienteDireccion,
                            TEM = model.TEM,
                            PorcentajeFinanciamiento = model.PorcentajeFinanciamiento,
                            MontoOperacion = model.MontoOperacion,
                            DescContrato = 0,
                            DescFactura = 0,
                            DescCobranza = model.DescCobranza,
                            IdTipoMoneda = model.IdTipoMoneda,
                            PorcentajeRetencion = model.PorcentajeRetencion,
                            UsuarioActualizacion = userName,
                            InteresMoratorio = model.InteresMoratorio,
                            IdCategoria = model.IdCategoria,
                            MotivoTransaccion = model.MotivoTransaccion,
                            SustentoComercial = model.SustentoComercial,
                            Plazo = model.Plazo

                        });

                        //if (operacionFlat == "E" && result.Succeeded)
                        //{
                        //    var resultado = await _evaluacionOperacionesProxy.Create(new EvaluacionOperacionesInsertDto
                        //    {
                        //        IdOperaciones = operacionId,
                        //        IdCatalogoEstado = 18,
                        //        UsuarioCreador = userName
                        //    });
                        //}
                        // else
                        // {

                        //if (result.Succeeded)
                        //{

                        //await _evaluacionOperacionesComentariosProxy.UpdateComentario(new EvaluacionUpdateOperacionesComentariosInsertDto
                        //{
                        //nIdOperacion = operacionId,
                        //Comentario = model.SustentoComercial,
                        // UsuarioCreador = userName
                        //});
                        //}

                        //}
                        return Json(result);
                    }
                    else
                    {
                        var result = await _operacionProxy.Create(new OperacionesInsertDto
                        {
                            IdGirador = model.IdGirador,
                            IdAdquiriente = model.IdAdquiriente,
                            IdGiradorDireccion = model.IdGiradorDireccion,
                            IdAdquirienteDireccion = model.IdAdquirienteDireccion,
                            TEM = model.TEM,
                            PorcentajeFinanciamiento = model.PorcentajeFinanciamiento,
                            MontoOperacion = model.MontoOperacion,
                            DescContrato = 0,
                            DescFactura = 0,
                            DescCobranza = model.DescCobranza,
                            IdTipoMoneda = model.IdTipoMoneda,
                            PorcentajeRetencion = model.PorcentajeRetencion,
                            UsuarioCreador = userName,
                            InteresMoratorio = model.InteresMoratorio,
                            IdCategoria = model.IdCategoria,
                            MotivoTransaccion = model.MotivoTransaccion,
                            SustentoComercial = model.SustentoComercial,
                            Plazo = model.Plazo
                        });
                        //if (result.Succeeded)
                        //{
                        //    var resultado = await _evaluacionOperacionesProxy.Create(new EvaluacionOperacionesInsertDto
                        //    {
                        //        IdOperaciones = result.Data,
                        //        IdCatalogoEstado = 1,
                        //        UsuarioCreador = userName
                        //    });
                        //    if (resultado.Succeeded)
                        //    {

                        //        await _evaluacionOperacionesComentariosProxy.Create(new EvaluacionOperacionesComentariosInsertDto
                        //        {
                        //            IdEvaluacionOperaciones = resultado.Data,
                        //            Comentario = model.SustentoComercial,
                        //            IdCatalogoTipoComentario = 1,
                        //            UsuarioCreador = userName
                        //        });
                        //    }

                        //}
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


        public async Task<IActionResult> ResultadoEvaluacion(int operacionId, int Estado, OperacionCreateModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var _estadoOperaciones = await _evaluacionOperacionesProxy.Create(new EvaluacionOperacionesInsertDto
            {
                IdOperaciones = operacionId,
                IdCatalogoEstado = Estado,
                UsuarioCreador = userName,

            });
            if (_estadoOperaciones.Succeeded)
            {
                 await _evaluacionOperacionesProxy.CreateEstadoFactura(new EvaluacionOperacionesEstadoInsertDto
                {
                    IdOperaciones = operacionId,
                    IdCatalogoEstado = Estado,
                    UsuarioCreador = userName,
                    Comentario = model.SustentoComercial
                });
            }

            return Json(new { succeeded = _estadoOperaciones.Succeeded, message = _estadoOperaciones.Message });
        }

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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AgregarFacturas(AgregarFactura model)
        //{
        //    string nroOperacion = string.Empty;
        //    if (ModelState.IsValid)
        //    {
        //        //ResponseData<List<OperacionesFacturaListDto>> lst = new ResponseData<List<OperacionesFacturaListDto>>();
        //        //lst = await _facturaOperacionesProxy.GetAllListFacturaByIdOperaciones(model.IdOperacionCabeceraFacturas);
        //        //if (lst.Data != null) {
        //        //    if (lst.Data.Count > 0)
        //        //        nroOperacion = lst.Data[0].nroOperacion;

        //        //}
        //        //var operacionDetalle = await _operacionProxy.GetOperaciones(model.IdOperacionCabeceraFacturas);
        //        //if (operacionDetalle != null)
        //        //{
        //        //    nroOperacion = operacionDetalle.Data.nNroOperacion;
        //        //}
        //        string randon = RandomString(10);
        //        var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        //var path = _configuration[$"PathDocumentos:{Operaciones}"].ToString() + "\\" + nroOperacion + _configuration[$"PathDocumentos:{Facturas}"].ToString();
        //        //var archivoXML = await _filesProxy.UploadFile(model.fileXml, randon + "_" + model.fileXml.FileName, path);
        //        //if (archivoXML.Succeeded)
        //        //{
        //        //    var result = await _facturaOperacionesProxy.Create(new OperacionesFacturaInsertDto
        //        //    {
        //        //        IdOperaciones = model.IdOperacionCabeceraFacturas,
        //        //        NroDocumento = model.nroDocumento,
        //        //        Monto = model.Monto,
        //        //        FechaEmision = model.fechaEmision,
        //        //        FechaVencimiento = model.fechaVencimiento,
        //        //        NombreDocumentoXML = model.fileXml.FileName,
        //        //        RutaDocumentoXML = Path.Combine(path, randon + "_" + model.fileXml.FileName),
        //        //        NombreDocumentoPDF = "",
        //        //        RutaDocumentoPDF = "",
        //        //        UsuarioCreador = userName,
        //        //        FechaPagoNegociado = model.FechaPagoNegociado
        //        //    });
        //        //    return Json(result);
        //        //}
        //        //else
        //        //{
        //        //    return Json(new { succeeded = false, message = "El archivo no se cargo, intente nuevamente..." });
        //        //}
        //        //}
        //        //else
        //        //{
        //        //    return Json(new { succeeded = false, message = "Ocurrió un error, intente nuevamente..." });
        //        //}
        //    }
        //}

        //AgregarDocumentos
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AgregarDocumentoSolicitud(AgregarDocumentoSolicitud model)
        //{
        //    string nroOperacion = string.Empty;
        //    int nIdSolicitud = 0;
        //    if (ModelState.IsValid)
        //    {

        //        var operacionDetalle = await _operacionProxy.GetOperaciones(model.IdOperacionCabeceraFacturas);
        //        if (operacionDetalle != null)
        //        {
        //            nroOperacion = operacionDetalle.Data.nNroOperacion;
        //            nIdSolicitud = operacionDetalle.Data.IdSolEvalOperacion;
        //        }
        //        string randon = RandomString(10);
        //        var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        var path = _configuration[$"PathDocumentos:{Operaciones}"].ToString() + "\\" + nroOperacion + _configuration[$"PathDocumentos:{TipoDocumento}"].ToString();
        //        var archivoXML = await _filesProxy.UploadFile(model.fileDocumentoXml, randon + "_" + model.fileDocumentoXml.FileName, path);

        //        //List<IFormFile> fill = new List<IFormFile>();
        //        //fill.Add(model.fileDocumentoXml);
        //        //ValidarMasivoValidacionFacturaDTO obj = new ValidarMasivoValidacionFacturaDTO()
        //        //{
        //        //    UsuarioCreador = userName,
        //        //    NombreSeccion = "prueba",
        //        //    fileDocumentoXml = fill

        //        //};
        //        //var prueba = await _procesoMasivoLoteFacturaProxy.Validar_Facturas_Xml(obj);
        //        if (archivoXML.Succeeded)
        //        {
        //            var result = await _facturaOperacionesProxy.CreateSolcitudDocumento(new DocumentosSolicitudperacionesInsertDto
        //            {
        //                nIdSolEvalOperaciones = nIdSolicitud,
        //                nTipoDocumento = model.nTipoDocumento,
        //                cNombreDocumento = model.fileDocumentoXml.FileName,
        //                cRutaDocumento = Path.Combine(path, randon + "_" + model.fileDocumentoXml.FileName),
        //                cUsuarioCreador = userName
        //            });
        //            return Json(result);
        //        }
        //        else
        //        {
        //            return Json(new { succeeded = false, message = "El archivo no se cargo, intente nuevamente..." });
        //        }
        //    }
        //    else
        //    {
        //        return Json(new { succeeded = false, message = "Ocurrió un error, intente nuevamente..." });
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AgregarDocumentos(AgregarFactura model)
        //{
        //    string nroOperacion = string.Empty;
        //    if (ModelState.IsValid)
        //    {
        //        //ResponseData<List<OperacionesFacturaListDto>> lst = new ResponseData<List<OperacionesFacturaListDto>>();
        //        //lst = await _facturaOperacionesProxy.GetAllListFacturaByIdOperaciones(model.IdOperacionCabeceraFacturas);
        //        //if (lst.Data != null) {
        //        //    if (lst.Data.Count > 0)
        //        //        nroOperacion = lst.Data[0].nroOperacion;

        //        //}
        //        var operacionDetalle = await _operacionProxy.GetOperaciones(model.IdOperacionCabeceraFacturas);
        //        if (operacionDetalle != null)
        //        {
        //            nroOperacion = operacionDetalle.Data.nNroOperacion;
        //        }
        //        string randon = RandomString(10);
        //        var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        var path = _configuration[$"PathDocumentos:{Operaciones}"].ToString() + "\\" + nroOperacion;
        //        var archivoXML = await _filesProxy.UploadFile(model.fileXml, randon + "_" + model.fileXml.FileName, path);
        //        if (archivoXML.Succeeded)
        //        {
        //            var result = await _facturaOperacionesProxy.Create(new OperacionesFacturaInsertDto
        //            {
        //                IdOperaciones = model.IdOperacionCabeceraFacturas,
        //                NroDocumento = model.nroDocumento,
        //                Monto = model.Monto,
        //                FechaEmision = model.fechaEmision,
        //                FechaVencimiento = model.fechaVencimiento,
        //                NombreDocumentoXML = model.fileXml.FileName,
        //                RutaDocumentoXML = Path.Combine(path, randon + "_" + model.fileXml.FileName),
        //                NombreDocumentoPDF = "",
        //                RutaDocumentoPDF = "",
        //                UsuarioCreador = userName,
        //                FechaPagoNegociado = model.FechaPagoNegociado
        //            });
        //            return Json(result);
        //        }
        //        else
        //        {
        //            return Json(new { succeeded = false, message = "El archivo no se cargo, intente nuevamente..." });
        //        }
        //    }
        //    else
        //    {
        //        return Json(new { succeeded = false, message = "Ocurrió un error, intente nuevamente..." });
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EliminarFactura(EliminarFactura model)
        //{
        //    var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var archivoXML = await _filesProxy.DeleteFiles(model.filePath);
        //    if (archivoXML.Succeeded)
        //    {
        //        var result = await _facturaOperacionesProxy.Delete(model.operacionFacturaId, userName);
        //        return Json(result);
        //    }
        //    else
        //    {
        //        return Json(new { succeeded = false, message = "El archivo no se elimino, intente nuevamente..." });
        //    }

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditarFactura(EditarFactura model)
        //{
        //    var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    var result = await _facturaOperacionesProxy.Editar(new OperacionesFacturaEditDto()
        //    {
        //        FechaPagoNegociado = model.dFechaPagoNegociado,
        //        UsuarioActualizacion = userName,
        //        IdOperacionesFacturas = model.nIdOperacionesFacturas,
        //        Estado = 0      //  <OAV - 30/01/2023>
        //    });
        //    return Json(result.Succeeded);


        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CargaMasiva(IFormFile fileExcel)
        //{
        //    string fileExtension = Path.GetExtension(fileExcel.FileName).ToLower();
        //    if (fileExtension == ".xlsx" || fileExtension == ".xls")
        //    {
        //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //        Stream stream = fileExcel.OpenReadStream();
        //        using (var reader = ExcelReaderFactory.CreateReader(stream))
        //        {
        //            var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //            var resultExcel = SetAsDataSet(reader);
        //            DataTable table = resultExcel.Tables[0];
        //            var ItemsExcel = new List<OperacionesInsertMasiveDto>(table.Rows.Count);
        //            foreach (DataRow row in table.Rows)
        //            {
        //                var item = row.ItemArray;
        //                var rowOperacion = new OperacionesInsertMasiveDto()
        //                {
        //                    RucGirador = item[0].ToString(),
        //                    RucAdquiriente = item[1].ToString(),
        //                    DOIInversionista = item[2].ToString(),
        //                    Moneda = item[3].ToString(),
        //                    TEM = decimal.Parse(item[4].ToString()),
        //                    PorcentajeFinanciamiento = decimal.Parse(item[5].ToString()),
        //                    MontoOperacion = decimal.Parse(item[6].ToString()),
        //                    DescContrato = decimal.Parse(item[7].ToString()),
        //                    DescFactura = decimal.Parse(item[8].ToString()),
        //                    DescCobranza = decimal.Parse(item[9].ToString()),
        //                    PorcentajeRetencion = decimal.Parse(item[10].ToString())
        //                };
        //                ItemsExcel.Add(rowOperacion);
        //            }
        //            var requestExcel = await _operacionProxy.CreateMasivo(new MasivoOperacionDto
        //            {
        //                Operaciones = ItemsExcel,
        //                UsuarioCreador = userName
        //            });
        //            return Json(requestExcel);
        //        }
        //    }
        //    else
        //    {
        //        return Json(new { succeeded = false, message = "Debe adjuntar un archivo Excel" });
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> CargaMasivaFacturas(int operacionId, IFormFile fileExcelFacturas, List<IFormFile> filesXml)
        //{
        //    var countFilesXML = filesXml.Count();
        //    if (countFilesXML == 0)
        //    {
        //        return Json(new { succeeded = false, message = "Debe adjuntar los archivos XML de las facturas." });
        //    }
        //    string fileExtension = Path.GetExtension(fileExcelFacturas.FileName).ToLower();
        //    if (fileExtension == ".xlsx" || fileExtension == ".xls")
        //    {
        //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //        Stream stream = fileExcelFacturas.OpenReadStream();
        //        using (var reader = ExcelReaderFactory.CreateReader(stream))
        //        {
        //            var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //            var resultExcel = SetAsDataSet(reader);
        //            DataTable table = resultExcel.Tables[0];
        //            var countRowsExcel = table.Rows.Count;
        //            if (countRowsExcel == countFilesXML)
        //            {
        //                var ItemsExcel = new List<OperacionesFacturaInsertMasivotDto>(countRowsExcel);
        //                foreach (DataRow row in table.Rows)
        //                {
        //                    var item = row.ItemArray;
        //                    var NroDocumento = new { Serie = item[0].ToString(), Numero = item[1].ToString() };
        //                    var rowFactura = new OperacionesFacturaInsertMasivotDto()
        //                    {
        //                        IdOperaciones = operacionId,
        //                        NroDocumento = JsonSerializer.Serialize(NroDocumento).ToString(),
        //                        Monto = decimal.Parse(item[2].ToString()),
        //                        FechaEmision = DateTime.Parse(item[3].ToString()),
        //                        FechaVencimiento = DateTime.Parse(item[4].ToString()),
        //                        NombreDocumentoXML = item[5].ToString(),
        //                        RutaDocumentoXML = Path.Combine("upload", "files", item[5].ToString()).ToString(),
        //                        NombreDocumentoPDF = "",
        //                        RutaDocumentoPDF = ""
        //                    };
        //                    ItemsExcel.Add(rowFactura);
        //                }
        //                var requestExcelFacturas = await _filesProxy.UploadFiles(new OperacionesFacturaSendMasivo
        //                {
        //                    Files = filesXml,
        //                    UsuarioCreador = userName,
        //                    Facturas = ItemsExcel
        //                });
        //                return Json(requestExcelFacturas);
        //            }
        //            else
        //            {
        //                return Json(new { succeeded = false, message = "El archivo excel tiene " + countRowsExcel + " registro(s) y se han adjuntado " + countFilesXML + " archivo(s) XML." });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return Json(new { succeeded = false, message = "Debe adjuntar un archivo Excel." });
        //    }
        //}

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
            //string filename = string.Empty;
            //ResponseData<List<OperacionesFacturaListDto>> lst = new ResponseData<List<OperacionesFacturaListDto>>();
            //lst = await _facturaOperacionesProxy.GetAllListFacturaByIdOperacionesFactura(Convert.ToInt32(nIdOperacionFactura));
            //if (lst.Data != null)
            //{
            //    if (lst.Data.Count > 0)
            //        filename = lst.Data[0].cRutaDocumentoXML;

            //}
            ////filename = _configuration[$"PathDocumentos:{Operaciones}"].ToString() + "\\" + filename;
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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EliminarDocumentoSolicitud(EliminarDocumentoSolicitud model)
        //{
        //    var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var archivoXML = await _filesProxy.DeleteFiles(model.filePath);
        //    if (archivoXML.Succeeded)
        //    {
        //        OperacionesSolicitudDeleteDto obj = new OperacionesSolicitudDeleteDto() { nIdDocumentoSolEvalOperaciones = model.nIdDocumentoSolEvalOperacion, UsuarioActualizacion = userName };
        //        var result = await _facturaOperacionesProxy.DeleteDocumento(obj);
        //        return Json(result);
        //    }
        //    else
        //    {
        //        return Json(new { succeeded = false, message = "El archivo no se elimino, intente nuevamente..." });
        //    }

        //}
    }
}