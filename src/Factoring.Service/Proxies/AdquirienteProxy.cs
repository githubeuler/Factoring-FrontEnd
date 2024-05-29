using Factoring.Model;
using Factoring.Model.Models.Adquiriente;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Service.Proxies
{
    public interface IAdquirienteProxy
    {
    
        Task<List<AdquirienteResponseListaDto>> GetAllListAdquirientelista();
    }

    public class AdquirienteProxy : IAdquirienteProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public AdquirienteProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<List<AdquirienteResponseListaDto>> GetAllListAdquirientelista()
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Adquiriente/lista");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<AdquirienteResponseListaDto>>>(json);
            return data.Data;
        }

    }
}