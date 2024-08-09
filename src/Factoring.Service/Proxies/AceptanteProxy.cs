using Factoring.Model.Models.Aceptante;
using Factoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Factoring.Service.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Factoring.Model.Models.Adquiriente;
using System.Data;

namespace Factoring.Service.Proxies
{
    public interface IAceptanteProxy
    {
        Task<ResponseData<List<AceptanteResponseDatatableDto>>> GetAllListAceptante(AceptanteRequestDatatableDto model);
        Task<ResponseData<int>> Delete(int idAdquiriente, string usuario);
        Task<ResponseData<AceptanteGetByIdDto>> GetAceptante(int id);
        Task<ResponseData<int>> Create(AdquirienteInsertDto model);
        Task<ResponseData<int>> Update(AdquirienteUpdateDto model);


    }
    public class AceptanteProxy: IAceptanteProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public AceptanteProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<ResponseData<List<AceptanteResponseDatatableDto>>> GetAllListAceptante(AceptanteRequestDatatableDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"aceptante?Pageno={model.Pageno}&PageSize={model.PageSize}" +
            $"&Sorting={model.Sorting}&SortOrder={model.SortOrder}&FilterRuc={model.FilterRuc}" +
            $"&FilterRazon={model.FilterRazon}" +
            $"&FilterIdPais={model.FilterIdPais}&FilterFecCrea={model.FilterFecCrea}" +
                $"&FilterIdSector={model.FilterIdSector}&FilterIdGrupoEconomico={model.FilterIdGrupoEconomico}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<AceptanteResponseDatatableDto>>>(json);
            return data;
        }

        public async Task<ResponseData<int>> Delete(int idAdquiriente, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"aceptante/delete?IdAdquiriente={idAdquiriente}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<AceptanteGetByIdDto>> GetAceptante(int id)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"aceptante/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<AceptanteGetByIdDto>>(json);
            return data;
        }
        public async Task<ResponseData<int>> Create(AdquirienteInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("aceptante", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Update(AdquirienteUpdateDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("aceptante/update", requestContent);
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
