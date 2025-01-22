using Factoring.Model.Models.Comunes;
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
    public interface IDataProxy
    {
        //Task<List<PaisListDto>> GetAllListPais();
        Task<List<SectorListDto>> GetAllListSector();
        Task<List<GrupoListDto>> GetAllListGrupo();
        Task<List<PaisListDto>> GetAllListPais();
    }

    public class DataProxy : IDataProxy
    {
        private readonly ProxyHttpClient _proxyHttpClient;
        private readonly IConfiguration _configuration;

        public DataProxy(ProxyHttpClient proxyHttpClient, IConfiguration configuration)
        {
            _proxyHttpClient = proxyHttpClient;
            _configuration = configuration;
        }
        public async Task<List<SectorListDto>> GetAllListSector()
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Pais/get-sector?idTipo=1");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<SectorListDto>>(json);
            return data;
        }

        public async Task<List<GrupoListDto>> GetAllListGrupo()
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Pais/get-grupo?idTipo=1");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<GrupoListDto>>(json);
            return data;
        }

        public async Task<List<PaisListDto>> GetAllListPais()
        {
            var client = _proxyHttpClient.GetHttp();
            var response = await client.GetAsync($"Pais/get-pais");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<PaisListDto>>(json);
            return data;
        }
    }
}