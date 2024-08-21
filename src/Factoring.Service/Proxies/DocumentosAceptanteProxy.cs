using Factoring.Model.Models.GiradorDocumentos;
using Factoring.Model;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factoring.Model.Models.DocumentosAceptante;

namespace Factoring.Service.Proxies
{
    public interface IDocumentosAceptanteProxy
    {
        Task<ResponseData<int>> Create(DocumentosAceptanteInsertDto model);
        Task<ResponseData<int>> Delete(int idGiradorDocumento, string usuario);
        Task<ResponseData<List<DocumentosAceptanteListDto>>> GetAllListAceptanteDocumentos(int id);
    }

    public class DocumentosAceptanteProxy : IDocumentosAceptanteProxy
    { 
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public DocumentosAceptanteProxy(
            ProxyHttpClient proxyHttpClient,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<int>> Create(DocumentosAceptanteInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("DocumentosAceptante", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Delete(int idGiradorDocumento, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"DocumentosAceptante/delete?IdAceptanteDocumento={idGiradorDocumento}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<DocumentosAceptanteListDto>>> GetAllListAceptanteDocumentos(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"DocumentosAceptante/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<DocumentosAceptanteListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}