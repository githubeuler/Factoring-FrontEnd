using Factoring.Model;
using Factoring.Model.Models.Ubigeo;
using Factoring.Service.Common;
using Newtonsoft.Json;

namespace Factoring.Service.Proxies
{
    public interface IUbigeoProxy
    {
        Task<ResponseData<List<UbigeoGetDto>>> GetUbigeo(int idPais, int tipo, string codigo);
    }

    public class UbigeoProxy : IUbigeoProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;

        public UbigeoProxy(
            ProxyHttpClient proxyHttpClient
        )
        {
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<List<UbigeoGetDto>>> GetUbigeo(int idPais, int tipo, string codigo)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Ubigeo?IdPais={idPais}&Tipo={tipo}&Codigo={codigo}");
            //response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<UbigeoGetDto>>>(json);
            return data;
        }
    }
}
