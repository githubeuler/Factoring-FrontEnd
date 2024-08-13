using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.DocumentosAceptante
{
    public class DocumentosAceptanteListDto
    {
        public int nIdAceptanteDocumento { get; set; }
        public int nIdTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string cNombreDocumento { get; set; }
        public string cRuta { get; set; }
        public DateTime dFechaCreacion { get; set; }
    }
}
