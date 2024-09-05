namespace Factoring.Model.Models.Fondeo
{
    public class FondeoResponseDatatableDto
    {
        public int nIdEstadoOperacion { get; set; }
        public int nIdFondeadorFactura { get; set; }
        public int nIdOperaciones { get; set; }
        public string cNroOperacion { get; set; }
        public int nIdGirador { get; set; }
        public string cGirador { get; set; }
        public int nNumeroAsignaciones { get; set; }
        public string cNumeroAsignacion { get; set; }
        public int nIdFondeador { get; set; }
        public string? cFondeadorAsignado { get; set; }
        public string cFechaAsignacion { get; set; }
        public int nEstadoFondeo { get; set; }
        public string cEstadoFondeo { get; set; }
        public int nIdMoneda { get; set; }
        public string cMoneda { get; set; }
        public int nIdTipoFondeo { get; set; }
        public int TotalRecords { get; set; }

        public decimal nPorcentajeCapitalFinanciado { get; set; }
        public decimal nPorcentajeComisionFactura { get; set; }
        public decimal nPorcentajeSpread { get; set; }
        public decimal nPorcentajeTasaAnualFondeo { get; set; }
        public decimal nPorcentajeTasaMensual { get; set; }
        public decimal nPorcentajeTasaMoraFondeo { get; set; }
        public string? dFechaDesembolsoFondeador { get; set; }
        public string? dFechaCobranzaFondeador { get; set; }

        public string nMontoADesembolsarFondeador { get; set; }
        public decimal nIgv { get; set; }

    }
    public class FondeoRequestDatatableDto
    {
        public int Pageno { get; set; }
        public string FilterNroOperacion { get; set; }
        public string FilterFondeadorAsignado { get; set; }
        public string FilterGirador { get; set; }
        public string FilterFechaRegistro { get; set; }
        public int FilterEstadoFondeo { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
        public int IdEstado { get; set; }
    }
    public class FondeoInsertDto
    {
        public int IdFondeadorFactura { get; set; }
        public string? UsuarioCreacion { get; set; }

    }

    public class FondeoUpdateDto
    {
        public int IdFondeadorFactura { get; set; }
        public int IdOperacion { get; set; }

        public int IdFondeador { get; set; }


        public int IdTipoProducto { get; set; }
        public string? FechaDesembolso { get; set; }
        public string? FechaCobranza { get; set; }
        public decimal PorTasaMensual { get; set; }
        public decimal PorComisionFactura { get; set; }
        public decimal PorSpread { get; set; }

        public decimal PorCapitalFinanciado { get; set; }
        public decimal PorTasaAnualFondeo { get; set; }
        public decimal PorTasaMoraFondeo { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public decimal Igv { get; set; }
    }


    public class FondeoUpdateStateDto
    {
        public int IdFondeadorFactura { get; set; }
        public int IdEstadoFondeo { get; set; }
        public string? Comentario { get; set; }
        public string? UsuarioModificacion { get; set; }
    }

}
