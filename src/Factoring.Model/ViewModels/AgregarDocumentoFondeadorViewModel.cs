using Microsoft.AspNetCore.Http;

namespace Factoring.Model.ViewModels
{
    public class AgregarDocumentoFondeadorViewModel
    {
        public string DOIDOCUMENTO { get; set; }
        public int IdFondeadorCabeceraDocumento { get; set; }
        public IFormFile fileDocumento { get; set; }
        public int TipoDocumento { get; set; }
        public string TipoDocumentoDesc { get; set; }
    }
}
