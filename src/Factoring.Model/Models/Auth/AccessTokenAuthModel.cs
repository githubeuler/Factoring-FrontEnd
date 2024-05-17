using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Factoring.Model.Models.Auth
{
    public class AccessTokenAuthModel
    {
        public AccessTokenAuthModel()
        {
            Menu = new List<MenuResponse>();
        }

        public string cCodigoUsuario { get; set; }
        public string cNombreUsuario { get; set; }
        public int nIdPais { get; set; }
        public string cNombrePais { get; set; }
        public string JWToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        public List<MenuResponse> Menu { get; set; }
    }
}
