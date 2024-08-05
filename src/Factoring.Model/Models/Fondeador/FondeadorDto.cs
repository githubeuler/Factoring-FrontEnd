namespace Factoring.Model.Models.Fondeador
{
    public class FondedorResponseDatatableDto
    {
        public int iIdFondeador { get; set; }
        public string cNroDocumento { get; set; }
        public string cNombreFondeador { get; set; }
        public string dFecRegistro { get; set; }
        public string iEstado { get; set; }
        public string NombreEstado { get; set; }
        public int TotalRecords { get; set; }
    }
    public class FondeadorRequestDatatableDto
    {
        public int Pageno { get; set; }
        public string FilterDoi { get; set; }
        public string FilterRazon { get; set; }
        public string FilterFecCrea { get; set; }
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
        public int iIdFondeador { get; set; }
        public int iTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string cNroDocumento { get; set; }
        public string cNombreFondeador { get; set; }


        public string cFormatoUbigeo { get; set; }
        public int iEstado { get; set; }
        public string NombreEstado { get; set; }
        public List<string> FormatoUbigeoPais { get; set; }
    }
    public class FondeadorUpdateRequestDto
    {
        public int IdFondeador { get; set; }
        public int TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string NombreFondeador { get; set; }
        public string UsuarioActualizacion { get; set; }
    }
}
