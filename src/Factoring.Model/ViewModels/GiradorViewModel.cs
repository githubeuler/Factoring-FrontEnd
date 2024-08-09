using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Factoring.Model.ViewModels
{
    public class GiradorViewModel
    {
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Fecha { get; set; }
        public int Pais { get; set; }
        public int Sector { get; set; }
        public int GrupoEconomico { get; set; }
        public string Estado { get; set; }
    }

    public class GiradorCreateModel
    {
        public int IdGirador { get; set; }

        //[Required]
        public int Estado { get; set; }

        
        [Required]
        public string RegUnicoEmpresa { get; set; }

        [Required]
        public string RazonSocial { get; set; }

       

        public string UsuarioCreador { get; set; }
        public string UsuarioActualizacion { get; set; }
       
      
      
        public string NombreEstado { get; set; }

        public string FechaInicioActividades { get; set; }
        public int IdActividadEconomica { get; set; }
        public string ActividadEconomica { get; set; }

        public string FechaFirmaContrato { get; set; }
        public string Antecedente { get; set; }


    }

    public class AgregarContactoGirador
    {
        public int IdGiradorCabecera { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string ApellidoPaterno { get; set; }

        [Required]
        public string ApellidoMaterno { get; set; }

        [Required]
        public string Celular { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int TipoContacto { get; set; }
    }

    public class AgregarRepresentanteLegal
    {
        public int IdGiradorCabeceraRepresentante { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string ApellidoPaterno { get; set; }

        [Required]
        public string ApellidoMaterno { get; set; }

        [Required]
        public string Celular { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public IFormFile poderLegal { get; set; }

        public string Ruta { get; set; }
        public string NombreDocumento { get; set; }
    }

    public class AgregarCuentaBancaria
    {
        public int IdGiradorCabeceraCuenta { get; set; }

        [Required]
        public string NroCuentaBancaria { get; set; }

        [Required]
        public string CCI { get; set; }

        [Required]
        public int Banco { get; set; }

        [Required]
        public int Moneda { get; set; }

        public string NombreBanco { get; set; }
        public string NombreMoneda { get; set; }

        [Required]
        public IFormFile docConst { get; set; }

        public string RutaDocConst { get; set; }
        public string NombreDocConst { get; set; }

        public bool bPredeterminado { get; set; }

        public int nValorSeleccionado { get; set; }
    }

    public class AgregarUbicacion
    {
        public int IdGiradorCabeceraUbicacion { get; set; }

        [Required]
        public string Ubigeo { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public int TipoDireccion { get; set; }
    }

    public class AgregarDocumentos
    {
        public int IdGiradorCabeceraDocumentos { get; set; }

        [Required]
        public int TipoDocumento { get; set; }

        [Required]
        public IFormFile FileDocumento { get; set; }
    }

    public class AgregarLinea
    {
        public int IdGirador { get; set; }
        public int IdInversionista { get; set; }
        public int IdTipoMoneda { get; set; }
        public decimal LineaMeta { get; set; }
    }

    public class GiradorCreateComentarioModel
    {
        public int IdEvaluacionComentarioGirador { get; set; }

        [Required]
        public int IdGirador { get; set; }

        [Required]
        public string Comentario { get; set; }

        public string Usuario { get; set; }
    }

    public class GiradorEstadoModel
    {
        [Required]
        public int IdGirador { get; set; }

        [Required]
        public int Estado { get; set; }

        public string UsuarioEvaluacion { get; set; }
    }


    public class AgregarGiradorConfirming
    {
        public int nIdGirador { get; set; }
        [Required]
        public int nIdAceptante { get; set; }
        [Required]
        public int nCategoria { get; set; }

    }

    public class AgregarGiradorCategoria
    {
        public int IdGiradorCabeceraCategoria { get; set; }
        public int Aceptante { get; set; }
        [Required]
        public int Categoria { get; set; }

    }

    public class EditarCuentaBancaria
    {
        public int IdGiradorCabeceraCuenta { get; set; }
        public int IdGirador { get; set; }
        public bool bPredeterminado { get; set; }
    }

}
