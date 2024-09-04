using Factoring.Model.Models.EvaluacionOperacion;
using Factoring.Model.Models.Fondeo;
using Factoring.Model.Models.Operaciones;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class FondeoController : Controller
    {
        private IConfiguration _configuration;
        private readonly IFondeoProxy _fondeoProxy;
        private readonly ICatalogoProxy _catalogoProxy;

        public FondeoController(
            IConfiguration configuration,
            IFondeoProxy fondeoProxy,
             ICatalogoProxy catalogoProxy
            ) {
            _configuration = configuration;
            _fondeoProxy = fondeoProxy;
            _catalogoProxy = catalogoProxy;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 1, Codigo = 130, Valor = "0" });
            ViewBag.Estados = _Estados.Data.ToList();
            return View();
        }
        public async Task<JsonResult> ListadoRegistros(FondeoViewModel fondeo)
        {
            try
            {
                var requestData = new FondeoRequestDatatableDto
                {
                    Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault())) == 0 ? 0 : (Convert.ToInt32(Request.Form["start"].FirstOrDefault())),
                    PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault())),
                    Sorting = "dFechaCreacion",
                    SortOrder = "desc",
                    FilterNroOperacion = fondeo.NroOperacion,
                    FilterFondeadorAsignado = fondeo.Fondeador,
                    FilterGirador = fondeo.Girador,
                    FilterFechaRegistro = fondeo.FechaRegistro,
                    FilterEstadoFondeo = fondeo.IdEstadoFondeo,
                    IdEstado = 1

                };
                var data = await _fondeoProxy.GetAllLisFondeo(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].TotalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> ActualizarFondeo(FondeoViewModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 4, Codigo = 103, Valor = "0" });
            //var nCant = _Estados.Data.Where(n => n.nId == model.nIdEstadoEvaluacion).ToList();
            //bool pRegistro = false;

            var _registroFondeo = await _fondeoProxy.Update(new FondeoUpdateDto
            {
                IdFondeadorFactura = model.IdFondeadorFactura,
                IdOperacion = model.IdOperacion,
                IdFondeador = model.IdFondeadorVal,
                IdTipoProducto = model.IdTipoProducto,
                PorTasaMensual = model.PorTasaMensual,
                PorComisionFactura = model.PorComisionFactura,
                PorSpread = model.PorSpread,
                PorCapitalFinanciado = model.PorCapitalFinanciado,
                PorTasaAnualFondeo = model.PorTasaAnualFondeo,
                PorTasaMoraFondeo = model.PorTasaMoraFondeo,
                FechaDesembolso = model.FechaDesembolso,
                FechaCobranza = model.FechaCobranza,
                UsuarioModificacion = userName


            });

            if (_registroFondeo.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.FechaDesembolso))
                {
                    var _ActualizaEstadoFondeo = await _fondeoProxy.UpdateState(new FondeoUpdateStateDto
                    {
                        IdFondeadorFactura = model.IdFondeadorFactura,
                        IdEstadoFondeo = 3,//DESEMBOLSADO
                        Comentario = string.Empty,
                        UsuarioModificacion = userName
                    });
                }
            }
           
           


            ////if (nCant.Count > 0 && model.nIdEstadoEvaluacion != 11)
            ////{
            ////    pRegistro = true;
            ////}

            //if (_estadoOperaciones.Succeeded && nCant.Count > 0)
            //{
            //    await _evaluacionOperacionesProxy.CreateEstadoFactura(new EvaluacionOperacionesEstadoInsertDto
            //    {
            //        IdOperaciones = model.nIdOperacionEval,
            //        IdCatalogoEstado = model.nIdEstadoEvaluacion,
            //        UsuarioCreador = userName,
            //        Comentario = model.cComentario,
            //        bRegistro = true
            //    });

            //    if (model.nIdEstadoEvaluacion == 10 || model.nIdEstadoEvaluacion == 11)
            //    {
            //        var oFactura = await _facturaOperacionesProxy.GetAllListFacturaByIdOperaciones(model.nIdOperacionEval);
            //        if (oFactura.Data.Count > 0)
            //        {
            //            foreach (var item in oFactura.Data)
            //            {
            //                await _evaluacionOperacionesProxy.UpdateCalculoFactura(new EvaluacionOperacionesCalculoInsertDto
            //                {
            //                    IdOperaciones = model.nIdOperacionEval,
            //                    IdOperacionesFactura = item.nIdOperacionesFacturas,
            //                    IdCatalogoEstado = model.nIdEstadoEvaluacion,
            //                    UsuarioCreador = userName,
            //                    cFecha = item.dFechaRegistro.ToString()
            //                });
            //            }
            //        }
            //    }
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
            //}

            return Json(_registroFondeo);//Json(_estadoOperaciones);
        }

        public async Task<IActionResult> AgregarFondeo(FondeoViewModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 4, Codigo = 103, Valor = "0" });
            //var nCant = _Estados.Data.Where(n => n.nId == model.nIdEstadoEvaluacion).ToList();
            //bool pRegistro = false;

            var _registroFondeo = await _fondeoProxy.Insert(new FondeoInsertDto
            {
                IdFondeadorFactura = model.IdFondeadorFactura,
                UsuarioCreacion = userName
            });

          

            return Json(_registroFondeo);//Json(_estadoOperaciones);
        }
        public async Task<IActionResult> ActualizaEstadoFondeo(FondeoViewModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var _Estados = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Tipo = 4, Codigo = 103, Valor = "0" });
            //var nCant = _Estados.Data.Where(n => n.nId == model.nIdEstadoEvaluacion).ToList();
            //bool pRegistro = false;

            var _registroFondeo = await _fondeoProxy.UpdateState(new FondeoUpdateStateDto
            {
                IdFondeadorFactura = model.IdFondeadorFactura,
                IdEstadoFondeo = 5,//ANULADO
                Comentario = string.Empty,
                UsuarioModificacion = userName
            });



            return Json(_registroFondeo);//Json(_estadoOperaciones);
        }
        public async Task<IActionResult> DescargarRegistroFondeoArchivo(string operacion, string fondeador, string girador, string fecha, string estado)
        {
            var response = await _fondeoProxy.GetReporteRegistroFondeoDonwload(new FondeoRequestDatatableDto
            {
                FilterNroOperacion = operacion,
                FilterFondeadorAsignado = fondeador,
                FilterGirador = girador,
                FilterFechaRegistro = (fecha == "undefined" ? null : fecha),
                FilterEstadoFondeo = Convert.ToInt32(estado),
                IdEstado = 1
            });

            string base64data = response.Data;
            string fileName = DateTime.Now.ToString() + "_reporte_registro_fondeo.xlsx";
            byte[] bytes = Convert.FromBase64String(base64data);
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
