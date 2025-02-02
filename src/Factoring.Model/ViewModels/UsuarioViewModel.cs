using System.ComponentModel.DataAnnotations;

namespace Factoring.Model.ViewModels
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }
        public string? CodigoUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public int IdPais { get; set; }
        public string? Pais { get; set; }
        public int IdRol { get; set; }
        public string? Rol { get; set; }
        public int IdEstado { get; set; }
        public string? Estado { get; set; }

    }

    public class UsuarioCreateModel
    {
        public int IdUsuario { get; set; }

        [Required]
        public int IdPais { get; set; }
        [Required]
        public int IdRol { get; set; }


        [Required]
        public string? CodigoUsuario { get; set; }

        [Required]
        public string? NombreUsuario { get; set; }

        [Required]
        public string? Correo { get; set; }

        //[Required]
        //public string? Contrasena { get; set; }
        [Required]
        public bool IdEstado { get; set; }

        [Required]
        public int IdTipoDocumento { get; set; }
        [Required]
        public string? NumeroDocumento { get; set; }
        [Required]
        public string? Telefono { get; set; }
        [Required]
        public string? Celular { get; set; }
        [Required]
        public string? Cargo { get; set; }

        public string? Ruc { get; set; }
        public string? RazonSocial { get; set; }



    }

}
