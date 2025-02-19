﻿using Factoring.Model;
using Factoring.Model.Models.Auth;
using Factoring.Model.Models.Catalogo;
using Factoring.Model.Models.Usuario;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Factoring.WebMvc.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IDataProxy _dataProxy;
        private readonly IUsuarioProxy _usuarioProxy;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPerfilMenuproxy _perfilMenuproxy;
        private readonly ICatalogoProxy _catalogoProxy;

        public UsuarioController(IDataProxy dataProxy, IUsuarioProxy usuarioProxy, IPerfilMenuproxy perfilMenuproxy, ICatalogoProxy catalogoProxy, IHttpContextAccessor httpContextAccessor)
        {
            _dataProxy = dataProxy;
            _usuarioProxy = usuarioProxy;
            _httpContextAccessor = httpContextAccessor;
            _perfilMenuproxy = perfilMenuproxy;
            _catalogoProxy = catalogoProxy;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        
        public async Task<IActionResult> IndexAsync()
        {

            var data = await _dataProxy.GetAllListPais();
            ViewBag.Pais = data;

            List<CatalogoResponseListDto> lstEstado = new List<CatalogoResponseListDto>();
            lstEstado.Add(new CatalogoResponseListDto() { nId = -1, cNombre = "TODOS" });
            lstEstado.Add(new CatalogoResponseListDto() { nId = 1, cNombre = "ACTIVO" });
            lstEstado.Add(new CatalogoResponseListDto() { nId = 0, cNombre = "INACTIVO" });

            ViewBag.Estados = lstEstado;

            return View(new UsuarioViewModel() { IdPais = 0,IdEstado = -1 });
        }
        public async Task<IActionResult> Registro(int? usuarioId)
        {
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<MenuResponse>>("ApplicationMenu") == null)
            {
                return Redirect("~/Account/Logout");
            }
            ViewBag.Title = ((usuarioId == null) ? "Registrar Usuario" : "Editar Usuario");
            ViewBag.IsEdit = usuarioId != null;
            var data = await _dataProxy.GetAllListPais();
            ViewBag.Pais = data.Where(x=>x.nIdPais > 0).ToList();


            var roles = await _perfilMenuproxy.GetAllListPerfil(new Model.Models.PerfilMenu.PerfilRequestDto() {Pageno = 0,PageSize=10000,Sorting= "nIdRoles",SortOrder= "ASC",cNombrePerfil = "" });

            ViewBag.Roles = roles.Data;

            var TipoDocumento = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 122, Tipo = 1, Valor = "1" });
            ViewBag.TipoDocumento = TipoDocumento.Data;

            //var ActividadesEconomicas = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 120, Tipo = 1, Valor = "1" });
            //ViewBag.ActividadesEconomicas = ActividadesEconomicas.Data.Select(x => new CatalogoResponseListDto { cNombre = x.nId + " - " + x.cNombre, nId = x.nId }).ToList(); ;
            //var TipoDocumentos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 108, Tipo = 1, Valor = "1" });
            //var TipoContactos = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 117, Tipo = 1, Valor = "1" });
            //ViewBag.TipoContactos = TipoContactos.Data;
            //var TipoContactosUbi = await _catalogoProxy.GetCatalogoList(new Model.Models.Catalogo.CatalogoListDto { Codigo = 121, Tipo = 1, Valor = "1" });
            //ViewBag.TipoContactosUbi = TipoContactosUbi.Data;
            //ViewBag.Documentos = TipoDocumentos.Data;

            if (usuarioId == null)
            {
                return View();
            }
            else
            {
                var usuarioDetalle = await _usuarioProxy.GetUsuario((int)usuarioId);
                if (usuarioDetalle.Succeeded == false)
                {
                    return Redirect("~/Usuario/Index");
                }

                //ViewBag.UbigeoPais = adquirienteDetalle.Data.FormatoUbigeoPais;
                //ViewBag.UbigeoPaisCount = adquirienteDetalle.Data.FormatoUbigeoPais.Count;
                //var UbigeoDepartamento = await _ubigeoProxy.GetUbigeo(adquirienteDetalle.Data.nIdPais, 1, "");
                //ViewBag.DepartamentoPais = UbigeoDepartamento.Data;
                UsuarioCreateModel usuarioData = new();
                if (ModelState.IsValid)
                {
                    usuarioData.IdUsuario = usuarioDetalle.Data.IdUsuario;
                    usuarioData.CodigoUsuario = usuarioDetalle.Data.CodigoUsuario;
                    usuarioData.NombreUsuario = usuarioDetalle.Data.NombreUsuario; //adquirienteDetalle.Data.dFechaInicioActividad == "01/01/1900" ? string.Empty : adquirienteDetalle.Data.dFechaInicioActividad; ;
                    usuarioData.Correo = usuarioDetalle.Data.Correo;
                    //HttpContext.Session.SetObjectAsJson("RucAceptante", adquirienteDetalle.Data.cRegUnicoEmpresa);
                    //usuarioData.Contrasena = usuarioDetalle.Data.Contrasena;
                    usuarioData.IdPais = usuarioDetalle.Data.IdPais;
                    usuarioData.IdRol = usuarioDetalle.Data.IdRol;
                    usuarioData.IdEstado = Convert.ToBoolean(usuarioDetalle.Data.IdEstado);
                    usuarioData.IdTipoDocumento = usuarioDetalle.Data.IdTipoDocumento;
                    usuarioData.NumeroDocumento = usuarioDetalle.Data.NumeroDocumento;
                    usuarioData.Cargo = usuarioDetalle.Data.Cargo;
                    usuarioData.Telefono = usuarioDetalle.Data.Telefono;
                    usuarioData.Celular = usuarioDetalle.Data.Celular;
                    usuarioData.Ruc = usuarioDetalle.Data.Ruc;
                    usuarioData.RazonSocial = usuarioDetalle.Data.RazonSocial;

                }
                return View(usuarioData);
            }
        }
        public async Task<JsonResult> GetUsuarioAllList(UsuarioViewModel model)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            try
            {
                var requestData = new UsuarioRequestDataTableDto();
                requestData.Pageno = (Convert.ToInt32(Request.Form["start"].FirstOrDefault()));
                requestData.PageSize = (Convert.ToInt32(Request.Form["length"].FirstOrDefault())) == 0 ? 10 : (Convert.ToInt32(Request.Form["length"].FirstOrDefault()));
                requestData.Sorting = "nIdUsuario";
                requestData.SortOrder = "desc";
                requestData.FilterCodigoUsuario = model.CodigoUsuario;
                requestData.FilterNombreUsuario = model.NombreUsuario;
                requestData.FilterActivo = model.IdEstado;
                requestData.FilterIdPais = model.IdPais;

                requestData.Usuario = userName;

                var data = await _usuarioProxy.GetAllListUsuario(requestData);
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
        public async Task<IActionResult> RegistrarUsuario(int usuarioId, UsuarioCreateModel usuario)
        {
            try
            {
               var userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                bool IsUsuarioExist = false;
                var UsuarioDetalle = await _usuarioProxy.GetUsuario(usuarioId);
                if (UsuarioDetalle.Succeeded == true)
                {
                    IsUsuarioExist = true;
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (IsUsuarioExist)
                        {
                            var result = await _usuarioProxy.Update(new UsuarioUpdateRequestDto
                            {
                                IdUsuario = usuarioId,
                                NombreUsuario = usuario.NombreUsuario.ToUpper(),
                                Correo = usuario.Correo.ToUpper(),
                                //Password = usuario.Contrasena,
                                UsuarioModificacion = userName,
                                IdPais = usuario.IdPais,
                                IdRol = usuario.IdRol,// cambiarE
                                Activo = Convert.ToInt32(usuario.IdEstado),
                                IdTipoDocumento = usuario.IdTipoDocumento,
                                NumeroDocumento = usuario.NumeroDocumento,
                                Telefono = usuario.Telefono,
                                Celular = usuario.Celular,
                                Cargo = usuario.Cargo,
                                Ruc = usuario.Ruc,
                                RazonSocial = usuario.RazonSocial
                                
                            });
                            result.Message = "Usuario actualizado correctamente.";
                            return Json(result);
                        }
                        else
                        {
                            var exists = await GetUsuario(usuario.CodigoUsuario);
                            if (exists == null)
                            {
                                var response = await _usuarioProxy.Create(new UsuarioRegistroRequestDto()
                                {
                                    CodigoUsuario = usuario.CodigoUsuario.ToUpper(),
                                    NombreUsuario = usuario.NombreUsuario.ToUpper(),
                                    Correo = usuario.Correo.ToUpper(),
                                    //Password = usuario.Contrasena,
                                    UsuarioCreador = userName,
                                    IdPais = usuario.IdPais,
                                    IdRol = usuario.IdRol,// cambiarE
                                    IdTipoDocumento = usuario.IdTipoDocumento,
                                    NumeroDocumento = usuario.NumeroDocumento,
                                    Telefono = usuario.Telefono,
                                    Celular = usuario.Celular,
                                    Cargo = usuario.Cargo,
                                      Ruc = usuario.Ruc,
                                    RazonSocial = usuario.RazonSocial
                                });
                                //response.Message = "Usuario creado correctamente.";
                                return Json(response);
                            }
                            else
                            {
                                return Json(new ResponseData<int>() { Succeeded = false, Message = "El Usuario " + usuario.CodigoUsuario + " ya existe." });
                            }

                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return Redirect("~/Usuario/Index");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<UsuarioResponseDataTableDto> GetUsuario(string? CodigoUsuario)
        {
            UsuarioResponseDataTableDto oRecord = null;
            try
            {
                var requestData = new UsuarioRequestDataTableDto();
                requestData.Pageno = 0;
                requestData.PageSize = 5;
                requestData.Sorting = "nIdUsuario";
                requestData.SortOrder = "asc";
                requestData.FilterCodigoUsuario = CodigoUsuario;
                requestData.FilterActivo = 1;
                var data = await _usuarioProxy.GetAllListUsuario(requestData);

                if (data.Data.Count > 0)
                    oRecord = data.Data[0];
            }
            catch (Exception)
            {
                throw;
            }

            return oRecord;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarUsuario(int usuarioId)
        {
            var userName = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _usuarioProxy.Delete(usuarioId, userName);
            return Json(result.Succeeded);
        }
    }
}
