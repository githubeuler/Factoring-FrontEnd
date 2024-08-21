using Factoring.Model.Models.Aceptante;
using Factoring.Model.Models.AceptanteContacto;
using Factoring.Model.Models.Adquiriente;
using Factoring.Model.Models.AdquirienteUbicacion;
using Factoring.Model.Models.Auth;
using Factoring.Model.Models.Catalogo;
using Factoring.Model.Models.ContactoGirador;
using Factoring.Model.Models.DocumentosAceptante;
using Factoring.Model.Models.Girador;
using Factoring.Model.Models.GiradorDocumentos;
using Factoring.Model.Models.GiradorUbicacion;
using Factoring.Model.ViewModels;
using Factoring.Model.ViewModels.Aceptante;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Factoring.WebMvc.Controllers
{
    public class AceptanteController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGiradorProxy _giradorProxy;
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IDocumentosAceptanteProxy _documentosAceptanteProxy;
        private readonly IConfiguration _configuration;
        private readonly IFilesProxy _filesProxy;
        private readonly IUbigeoProxy _ubigeoProxy;
        private readonly IGiradorUbicacionProxy _giradorUbicacionProxy;
        private readonly IContactoAceptanteProxy _contactoAceptanteProxy;
        private readonly IAdquirienteUbicacionProxy _adquirienteUbicacionProxy;
        private readonly IAceptanteProxy _aceptanteProxy;

        public const string AceptanteDocumentos = "AceptanteDocumentos";
        public AceptanteController(
            IHttpContextAccessor httpContextAccessor,
            IGiradorProxy giradorProxy,
            ICatalogoProxy catalogoProxy,
            IDocumentosAceptanteProxy documentosAceptanteProxy,
            IConfiguration configuration,
            IFilesProxy filesProxy,
            IUbigeoProxy ubigeoProxy,
            IGiradorUbicacionProxy giradorUbicacionProxy,
            IContactoAceptanteProxy contactoAceptanteProxy,
            IAceptanteProxy aceptanteProxy,
            IAdquirienteUbicacionProxy adquirienteUbicacionProxy)
        {
            _httpContextAccessor = httpContextAccessor;
            _giradorProxy = giradorProxy;
            _catalogoProxy = catalogoProxy;
            _documentosAceptanteProxy = documentosAceptanteProxy;
            _configuration = configuration;
            _filesProxy = filesProxy;
            _ubigeoProxy = ubigeoProxy;
            _giradorUbicacionProxy = giradorUbicacionProxy;
            _contactoAceptanteProxy = contactoAceptanteProxy;
            _aceptanteProxy = aceptanteProxy;
            _adquirienteUbicacionProxy = adquirienteUbicacionProxy;
        }
        public async Task<IActionResult> IndexAsync()
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            return View();
        }

        public async Task<JsonResult> GetAceptanteAllList(AceptanteViewModel model)
        {
            try
            {
                var requestData = new AceptanteRequestDatatableDto();
                requestData.Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault()));
                requestData.PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault()));
                requestData.Sorting = "nIdAdquiriente";
                requestData.SortOrder = "asc";
                requestData.FilterRuc = model.Ruc;
                requestData.FilterRazon = model.RazonSocial;
                requestData.FilterIdPais = model.Pais;
                requestData.FilterFecCrea = model.Fecha;
                requestData.FilterIdSector = model.Sector;
                requestData.FilterIdGrupoEconomico = model.GrupoEconomico;
                var data = await _aceptanteProxy.GetAllListAceptante(requestData);
                var recordsTot = data.Data.Count > 0 ? data.Data[0].TotalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTot, recordsFiltered = recordsTot });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Registro(int? adquirienteId)
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            ViewBag.Title = ((adquirienteId == null) ? "Registrar Aceptante" : "Editar Aceptante");
            ViewBag.IsEdit = adquirienteId != null;
            var ActividadesEconomicas = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 120, Tipo = 1, Valor = "1" });
            ViewBag.ActividadesEconomicas = ActividadesEconomicas.Data.Select(x => new CatalogoResponseListDto { cNombre = x.nId + " - " + x.cNombre, nId = x.nId }).ToList(); ;
            var TipoDocumentos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 108, Tipo = 1, Valor = "1" });
            var TipoContactos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 117, Tipo = 1, Valor = "1" });
            ViewBag.TipoContactos = TipoContactos.Data;
            var TipoContactosUbi = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 121, Tipo = 1, Valor = "1" });
            ViewBag.TipoContactosUbi = TipoContactosUbi.Data;
            ViewBag.Documentos = TipoDocumentos.Data;

            if (adquirienteId == null)
            {
                return View();
            }
            else
            {
                var adquirienteDetalle = await _aceptanteProxy.GetAceptante((int)adquirienteId);
                if (adquirienteDetalle.Succeeded == false)
                {
                    return Redirect("~/Aceptante/Index");
                }

                ViewBag.UbigeoPais = adquirienteDetalle.Data.FormatoUbigeoPais;
                ViewBag.UbigeoPaisCount = adquirienteDetalle.Data.FormatoUbigeoPais.Count;
                var UbigeoDepartamento = await _ubigeoProxy.GetUbigeo(adquirienteDetalle.Data.nIdPais, 1, "");
                ViewBag.DepartamentoPais = UbigeoDepartamento.Data;
                AceptanteCreateModel adquirienteData = new();
                if (ModelState.IsValid)
                {
                    adquirienteData.IdAdquiriente = adquirienteDetalle.Data.nIdAdquiriente;
                    adquirienteData.IdActividadEconomica = adquirienteDetalle.Data.nIdActividadEconomica;
                    adquirienteData.FechaInicioActividades = adquirienteDetalle.Data.dFechaInicioActividad == "01/01/1900" ? string.Empty : adquirienteDetalle.Data.dFechaInicioActividad; ;
                    adquirienteData.RegUnicoEmpresa = adquirienteDetalle.Data.cRegUnicoEmpresa;
                    HttpContext.Session.SetObjectAsJson("RucAceptante", adquirienteDetalle.Data.cRegUnicoEmpresa);
                    adquirienteData.RazonSocial = adquirienteDetalle.Data.cRazonSocial;
                    adquirienteData.FechaFirmaContrato = adquirienteDetalle.Data.dFechaFirmaContrato == "01/01/1900" ? string.Empty : adquirienteDetalle.Data.dFechaFirmaContrato;
                    adquirienteData.Antecedente = adquirienteDetalle.Data.cAntecedente;
                    adquirienteData.Estado = adquirienteDetalle.Data.nEstado;
                    adquirienteData.NombreEstado = adquirienteDetalle.Data.NombreEstado;
                }
                return View(adquirienteData);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroProceso(int adquirienteId, AdquirienteCreateModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool IsAdquirienteExist = false;
            var AceptanteDetalle = await _aceptanteProxy.GetAceptante(adquirienteId);
            if (AceptanteDetalle.Succeeded == true)
            {
                IsAdquirienteExist = true;
            }
            try
            {
                if (IsAdquirienteExist)
                {
                    var result = await _aceptanteProxy.Update(new AdquirienteUpdateDto
                    {
                        IdAdquiriente = adquirienteId,
                        RegUnicoEmpresa = model.RegUnicoEmpresa,
                        RazonSocial = model.RazonSocial.ToUpper(),
                        FechaInicioActividad = model.FechaInicioActividades ?? string.Empty,
                        IdActividadEconomica = model.IdActividadEconomica,
                        FechaFirmaContrato = model.FechaFirmaContrato ?? string.Empty,
                        Antecedente = model.Antecedente ?? string.Empty,
                        UsuarioActualizacion = userName
                    });
                    return Json(result);
                }
                else
                {
                    var result = await _aceptanteProxy.Create(new AdquirienteInsertDto
                    {
                        RegUnicoEmpresa = model.RegUnicoEmpresa,
                        RazonSocial = model.RazonSocial.ToUpper(),
                        UsuarioCreador = userName
                    });

                    return Json(result);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
      
        public async Task<IActionResult> Detalle(int? adquirienteId)
        {
            ViewBag.Title = "Registro Aceptante: " + ((adquirienteId == null) ? "" : "Detalle Aceptante");
            if (adquirienteId == null)
            {
                return Redirect("~/Aceptante/Index");
            }
            else
            {
                var aceptanteDetalle = await  _aceptanteProxy.GetAceptante(adquirienteId.Value);
                if (aceptanteDetalle.Succeeded == false)
                {
                    return Redirect("~/Aceptante/Index");
                }
                ViewBag.UbigeoPais = aceptanteDetalle.Data.FormatoUbigeoPais;
                AceptanteCreateModel aceptanteData = new();
                if (ModelState.IsValid)
                {
                    aceptanteData.IdAdquiriente = aceptanteDetalle.Data.nIdAdquiriente;
                    aceptanteData.RegUnicoEmpresa = aceptanteDetalle.Data.cRegUnicoEmpresa;
                    HttpContext.Session.SetObjectAsJson("RucAceptante", aceptanteDetalle.Data.cRegUnicoEmpresa);
                    aceptanteData.RazonSocial = aceptanteDetalle.Data.cRazonSocial;
                    aceptanteData.Estado = aceptanteDetalle.Data.nEstado;
                    aceptanteData.NombreEstado = aceptanteDetalle.Data.NombreEstado;
                }
                return View(aceptanteData);
            }
        }

        public async Task<IActionResult> GetAllContactos(int aceptanteId)
        {
            return Json(await _contactoAceptanteProxy.GetAllListAceptante(aceptanteId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarContacto(AgregarContactoAdquiriente model)
        {
            var result = await _contactoAceptanteProxy.Create(new ContactoAdquirienteCreateDto
            {
                IdAdquiriente = model.IdAdquirienteCabecera,
                Nombre = model.Nombre.ToUpper(),
                ApellidoPaterno = model.ApellidoPaterno.ToUpper(),
                ApellidoMaterno = model.ApellidoMaterno.ToUpper(),
                Celular = model.Celular,
                Email = model.Email.ToLower(),
                IdTipoContacto = model.TipoContacto
            });
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarContacto(int aceptanteContactoId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _contactoAceptanteProxy.Delete(aceptanteContactoId, userName);
            return Json(result.Succeeded);
        }

        public async Task<IActionResult> GetAllDocumentos(int aceptanteId)
        {
            var documentos = await _documentosAceptanteProxy.GetAllListAceptanteDocumentos(aceptanteId);
            return Json(documentos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDocumentos(AgregarDocumentosAceptante model)
        {
            if (ModelState.IsValid)
            {
                var TipoDocumentos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 108, Tipo = 1, Valor = "1" });

                var item1 = TipoDocumentos.Data.SingleOrDefault(x => x.nId == model.TipoDocumento);

                var path = _configuration[$"PathDocumentos:{AceptanteDocumentos}"].ToString() + "\\" + DateTime.Now.ToString("dd-MM-yyyy") + "\\" + item1.cNombre;
                var ruc = HttpContext.Session.GetObjectFromJson<string>("RucAceptante");
                path = path.Replace("ruc", ruc);
                var archivoPDF = await _filesProxy.UploadFile(model.FileDocumento, model.FileDocumento.FileName, path);
                if (archivoPDF.Succeeded)
                {
                    var result = await _documentosAceptanteProxy.Create(new DocumentosAceptanteInsertDto
                    {
                        IdAceptante = model.IdAceptanteCabeceraDocumentos,
                        IdTipoDocumento = model.TipoDocumento,
                        Ruta = Path.Combine(path, model.FileDocumento.FileName),
                        NombreDocumento = model.FileDocumento.FileName
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDocumentos(int aceptanteDocumentoId, string filePath)
        {
            var archivoXML = await _filesProxy.DeleteFiles(filePath);
            if (archivoXML.Succeeded)
            {
                var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = await _documentosAceptanteProxy.Delete(aceptanteDocumentoId, userName);
                return Json(result.Succeeded);
            }
            else
            {
                return Json(new { succeeded = false, message = "El archivo no se elimino, intente nuevamente..." });
            }

        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string idDocumento, string tipoDocumento)
        {
            GiradorFileNameEntidad giradorFileNameEntidad = new GiradorFileNameEntidad();

            giradorFileNameEntidad.IdDocumento = Convert.ToInt32(idDocumento);
            giradorFileNameEntidad.IdTipo = Convert.ToInt32(tipoDocumento);
            var EntidadDocumento = await _giradorProxy.GetObtenerFileName(giradorFileNameEntidad);
            var bytesFile = await _filesProxy.DownloadFile(EntidadDocumento.Data.Ruta);

            return File(bytesFile, "application/octet-stream", EntidadDocumento.Data.FileName);
        }

        public async Task<IActionResult> ListarUbigeos(int pais, int tipo, string codigo)
        {
            var result = await _ubigeoProxy.GetUbigeo(pais, tipo, codigo);
            return Json(result.Data);
        }
        public async Task<IActionResult> GetAllUbicaciones(int adquirienteId)
        {
            var ubicaciones = await _adquirienteUbicacionProxy.GetAllListDireccionAdquiriente(adquirienteId);
            return Json(ubicaciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarUbicacion(AgregarUbicacionAdquiriente model)
        {
            var result = await _adquirienteUbicacionProxy.Create(new UbicacionAdquirienteInsertDto
            {
                IdAdquiriente = model.IdAdquirienteCabeceraUbicacion,
                FormatoUbigeo = model.Ubigeo,
                Direccion = model.Direccion,
                IdTipoDireccion = model.TipoDireccion
            });
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarUbicacion(int adquirienteUbicacionId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _adquirienteUbicacionProxy.Delete(adquirienteUbicacionId, userName);
            return Json(result.Succeeded);
        }

    }
}
