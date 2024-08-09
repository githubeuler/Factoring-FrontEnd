using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Cavali
{
    public class Body
    {
        public int processId { get; set; }
        public string processCode { get; set; }
        public string message { get; set; }
    }

    public class Valores
    {
        public Valores()
        {
            body = new Body();
            statusCode = false;
        }
        public bool statusCode { get; set; }
        public Body body { get; set; }
    }

    public class ResponseCavaliInvoice4012
    {
        public ResponseCavaliInvoice4012()
        {
            Valores = new Valores();
        }

        public bool Error { get; set; }
        public string Mensaje { get; set; }
        public Valores Valores { get; set; }
    }

}
