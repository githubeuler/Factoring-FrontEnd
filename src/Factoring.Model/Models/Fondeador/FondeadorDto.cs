namespace Factoring.Model.Models.Fondeador
{
    public class FondedorResponseDatatableDto
    {
        public int nIdFondeador { get; set; }
        public string cNroDocumento { get; set; }
        public string cNombreFondeador { get; set; }
        public string dFecRegistro { get; set; }
        public string nEstado { get; set; }
        public string NombreEstado { get; set; }
        public int TotalRecords { get; set; }
    }
    public class FondeadorRequestDatatableDto
    {
        public int Pageno { get; set; }
        public string FilterDoi { get; set; }
        public string FilterRazon { get; set; }
        public string FilterFecCrea { get; set; }
        public string Usuario { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
        public int IdEstado { get; set; }
    }
    public class FondeadorRegistroRequestDto
    {
        public int IdDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string NombreFondeador { get; set; }
        public string UsuarioCreador { get; set; }
    }
    public class FondeadorSingleDto
    {
        public int nIdFondeador { get; set; }
        public int nTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string cNroDocumento { get; set; }
        public string cNombreFondeador { get; set; }


        public string cFormatoUbigeo { get; set; }
        public int iEstado { get; set; }
        public string NombreEstado { get; set; }
        public List<string> FormatoUbigeoPais { get; set; }

        public int nIdProducto { get; set; }
        public int nIdInteresCalculado { get; set; }
        public int nIdTipoFondeo { get; set; }
        public string cDistribucionFondeador { get; set; }
    }
    public class FondeadorUpdateRequestDto
    {
        public int IdFondeador { get; set; }
        public int TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string NombreFondeador { get; set; }
        public string UsuarioActualizacion { get; set; }

        public int IdProducto { get; set; }
        public int IdInteresCalculado { get; set; }
        public int IdTipoFondeo { get; set; }
        public string DistribucionFondeador { get; set; }

    }
}
