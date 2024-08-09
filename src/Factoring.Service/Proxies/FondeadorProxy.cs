using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Factoring.Model;
using Factoring.Model.Models.Externos;
using Factoring.Model.Models.Inversionista;
using Factoring.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Factoring.Service.Proxies
{
    public interface IFondeadorProxy
    {
        Task<List<DivisoFondeadores>> GetAllListFondeadoreslista();
    }

    public class FondeadorProxy : IFondeadorProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public FondeadorProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<List<DivisoFondeadores>> GetAllListFondeadoreslista()
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Fondeador/lista-fondeadores");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<InversionistaSGC>(json);
            return data.data;
        }
    }
}