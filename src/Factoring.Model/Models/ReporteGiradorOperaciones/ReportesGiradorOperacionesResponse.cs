namespace Factoring.Model.Models.ReporteGiradorOperaciones
{
    public class ReportesGiradorOperacionesResponse
    {
        public int nNroOperacion { get; set; }
        public string Girador { get; set; }
        public string Aceptante { get; set; }
        public string NroFactura { get; set; }
        public decimal nMontoOperacion { get; set; }
        public string Estado { get; set; }
        public DateTime? dFechaDesembolso { get; set; }
        public DateTime? dFechaVencimiento { get; set; }
        public DateTime? dFechaCobranza { get; set; }
        public DateTime? dFechaCreacionOperacion { get; set; }
        public string cMoneda { get; set; }
        public decimal ImporteNetoFactura { get; set; }
        public string nPorcentajeFinanciamiento { get; set; }
        public string nTEM { get; set; }
        public string ComisionCobranza { get; set; }
        public int nNroDocumento { get; set; }
        public int nDiasAdelanto { get; set; }
        public decimal nImporteaFinanciar { get; set; }
        public string nTasaAnual { get; set; }
        public string Interes { get; set; }
        public decimal CostoComisionCobranza { get; set; }
        public decimal CostoRegistroFactura { get; set; }
        public decimal CostoElaboracionContrato { get; set; }
        public decimal nIgv { get; set; }
        public decimal? MontoaDesembolsarGirador { get; set; }
        public DateTime? dFechaAceptante { get; set; }
        public int TotalRecords { get; set; }
        public string interesMoratorio { get; set; }
        public decimal devolucionTotalGirador { get; set; }
        public decimal planCobertura { get; set; }
        public decimal retencion { get; set; }
        public decimal devolucionEstimadaGirador { get; set; }
        public string interesCompuestoGirador { get; set; }
        public decimal desenbolsoProtectum { get; set; }
        public decimal desenbolsoPalante { get; set; }
        public string Adquirente { get; set; }
        public decimal nComisionEstruccturacionTotal { get; set; }
        public DateTime? dFechaPagoNegociado { get; set; }  //  <OAV - 11/01/2023>
        public DateTime? dFechaEmision { get; set; }        //  <OAV - 11/01/2023>
        public int IdGirador { get; set; }                  //  <OAV - 12/01/2023>
        public int IdAdquiriente { get; set; }              //  <OAV - 12/01/2023>
        public int IdTipoMoneda { get; set; }               //  <OAV - 12/01/2023>
        public string NombreDocumentoXML { get; set; }      //  <OAV - 12/01/2023>
        public int IdCategoria { get; set; }                //  <OAV - 25/01/2023>
    }

    public class ReportesDesembolsoCobranzaResponse
    {
        public int nIdOperaciones { get; set; }
        public int nIdOperacionesFactura { get; set; }
        public int nNroOperacion { get; set; }
        public string Adquirente { get; set; }
        public string cNroDocumento { get; set; }
        public decimal nMontoOperacion { get; set; }
        public decimal nIgv { get; set; }
        public decimal MontoaDesembolsarGirador { get; set; }
        public decimal nPorcentajeFinanciamiento { get; set; }
        public string cSerieFactura { get; set; }
        public decimal nMontoFactura { get; set; }
        public decimal nMontoNoFinanciado { get; set; }
        public decimal nMora { get; set; }
        public decimal nInteres { get; set; }
        public string cEstado { get; set; }
        public string nEstadoFactura { get; set; }
        public string Girador { get; set; }
        public int nIdGirador { get; set; }
        public int nEstadoGirador { get; set; }
        public int TotalRecords { get; set; }

    }

    public class ReportesCobranzaResponse
    {
        public int nIdOperaciones { get; set; }
        public int nIdOperacionesFactura { get; set; }
        public int nNroOperacion { get; set; }
        public string Adquirente { get; set; }
        public string cNroDocumento { get; set; }
        public decimal nMontoOperacion { get; set; }
        public decimal nIgv { get; set; }
        public decimal MontoaDesembolsarGirador { get; set; }
        public decimal nPorcentajeFinanciamiento { get; set; }
        public string cSerieFactura { get; set; }
        public decimal nMontoFactura { get; set; }
        public decimal nMontoNoFinanciado { get; set; }
        public decimal nMora { get; set; }
        public decimal nInteres { get; set; }
        public string cEstado { get; set; }
        public string nEstadoFactura { get; set; }
        public string Girador { get; set; }
        public int nIdGirador { get; set; }
        public int nEstadoGirador { get; set; }
        public int TotalRecords { get; set; }

    }
    public class ReportesDesembolsoResponse
    {
        public int nIdOperaciones { get; set; }
        public int nIdOperacionesFactura { get; set; }
        public int nNroOperacion { get; set; }
        public string Adquirente { get; set; }
        public string cNroDocumento { get; set; }
        public decimal nMontoOperacion { get; set; }
        public decimal nIgv { get; set; }
        public decimal MontoaDesembolsarGirador { get; set; }
        public decimal nPorcentajeFinanciamiento { get; set; }
        public string cSerieFactura { get; set; }
        public decimal nMontoFactura { get; set; }
        public decimal nMontoNoFinanciado { get; set; }
        public decimal nMora { get; set; }
        public decimal nInteres { get; set; }
        public string cEstado { get; set; }
        public string nEstadoFactura { get; set; }
        public string Girador { get; set; }
        public int nIdGirador { get; set; }
        public int nEstadoGirador { get; set; }
        public int TotalRecords { get; set; }
        public int nIdModalidad { get; set; }
        public string cModalidad { get; set; }
        public int nIdFondeador { get; set; }
        public string cFondeador { get; set; }

    }

    public class DesembolsoDTO
    {
        public int nIdOperaciones { get; set; }
        public int nIdOperacionesFactura { get; set; }
        public int nNroOperacion { get; set; }
        public string Adquirente { get; set; }
        public string cNroDocumento { get; set; }
        public decimal nMontoOperacion { get; set; }
        public decimal nIgv { get; set; }
        public decimal MontoaDesembolsarGirador { get; set; }
        public decimal nPorcentajeFinanciamiento { get; set; }
        public string cSerieFactura { get; set; }
        public decimal nMontoFactura { get; set; }
        public decimal nMontoNoFinanciado { get; set; }
        public decimal nMora { get; set; }
        public decimal nInteres { get; set; }
        public string cEstado { get; set; }
        public string nEstado { get; set; }
        public int TotalRecords { get; set; }
        public string Girador { get; set; }
        public int nIdGirador { get; set; }
        public int nEstadoGirador { get; set; }
        public int nIdModalidad { get; set; }
        public string cModalidad { get; set; }
        public int nIdFondeador { get; set; }
        public string cFondeador { get; set; }
        public int? nIdCatalogoSubEstado { get; set; }
        public string cDocumento { get; set; }

    }
}
