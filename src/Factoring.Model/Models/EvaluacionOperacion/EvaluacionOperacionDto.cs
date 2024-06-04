using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.EvaluacionOperacion
{
    public class EvaluacionOperacionesInsertDto
    {
        public int IdOperaciones { get; set; }
        public int IdCatalogoEstado { get; set; }
        public string UsuarioCreador { get; set; }
        public string? Comentario { get; set; }
    }
    public class EvaluacionOperacionesEstadoInsertDto
    {
        public int IdOperaciones { get; set; }
        public int IdOperacionesFactura { get; set; }
        public int IdCatalogoEstado { get; set; }
        public string UsuarioCreador { get; set; }
        public string? Comentario { get; set; }
    }

    public class EvaluacionOperacionesCalculoInsertDto
    {
        public int IdOperaciones { get; set; }
        public int IdOperacionesFactura { get; set; }
        public int? IdCatalogoEstado { get; set; }
        public string UsuarioCreador { get; set; }
        public string? cFecha { get; set; }
    }
}
