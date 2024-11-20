using Factoring.Model;
using Factoring.Model.Models.Auth;
using Factoring.Model.Models.Catalogo;
using Factoring.Model.Models.ContactoGirador;
using Factoring.Model.Models.Girador;
using Factoring.Model.Models.GiradorDocumentos;
using Factoring.Model.Models.GiradorUbicacion;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class GiradorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGiradorProxy _giradorProxy;
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IContactoGiradorProxy _contactoGiradorProxy;
        private readonly IDocumentosGiradorProxy _giradorDocumentosProxy;
        private readonly IConfiguration _configuration;
        private readonly IFilesProxy _filesProxy;
        private readonly IUbigeoProxy _ubigeoProxy;
        private readonly IGiradorUbicacionProxy _giradorUbicacionProxy;


        public const string GiradorDocumentos = "GiradorDocumentos";

        public GiradorController(
            IHttpContextAccessor httpContextAccessor,
            IGiradorProxy giradorProxy,
            ICatalogoProxy catalogoProxy,
            IContactoGiradorProxy contactoGiradorProxy,
            IDocumentosGiradorProxy giradorDocumentosProxy,
            IConfiguration configuration,
             IFilesProxy filesProxy,
             IUbigeoProxy ubigeoProxy,
             IGiradorUbicacionProxy giradorUbicacionProxy)
        {
            _httpContextAccessor = httpContextAccessor;
            _giradorProxy = giradorProxy;
            _catalogoProxy = catalogoProxy;
            _contactoGiradorProxy = contactoGiradorProxy;
            _giradorDocumentosProxy = giradorDocumentosProxy;
            _configuration = configuration;
            _filesProxy = filesProxy;
            _ubigeoProxy = ubigeoProxy;
            _giradorUbicacionProxy = giradorUbicacionProxy;
        }
        public async Task<IActionResult> IndexAsync()
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            return View();
        }

        public async Task<JsonResult> GetGiradorAllList(GiradorViewModel model)
        {
            try
            {
                var requestData = new GiradorRequestDatatableDto();
                requestData.Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault()));
                requestData.PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault()));
                requestData.Sorting = "nIdGirador";
                requestData.SortOrder = "asc";
                requestData.FilterRuc = model.Ruc;
                requestData.FilterRazon = model.RazonSocial;
                requestData.FilterIdPais = model.Pais;
                requestData.FilterFecCrea = model.Fecha;
                requestData.FilterIdGrupoEconomico = model.GrupoEconomico;
                requestData.FilterIdSector = model.Sector;
                var data = await _giradorProxy.GetAllListGirador(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].TotalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Registro(int? giradorId)
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            ViewBag.Title = ((giradorId == null) ? "Registrar Girador" : "Editar Girador");
            ViewBag.IsEdit = giradorId != null;
            //ViewBag.Paises = await _dataProxy.GetAllListPais();
            var ActividadesEconomicas = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 120, Tipo = 1, Valor = "1" });

            ViewBag.ActividadesEconomicas = ActividadesEconomicas.Data.Select(x => new CatalogoResponseListDto { cNombre = x.nId + " - " + x.cNombre, nId = x.nId }).ToList(); ;
            //ViewBag.Grupo = await _dataProxy.GetAllListGrupo();
            var TipoDocumentos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 118, Tipo = 1,Valor = "1" });
            //var Categorias = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 116, Tipo = 1 });

            //var Bancos = await _bancoProxy.GetList(new Model.Models.Banco.BancoDto() { nTipo = 1 });
            //ViewBag.Bancos = Bancos.Data;
            //var Monedas = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 101, Tipo = 1 });
            //ViewBag.Monedas = Monedas.Data;
            var TipoContactos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 117, Tipo = 1,Valor = "1" });
            ViewBag.TipoContactos = TipoContactos.Data;
            var TipoContactosUbi = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 121, Tipo = 1,Valor = "1" });
            ViewBag.TipoContactosUbi = TipoContactosUbi.Data;

            var item1 = TipoDocumentos.Data.SingleOrDefault(x => x.cNombre == "COMPRAS");
            var item2 = TipoDocumentos.Data.SingleOrDefault(x => x.cNombre == "VENTAS");

            TipoDocumentos.Data.Remove(item1);
            TipoDocumentos.Data.Remove(item2);
            ViewBag.Documentos = TipoDocumentos.Data;
            //ViewBag.Categorias = Categorias.Data;

            if (giradorId == null)
            {
                return View();
            }
            else
            {
                var giradorDetalle = await _giradorProxy.GetGirador((int)giradorId);
                if (giradorDetalle.Succeeded == false)
                {
                    return Redirect("~/Girador/Index");
                }

                ViewBag.UbigeoPais = giradorDetalle.Data.FormatoUbigeoPais;
                ViewBag.UbigeoPaisCount = giradorDetalle.Data.FormatoUbigeoPais.Count;

                var UbigeoDepartamento = await _ubigeoProxy.GetUbigeo(giradorDetalle.Data.nIdPais, 1, "");
                ViewBag.DepartamentoPais = UbigeoDepartamento.Data;

                //ViewBag.ListInversionista = await _divisoExternProxy.GetAllListFondeadoreslista();

                //ViewBag.ListAdquiriente = await _adquirienteProxy.GetAllListAdquirientelista(); 
                GiradorCreateModel giradorData = new();
                if (ModelState.IsValid)
                {
                    giradorData.IdGirador = giradorDetalle.Data.nIdGirador;
                    giradorData.IdActividadEconomica = giradorDetalle.Data.nIdActividadEconomica;
                    giradorData.FechaInicioActividades = giradorDetalle.Data.dFechaInicioActividad == "01/01/1900" ? string.Empty : giradorDetalle.Data.dFechaInicioActividad; ;
                    giradorData.RegUnicoEmpresa = giradorDetalle.Data.cRegUnicoEmpresa;
                    HttpContext.Session.SetObjectAsJson("RucGirador", giradorDetalle.Data.cRegUnicoEmpresa);
                    giradorData.RazonSocial = giradorDetalle.Data.cRazonSocial;
                    giradorData.FechaFirmaContrato = giradorDetalle.Data.dFechaFirmaContrato == "01/01/1900" ? string.Empty : giradorDetalle.Data.dFechaFirmaContrato;
                    giradorData.Antecedente = giradorDetalle.Data.cAntecedente;
                    giradorData.Estado = giradorDetalle.Data.nEstado;
                    giradorData.NombreEstado = giradorDetalle.Data.NombreEstado;
                }
                return View(giradorData);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroProceso(int giradorId, GiradorCreateModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool IsGiradorExist = false;
            var giradorDetalle = await _giradorProxy.GetGirador(giradorId);
            if (giradorDetalle.Succeeded == true)
            {
                IsGiradorExist = true;
            }
            //if (ModelState.IsValid)
            //{
            try
            {
                if (IsGiradorExist)
                {
                    var result = await _giradorProxy.Update(new GiradorUpdateDto
                    {
                        IdGirador = giradorId,

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
                    var exists = await GetGirador(model.RegUnicoEmpresa);
                    if (exists == null)
                    {
                        var result = await _giradorProxy.Create(new GiradorCreateDto
                        {

                            RegUnicoEmpresa = model.RegUnicoEmpresa,
                            RazonSocial = model.RazonSocial.ToUpper(),
                            UsuarioCreador = userName
                        });

                        return Json(result);
                    }
                    else
                    {
                        return Json(new ResponseData<int>() { Succeeded = false, Message = "El Girador " + model.RegUnicoEmpresa + " ya existe." });
                    }

                  
                }
            }
            catch (Exception)
            {
                throw;
            }
            //}
            //return Redirect("~/Girador/Index");
        }

        public async Task<IActionResult> Detalle(int? giradorId)
        {
            ViewBag.Title = "Girador de Facturas: " + ((giradorId == null) ? "" : "Detalle Girador");
            if (giradorId == null)
            {
                return Redirect("~/Girador/Index");
            }
            else
            {
                var giradorDetalle = await _giradorProxy.GetGirador((int)giradorId);
                if (giradorDetalle.Succeeded == false)
                {
                    return Redirect("~/Girador/Index");
                }
                ViewBag.UbigeoPais = giradorDetalle.Data.FormatoUbigeoPais;
                GiradorCreateModel giradorData = new();
                if (ModelState.IsValid)
                {
                    /*giradorData.IdGirador = giradorDetalle.Data.nIdGirador;
                    //giradorData.NombrePais = giradorDetalle.Data.cNombrePais;
                    giradorData.RegUnicoEmpresa = giradorDetalle.Data.cRegUnicoEmpresa;
                    HttpContext.Session.SetObjectAsJson("RucGirador", giradorDetalle.Data.cRegUnicoEmpresa);
                    giradorData.RazonSocial = giradorDetalle.Data.cRazonSocial;
                    //giradorData.NombreSector = giradorDetalle.Data.cNombreSector;
                    //giradorData.NombreGrupoEconomico = giradorDetalle.Data.cNombreGrupoEconomico;
                    //giradorData.Venta = giradorDetalle.Data.nVenta;
                    //giradorData.Compra = giradorDetalle.Data.nCompra;
                    giradorData.Estado = giradorDetalle.Data.nEstado;
                    giradorData.NombreEstado = giradorDetalle.Data.NombreEstado;
                    */
                    var ActividadesEconomicas = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 120, Tipo = 1, Valor = "1" });


                    giradorData.IdGirador = giradorDetalle.Data.nIdGirador;
                    giradorData.IdActividadEconomica = giradorDetalle.Data.nIdActividadEconomica;
                    giradorData.FechaInicioActividades = giradorDetalle.Data.dFechaInicioActividad == "01/01/1900" ? string.Empty : giradorDetalle.Data.dFechaInicioActividad; ;
                    giradorData.RegUnicoEmpresa = giradorDetalle.Data.cRegUnicoEmpresa;
                    HttpContext.Session.SetObjectAsJson("RucGirador", giradorDetalle.Data.cRegUnicoEmpresa);
                    giradorData.RazonSocial = giradorDetalle.Data.cRazonSocial;
                    giradorData.FechaFirmaContrato = giradorDetalle.Data.dFechaFirmaContrato == "01/01/1900" ? string.Empty : giradorDetalle.Data.dFechaFirmaContrato;
                    giradorData.Antecedente = giradorDetalle.Data.cAntecedente;
                    giradorData.Estado = giradorDetalle.Data.nEstado;
                    giradorData.NombreEstado = giradorDetalle.Data.NombreEstado;
                    giradorData.ActividadEconomica = giradorDetalle.Data.nIdActividadEconomica == 0 ? string.Empty : ActividadesEconomicas.Data.Where(x => x.nId == giradorDetalle.Data.nIdActividadEconomica).ToList().FirstOrDefault().cNombre;


                }
                return View(giradorData);
            }
        }

        public async Task<IActionResult> GetAllContactos(int giradorId)
        {
            return Json(await _contactoGiradorProxy.GetAllListGirador(giradorId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarContacto(AgregarContactoGirador model)
        {
            var result = await _contactoGiradorProxy.Create(new ContactoGiradorCreateDto
            {
                IdGirador = model.IdGiradorCabecera,
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
        public async Task<IActionResult> EliminarContacto(int giradorContactoId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _contactoGiradorProxy.Delete(giradorContactoId, userName);
            return Json(result.Succeeded);
        }

        public async Task<IActionResult> GetAllDocumentos(int giradorId)
        {
            var documentos = await _giradorDocumentosProxy.GetAllListGiradorDocumentos(giradorId);
            return Json(documentos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDocumentos(AgregarDocumentos model)
        {
            if (ModelState.IsValid)
            {
                var TipoDocumentos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 118, Tipo = 1 ,Valor ="1"});

                var item1 = TipoDocumentos.Data.SingleOrDefault(x => x.nId == model.TipoDocumento);

                var path = _configuration[$"PathDocumentos:{GiradorDocumentos}"].ToString() + "\\" + DateTime.Now.ToString("dd-MM-yyyy") + "\\" + item1.cNombre;
                var ruc = HttpContext.Session.GetObjectFromJson<string>("RucGirador");
                path = path.Replace("ruc", ruc);
                var archivoPDF = await _filesProxy.UploadFile(model.FileDocumento, model.FileDocumento.FileName, path);
                if (archivoPDF.Succeeded)
                {
                    var result = await _giradorDocumentosProxy.Create(new DocumentosGiradorInsertDto
                    {
                        IdGirador = model.IdGiradorCabeceraDocumentos,
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
        public async Task<IActionResult> EliminarDocumentos(int giradorDocumentoId, string filePath)
        {
            var archivoXML = await _filesProxy.DeleteFiles(filePath);
            if (archivoXML.Succeeded)
            {
                var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var result = await _giradorDocumentosProxy.Delete(giradorDocumentoId, userName);
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
        public async Task<IActionResult> GetAllUbicaciones(int giradorId)
        {
            var ubicaciones = await _giradorUbicacionProxy.GetAllListDireccionGirador(giradorId);
            return Json(ubicaciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarUbicacion(AgregarUbicacion model)
        {
            var result = await _giradorUbicacionProxy.Create(new UbicacionGiradorInsertDto
            {
                IdGirador = model.IdGiradorCabeceraUbicacion,
                FormatoUbigeo = model.Ubigeo,
                Direccion = model.Direccion,
                IdTipoDireccion = model.TipoDireccion
            });
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarUbicacion(int giradorUbicacionId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _giradorUbicacionProxy.Delete(giradorUbicacionId, userName);
            return Json(result.Succeeded);
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





    }
}
