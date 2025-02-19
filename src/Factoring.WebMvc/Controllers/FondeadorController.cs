using Factoring.Model;
using Factoring.Model.Models.Fondeador;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class FondeadorController : Controller
    {
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IFondeadorProxy _fondeadorProxy;
        private readonly ICavaliFactoringFondeadorProxy _cavaliFactoringFondeadorProxy;
        private readonly IFilesProxy _filesProxy;
        private readonly IDocumentoFondeadorProxy _documentoFondeadorProxy;

        private IConfiguration _configuration;
        public FondeadorController(
            ICatalogoProxy catalogoProxy,
            IFondeadorProxy fondeadorProxy,
             ICavaliFactoringFondeadorProxy cavaliFactoringFondeadorProxy,
              IConfiguration configuration,
               IFilesProxy filesProxy,
                IDocumentoFondeadorProxy documentoFondeadorProxy
            )
        {
            _catalogoProxy = catalogoProxy;
            _fondeadorProxy = fondeadorProxy;
            _cavaliFactoringFondeadorProxy = cavaliFactoringFondeadorProxy;
            _configuration = configuration;
            _filesProxy = filesProxy;
            _documentoFondeadorProxy = documentoFondeadorProxy;
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
            var ListaProducto = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto() { Codigo = 124, Tipo = 1, Valor = "1" });
            var ListaInteresCalculo = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto() { Codigo = 125, Tipo = 1, Valor = "1" });
            var ListaTipoFondeo = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto() { Codigo = 126, Tipo = 1, Valor = "1" });

            var ListaTipoDocumentoFondeador = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto() { Codigo = 127, Tipo = 1, Valor = "1" });

            ViewBag.ListaTipoDocumento = ListaTipoDocumento.Data;
            ViewBag.ListaProducto = ListaProducto.Data;
            ViewBag.ListaInteresCalculo = ListaInteresCalculo.Data;
            ViewBag.ListaTipoFondeo = ListaTipoFondeo.Data;
            ViewBag.ListaTipoDocumentoFondeador = ListaTipoDocumentoFondeador.Data;

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
                //ViewBag.UbigeoPaisCount = fondeadorDetalle.Data.FormatoUbigeoPais.Count;

                //var UbigeoDepartamento = await _ubigeoProxy.GetUbigeo(fondeadorDetalle.Data.iPais, 1, "");
                //ViewBag.DepartamentoPais = UbigeoDepartamento.Data;

                //ViewBag.ListInversionista = await _divisoExternProxy.GetAllListFondeadoreslista();

                FondeadorRegistroViewModel fondeadorData = new();
                if (ModelState.IsValid)
                {
                    fondeadorData.IdFondeador = fondeadorDetalle.Data.nIdFondeador;
                    //fondeadorData.IdPais = fondeadorDetalle.Data.iPais;
                    fondeadorData.IdTipoDocumento = fondeadorDetalle.Data.nTipoDocumento;
                    fondeadorData.RazonSocial = fondeadorDetalle.Data.cNombreFondeador;
                    fondeadorData.DOI = fondeadorDetalle.Data.cNroDocumento;
                    fondeadorData.IdProducto = fondeadorDetalle.Data.nIdProducto;
                    fondeadorData.IdInteresCalculado = fondeadorDetalle.Data.nIdInteresCalculado;
                    fondeadorData.IdTipoFondeo = fondeadorDetalle.Data.nIdTipoFondeo;
                    fondeadorData.DistribucionFondeador = fondeadorDetalle.Data.cDistribucionFondeador?? string.Empty;
                    ViewBag.DOI = fondeadorData.DOI;
                }
                return View(fondeadorData);
            }
        }
        public async Task<JsonResult> ListadoRegistros(FondeadorViewModel fondeador)
        {
            string userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
                    IdEstado = fondeador.IdEstado,
                    Usuario = userName


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

        public async Task<JsonResult> ListadoRegistrosByTipoFondeo(int tipoFondeo)
        {
            try
            {

                var data = await _fondeadorProxy.GetFondeadorByTipoFondeo(tipoFondeo);
                return Json(new { data = data.Data });
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
                                NombreFondeador = fondeador.RazonSocial,
                                NroDocumento = fondeador.DOI,
                                TipoDocumento = fondeador.IdTipoDocumento,
                                UsuarioActualizacion = userName,
                                IdProducto = fondeador.IdProducto,
                                IdInteresCalculado = fondeador.IdInteresCalculado,
                                IdTipoFondeo = fondeador.IdTipoFondeo,
                                DistribucionFondeador = fondeador.IdProducto == 1 ? string.Empty : fondeador.DistribucionFondeador
                            });
                            return Json(result);
                        }
                        else
                        {
                            var exists = await GetFondeaor(fondeador.DOI);
                            if (exists == null)
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
                            else
                            {
                                return Json(new ResponseData<int>() { Succeeded = false, Message = "El Fondeador " + fondeador.DOI + " ya existe." });
                            }
                           
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarCavaliFactoring(AgregarCavaliFactoringFondeadorViewModel model)
        {
            var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await _cavaliFactoringFondeadorProxy.Create(new CavaliFactoringFondeadorRegistroDto()
            {
                IdFondeador = model.IdFondeadorCabeceraCF,
                CodParticipante = model.CodigoParticipante,
                CodRUT = model.CodigoRUT,
                UsuarioCreador = userName
            });
            return Json(result);
        }
        public async Task<IActionResult> GetAllCavaliFactoring(int fondeadorId)
        {
            return Json(await _cavaliFactoringFondeadorProxy.GetAllListCavaliFactoring(fondeadorId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCavaliFactoring(int fondeadorCavaliFactoringId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _cavaliFactoringFondeadorProxy.Delete(fondeadorCavaliFactoringId, userName);
            return Json(result.Succeeded);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDocumento(AgregarDocumentoFondeadorViewModel model)
        {
            var Ruta = _configuration["DirectorySGP"].ToString() + @"\Fondeador\" + model.DOIDOCUMENTO.ToString() + @"\Documento\" + DateTime.Now.ToString("dd_MM_yyyy") + @"\" + model.TipoDocumentoDesc + @"\";
            var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var archivoPDF = await _filesProxy.UploadFile(model.fileDocumento, model.fileDocumento.FileName, Ruta);
            if (archivoPDF.Succeeded)
            {
                var result = await _documentoFondeadorProxy.Create(new DocumentoFondeadorRegistroDto()
                {
                    IdFondeador = model.IdFondeadorCabeceraDocumento,
                    IdTipoDocumento = model.TipoDocumento,
                    NombreDocumento = model.fileDocumento.FileName,
                    RutaDocumento = Ruta,
                    UsuarioCreador = userName
                });
                return Json(result);
            }
            else
            {
                return Json(new { succeeded = false, message = "El archivo no se cargo, intente nuevamente..." });
            }

        }
        public async Task<IActionResult> GetAllDocumento(int fondeadorId)
        {
            return Json(await _documentoFondeadorProxy.GetAllList(fondeadorId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDocumento(int fondeadorDocumentoId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _documentoFondeadorProxy.Delete(fondeadorDocumentoId, userName);
            return Json(result.Succeeded);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string filename, string ruta)
        {

            var bytesFile = await _filesProxy.DownloadFile(ruta.Replace("/", @"\") + filename);
            return File(bytesFile, "application/octet-stream", filename);
        }

        private async Task<FondedorResponseDatatableDto> GetFondeaor(string sRUC)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            FondedorResponseDatatableDto oRecord = null;
            try
            {
                var requestData = new FondeadorRequestDatatableDto();
                requestData.Pageno = 0;
                requestData.PageSize = 5;
                requestData.Sorting = "nIdFondeador";
                requestData.SortOrder = "asc";
                requestData.FilterDoi = sRUC;
                requestData.IdEstado = 1;
                requestData.Usuario = userName;
                var data = await _fondeadorProxy.GetAllLisFondeador(requestData);

                if (data.Data.Count > 0)
                    oRecord = data.Data[0];
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }


    }
}
