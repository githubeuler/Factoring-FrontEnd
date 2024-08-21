using Factoring.Model.Models.OperacionesFactura;
using Factoring.Model;
using Factoring.Service.Common;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Text;
using Factoring.Model.Models.Cavali;

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
        Task<ResponseData<int>> EditarMonto(OperacionesFacturaEditMontoDto model);
        Task<ResponseData<List<OperacionesFacturaResponseDataTable>>> GetBandejaFacturas(OperacionesFacturaRequestDataTableDto model);
        Task<ResponseData<int>> ValidarEstadoFactura(OperacionesFacturaListDto OperacionFactura);
        Task<ResponseData<ResponseCavaliInvoice4012>> OperacionCavaliInvoicesSend4012(OperacionesFacturaLoteCavali model);
        Task<ResponseData<ResponseCavaliRemove4008>> OperacionCavaliRemove4008(OperacionesFacturaRemoveCavali model);
        Task<ResponseData<FondeadorGetPermisosCabecera>> ObtenerAsignaciones(OperacionesFacturaValidaAsignacion model);
        Task<ResponseData<ResponseCavaliRedeem4007>> OperacionCavaliRedeem4007(OperacionesFacturaRemoveCavali model);
        Task<ResponseData<FacturasGetCabeceraRegistro>> ObtenerValidacionAsignaciones(RequestOperacionesFacturaValidacion model);
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

        public async Task<ResponseData<int>> EditarMonto(OperacionesFacturaEditMontoDto model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/edit-monto", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<List<OperacionesFacturaResponseDataTable>>> GetBandejaFacturas(OperacionesFacturaRequestDataTableDto model)
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"OperacionesFactura/bandeja-factura?Pageno={model.Pageno}&PageSize={model.PageSize}&FechaCreacion={model.FechaCreacion}" +
                $"&nEstado={model.Estado}&Sorting={model.Sorting}&SortOrder={model.SortOrder}&FilterNroOperacion={model.FilterNroOperacion}");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ResponseData<List<OperacionesFacturaResponseDataTable>>>(json);
            return data;
        }

        public async Task<ResponseData<int>> ValidarEstadoFactura(OperacionesFacturaListDto OperacionFactura)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(OperacionFactura);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/ValidateEstadoOperacionFactura", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<int>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<ResponseCavaliInvoice4012>> OperacionCavaliInvoicesSend4012(OperacionesFacturaLoteCavali model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                client.Timeout = TimeSpan.FromMinutes(15);

                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/invoices-cavaly-send", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<ResponseCavaliInvoice4012>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<ResponseCavaliRemove4008>> OperacionCavaliRemove4008(OperacionesFacturaRemoveCavali model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                client.Timeout = TimeSpan.FromMinutes(15);

                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/remove-cavaly-send", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<ResponseCavaliRemove4008>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<ResponseCavaliRedeem4007>> OperacionCavaliRedeem4007(OperacionesFacturaRemoveCavali model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                client.Timeout = TimeSpan.FromMinutes(15);

                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/redeem-cavaly-send", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<ResponseCavaliRedeem4007>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<FondeadorGetPermisosCabecera>> ObtenerAsignaciones(OperacionesFacturaValidaAsignacion model)
        {
            try
            {
                //Task<ResponseData<List<FondeadorGetPermisos>>> 
                var client = _proxyHttpClient.GetHttp();
                client.Timeout = TimeSpan.FromMinutes(15);

                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/validate-asignacion-inversionista", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<FondeadorGetPermisosCabecera>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       public async Task<ResponseData<FacturasGetCabeceraRegistro>> ObtenerValidacionAsignaciones(RequestOperacionesFacturaValidacion model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                client.Timeout = TimeSpan.FromMinutes(15);
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("OperacionesFactura/validate-envio-cavali", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<FacturasGetCabeceraRegistro>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}