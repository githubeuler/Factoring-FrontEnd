using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Cavali
{

    public class ResponseCavaliRedeem4007
    {
        public ResponseCavaliRedeem4007()
        {
            Valores = new Valores();
        }

        public bool succeeded { get; set; }
        public string message { get; set; }
        public string errors { get; set; }
        public Valores Valores { get; set; }
    }

}
