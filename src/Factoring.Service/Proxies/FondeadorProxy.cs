using Factoring.Model;
using Factoring.Model.Models.Fondeador;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IFondeadorProxy
    {
        Task<ResponseData<List<FondedorResponseDatatableDto>>> GetAllLisFondeador(FondeadorRequestDatatableDto request);
        Task<ResponseData<int>> Create(FondeadorRegistroRequestDto request);
        Task<ResponseData<FondeadorSingleDto>> GetFondeador(int id);
        Task<ResponseData<int>> Update(FondeadorUpdateRequestDto model);
    }
    public class FondeadorProxy : IFondeadorProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public FondeadorProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }
        public async Task<ResponseData<List<FondedorResponseDatatableDto>>> GetAllLisFondeador(FondeadorRequestDatatableDto request)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Fondeador?Pageno={request.Pageno}&PageSize={request.PageSize}" +
                $"&Sorting={request.Sorting}&SortOrder={request.SortOrder}&FilterDoi={request.FilterDoi}" +
                $"&FilterRazon={request.FilterRazon}&FilterFecCrea={request.FilterFecCrea}&IdEstado={request.IdEstado}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<FondedorResponseDatatableDto>>>(json);
            return data;
        }

        public async Task<ResponseData<int>> Create(FondeadorRegistroRequestDto request)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Fondeador", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<FondeadorSingleDto>> GetFondeador(int id)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Fondeador/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<FondeadorSingleDto>>(json);
            return data;
        }

        public async Task<ResponseData<int>> Update(FondeadorUpdateRequestDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Fondeador/update", requestContent);
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
