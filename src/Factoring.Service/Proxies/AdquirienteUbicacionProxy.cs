using Factoring.Model;
using Factoring.Model.Models.AdquirienteUbicacion;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IAdquirienteUbicacionProxy
    {
        Task<ResponseData<int>> Create(UbicacionAdquirienteInsertDto model);
        Task<ResponseData<int>> Delete(int idAdquirienteDireccion, string usuario);
        Task<ResponseData<List<UbicacionAdquirienteListDto>>> GetAllListDireccionAdquiriente(int id);
    }

    public class AdquirienteUbicacionProxy : IAdquirienteUbicacionProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public AdquirienteUbicacionProxy(ProxyHttpClient proxyHttpClient,
            IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<ResponseData<int>> Create(UbicacionAdquirienteInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("UbicacionAdquiriente", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Delete(int idAdquirienteDireccion, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"UbicacionAdquiriente/delete?IdAdquirienteContacto={idAdquirienteDireccion}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<UbicacionAdquirienteListDto>>> GetAllListDireccionAdquiriente(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"UbicacionAdquiriente/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<UbicacionAdquirienteListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
