using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.ViewModels
{
    public class AceptanteViewModel
    {
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Fecha { get; set; }
        public int Pais { get; set; }
        public int Sector { get; set; }
        public int GrupoEconomico { get; set; }
        public string Estado { get; set; }
    }

    public class AceptanteCreateModel
    {
        public int IdAceptante { get; set; }

        [Required]
        public int Pais { get; set; }

        [Required]
        public string RegUnicoEmpresa { get; set; }

        [Required]
        public string RazonSocial { get; set; }

        [Required]
        public int Sector { get; set; }

        [Required]
        public int GrupoEconomico { get; set; }

        public string UsuarioCreador { get; set; }
        public string UsuarioActualizacion { get; set; }
        public string NombrePais { get; set; }
        public string NombreSector { get; set; }
        public string NombreGrupoEconomico { get; set; }
        public string NombreEstado { get; set; }
    }

    public class AgregarContactoAceptante
    {
        public int IdAceptanteCabecera { get; set; }

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

    public class AgregarUbicacionAceptante
    {
        public int IdAceptanteCabeceraUbicacion { get; set; }

        [Required]
        public string Ubigeo { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public int TipoDireccion { get; set; }
    }

    public class AgregarLineaAceptante
    {
        public int IdAceptante { get; set; }
        public int IdInversionista { get; set; }
        public int IdTipoMoneda { get; set; }
        public decimal LineaMeta { get; set; }
    }
}
