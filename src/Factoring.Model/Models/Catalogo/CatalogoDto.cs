using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Catalogo
{
    public class CatalogoListDto
    {
        public int Tipo { get; set; }
        public int Codigo { get; set; }
        public string Valor { get; set; }
        public int CodigoParametro { get; set; }
    }

    public class CatalogoResponseListDto
    {
        public int nId { get; set; }
        public string cNombre { get; set; }
    }
}