using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.ViewModels.AceptanteContacto
{
    public class ContactoAceptanteCreateDto
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public int IdAceptante { get; set; }
        public int IdTipoContacto { get; set; }
    }

    public class ContactoAceptanteDeleteDto
    {
        public string UsuarioActualizacion { get; set; }
        public int IdAceptanteContacto { get; set; }
    }

    public class ContactoAceptanteResponseListDto
    {
        public string nIdAceptanteContacto { get; set; }
        public string cNombre { get; set; }
        public string cApellidoPaterno { get; set; }
        public string cApellidoMaterno { get; set; }
        public string cCelular { get; set; }
        public string cEmail { get; set; }
        public string nIdTipoContacto { get; set; }
        public string NombreTipoContacto { get; set; }
    }
}