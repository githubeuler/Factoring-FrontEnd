using Factoring.Model;
using Factoring.Model.Models.Adquiriente;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;


namespace Factoring.Service.Proxies
{
    public interface IAdquirienteProxy
    {
    
        Task<List<AdquirienteResponseListaDto>> GetAllListAdquirientelista();
        Task<ResponseData<List<AdquirienteResponseDatatableDto>>> GetAllListAdquiriente(AdquirienteRequestDatatableDto model);
        Task<ResponseData<int>> Create(AdquirienteInsertDto model);
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
        public async Task<ResponseData<int>> Create(AdquirienteInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("adquiriente", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<List<AdquirienteResponseDatatableDto>>> GetAllListAdquiriente(AdquirienteRequestDatatableDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Aceptante?Pageno={model.Pageno}&PageSize={model.PageSize}" +
            $"&Sorting={model.Sorting}&SortOrder={model.SortOrder}&FilterRuc={model.FilterRuc}" +
            $"&FilterRazon={model.FilterRazon}" +
            $"&FilterIdPais={model.FilterIdPais}&FilterFecCrea={model.FilterFecCrea}" +
                $"&FilterIdSector={model.FilterIdSector}&FilterIdGrupoEconomico={model.FilterIdGrupoEconomico}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<AdquirienteResponseDatatableDto>>>(json);
            return data;
        }
    }
}