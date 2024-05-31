using Factoring.Model.Models.OperacionesFactura;
using Factoring.Model;
using Factoring.Model.Models.OperacionesFactura;
using Factoring.Service.Common;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Factoring.Service.Proxies
{
    public interface IFacturaOperacionesProxy
    {
        Task<ResponseData<int>> Create(OperacionesFacturaInsertDto model);
        Task<ResponseData<int>> Delete(int idAdquirienteDireccion, string usuario);
        Task<ResponseData<int>> Editar(OperacionesFacturaEditDto model);
        Task<ResponseData<OperacionesFacturaListDto>> GetInvoiceByNumber(int IdGirador, int IdAdquiriente, string NroFactura);
        Task<ResponseData<List<OperacionesFacturaListDto>>> GetAllListFacturaByIdOperaciones(int id);
        Task<ResponseData<List<OperacionesFacturaListDto>>> GetAllListFacturaByIdOperacionesFactura(int id);
    }
    public class FacturaOperacionesProxy : IFacturaOperacionesProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public FacturaOperacionesProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }

        public async Task<ResponseData<List<OperacionesFacturaListDto>>> GetAllListFacturaByIdOperaciones(int id)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"OperacionesFactura/{id}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<OperacionesFacturaListDto>>>(json);
                data.Data.ForEach(m => m.nMontoTotal = data.Data.Sum(x => x.nMonto));
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Create(OperacionesFacturaInsertDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Delete(int idOperacionFactura, string usuario)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"OperacionesFactura/delete?IdOperacionesFacturas={idOperacionFactura}&UsuarioActualizacion={usuario}");
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<int>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> Editar(OperacionesFacturaEditDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/edit", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<OperacionesFacturaListDto>> GetInvoiceByNumber(int IdGirador, int IdAdquiriente, string NroFactura)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var response = await client.GetAsync($"OperacionesFactura/GetInvoiceByNumber/{IdGirador}/{IdAdquiriente}/{NroFactura}");
                var json = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<ResponseData<OperacionesFacturaListDto>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<List<OperacionesFacturaListDto>>> GetAllListFacturaByIdOperacionesFactura(int id)
        {
            try
            {
                OperacionesFacturaListDto operacionesFacturas = new OperacionesFacturaListDto();
                operacionesFacturas.nIdOperacionesFacturas = id;

                var client = _proxyHttpClient.GetHttp();
                client.Timeout = TimeSpan.FromMinutes(15);

                var us = JsonConvert.SerializeObject(operacionesFacturas);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/consultar-factura", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ResponseData<List<OperacionesFacturaListDto>>>(json);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}