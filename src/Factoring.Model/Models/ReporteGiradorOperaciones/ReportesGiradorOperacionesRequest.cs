
namespace Factoring.Model.Models.ReporteGiradorOperaciones
{
    public class ReportesGiradorOperacionesRequest
    {
        public int Pageno { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
        public string cNombreGirador { get; set; }
        public int nNrOperacion { get; set; }
        public string nNroFactura { get; set; }
        public string dFechaDesembolsoIni { get; set; }
        public string dFechaDesembolsoFin { get; set; }
        public int nEstado { get; set; }
    }

    public class ReportesDesembolsoCobranzaRequest
    {
        public int Pageno { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
        public string cNomAdquirente { get; set; }
        public string cNroDocumento { get; set; }
        public int nIdFactura { get; set; }
        public string cNroSerieFactura { get; set; }
        public int nEstado { get; set; }
        public string cNomGirador { get; set; }
    }

    public class ReportesCobranzaRequest
    {
        public int Pageno { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
        public string cNomAdquirente { get; set; }
        public string cNroDocumento { get; set; }
        public int nIdFactura { get; set; }
        public string cNroSerieFactura { get; set; }
        public int nEstado { get; set; }
        public string cNomGirador { get; set; }
    }

    public class ReportesDesembolsoRequest
    {
        public int Pageno { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
        public string cNomAdquirente { get; set; }
        public string cNroDocumento { get; set; }
        public int nIdFactura { get; set; }
        public string cNroSerieFactura { get; set; }
        public int nEstado { get; set; }
        public string cNomGirador { get; set; }
    }
}
