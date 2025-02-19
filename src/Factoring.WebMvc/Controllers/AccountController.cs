﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Factoring.Model.Models.Auth;
using Factoring.Model.ViewModels;
using Factoring.Service.Proxies;
using Factoring.WebMvc.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections;
using Factoring.Model.Models.Usuario;

namespace Factoring.WebMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthProxy _authProxy;
        private readonly IMemoryCache _memoryCache;

        public AccountController(IAuthProxy authProxy,
            IMemoryCache memoryCache)
        {
            _authProxy = authProxy;
            _memoryCache = memoryCache;
        }

        public IActionResult Login(string returnUrl)
        {
            //***********************************************************************
            //  <I OAV - 21/12/2022>
            //  RECOPILACION DE CACHE
            //***********************************************************************
            if (!_memoryCache.TryGetValue(CacheKeys.Entry, out DateTime cacheValue))
            {
                // Key not in cache, so get data.
                cacheValue = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(System.TimeSpan.FromSeconds(60));

                // Save data in cache.
                _memoryCache.Set(CacheKeys.Entry, cacheValue, cacheEntryOptions);
            }
            //  <F OAV>

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _authProxy.Authenticate(new LoginAuthModel
            {
                Username = model.Username,
                Password = model.Password
            });

            if (result.Succeeded)
            {
                //***********************************************************************
                //  <I OAV - 21/12/2022>
                //  LIMPIEZA DE CACHE
                //***********************************************************************
                _memoryCache.Remove(CacheKeys.Entry);
                //  <F OAV>
                if (result.Data.MustChangePassword == 0)
                {
                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, result.Data.cCodigoUsuario),
                    new Claim(ClaimTypes.Name, result.Data.cNombreUsuario),
                    new Claim("country_claim", result.Data.cNombrePais),
                    new Claim("access_token", result.Data.JWToken)
                };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme
                    );
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true
                        }
                    );
                }

                var accionOperacion = await GetAccionOperacion(result.Data.Menu,"3",1);
                HttpContext.Session.SetObjectAsJson("nIdAccionMenuOpe", accionOperacion);
                var accionOperacionCavl = await GetAccionOperacion(result.Data.Menu, "7",2);
                HttpContext.Session.SetObjectAsJson("nIdAccionMenuOpeCavl", accionOperacionCavl);
                HttpContext.Session.SetObjectAsJson("ApplicationMenu", result.Data.Menu);

            }
            return Ok(result);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }
        private async Task<AccionRol> GetAccionOperacion(List<MenuResponse> lista, string nIdMenu,int nIdOpcion)
        {
            AccionRol accionRol = new();
            var result = lista.Where(item => item.nIdMenu == nIdMenu)
                .Select(item => item.cMenuPermisos)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(result))
            {
               var acction= await _authProxy.GetAcctionRol(result, nIdOpcion);
                accionRol = acction.Data;
            }
            return accionRol;
        }
        public IActionResult ChangePassword(string token)
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel() {
                Token = token
            };
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            model.Token = model.Token.Replace(".AM", "");
            var result = await _authProxy.ChangePassword(new ChangeAuthModel
            {
                NewPassword = model.NewPassword,
                CurrentPassword = model.CurrentPassword,
                Token = model.Token
            });
            if (result.Succeeded)
            {
                //***********************************************************************
                //  <I OAV - 21/12/2022>
                //  LIMPIEZA DE CACHE
                //***********************************************************************
                _memoryCache.Remove(CacheKeys.Entry);
                //  <F OAV>

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, result.Data.cCodigoUsuario),
                    new Claim(ClaimTypes.Name, result.Data.cNombreUsuario),
                    new Claim("country_claim", result.Data.cNombrePais),
                    new Claim("access_token", model.Token)
                };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme
                );
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    }
                );
            }
            return Ok(result);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(int IdUsuario,string CodigoUsuario)
        {

            var result = await _authProxy.ResetPassword(new ResetPasswordModel
            {
                CodigoUsuario = CodigoUsuario,
                IdUsuario = IdUsuario
            });
            //if (result.Succeeded)
            //{
                
            //}
            return Ok(result);
        }


    }
}
