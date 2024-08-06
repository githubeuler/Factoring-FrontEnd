namespace Factoring.Model.Models.Fondeador
{
    public class DocumentoFondeadorRegistroDto
    {
        public string RutaDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public string UsuarioCreador { get; set; }
        public int IdFondeador { get; set; }
    }
}
