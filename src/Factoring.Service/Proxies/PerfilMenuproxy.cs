using Factoring.Model;
using Factoring.Model.Models.Adquiriente;
using Factoring.Model.Models.PerfilMenu;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IPerfilMenuproxy
    {

        Task<ResponseData<List<MenuAccionesResponseDto>>> GetAllMenuModulo(int nIdRol);
        Task<ResponseData<List<PerfilResponseDto>>> GetAllListPerfil(PerfilRequestDto model);

    }
    public class PerfilMenuproxy : IPerfilMenuproxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;
        public PerfilMenuproxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }
        public async Task<ResponseData<List<PerfilResponseDto>>> GetAllListPerfil(PerfilRequestDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"ModuloMenu/get-perfil?Pageno={model.Pageno}&PageSize={model.PageSize}" +
            $"&Sorting={model.Sorting}&SortOrder={model.SortOrder}&cNombrePerfil={model.cNombrePerfil}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<PerfilResponseDto>>>(json);
            return data;
        }
        public async Task<ResponseData<List<MenuAccionesResponseDto>>> GetAllMenuModulo(int nIdRol)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"ModuloMenu/get-menu-modulo?nRol={nIdRol}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<MenuAccionesResponseDto>>>(json);
            return data;
        }

    }
}
