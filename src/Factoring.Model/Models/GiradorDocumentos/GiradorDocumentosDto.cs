namespace Factoring.Model.Models.GiradorDocumentos
{
    public class DocumentosGiradorListDto
    {
        public int nIdGiradorDocumento { get; set; }
        public int nIdTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string cNombreDocumento { get; set; }
        public string cRuta { get; set; }
        public DateTime dFechaCreacion { get; set; }
    }

    public class DocumentosGiradorInsertDto
    {
        public int IdTipoDocumento { get; set; }
        public string Ruta { get; set; }
        public int IdGirador { get; set; }
        public string NombreDocumento { get; set; }

    }
    public class DocumentosGiradorDeleteDto
    {
        public int IdGiradorDocumento { get; set; }
        public string UsuarioActualizacion { get; set; }

    }
}
