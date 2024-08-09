using Factoring.Model;
using Factoring.Model.Models.AceptanteContacto;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Service.Proxies
{
    public interface IContactoAceptanteProxy
    {
        Task<ResponseData<int>> Create(ContactoAdquirienteCreateDto model);
        Task<ResponseData<int>> Delete(int idAdquirienteContacto, string usuario);
        Task<ResponseData<List<ContactoAdquirienteResponseListDto>>> GetAllListGirador(int id);
    }
    public class ContactoAceptanteProxy : IContactoAceptanteProxy
    {
        private readonly IConfiguration _configuration;
        private readonly ProxyHttpClient _proxyHttpClient;
        public ContactoAceptanteProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<ResponseData<int>> Create(ContactoAdquirienteCreateDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("AceptanteContacto", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<int>> Delete(int idAdquirienteContacto, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"AceptanteContacto/delete?IdAdquirienteContacto={idAdquirienteContacto}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<List<ContactoAdquirienteResponseListDto>>> GetAllListGirador(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"AceptanteContacto/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<ContactoAdquirienteResponseListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}