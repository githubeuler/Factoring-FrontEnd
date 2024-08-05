using Factoring.Model.Models.Fondeador;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class FondeadorController : Controller
    {
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IFondeadorProxy _fondeadorProxy;
        public FondeadorController(
            ICatalogoProxy catalogoProxy,
            IFondeadorProxy fondeadorProxy
            )
        {
            _catalogoProxy = catalogoProxy;
            _fondeadorProxy = fondeadorProxy;
        }

        public async Task<IActionResult> IndexAsync()
        {
            
            var Estado = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto() { Codigo = 119,Tipo = 1,Valor = "1" }); //101
            ViewBag.Estados = Estado.Data;
            return View(new FondeadorViewModel() { IdEstado = 1 });
        }

        public async Task<IActionResult> RegistroAsync(int? fondeadorId)
        {
            ViewBag.Title = ((fondeadorId == null) ? "Registrar Fondeador" : "Editar Fondeador");
            ViewBag.IsEdit = fondeadorId != null;

            var ListaTipoDocumento = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto() { Codigo = 122, Tipo = 1, Valor = "1" });

            ViewBag.ListaTipoDocumento = ListaTipoDocumento.Data;

            if (fondeadorId == null)
            {
                return View();
            }
            else
            {

                var fondeadorDetalle = await _fondeadorProxy.GetFondeador((int)fondeadorId);
                if (fondeadorDetalle.Succeeded == false)
                {
                    return Redirect("~/Fondeador/Index");
                }

                ViewBag.UbigeoPais = fondeadorDetalle.Data.FormatoUbigeoPais;
                ViewBag.UbigeoPaisCount = fondeadorDetalle.Data.FormatoUbigeoPais.Count;

                //var UbigeoDepartamento = await _ubigeoProxy.GetUbigeo(fondeadorDetalle.Data.iPais, 1, "");
                //ViewBag.DepartamentoPais = UbigeoDepartamento.Data;

                //ViewBag.ListInversionista = await _divisoExternProxy.GetAllListFondeadoreslista();

                FondeadorRegistroViewModel fondeadorData = new();
                if (ModelState.IsValid)
                {
                    fondeadorData.IdFondeador = fondeadorDetalle.Data.iIdFondeador;
                    //fondeadorData.IdPais = fondeadorDetalle.Data.iPais;
                    fondeadorData.IdTipoDocumento = fondeadorDetalle.Data.iTipoDocumento;
                    fondeadorData.RazonSocial = fondeadorDetalle.Data.cNombreFondeador;
                    fondeadorData.DOI = fondeadorDetalle.Data.cNroDocumento;
                    ViewBag.DOI = fondeadorData.DOI;
                }
                return View(fondeadorData);
            }
        }
        public async Task<JsonResult> ListadoRegistros(FondeadorViewModel fondeador)
        {
            try
            {
                var requestData = new FondeadorRequestDatatableDto
                {
                    Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault())) == 0 ? 0 : (Convert.ToInt32(Request.Form["start"].FirstOrDefault())),
                    PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault())),
                    Sorting = "nIdFondeador",
                    SortOrder = "desc",
                    FilterDoi = fondeador.FilterDoi,
                    FilterRazon = fondeador.FilterRazon,
                    FilterFecCrea = fondeador.FilterFecCrea,
                    IdEstado = fondeador.IdEstado

                };
                var data = await _fondeadorProxy.GetAllLisFondeador(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].TotalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarFondeador(int fondeadorId, FondeadorRegistroViewModel fondeador)
        {
            try
            {
                var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                bool IsFondeadorExist = false;
                var fondeadorDetalle = await _fondeadorProxy.GetFondeador(fondeadorId);
                if (fondeadorDetalle.Succeeded == true)
                {
                    IsFondeadorExist = true;
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (IsFondeadorExist)
                        {
                            var result = await _fondeadorProxy.Update(new FondeadorUpdateRequestDto
                            {
                                IdFondeador = fondeadorId,
                                NombreFondeador = fondeador.RazonSocial.ToUpper(),
                                NroDocumento = fondeador.DOI,
                                TipoDocumento = fondeador.IdTipoDocumento,
                                UsuarioActualizacion = userName
                            });
                            return Json(result);
                        }
                        else
                        {
                            var requestVentaCartera = await _fondeadorProxy.Create(new FondeadorRegistroRequestDto()
                            {
                                IdDocumento = fondeador.IdTipoDocumento,
                                NroDocumento = fondeador.DOI,
                                NombreFondeador = fondeador.RazonSocial,
                                UsuarioCreador = userName
                            });
                            return Json(requestVentaCartera);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return Redirect("~/Fondeador/Index");

            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
