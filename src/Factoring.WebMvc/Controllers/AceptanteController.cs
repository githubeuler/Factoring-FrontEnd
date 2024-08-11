using Factoring.Model.Models.Adquiriente;
using Factoring.Model.Models.AceptanteUbicacion;
using Factoring.Model.Models.Auth;
using Factoring.Model.ViewModels;
using Factoring.Model.ViewModels.AceptanteContacto;
using Factoring.Model.ViewModels.AceptanteLinea;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using Factoring.WebMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Factoring.Model.Models.Aceptante;
using Factoring.Model.ViewModels.Aceptante;
using Factoring.Model.Models.AdquirienteUbicacion;
using Factoring.Model.Models.AceptanteContacto;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class AceptanteController : Controller
    {
        private readonly ILogger<AceptanteController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProxy _dataProxy;
        //private readonly IAdquirienteProxy _adquirienteProxy;
        private readonly IAceptanteProxy _aceptanteProxy;
        //private readonly IContactoAdquirienteProxy _contactoAdquirienteProxy;
        private readonly IContactoAceptanteProxy _contactoAceptanteProxy;
        private readonly IAdquirienteUbicacionProxy _adquirienteUbicacionProxy;
        private readonly IUbigeoProxy _ubigeoProxy;
        //private readonly ILineaAdquirienteProxy _lineaAdquirienteProxy;
        //private readonly IComentariosProxy _comentariosProxy;
        //private readonly IDivisoExternProxy _divisoExternProxy;
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IFondeadorProxy _fondeadorProxy;

        public AceptanteController(
            ILogger<AceptanteController> logger,
            IHttpContextAccessor httpContextAccessor,
            IDataProxy dataProxy,
            //IAdquirienteProxy adquirienteProxy,
            IAceptanteProxy aceptanteProxy,
            //IContactoAdquirienteProxy contactoAdquirienteProxy,
            IContactoAceptanteProxy contactoAceptanteProxy,
            IAdquirienteUbicacionProxy adquirienteUbicacionProxy,
            IUbigeoProxy ubigeoProxy,
            //ILineaAdquirienteProxy lineaAdquirienteProxy,
            //IComentariosProxy comentariosProxy,
            //IDivisoExternProxy divisoExternProxy,
             ICatalogoProxy catalogoProxy,
             IFondeadorProxy fondeadorProxy
        )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _dataProxy = dataProxy;
            //_adquirienteProxy = adquirienteProxy;
            _aceptanteProxy = aceptanteProxy;
            //_contactoAdquirienteProxy = contactoAdquirienteProxy;
            _contactoAceptanteProxy= contactoAceptanteProxy;
            _adquirienteUbicacionProxy = adquirienteUbicacionProxy;
            _ubigeoProxy = ubigeoProxy;
            //_lineaAdquirienteProxy = lineaAdquirienteProxy;
            //_comentariosProxy = comentariosProxy;
            //_divisoExternProxy = divisoExternProxy;
            _catalogoProxy = catalogoProxy;
            _fondeadorProxy = fondeadorProxy;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            //ViewBag.Paises = await _dataProxy.GetAllListPais();
            ViewBag.Sector = await _dataProxy.GetAllListSector();
            ViewBag.Grupo = await _dataProxy.GetAllListGrupo();
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

        public async Task<IActionResult> DeleteAceptante(int[] selectedAdquiriente)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            foreach (int id in selectedAdquiriente)
            {
                await _aceptanteProxy.Delete(id, userName);
            }
            return Json(new { succeeded = true, message = "Registros eliminados correctamente..." });
        }
 
        public async Task<IActionResult> Registro(int? adquirienteId)
        {
            ViewBag.Title = "Registro Aceptante: " + ((adquirienteId == null) ? "Registrar" : "Editar");
            ViewBag.IsEdit = adquirienteId != null;
            //ViewBag.Paises = await _dataProxy.GetAllListPais();
            ViewBag.Sector = await _dataProxy.GetAllListSector();
            ViewBag.Grupo = await _dataProxy.GetAllListGrupo();
            if (adquirienteId == null)
            {
                return View();
            }
            else
            {
                var adquirienteDetalle = await _aceptanteProxy.GetAceptante((int)adquirienteId);
                ViewBag.UbigeoPais = adquirienteDetalle.Data.FormatoUbigeoPais;
                ViewBag.UbigeoPaisCount = adquirienteDetalle.Data.FormatoUbigeoPais.Count;
                var UbigeoDepartamento = await _ubigeoProxy.GetUbigeo(adquirienteDetalle.Data.nIdPais, 1, "");
                ViewBag.DepartamentoPais = UbigeoDepartamento.Data;
                ViewBag.ListInversionista = await _fondeadorProxy.GetAllListFondeadoreslista();

                var Monedas = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 101, Tipo = 1 });
                ViewBag.Monedas = Monedas.Data;
                var TipoContactos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 128, Tipo = 1 });
                ViewBag.TipoContactos = TipoContactos.Data;
                var TipoContactosUbi = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 129, Tipo = 1 });
                ViewBag.TipoContactosUbi = TipoContactosUbi.Data;


                if (adquirienteDetalle.Succeeded == false)
                {
                    return Redirect("~/Aceptante/Index");
                }
                AdquirienteCreateModel adquirienteData = new();
                if (ModelState.IsValid)
                {
                    adquirienteData.IdAdquiriente = adquirienteDetalle.Data.nIdAdquiriente;
                    adquirienteData.Pais = adquirienteDetalle.Data.nIdPais;
                    adquirienteData.RegUnicoEmpresa = adquirienteDetalle.Data.cRegUnicoEmpresa;
                    adquirienteData.RazonSocial = adquirienteDetalle.Data.cRazonSocial;
                    adquirienteData.Sector = adquirienteDetalle.Data.nIdSector;
                    adquirienteData.GrupoEconomico = adquirienteDetalle.Data.nIdGrupoEconomico;
                    adquirienteData.NombreEstado = adquirienteDetalle.Data.NombreEstado;
                }
                return View(adquirienteData);
            }
        }
        
        public async Task<IActionResult> Detalle(int? adquirienteId)
        {
            ViewBag.Title = "Registro Aceptante: " + ((adquirienteId == null) ? "" : "Detalle");
            if (adquirienteId == null)
            {
                return Redirect("~/Aceptante/Index");
            }
            else
            {
                var adquirienteDetalle = await _aceptanteProxy.GetAceptante((int)adquirienteId);
                ViewBag.UbigeoPais = adquirienteDetalle.Data.FormatoUbigeoPais;

                if (adquirienteDetalle.Succeeded == false)
                {
                    return Redirect("~/Aceptante/Index");
                }
                AdquirienteCreateModel adquirienteData = new();
                if (ModelState.IsValid)
                {
                    adquirienteData.IdAdquiriente = adquirienteDetalle.Data.nIdAdquiriente;
                    adquirienteData.NombrePais = adquirienteDetalle.Data.cNombrePais;
                    adquirienteData.RegUnicoEmpresa = adquirienteDetalle.Data.cRegUnicoEmpresa;
                    adquirienteData.RazonSocial = adquirienteDetalle.Data.cRazonSocial;
                    adquirienteData.NombreSector = adquirienteDetalle.Data.cNombreSector;
                    adquirienteData.NombreGrupoEconomico = adquirienteDetalle.Data.cNombreGrupoEconomico;
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
            var giradorDetalle = await _aceptanteProxy.GetAceptante(adquirienteId);
            if (giradorDetalle.Succeeded == true)
            {
                IsAdquirienteExist = true;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (IsAdquirienteExist)
                    {
                        var result = await _aceptanteProxy.Update(new AdquirienteUpdateDto
                        {
                            IdAdquiriente = adquirienteId,
                            IdPais = model.Pais,
                            RegUnicoEmpresa = model.RegUnicoEmpresa,
                            RazonSocial = model.RazonSocial.ToUpper(),
                            IdSector = model.Sector,
                            IdGrupoEconomico = model.GrupoEconomico,
                            UsuarioActualizacion = userName
                        });
                        return Json(result);
                    }
                    else
                    {
                        var result = await _aceptanteProxy.Create(new AdquirienteInsertDto
                        {
                            IdPais = model.Pais,
                            RegUnicoEmpresa = model.RegUnicoEmpresa,
                            RazonSocial = model.RazonSocial.ToUpper(),
                            IdSector = model.Sector,
                            IdGrupoEconomico = model.GrupoEconomico,
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
            return Redirect("~/Aceptante/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarRegistro(int adquirienteId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _aceptanteProxy.Delete(adquirienteId, userName);
            return Json(result.Succeeded);
        }

        public async Task<IActionResult> GetAllContactos(int adquirienteId)
        {
            return Json(await _contactoAceptanteProxy.GetAllListGirador(adquirienteId));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarContacto(AgregarContactoAdquiriente model)
        {
            var result = await _contactoAceptanteProxy.Create(new ContactoAdquirienteCreateDto
            {
                IdAdquiriente = model.IdAdquirienteCabecera,
                Nombre = string.IsNullOrEmpty(model.Nombre) ? string.Empty : model.Nombre.ToUpper(),
                ApellidoPaterno = string.IsNullOrEmpty(model.ApellidoPaterno) ? string.Empty : model.ApellidoPaterno.ToUpper(),
                ApellidoMaterno = string.IsNullOrEmpty(model.ApellidoMaterno) ? string.Empty : model.ApellidoMaterno.ToUpper(),
                Celular = string.IsNullOrEmpty(model.Celular) ? string.Empty : model.Celular,
                Email = model.Email.ToLower(),
                IdTipoContacto = model.TipoContacto
            });
            return Json(result);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarContacto(int adquirienteContactoId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _contactoAceptanteProxy.Delete(adquirienteContactoId, userName);
            return Json(result.Succeeded);
        }
        public async Task<IActionResult> ListarUbigeos(int pais, int tipo, string codigo)
        {
            var result = await _ubigeoProxy.GetUbigeo(pais, tipo, codigo);
            return Json(result.Data);
        }

        public async Task<IActionResult> GetAllUbicaciones(int adquirienteId)
        {
            return Json(await _adquirienteUbicacionProxy.GetAllListDireccionAdquiriente(adquirienteId));
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

        public async Task<IActionResult> GetAllLinea(int adquirienteLineaId)
        {
            return Json(null);
            //return Json(await _lineaAdquirienteProxy.GetAllListAdquirienteLinea(adquirienteLineaId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarLinea(AgregarLineaAdquiriente model)
        {
            //var result = await _lineaAdquirienteProxy.Create(new LineaAdquirienteInsertDto
            //{
            //    nIdAdquiriente = model.IdAdquiriente,
            //    nIdInversionista = model.IdInversionista,
            //    nIdTipoMoneda = model.IdTipoMoneda,
            //    LineaMeta = model.LineaMeta,
            //    LineaDisponible = model.LineaMeta
            //});
            return Json(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarLinea(int adquirienteLineaId)
        {
            //var result = await _lineaAdquirienteProxy.Delete(adquirienteLineaId);
            return Json(null);
        }

        public async Task<IActionResult> GetAllComentariosAdquiriente(int adquirienteId)
        {
            //var comentarios = await _comentariosProxy.GetAllListComentarios(2, adquirienteId);
            return Json(null);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}