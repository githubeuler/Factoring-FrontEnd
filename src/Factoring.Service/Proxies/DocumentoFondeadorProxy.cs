using Factoring.Model;
using Factoring.Model.Models.Fondeador;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IDocumentoFondeadorProxy
    {
        Task<ResponseData<int>> Create(DocumentoFondeadorRegistroDto model);
        Task<ResponseData<int>> Delete(int idDocumentoFondeador, string usuario);
        Task<ResponseData<List<DocumentoFondeadorResponseListDto>>> GetAllList(int id);
    }
    public class DocumentoFondeadorProxy : IDocumentoFondeadorProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public DocumentoFondeadorProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<int>> Create(DocumentoFondeadorRegistroDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("FondeadorDocumento", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Delete(int idDocumentoFondeador, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"FondeadorDocumento/delete?IdFondeadorDocumento={idDocumentoFondeador}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<DocumentoFondeadorResponseListDto>>> GetAllList(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"FondeadorDocumento/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<DocumentoFondeadorResponseListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}