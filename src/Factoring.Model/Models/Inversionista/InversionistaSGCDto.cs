using Factoring.Model.Models.Externos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Inversionista
{
    public class InversionistaSGC
    {
        public string? succeeded { get; set; }
        public string? message { get; set; }
        public string? errors { get; set; }

        public List<DivisoFondeadores>? data { get; set; }
    }
}
