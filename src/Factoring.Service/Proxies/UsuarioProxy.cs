using Factoring.Model;
using Factoring.Model.Models.Fondeador;
using Factoring.Model.Models.Usuario;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IUsuarioProxy
    {
        Task<ResponseData<List<UsuarioResponseDataTableDto>>> GetAllListUsuario(UsuarioRequestDataTableDto model);
        Task<ResponseData<UsuarioSingleDto>> GetUsuario(int id);
        Task<ResponseData<int>> Create(UsuarioRegistroRequestDto request);
        Task<ResponseData<int>> Update(UsuarioUpdateRequestDto model);
        Task<ResponseData<int>> Delete(int idUsuario, string usuario);
    }
    public class UsuarioProxy : IUsuarioProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public UsuarioProxy(
            ProxyHttpClient proxyHttpClient,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<int>> Create(UsuarioRegistroRequestDto request)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(request);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Usuario", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Delete(int idUsuario, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"Usuario/delete?IdUsuario={idUsuario}&UsuarioModificacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<UsuarioResponseDataTableDto>>> GetAllListUsuario(UsuarioRequestDataTableDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Usuario?Pageno={model.Pageno}&PageSize={model.PageSize}" +
            $"&Sorting={model.Sorting}&SortOrder={model.SortOrder}&FilterCodigoUsuario={model.FilterCodigoUsuario}" +
            $"&FilterNombreUsuario={model.FilterNombreUsuario}&FilterActivo={model.FilterActivo}&FilterIdPais={model.FilterIdPais}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<UsuarioResponseDataTableDto>>>(json);
            return data;
        }

        public async Task<ResponseData<UsuarioSingleDto>> GetUsuario(int id)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Usuario/obtener-usuario/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<UsuarioSingleDto>>(json);
            return data;
        }

        public async Task<ResponseData<int>> Update(UsuarioUpdateRequestDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Usuario/update", requestContent);
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
