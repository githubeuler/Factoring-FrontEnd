﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.PerfilMenu
{
    public class PerfilResponseDto
    {
        //public int nIdMenuDetalle { get; set; }
        public int nIdRoles { get; set; }
        public string cNombreRol { get; set; }
        public int nActivo { get; set; }
        public string cDesEstado { get; set; }
        
        public  int TotalRecords { get; set; }
    }
}
