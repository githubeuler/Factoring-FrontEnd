using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.AceptanteContacto
{
    public class ContactoAdquirienteCreateDto
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public int IdAdquiriente { get; set; }
        public int IdTipoContacto { get; set; }
    }

    public class ContactoAdquirienteDeleteDto
    {
        public string UsuarioActualizacion { get; set; }
        public int IdAdquirienteContacto { get; set; }
    }

    public class ContactoAdquirienteResponseListDto
    {
        public string nIdAdquirienteContacto { get; set; }
        public string cNombre { get; set; }
        public string cApellidoPaterno { get; set; }
        public string cApellidoMaterno { get; set; }
        public string cCelular { get; set; }
        public string cEmail { get; set; }
        public string nIdTipoContacto { get; set; }
        public string NombreTipoContacto { get; set; }
    }
}