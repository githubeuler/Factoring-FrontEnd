﻿using Microsoft.AspNetCore.Http;

namespace Factoring.Model.Models.OperacionesFactura
{
    public class OperacionesFacturaListDto
    {
        public int nIdOperacionesFacturas { get; set; }
        public string nroOperacion { get; set; }
        public string cNroDocumento { get; set; }
        public decimal nMontoOperacion { get; set; }
        public decimal nMonto { get; set; }
        public decimal nMontoTotal { get; set; }
        public DateTime dFechaEmision { get; set; }
        public DateTime dFechaVencimiento { get; set; }
        public DateTime dFechaPagoNegociado { get; set; }
        public string cNombreDocumentoXML { get; set; }
        public string cRutaDocumentoXML { get; set; }
        public string cNombreDocumentoPDF { get; set; }
        public string cRutaDocumentoPDF { get; set; }
        public int nEstado { get; set; }
        public string NombreEstado { get; set; }
        public string cFormatoDocumento { get; set; }
        public string cIdEstadoFacturaHistorico { get; set; }
        public string cFactura { get; set; }
        public DateTime? dFechaRegistro { get; set; }
        public int TotalRecords { get; set; }
    }

    public class OperacionesFacturaLoteCavali
    {
        public int FlagRegisterProcess { get; set; }
        public int FlagAcvProcess { get; set; }
        public int FlagTransferProcess { get; set; }
        public int CodParticipante { get; set; }
        public string UsuarioCreador { get; set; }
        public List<int> Invoices { get; set; }
        public int InvoicesFactura { get; set; }
        public int nCategoriaFondeador { get; set; }
        public int? nCantidadAsignacion { get; set; }
        public int? nIdGiradorPlus { get; set; }
        public bool bFondeadorPlus { get; set; }
        public bool bSegundoFlagDiferente { get; set; }
    }
    public class OperacionesFacturaValidaAsignacion
    {
        public int Id { get; set; }
        public List<int> nLstIdFacturas { get; set; }
        public List<string> sLstIdFacturas { get; set; }
        public string sIdFacturas { get; set; }
        public int nIdOpcionOperacion { get; set; }
    }


    public class RequestOperacionesFacturaValidacion
    {
        public List<int>? nLstIdFacturas { get; set; }
        public int nIdOpcionOperacion { get; set; }
        public int nTipo { get; set; }
    }

    public class FacturasGetRegistro
    {
        public int nIdOperaciones { get; set; }
        public int nIdOperacionesFacturas { get; set; }
        public int nEstadoFactura { get; set; }
        public int nIdFondeador { get; set; }
        public string? dFechaDesembolsoFondeador { get; set; }
        public string? dFechaDesembolso { get; set; }
        public int nIdCategoria { get; set; }
        public int nCantOperacion { get; set; }
        public int nCantFacturasRecepcionada { get; set; }
        public int nCantFacturasEvaluada { get; set; }
        public bool bFondeadorPlus { get; set; }
        public int nNumeroAsignaciones { get; set; }
        public int nIdEstadoOperacionFactura { get; set; }
        public int nCodFondeadorPlus { get; set; }
        public int nIdFactura { get; set; }
        public string? dFechacAsignacion { get; set; }
    }

    public class FacturasGetCabeceraRegistro
    {
        public List<FacturasGetRegistro> listaFacturas { get; set; }
        public int? nActivarTransferencia { get; set; }
    }
    public class FondeadorGetPermisosSGC
    {
        public int iIdFondeador { get; set; }
        public int iMetodoFondeo { get; set; }
        public int transferencia { get; set; }
        public int traspaso { get; set; }
        public string cNombreFondeador { get; set; }
        public int cantidadInversionistas { get; set; }

    }
    public class OperacionesFacturaRemoveCavali
    {
        public int IdMotivo { get; set; }
        public DateTime fechaPago { get; set; }
        public string UsuarioCreador { get; set; }
        public List<int> Invoices { get; set; }
        public List<string> sInvoices { get; set; }
        public int InvoicesFactura { get; set; }
    }

    public class FondeadorGetPermisos
    {
        public int iIdFondeador { get; set; }
        public int iMetodoFondeo { get; set; }
        public int transferencia { get; set; }
        public int traspaso { get; set; }
        public string cNombreFondeador { get; set; }
        public int cantidadInversionistas { get; set; }

    }

    public class FondeadorGetPermisosCabecera
    {
        public int? nActivarTransferencia { get; set; }
        public List<FondeadorGetPermisos> listaFondeador { get; set; }

    }
    public class OperacionesFacturaSendMasivo
    {
        public List<OperacionesFacturaInsertMasivotDto> Facturas { get; set; }

