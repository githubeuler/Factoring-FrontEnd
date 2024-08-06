using Factoring.Model;
using Factoring.Model.Models.Fondeador;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface ICavaliFactoringFondeadorProxy
    {
        Task<ResponseData<int>> Create(CavaliFactoringFondeadorRegistroDto model);
        Task<ResponseData<int>> Delete(int idCavaliFactoringFondeador, string usuario);
        Task<ResponseData<List<CavaliFactoringFondeadorResponseListDto>>> GetAllListCavaliFactoring(int id);
    }
    public class CavaliFactoringFondeadorProxy : ICavaliFactoringFondeadorProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public CavaliFactoringFondeadorProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<int>> Create(CavaliFactoringFondeadorRegistroDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("FondeadorCavali", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Delete(int idCavaliFactoringFondeador, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"FondeadorCavali/delete?IdFondeadorCavali={idCavaliFactoringFondeador}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<CavaliFactoringFondeadorResponseListDto>>> GetAllListCavaliFactoring(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"FondeadorCavali/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<CavaliFactoringFondeadorResponseListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
