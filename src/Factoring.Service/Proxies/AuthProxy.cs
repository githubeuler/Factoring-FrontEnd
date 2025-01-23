using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Factoring.Model;
using Factoring.Model.Models.Auth;
using Factoring.Service.Common;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Factoring.Model.Models.Usuario;

namespace Factoring.Service.Proxies
{
    public interface IAuthProxy
    {
        Task<ResponseData<AccessTokenAuthModel>> Authenticate(LoginAuthModel model);
        Task<ResponseData<AccessTokenAuthModel>> ChangePassword(ChangeAuthModel model);
        Task<ResponseData<int>> ResetPassword(ResetPasswordModel model);

    }

    public class AuthProxy : IAuthProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public AuthProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }
        public async Task<ResponseData<AccessTokenAuthModel>> ChangePassword(ChangeAuthModel model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Account/change-password", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<AccessTokenAuthModel>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResponseData<AccessTokenAuthModel>> Authenticate(LoginAuthModel model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Account", requestContent);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ResponseData<AccessTokenAuthModel>>(json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ResponseData<int>> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var client = _proxyHttpClient.GetHttp();
                var us = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(us, Encoding.UTF8, _configuration["ContentTypeRequest"].ToString());
                var response = await client.PostAsync("Account/reset-password", requestContent);
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
