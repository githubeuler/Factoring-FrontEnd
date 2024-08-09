using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.ViewModels.AceptanteLinea
{
    public class LineAceptanteListDto
    {
        public int nIdAceptante { get; set; }
        public string Inversionista { get; set; }
        public string TipoMoneda { get; set; }
        public decimal LineaMeta { get; set; }
        public decimal LineaDisponible { get; set; }
        public int nIdInversionista { get; set; }
        public int nIdTipoMoneda { get; set; }
        public int nIdAceptanteLinea { get; set; }
    }

    public class LineaAceptanteInsertDto
    {
        public int nIdAceptante { get; set; }
        public int nIdInversionista { get; set; }
        public int nIdTipoMoneda { get; set; }
        public decimal LineaMeta { get; set; }
        public decimal LineaDisponible { get; set; }

    }
    public class LineaAceptanteDeleteDto
    {
        public int IdAceptanteLinea { get; set; }
    }
}
