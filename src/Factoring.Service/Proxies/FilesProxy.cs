using Factoring.Model.Models.OperacionesFactura;
using Factoring.Model;
using Factoring.Service.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Factoring.Service.Proxies
{
    public interface IFilesProxy
    {
        Task<ResponseData<string>> UploadFile(IFormFile attachment, string filename, string NombreSeccion);
        Task<byte[]> DownloadFile(string filename);
        Task<ResponseData<string>> UploadFiles(OperacionesFacturaSendMasivo model);
        Task<ResponseData<string>> DeleteFiles(string FilePath);
    }
    public class FilesProxy : IFilesProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public FilesProxy(
            ProxyHttpClient proxyHttpClient,
            IConfiguration configuration
        )
        {
            _configuration = configuration;
            _proxyHttpClient = proxyHttpClient;
        }

        public async Task<ResponseData<string>> UploadFiles(OperacionesFacturaSendMasivo model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                foreach (var item in model.Files)
                {
                    byte[] data;
                    //byte[] data = new byte[1000000];
                    using (var br = new BinaryReader(item.OpenReadStream()))
                        data = br.ReadBytes((int)item.OpenReadStream().Length);

                    ByteArrayContent bytes = new ByteArrayContent(data);
                    multiContent.Add(bytes, "Files", item.FileName);
                }
                multiContent.Add(new StringContent(model.UsuarioCreador), "UsuarioCreador");

                var fac = JsonConvert.SerializeObject(model.Facturas);

                multiContent.Add(new StringContent(fac), "FacturasStr");


                var result = await client.PostAsync("OperacionesFactura/masivo-facturas", multiContent);
                var json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<string>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<ResponseData<string>> UploadFile(IFormFile attachment, string filename, string NombreSeccion)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                client.MaxResponseContentBufferSize = 9999999;
                //byte[] fileArray = new byte[1000000];
                byte[] data;
                using (var br = new BinaryReader(attachment.OpenReadStream()))
                    data = br.ReadBytes((int)attachment.OpenReadStream().Length);

                //if (attachment.Length > 0)
                //{
                //    using (var ms = new MemoryStream())
                //    {
                //        attachment.CopyTo(ms);
                //        fileArray = ms.ToArray();
                //    }
                //}

                ByteArrayContent bytes = new ByteArrayContent(data);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(bytes, "Attachment", attachment.FileName);
                multiContent.Add(new StringContent(filename), "Filename");
                multiContent.Add(new StringContent(NombreSeccion), "NombreSeccion");
                var result = await client.PostAsync("Files", multiContent);
                var json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<string>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> DownloadFile(string filename)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                client.MaxResponseContentBufferSize = 9999999;
                var result = await client.PostAsync($"Files/download-file?filename={System.Net.WebUtility.UrlEncode(filename)}", null);
                return await result.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<string>> DeleteFiles(string FilePath)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                client.MaxResponseContentBufferSize = 9999999;


                MultipartFormDataContent multiContent = new MultipartFormDataContent();


                multiContent.Add(new StringContent(FilePath), "FilePath");
                var result = await client.PostAsync("Files/delete-file", multiContent);
                var json = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<string>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
