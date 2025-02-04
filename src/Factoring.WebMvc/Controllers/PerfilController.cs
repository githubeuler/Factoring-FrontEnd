using Factoring.Model;
using Factoring.Model.Models.AceptanteContacto;
using Factoring.Model.Models.Auth;
using Factoring.Model.Models.EvaluacionOperacion;
using Factoring.Model.Models.Fondeo;
using Factoring.Model.Models.Operaciones;
using Factoring.Model.Models.PerfilMenu;
using Factoring.Model.Models.Usuario;
using Factoring.Model.ViewModels;
using Factoring.Model.ViewModels.Aceptante;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private IConfiguration _configuration;
        private readonly IPerfilMenuproxy _perfilMenuproxy;
        private readonly ICatalogoProxy _catalogoProxy;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PerfilController(
         IConfiguration configuration,
         IPerfilMenuproxy perfilMenuproxy,
         ICatalogoProxy catalogoProxy,
         IHttpContextAccessor httpContextAccessor
         )
        {
            _configuration = configuration;
            _perfilMenuproxy = perfilMenuproxy;
            _catalogoProxy = catalogoProxy;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult IndexAsync()
        {
            return View();
        }

        public async Task<JsonResult> ListadoRegistros(PerfilViewModel perfil)
        {
            try
            {
                var requestData = new PerfilRequestDto
                {
                    Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault())) == 0 ? 0 : (Convert.ToInt32(Request.Form["start"].FirstOrDefault())),
                    PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault())),
                    Sorting = "nIdRoles",
                    SortOrder = "desc",
                    cNombrePerfil = perfil.cNombrePerfil
                };
                var data = await _perfilMenuproxy.GetAllListPerfil(requestData);
                var recordsTotal = data.Data.Count > 0 ? data.Data[0].TotalRecords : 0;
                return Json(new { data = data.Data, recordsTotal = recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> Registro(int? nRolId, int? nOpcion)
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            ViewBag.Title = ((nRolId == null) ? "Registrar Rol" : "Editar Rol");
            ViewBag.IsEdit = nRolId != null;
            ViewBag.vVer = 0;
            //if (nOpcion == 2)
            //{
            //    ViewBag.vNuevo = 3;
            //}
            if (nRolId == null)
            {
                ViewBag.vNuevo = 1;
                return View();
            }
            else
            {

                if (nOpcion == 2)
                {
                    ViewBag.vNuevo = 3;
                }
                else
                {
                    ViewBag.vNuevo = 2;
                }
                var result = await _perfilMenuproxy.GetAllListPerfilEdit(nRolId.Value);
                if (result.Succeeded == false)
                {
                    return Redirect("~/Perfil/Index");
                }
                PerfilCreateModelNew perfilData = new();

                if (ModelState.IsValid)
                {
                    perfilData.nIdRol = result.Data.nIdRol;
                    perfilData.cNombreRol = result.Data.cNombreRol;
                }
                return View(perfilData);
            }
        }

        public async Task<IActionResult> GetAllMenu(int nIdRol)
        {
            return Json(await _perfilMenuproxy.GetAllMenuModulo(nIdRol));
        }
        //[HttpPost]
        //public async Task<IActionResult> RegistrarPerfil([FromBody] PerfilCreateModelNew model)
        //{
        //    if (model == null || model.ListaMenu == null || !model.ListaMenu.Any())
        //    {
        //        return BadRequest("No se recibieron datos válidos.");
        //    }
        //    ResponseData<int> resultado = new ResponseData<int>();

        //    foreach (var item in model.ListaMenu)
        //    {
        //        var acciones = new List<string>();
        //        if (item.Insertar) acciones.Add("INSERTAR");
        //        if (item.Actualizar) acciones.Add("ACTUALIZAR");
        //        if (item.Consultar) acciones.Add("CONSULTAR");
        //        if (item.Eliminar) acciones.Add("ELIMINAR");
        //        string sMenu = string.Join(",", acciones);
        //        if (sMenu.Length > 0)
        //        {
        //            ModuloDTO moduloDTO = new()
        //            {
        //                cRol = model.cNombreRol,
        //                nIdMenu = item.nIdMenuDetalle,
        //                filter_Acciones = sMenu
        //            };
        //            var result2 = await _perfilMenuproxy.Create(moduloDTO);
        //            resultado = result2;
        //        }
        //    }
        //    return Json(resultado);
        //}
        [HttpPost]
        public async Task<IActionResult> RegistrarPerfil([FromBody] PerfilCreateModelNew model)
        {
            if (model == null)
            {
                return BadRequest("No se recibieron datos válidos.");
            }
            ResponseData<int> resultado = new ResponseData<int>();

            //foreach (var item in model.ListaMenu)
            //{
            //    var acciones = new List<string>();
            //    if (item.Insertar) acciones.Add("INSERTAR");
            //    if (item.Actualizar) acciones.Add("ACTUALIZAR");
            //    if (item.Consultar) acciones.Add("CONSULTAR");
            //    if (item.Eliminar) acciones.Add("ELIMINAR");
            //    string sMenu = string.Join(",", acciones);
            //    if (sMenu.Length > 0)
            //    {
            //        ModuloDTO moduloDTO = new()
            //        {
            //            cRol = model.cNombreRol,
            //            nIdMenu = item.nIdMenuDetalle,
            //            filter_Acciones = sMenu
            //        };
            //        var result2 = await _perfilMenuproxy.Create(moduloDTO);
            //        resultado = result2;
            //    }
            //}
            ModuloDTO moduloDTO = new()
            {
                cRol = model.cNombreRol,
                nIdRol= model.nIdRol
            };
            var result2 = await _perfilMenuproxy.Create(moduloDTO);
            resultado = result2;
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarPerfil(int idRol)
        {
            RequestMenuDto obj = new()
            {
                nIdRol = idRol
            };
            var result = await _perfilMenuproxy.Update(obj);
            return Json(result.Succeeded);
        }

        //[HttpPost]
        public async Task<IActionResult> RegistrarPerfilAccion(PerfilCreateModelNew model)
        {
            ResponseData<int> resultado = new ResponseData<int>();
            ModuloNewDTO request = new()
            {
                nIdMenu = model.nIdMenuAccion,
                nIdRol = model.nIdRolAccion,
                filter_Acciones = model.cIdAccion
            };
            var result2 = await _perfilMenuproxy.CreateAccion(request);
            resultado = result2;
            return Json(resultado);
        }

        public async Task<IActionResult> GetAllMenuAcciones(int nIdRol, int nIdMenu)
        {
            AccionesRequestDto request = new()
            {
                nIdRol = nIdRol,
                nIdMenu = nIdMenu
            };
            return Json(await _perfilMenuproxy.GetAllMenuAcciones(request));
        }



    }

}
