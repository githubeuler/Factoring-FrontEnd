
namespace Factoring.Model.Models.Operaciones
{
    public class OperacionesRequestDataTableDto
    {
        public int Pageno { get; set; }
        public string? FilterNroOperacion { get; set; }
        public string? FilterRazonGirador { get; set; }
        public string? FilterRazonAdquiriente { get; set; }
        public string? FilterFecCrea { get; set; }
        public string? Estado { get; set; }
        public int PageSize { get; set; }
        public string? Sorting { get; set; }
        public string ? SortOrder { get; set; }
    }
    public class OperacionesResponseDataTable
    {
        public int nIdOperaciones { get; set; }
        public string nNroOperacion { get; set; }
        public string cRazonSocialGirador { get; set; }
        public string cRazonSocialAdquiriente { get; set; }
        public DateTime dFechaCreacion { get; set; }
        public string nEstado { get; set; }
        public string NombreEstado { get; set; }
        public int nAprobadoRiesgo { get; set; }
        public int TotalRecords { get; set; }
    }

    public class OperacionesUpdateDto
    {
        public int IdOperaciones { get; set; }
        public int IdGirador { get; set; }
        public int IdAdquiriente { get; set; }
        //public int IdInversionista { get; set; }
        public int IdTipoMoneda { get; set; }
        public int IdGiradorDireccion { get; set; }
        public int IdAdquirienteDireccion { get; set; }
        public decimal TEM { get; set; }
        public decimal PorcentajeFinanciamiento { get; set; }
        public decimal PorcentajeRetencion { get; set; }
        public decimal MontoOperacion { get; set; }
        public decimal DescContrato { get; set; }
        public decimal DescFactura { get; set; }
        public decimal DescCobranza { get; set; }
        public string UsuarioActualizacion { get; set; }
        public decimal InteresMoratorio { get; set; }

        //*************Ini-09-01-2023-RCARRILLO******//
        public int IdCategoria { get; set; }
        public string SustentoComercial { get; set; }
        public string MotivoTransaccion { get; set; }
        public int Plazo { get; set; }
        public int IdSolEvalOperacion { get; set; }
        //*************Fin-09-01-2023-RCARRILLO******//
    }

    public class EstadoOperacionUpdateDto
    {
        public int nIdOperaciones { get; set; }
        public int IdEstadoGiradorEvaluacion { get; set; }
        public int IdEstadoAquirienteEvaluacion { get; set; }
        public int IdEstadoOperacionEvaluacion { get; set; }
        public string UsuarioActualizacion { get; set; }
    }

    public class OperacionesComentariosAllDto
    {
        public int nIdOperaciones { get; set; }
        public DateTime dFechaCreacion { get; set; }
        public int nIdEvaluacionOperaciones { get; set; }
        public int nIdCatalogoEstado { get; set; }
        public string NombreCatalogoEstado { get; set; }
        public string ComentarioEvaluacion { get; set; }
    }

    public class OperacionSingleResponseDto
    {
        public int nIdOperaciones { get; set; }
        public string nNroOperacion { get; set; }
        public int nEstado { get; set; }
        public string NombreEstado { get; set; }
        public int nIdGirador { get; set; }
        public string cRazonSocialGirador { get; set; }
        public string cRegUnicoEmpresaGirador { get; set; }

        public int nIdAdquiriente { get; set; }
        public string cRazonSocialAdquiriente { get; set; }
        public string cRegUnicoEmpresaAdquiriente { get; set; }

        public int nIdInversionista { get; set; }
        public string cNombreInversionista { get; set; }

        public int nIdGiradorDireccion { get; set; }
        public string DireccionGirador { get; set; }
        public decimal nPorcentajeRetencion { get; set; }

        public int nIdAdquirienteDireccion { get; set; }
        public string DireccionAdquiriente { get; set; }

        public decimal nTEM { get; set; }
        public decimal nPorcentajeFinanciamiento { get; set; }
        public decimal nMontoOperacion { get; set; }
        public decimal nDescContrato { get; set; }
        public decimal nDescFactura { get; set; }
        public decimal nDescCobranza { get; set; }
        public int nIdTipoMoneda { get; set; }
        public string Moneda { get; set; }
        public decimal InteresMoratorio { get; set; }
        public List<string> SerieDocumentoPais { get; set; }
        public int IdCategoria { get; set; }
        public string SustentoComercial { get; set; }
        public string MotivoTransaccion { get; set; }
        public int Plazo { get; set; }
        public int IdSolEvalOperacion { get; set; }
        public string cDesCategoria { get; set; }
        public int nIdModalidad { get; set; }
        public string cModalidad { get; set; }
        public int nIdFondeador { get; set; }
        public string cFondeador { get; set; }
        public string dFechaTransferenciaCavali { get; set; }
        public string dFechaDesembolso { get; set; }
        public string dFechaCobranza { get; set; }

    }
    public class OperacionesInsertDto
    {
        public int IdGirador { get; set; }
        public int IdAdquiriente { get; set; }
        //public int IdInversionista { get; set; }
        public int IdTipoMoneda { get; set; }
        public int IdGiradorDireccion { get; set; }
        public int IdAdquirienteDireccion { get; set; }
        public decimal TEM { get; set; }
        public decimal PorcentajeFinanciamiento { get; set; }
        public decimal PorcentajeRetencion { get; set; }
        public decimal MontoOperacion { get; set; }
        public decimal DescContrato { get; set; }
        public decimal DescFactura { get; set; }
        public decimal DescCobranza { get; set; }
        public string UsuarioCreador { get; set; }
        public decimal InteresMoratorio { get; set; }
        public int IdCategoria { get; set; }
        public string SustentoComercial { get; set; }
        public string MotivoTransaccion { get; set; }
        public int Plazo { get; set; }
        public int CantidadFactura { get; set; }

