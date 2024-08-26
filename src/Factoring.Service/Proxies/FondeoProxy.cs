using Factoring.Model;
using Factoring.Model.Models.Fondeo;
using Factoring.Model.Models.Operaciones;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
   

    public interface IFondeoProxy
    {
        Task<ResponseData<List<FondeoResponseDatatableDto>>> GetAllLisFondeo(FondeoRequestDatatableDto request);
        Task<ResponseData<int>> Update(FondeoUpdateDto model);
        Task<ResponseData<int>> Insert(FondeoInsertDto model);
        Task<ResponseData<int>> UpdateState(FondeoUpdateStateDto model);
        Task<ResponseData<string>> GetReporteRegistroFondeoDonwload(FondeoRequestDatatableDto model);
    }

    public class FondeoProxy : IFondeoProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public FondeoProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<List<FondeoResponseDatatableDto>>> GetAllLisFondeo(FondeoRequestDatatableDto request)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Fondeo?Pageno={request.Pageno}&PageSize={request.PageSize}" +
                $"&Sorting={request.Sorting}&SortOrder={request.SortOrder}&FilterNroOperacion={request.FilterNroOperacion}&FilterFondeadorAsignado={request.FilterFondeadorAsignado}&FilterGirador={request.FilterGirador}&FilterNroOperacion={request.FilterNroOperacion}" +
                $"&FilterFechaRegistro={request.FilterFechaRegistro}&FilterEstadoFondeo={request.FilterEstadoFondeo}&IdEstado={request.IdEstado}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<FondeoResponseDatatableDto>>>(json);
            return data;
        }

        public async Task<ResponseData<int>> Insert(FondeoInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Fondeo", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Update(FondeoUpdateDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Fondeo/update", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> UpdateState(FondeoUpdateStateDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var us = JsonConvert.SerializeObject(model);
            var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
            var response = await client.PostAsync("Fondeo/update-state", requestContent);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseData<int>>(json);
        }

        public async Task<ResponseData<string>> GetReporteRegistroFondeoDonwload(FondeoRequestDatatableDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            client.Timeout = TimeSpan.FromMinutes(5);
            var response = await client.GetAsync($"Fondeo/get-registro-fondeo-base64?&FilterNroOperacion={model.FilterNroOperacion}" +
                $"&FilterRazonFondeador={model.FilterFondeadorAsignado}&FilterRazonGirador={model.FilterGirador}&FilterFecCrea={model.FilterFechaRegistro}" +
                $"&Estado={model.FilterEstadoFondeo}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<string>>(json);
            return data;
        }
    }
}
