﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Cavali
{

    public class ResponseCavaliRemove4008
    {
        public ResponseCavaliRemove4008()
        {
            Valores = new Valores();
        }

        public bool Error { get; set; }
        public string Mensaje { get; set; }
        public Valores Valores { get; set; }
    }

}
