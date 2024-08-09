using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Externos
{
    public class DivisoFondeadores
    {
        public int? nIdFondeador { get; set; }
        public string? cFondeador { get; set; }
        public string? cDocumento { get; set; }
        public string? cCodParticipante { get; set; }

        public int? iIdFondeador { get; set; }
        public int? iTipoDocumento { get; set; }
        public string? tipoDocumento { get; set; }
        public string? cNroDocumento { get; set; }
        public string? cNombreFondeador { get; set; }
        public int? iTipodeNegocio { get; set; }
        public string? tipoNegocio { get; set; }
        public int? iPais { get; set; }
        public string? cNombrePais { get; set; }
        public string? cFormatoUbigeo { get; set; }

    }
}