        public List<IFormFile> Files { get; set; }
        public string UsuarioCreador { get; set; }

    }

    public class OperacionesFacturaInsertMasivotDto
    {

        public int IdOperaciones { get; set; }
        public string NroDocumento { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string NombreDocumentoXML { get; set; }
        public string RutaDocumentoXML { get; set; }
        public string NombreDocumentoPDF { get; set; }
        public string RutaDocumentoPDF { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaPagoNegociado { get; set; }        //  <OAV - 13/01/2023>
    }


    public class OperacionesFacturaInsertDto
    {

        public int IdOperaciones { get; set; }
        public string NroDocumento { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string NombreDocumentoXML { get; set; }
        public string RutaDocumentoXML { get; set; }
        public string NombreDocumentoPDF { get; set; }
        public string RutaDocumentoPDF { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime FechaPagoNegociado { get; set; }

    }

    public class OperacionesFacturaEditDto
    {

        public int IdOperacionesFacturas { get; set; }
        public string UsuarioActualizacion { get; set; }
        public DateTime FechaPagoNegociado { get; set; }
        public int Estado { get; set; }       //  <OAV - 30/01/2023>
    }
    public class OperacionesFacturaEditMontoDto
    {
        public int nIdOperaciones { get; set; }
        public int nIdOperacionesFacturas { get; set; }
        public string? cUsuarioActualizacion { get; set; }
        public decimal nMonto { get; set; }
    }
    public class OperacionesFacturaDeleteDto
    {
        public int IdOperacionesFacturas { get; set; }
        public string UsuarioActualizacion { get; set; }
    }

    public class OperacionesFacturaRequestDataTableDto
    {
        public int? Pageno { get; set; }
        public string? FilterNroOperacion { get; set; }
        public int? PageSize { get; set; }
        public string? Sorting { get; set; }
        public string? SortOrder { get; set; }
        public string? NroOperacion { get; set; }
        public int? Estado { get; set; }
        public string? FechaCreacion { get; set; }
        public string Usuario { get; set; }
    }


    public class OperacionesFacturaResponseDataTable
    {
        public int nIdOperacionesFacturas { get; set; }

        public string cNroDocumento { get; set; }
        public string Serie { get; set; }
        public string Estado { get; set; }
        public int EstadoId { get; set; }
        public string Numero { get; set; }
        public decimal nMonto { get; set; }
        public DateTime dFechaVencimiento { get; set; }
        public DateTime dFechaPago { get; set; }
        public string nNroOperacion { get; set; }
        public int totalRecords { get; set; }
        public string NombreEstado { get; set; }
        public string cProcessResult { get; set; }
        public DateTime dFechaPagoNegociado { get; set; }

    }

    public class DocumentosSolicitudperacionesInsertDto
    {

        public int nIdSolEvalOperaciones { get; set; }
        public int nTipoDocumento { get; set; }
        public string cNombreDocumento { get; set; }
        public string cRutaDocumento { get; set; }
        public int nEstado { get; set; }
        public string cUsuarioCreador { get; set; }
        public DateTime? dFechaCreacion { get; set; }
    }

    public class DocumentoSolicitudOperacionListDto
    {
        public int nIdDocumentoSolEvalOperacion { get; set; }
        public int IdSolEvalOperacion { get; set; }
        public int nTipoDocumento { get; set; }
        public string DesDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string cRutaDocumento { get; set; }

    }

    public class DocumentoSolicitudOperacionListIdDto
    {
        public int nIdDocumentoSolEvalOperaciones { get; set; }
        public int IdSolEvalOperacion { get; set; }
        public string cRutaDocumento { get; set; }
    }

    public class OperacionesSolicitudDeleteDto
    {
        public int nIdDocumentoSolEvalOperaciones { get; set; }
        public string UsuarioActualizacion { get; set; }
    }

    public class OperacionesCavaliListDto
    {
        public string cFactura { get; set; }
        public int nAcqResponse { get; set; }
        public string cRespuestaAceptante { get; set; }
        public DateTime cAcqResponseDate { get; set; }
        public DateTime dFechaActualizacion { get; set; }

    }

    public class OperacionesFacturaLoteMasivoCavali
    {
        public List<OperacionesFacturaListDto> Invoices { get; set; }
        public int FlagRegisterProcess { get; set; } //flagAnotadoCta
        public int FlagTransferProcess { get; set; } //FlagTrans
        public int CodParticipante { get; set; }
        public int FlagAcvProcess { get; set; }
        public string UsuarioCreador { get; set; }
        public int InvoicesFactura { get; set; }
        public int nIdModalidad { get; set; }
        public int nNroEnvio { get; set; }
    }
}


