using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Operaciones
{
    public class OperacionesFacturaCalculoDto
    {
        public int nIdOperacionesFacturas { get; set; }
        public int nIdOperaciones { get; set; }
        public string? cNroDocumento { get; set; }
        public decimal nMontoInteres { get; set; }
        public decimal nMontoIgvServicios { get; set; }
        public decimal nMontoADesembolsar { get; set; }
        public decimal nInteresMoratorio { get; set; }
        public decimal nDevolucionEstimada { get; set; }
    }
}