        public int IdSolEvalOperacion { get; set; }
    }
  
    public class OperacionesGetByIdDto
    {
        public int nIdOperaciones { get; set; }
        public string nNroOperacion { get; set; }
        public int nEstado { get; set; }
        public string NombreEstado { get; set; }
        public int nIdGirador { get; set; }
        public int nIdAdquiriente { get; set; }
        public int nIdInversionista { get; set; }
        public int nIdGiradorDireccion { get; set; }
        public int nIdAdquirienteDireccion { get; set; }
        public decimal nTEM { get; set; }
        public decimal nPorcentajeFinanciamiento { get; set; }
        public decimal nMontoOperacion { get; set; }
        public decimal nDescContrato { get; set; }
        public decimal nDescFactura { get; set; }
        public decimal nDescCobranza { get; set; }
        public int nIdTipoMoneda { get; set; }
    }

    public class OperacionesDeleteDto
    {
        public int IdOperaciones { get; set; }
        public string UsuarioActualizacion { get; set; }
    }

    public class OperacionCavali4012Request
    {
        public string ProcessNumber { get; set; }
        public int FlagRegisterProcess { get; set; }
        public int FlagAcvProcess { get; set; }
        public int FlagTransferProcess { get; set; }
    }

    public class OperacionCavali4012Response
    {
        public string ProcessNumber { get; set; }
        public int FlagRegisterProcess { get; set; }
        public int FlagAcvProcess { get; set; }
        public int FlagTransferProcess { get; set; }
        public int CodParticipante { get; set; }
    }

    public class ProcessDetail
    {
        public string processNumber { get; set; }
        public int flagRegisterProcess { get; set; }
        public int flagAcvProcess { get; set; }
        public int flagTransferProcess { get; set; }
        public int invoiceCount { get; set; }
    }

    public class Payment
    {
        public int number { get; set; }
        public double netAmount { get; set; }
        public string paymentDate { get; set; }
    }

    public class PaymentDetail
    {
        public PaymentDetail()
        {
            payment = new List<Payment>();
        }

        public List<Payment> payment { get; set; }
    }

    public class Invoice
    {
        public Invoice()
        {
            paymentDetail = new PaymentDetail();
        }

        public string fileName { get; set; }
        public string fileXml { get; set; }
        public int holderCode { get; set; }
        public int participantDestination { get; set; }
        public string expirationDate { get; set; }
        public string department { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string addressSupplier { get; set; }
        public string acqDepartment { get; set; }
        public string acqProvince { get; set; }
        public string acqDistrict { get; set; }
        public string addressAcquirer { get; set; }
        public int typePayment { get; set; }
        public int numberQuota { get; set; }
        public string deliverDateAcq { get; set; }
        public string acceptedDate { get; set; }
        public string paymentDate { get; set; }
        public int netAmount { get; set; }
        public string otherOne { get; set; }
        public string otherTwo { get; set; }
        public PaymentDetail paymentDetail { get; set; }
        public int constancyEmission { get; set; }
        public string additionalFieldOne { get; set; }
        public string additionalFieldTwo { get; set; }
    }

    public class InvoiceDetail
    {
        public InvoiceDetail()
        {
            invoice = new List<Invoice>();
        }

        public List<Invoice> invoice { get; set; }
    }

    public class InvoiceRoot
    {
        public InvoiceRoot()
        {
            processDetail = new ProcessDetail();
            invoiceDetail = new InvoiceDetail();
        }

        public ProcessDetail processDetail { get; set; }
        public InvoiceDetail invoiceDetail { get; set; }
    }

    public class InvoicePlanilla
    {
        public int actionType { get; set; }
        public int stateType { get; set; }
        public int dateType { get; set; }
        public int processNumber { get; set; }
    }
    public class OperacionesReturnMassive
    {
        public int IdOperacion { get; set; }
    }

    public class MasivoOperacionDto
    {
        public MasivoOperacionDto()
        {
            Operaciones = new List<OperacionesInsertMasiveDto>();
        }
        public List<OperacionesInsertMasiveDto> Operaciones { get; set; }

        public string UsuarioCreador { get; set; }
    }

    public class OperacionesInsertMasiveDto
    {
        public string RucGirador { get; set; }
        public string RucAdquiriente { get; set; }
        //public int IdInversionista { get; set; }
        public string DOIInversionista { get; set; }
        //public int IdTipoMoneda { get; set; }
        public string Moneda { get; set; }
        public decimal TEM { get; set; }
        public decimal PorcentajeFinanciamiento { get; set; }
        public decimal PorcentajeRetencion { get; set; }
        public decimal MontoOperacion { get; set; }
        public decimal DescContrato { get; set; }
        public decimal DescFactura { get; set; }
        public decimal DescCobranza { get; set; }
    }
}
