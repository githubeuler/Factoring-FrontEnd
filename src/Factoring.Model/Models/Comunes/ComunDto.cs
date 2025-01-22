using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Comunes
{
    public class SectorListDto
    {
        public int nIdSector { get; set; }
        public string cNombreSector { get; set; }
    }

    public class GrupoListDto
    {
        public int nIdGrupoEconomico { get; set; }
        public string cNombreGrupoEconomico { get; set; }
    }

    public class PaisListDto
    {
        public int nIdPais { get; set; }
        public string cNombrePais { get; set; }
    }
}
