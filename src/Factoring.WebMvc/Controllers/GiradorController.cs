using Factoring.Model.Models.Auth;
using Factoring.Model.Models.ContactoGirador;
using Factoring.Model.Models.Girador;
using Factoring.Model.Models.GiradorDocumentos;
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


        public const string GiradorDocumentos = "GiradorDocumentos";

        public GiradorController(
            IHttpContextAccessor httpContextAccessor,
            IGiradorProxy giradorProxy,
            ICatalogoProxy catalogoProxy,
            IContactoGiradorProxy contactoGiradorProxy,
            IDocumentosGiradorProxy giradorDocumentosProxy,
            IConfiguration configuration,
             IFilesProxy filesProxy)
        {
            _httpContextAccessor = httpContextAccessor;
            _giradorProxy = giradorProxy;
            _catalogoProxy = catalogoProxy;
            _contactoGiradorProxy = contactoGiradorProxy;
            _giradorDocumentosProxy = giradorDocumentosProxy;
            _configuration = configuration;
            _filesProxy = filesProxy;
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
            //ViewBag.Sector = await _dataProxy.GetAllListSector();
            //ViewBag.Grupo = await _dataProxy.GetAllListGrupo();
            var TipoDocumentos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 118, Tipo = 1,Valor = "1" });
            //var Categorias = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 116, Tipo = 1 });

            //var Bancos = await _bancoProxy.GetList(new Model.Models.Banco.BancoDto() { nTipo = 1 });
            //ViewBag.Bancos = Bancos.Data;
            //var Monedas = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 101, Tipo = 1 });
            //ViewBag.Monedas = Monedas.Data;
            var TipoContactos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 117, Tipo = 1,Valor = "1" });
            ViewBag.TipoContactos = TipoContactos.Data;
            //var TipoContactosUbi = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 102, Tipo = 1 });
            //ViewBag.TipoContactosUbi = TipoContactosUbi.Data;

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

                //ViewBag.UbigeoPais = giradorDetalle.Data.FormatoUbigeoPais;
                //ViewBag.UbigeoPaisCount = giradorDetalle.Data.FormatoUbigeoPais.Count;

                //var UbigeoDepartamento = await _ubigeoProxy.GetUbigeo(giradorDetalle.Data.nIdPais, 1, "");
                //ViewBag.DepartamentoPais = UbigeoDepartamento.Data;

                //ViewBag.ListInversionista = await _divisoExternProxy.GetAllListFondeadoreslista();

                //ViewBag.ListAdquiriente = await _adquirienteProxy.GetAllListAdquirientelista(); 
                GiradorCreateModel giradorData = new();
                if (ModelState.IsValid)
                {
                    giradorData.IdGirador = giradorDetalle.Data.nIdGirador;
                   
                    giradorData.RegUnicoEmpresa = giradorDetalle.Data.cRegUnicoEmpresa;
                    HttpContext.Session.SetObjectAsJson("RucGirador", giradorDetalle.Data.cRegUnicoEmpresa);
                    giradorData.RazonSocial = giradorDetalle.Data.cRazonSocial;
                   
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
                           
                            UsuarioActualizacion = userName
                        });
                        return Json(result);
                    }
                    else
                    {
                        var result = await _giradorProxy.Create(new GiradorCreateDto
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
                    giradorData.IdGirador = giradorDetalle.Data.nIdGirador;
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



    }
}
