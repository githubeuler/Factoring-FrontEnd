using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.AceptanteUbicacion
{
    public class UbicacionAceptanteInsertDto
    {
        public string FormatoUbigeo { get; set; }
        public string Direccion { get; set; }
        public int IdTipoDireccion { get; set; }
        public int IdAceptante { get; set; }
    }


    public class UbicacionAceptanteListDto
    {
        public int nIdAceptanteDireccion { get; set; }
        public string cFormatoUbigeo { get; set; }
        public string cDireccion { get; set; }
    }

    public class UbicacionAceptanteDeleteDto
    {
        public int IdAceptanteDireccion { get; set; }
        public string UsuarioActualizacion { get; set; }
    }
}
