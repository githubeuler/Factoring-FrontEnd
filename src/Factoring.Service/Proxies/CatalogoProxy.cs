using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Factoring.Model;
using Factoring.Model.Models.Catalogo;
using Factoring.Service.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Factoring.Service.Proxies
{
    public interface ICatalogoProxy
    {
        Task<ResponseData<List<CatalogoResponseListDto>>> GetCatalogoList(CatalogoListDto model);
        Task<ResponseData<List<CatalogoResponseListDto>>> GetGategoriaGirador(CatalogoListDto model);
    }
    public class CatalogoProxy : ICatalogoProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public CatalogoProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }
        public async Task<ResponseData<List<CatalogoResponseListDto>>> GetCatalogoList(CatalogoListDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Catalogo?Tipo={model.Tipo}&Codigo={model.Codigo}&Valor={model.Valor}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<CatalogoResponseListDto>>>(json);
            return data;
        }
        public async Task<ResponseData<List<CatalogoResponseListDto>>> GetGategoriaGirador(CatalogoListDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Catalogo/get-catogoria-girador?Codigo={model.Codigo}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<CatalogoResponseListDto>>>(json);
            return data;

        }

    }
}
