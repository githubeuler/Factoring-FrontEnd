using Factoring.Model.Models.Girador;
using Factoring.Model;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IGiradorProxy
    {
        Task<ResponseData<List<GiradorResponseDatatableDto>>> GetAllListGirador(GiradorRequestDatatableDto model);
        Task<ResponseData<int>> Create(GiradorCreateDto model);
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
        public async Task<ResponseData<int>> Create(GiradorCreateDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("girador", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<List<GiradorResponseDatatableDto>>> GetAllListGirador(GiradorRequestDatatableDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Girador?Pageno={model.Pageno}&PageSize={model.PageSize}" +
            $"&Sorting={model.Sorting}&SortOrder={model.SortOrder}&FilterRuc={model.FilterRuc}" +
            $"&FilterRazon={model.FilterRazon}&FilterIdPais={model.FilterIdPais}&FilterFecCrea={model.FilterFecCrea}" +
                $"&FilterIdSector={model.FilterIdSector}&FilterIdGrupoEconomico={model.FilterIdGrupoEconomico}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<GiradorResponseDatatableDto>>>(json);
            return data;
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