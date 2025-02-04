using Factoring.Model;
using Factoring.Model.Models.Adquiriente;
using Factoring.Model.Models.AdquirienteUbicacion;
using Factoring.Model.Models.Operaciones;
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
        Task<ResponseData<int>> Create(ModuloDTO model);
        Task<ResponseData<PerfilResponseEditDto>> GetAllListPerfilEdit(int nIdRol);
        Task<ResponseData<int>> Update(RequestMenuDto model);
        Task<ResponseData<List<AccionesResponseDto>>> GetAllMenuAcciones(AccionesRequestDto model);
        Task<ResponseData<int>> CreateAccion(ModuloNewDTO model);

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

        public async Task<ResponseData<int>> Create(ModuloDTO model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("ModuloMenu", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ResponseData<int>> CreateAccion(ModuloNewDTO model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("ModuloMenu/add-accion", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ResponseData<int>> Update(RequestMenuDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("ModuloMenu/update-perfil", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<PerfilResponseEditDto>> GetAllListPerfilEdit(int nIdRol)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"ModuloMenu/get-perfil-edit?nIdRol={nIdRol}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<PerfilResponseEditDto>>(json);
            return data;
        }
        public async Task<ResponseData<List<AccionesResponseDto>>> GetAllMenuAcciones(AccionesRequestDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"ModuloMenu/get-menu-acciones?nIdRol={model.nIdRol}&nIdMenu={model.nIdMenu}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<AccionesResponseDto>>>(json);
            return data;
        }
    }
}
