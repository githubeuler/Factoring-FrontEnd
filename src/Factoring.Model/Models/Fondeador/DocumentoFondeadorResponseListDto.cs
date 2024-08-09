namespace Factoring.Model.Models.Fondeador
{
    public class DocumentoFondeadorResponseListDto
    {
        public int nIdFondeadorDocumento { get; set; }
        public int nIdFondeadorTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string cNombreDocumento { get; set; }
        public string cRutaDocumento { get; set; }
        public DateTime dFechaCreacion { get; set; }
    }
}
