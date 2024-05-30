using Factoring.Model.Models.Operaciones;
using Factoring.Model;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IOperacionProxy
    {
        Task<ResponseData<int>> Create(OperacionesInsertDto model);
        Task<ResponseData<List<OperacionesResponseDataTable>>> GetAllListOperaciones(OperacionesRequestDataTableDto model);
        Task<ResponseData<OperacionSingleResponseDto>> GetOperaciones(int id);
    }
    public class OperacionProxy : IOperacionProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public OperacionProxy(
            ProxyHttpClient proxyHttpClient,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<int>> Create(OperacionesInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Operaciones", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<OperacionesResponseDataTable>>> GetAllListOperaciones(OperacionesRequestDataTableDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Operaciones?Pageno={model.Pageno}&PageSize={model.PageSize}" +
            $"&Sorting={model.Sorting}&SortOrder={model.SortOrder}&FilterNroOperacion={model.FilterNroOperacion}" +
            $"&FilterRazonGirador={model.FilterRazonGirador}&FilterRazonAdquiriente={model.FilterRazonAdquiriente}&FilterFecCrea={model.FilterFecCrea}" +
            $"&Estado={model.Estado}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<OperacionesResponseDataTable>>>(json);
            return data;
        }

        public async Task<ResponseData<OperacionSingleResponseDto>> GetOperaciones(int id)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Operaciones/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<OperacionSingleResponseDto>>(json);
            return data;
        }
    }

}
