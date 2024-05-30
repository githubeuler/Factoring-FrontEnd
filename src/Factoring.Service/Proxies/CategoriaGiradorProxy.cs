using Factoring.Model;
using Factoring.Model.Models.CategoriaGirador;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Factoring.Service.Proxies
{
    public interface ICategoriaGiradorProxy
    {
        Task<ResponseData<List<CategoriaGiradorDto>>> GetAllCategoriasListGirador(int id);
    }

    public class CategoriaGiradorProxy : ICategoriaGiradorProxy
    {
        private readonly IConfiguration _configuration;
        private readonly ProxyHttpClient _proxyHttpClient;

        public CategoriaGiradorProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<ResponseData<List<CategoriaGiradorDto>>> GetAllCategoriasListGirador(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"CategoriaGirador/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<CategoriaGiradorDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
