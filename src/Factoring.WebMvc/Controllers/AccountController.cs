using Microsoft.AspNetCore.Authentication;
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
        [ValidateAntiForgeryToken]
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
    }
}
