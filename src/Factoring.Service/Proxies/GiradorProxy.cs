using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Factoring.Model;
//using Factoring.Model.Models.Girador;
using Factoring.Service.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Factoring.Model.Models.Girador;

namespace Factoring.Service.Proxies
{
    public interface IGiradorProxy
    {
        Task<List<GiradorResponseListaDto>> GetAllListGiradorlista();
     
    }

    public class GiradorProxy : IGiradorProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public GiradorProxy(
            ProxyHttpClient proxyHttpClient,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }


        public async Task<List<GiradorResponseListaDto>> GetAllListGiradorlista()
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"girador/lista");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<GiradorResponseListaDto>>>(json);
            return data.Data;
        }

    }
}