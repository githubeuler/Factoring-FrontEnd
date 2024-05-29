using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

}
