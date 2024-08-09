using Factoring.Model;
using Factoring.Model.Models.GiradorDocumentos;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IDocumentosGiradorProxy
    {
        Task<ResponseData<int>> Create(DocumentosGiradorInsertDto model);
        Task<ResponseData<int>> Delete(int idGiradorDocumento, string usuario);
        Task<ResponseData<List<DocumentosGiradorListDto>>> GetAllListGiradorDocumentos(int id);
    }

    public class DocumentosGiradorProxy : IDocumentosGiradorProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public DocumentosGiradorProxy(
            ProxyHttpClient proxyHttpClient,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<int>> Create(DocumentosGiradorInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("DocumentosGirador", requestContent);
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
                var response = await client.GetAsync($"DocumentosGirador/delete?IdGiradorDocumento={idGiradorDocumento}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<DocumentosGiradorListDto>>> GetAllListGiradorDocumentos(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"DocumentosGirador/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<DocumentosGiradorListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}