using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Factoring.Service.Common
{
    public class ProxyHttpClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProxyHttpClient(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public const string desarrollo = "Desarrollo";
        public const string produccion = "Produccion";

        public HttpClient GetHttp()
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(_configuration[$"APIs:{desarrollo}"].ToString());
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue(_configuration["ContentTypeRequest"].ToString()));

            if (_httpContextAccessor.HttpContext.User != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;
                var access_token = claims.Single(x => x.Type.Equals("access_token")).Value;
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
            }

            return client;
        }
    }
}
