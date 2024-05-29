using Factoring.Model;
using Factoring.Model.Models.OperacionesFactura;
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
    public interface IFacturaOperacionesProxy
    {
        Task<ResponseData<List<OperacionesFacturaListDto>>> GetAllListFacturaByIdOperaciones(int id);
    }

    public class FacturaOperacionesProxy : IFacturaOperacionesProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public FacturaOperacionesProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<ResponseData<List<OperacionesFacturaListDto>>> GetAllListFacturaByIdOperaciones(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"OperacionesFactura/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<OperacionesFacturaListDto>>>(json);
                data.Data.ForEach(m => m.nMontoTotal = data.Data.Sum(x => x.nMonto));
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}