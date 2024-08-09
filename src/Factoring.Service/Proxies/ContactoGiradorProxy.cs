using Factoring.Model;
using Factoring.Model.Models.ContactoGirador;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IContactoGiradorProxy
    {
        Task<ResponseData<int>> Create(ContactoGiradorCreateDto model);
        Task<ResponseData<int>> Delete(int idContactoGirador, string usuario);
        Task<ResponseData<List<ContactoGiradorResponseListDto>>> GetAllListGirador(int id);
    }

    public class ContactoGiradorProxy : IContactoGiradorProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public ContactoGiradorProxy(
            ProxyHttpClient proxyHttpClient,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<int>> Create(ContactoGiradorCreateDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("contactogirador", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Delete(int idGirador, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"contactogirador/delete?IdGiradorContacto={idGirador}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<ContactoGiradorResponseListDto>>> GetAllListGirador(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"contactogirador/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<ContactoGiradorResponseListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}