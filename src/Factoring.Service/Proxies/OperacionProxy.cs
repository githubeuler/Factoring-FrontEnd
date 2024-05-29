using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Factoring.Model;
//using Factoring.Model.Models.Cavali;
using Factoring.Model.Models.Operaciones;
using Factoring.Service.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Factoring.Service.Proxies
{
    public interface IOperacionProxy
    {
        Task<ResponseData<List<OperacionesResponseDataTable>>> GetAllListOperaciones(OperacionesRequestDataTableDto giradorRequestDatatableDto);
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
