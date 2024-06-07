using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Factoring.Model.ViewModels
{
    public class OperacionViewModel
    {
        public string NroOperacion { get; set; }
        public string RazonGirador { get; set; }
        public string RazonAdquiriente { get; set; }
        public string FechaCreacion { get; set; }
        public string Estado { get; set; }

        public int nIdOperacionEval { get; set; }

        public int nIdEstadoEvaluacion { get; set; }
        public string? cComentario { get; set; }
    }

    public class OperacionSingleViewModel
    {
        public int IdOperacion { get; set; }
        public int IdGirador { get; set; }
        public int IdAdquiriente { get; set; }
        public int IdInversionista { get; set; }
        public int IdGiradorDireccion { get; set; }
        public int IdAdquirienteDireccion { get; set; }
        public int IdCategoria { get; set; }    //  OAV - 02/01/2023
        public decimal TEM { get; set; }
        public decimal PorcentajeFinanciamiento { get; set; }
        public decimal MontoOperacion { get; set; }
        public decimal DescContrato { get; set; }
        public decimal DescFactura { get; set; }
        public decimal DescCobranza { get; set; }
        public int IdTipoMoneda { get; set; }
        public decimal PorcentajeRetencion { get; set; }
        public int Estado { get; set; }
        public string UsuarioCreador { get; set; }
        public string Moneda { get; set; }
        public string UsuarioActualizacion { get; set; }
        public string RazonSocialGirador { get; set; }
        public string RegUnicoEmpresaGirador { get; set; }
        public string RazonSocialAdquiriente { get; set; }
        public string RegUnicoEmpresaAdquiriente { get; set; }
        public string NombreInversionista { get; set; }
        public string DireccionGirador { get; set; }
        public string DireccionAdquiriente { get; set; }
        public string CategoriaGirador { get; set; }        //  OAV - 02/01/2023
        public string NombreEstado { get; set; }
        public decimal InteresMoratorio { get; set; }
        public string Categoria { get; set; } //  EATB - 25/01/2023
        public string MotivoTransaccion { get; set; }//  EATB - 25/01/2023
        public string SustentoComercial { get; set; }//  EATB - 25/01/2023
        public int Plazo { get; set; }//  EATB - 25/01/2023
        public string cDesCategoria { get; set; }// RCR-2023-01-26

        public int IdTipoDocumentoSUNAT { get; set; }
        public string nNroOperacion { get; set; }

    }

    public class OperacionCreateModel
    {
        public string? nNroOperacion { get; set; }
        public int IdOperacion { get; set; }
    
        public int IdGirador { get; set; }
  
        public int IdAdquiriente { get; set; }

        //[Required]
        public int IdGiradorCod { get; set; }
        //[Required]
        public int IdAdquirienteCod { get; set; }
        //[Required]
        //public int IdInversionista { get; set; }
        //[Required]
        //public int IdGiradorDireccion { get; set; }
        //[Required]
        //public int IdAdquirienteDireccion { get; set; }
        [Required]
        public decimal TEM { get; set; }
        [Required]
        public decimal PorcentajeFinanciamiento { get; set; }
        [Required]
        public decimal MontoOperacion { get; set; }
        [Required]
        public decimal DescContrato { get; set; }
        [Required]
        public decimal DescFactura { get; set; }
        [Required]
        public decimal DescCobranza { get; set; }
        [Required]
        public int IdTipoMoneda { get; set; }
        [Required]
        public decimal InteresMoratorio { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        //public decimal PorcentajeRetencion { get; set; }
        public int Estado { get; set; }
        public string? NombreEstado { get; set; }
        //public string ComentarioOperaciones { get; set; }
        //public string UsuarioCreador { get; set; }
        //public string UsuarioActualizacion { get; set; }

        //*************Ini-09-01-2023-RCARRILLO******//

        //[Required]
        //public string MotivoTransaccion { get; set; }

        //[Required]
        public string? SustentoComercial { get; set; }

      

        //public int Plazo { get; set; }

        //public int TipoDocumento { get; set; }
        //*************Fin-09-01-2023-RCARRILLO******//

    }

    public class AgregarFactura
    {
        public int IdOperacionCabeceraFacturas { get; set; }

        //[Required]
        //public string nroDocumento { get; set; }

        //[Required]
        //public decimal Monto { get; set; }

        //[Required]
        //public DateTime fechaEmision { get; set; }

        //[Required]
        //public DateTime fechaVencimiento { get; set; }


        public int nIdGiradorFact { get; set; }


        public int nIdAdquirenteFact { get; set; }

        [Required]
        public IFormFile fileXml { get; set; }

        //public string UsuarioCreador { get; set; }
        //[Required]
        //public DateTime FechaPagoNegociado { get; set; }
    }

    public class EliminarFactura
    {
        public int operacionFacturaId { get; set; }
        public string filePath { get; set; }
    }
    public class EditarFactura
    {
        public int nIdOperacionesFacturas { get; set; }
        public DateTime dFechaPagoNegociado { get; set; }
        public int Estado { get; set; }     //  <OAV - 30/01/2023>
    }

    public class AgregarDocumentoSolicitud
    {
        public int IdOperacionCabeceraFacturas { get; set; }
        //public int nIdSolEvalOperaciones { get; set; }

        [Required]
        public int nTipoDocumento { get; set; }

        [Required]
        public IFormFile fileDocumentoXml { get; set; }
    }

    public class EliminarDocumentoSolicitud
    {
        public int nIdDocumentoSolEvalOperacion { get; set; }
        public string filePath { get; set; }
    }
}
