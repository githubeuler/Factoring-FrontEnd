using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.ViewModels
{
    public class PerfilCreateModel
    {
        public int nIdRol { get; set; }
        public string? cNombreRol { get; set; }
        public List<MenuModuloModel>? ListaMenu { get; set; }
    }
    public class MenuModuloModel
    {
        public int nIdMenuDetalle { get; set; }
        public int nIdMenu { get; set; }
        public bool cActualizar { get; set; }
        public bool cConsultar { get; set; }
        public bool cEliminar { get; set; }
        public bool cInsertar { get; set; }
    }
    public class PerfilCreateModelNew
    {
        public int nIdRol { get; set; }
        public string? cNombreRol { get; set; }
        public List<MenuModuloModelNew>? ListaMenu { get; set; }
    }

    public class MenuModuloModelNew
    {
        public int nIdMenuDetalle { get; set; }
        public int nIdMenu { get; set; }
        public bool Insertar { get; set; } 
        public bool Actualizar { get; set; }
        public bool Consultar { get; set; }
        public bool Eliminar { get; set; }
    }


}
