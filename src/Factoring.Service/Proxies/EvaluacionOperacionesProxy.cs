using Factoring.Model;
using Factoring.Model.Models.EvaluacionOperacion;
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
    public interface IEvaluacionOperacionesProxy
    {
        Task<ResponseData<int>> Create(EvaluacionOperacionesInsertDto model);
        Task<ResponseData<int>> CreateEstadoFactura(EvaluacionOperacionesEstadoInsertDto model);
    }
   public class EvaluacionOperacionesProxy : IEvaluacionOperacionesProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public EvaluacionOperacionesProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<ResponseData<int>> Create(EvaluacionOperacionesInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("EstadosOperacion", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<int>> CreateEstadoFactura(EvaluacionOperacionesEstadoInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("EstadosOperacion/estado-facura", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}