using System.ComponentModel.DataAnnotations;

namespace Factoring.Model.ViewModels
{
    public class FondeadorViewModel
    {
        public int IdEstado { get; set; }
        public int Pageno { get; set; }
        public string FilterDoi { get; set; }
        public string FilterRazon { get; set; }
        public string FilterFecCrea { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
    }

    public class FondeadorCreateModel
    {
        public int IdFondeador { get; set; }
        public int Estado { get; set; }
        [Required]
        public int IdTipoDocumento { get; set; }
        [Required]
        public string DOI { get; set; }
        [Required]
        public string RazonSocial { get; set; }
        public string UsuarioCreador { get; set; }
        public string UsuarioActualizacion { get; set; }
        public string NombreEstado { get; set; }

    }

    public class FondeadorRegistroViewModel
    {
        public int IdFondeador { get; set; }
        public int IdTipoDocumento { get; set; }
        public string DOI { get; set; }
        public string RazonSocial { get; set; }
        public int IdProducto { get; set; } = 0;
        public int IdInteresCalculado { get; set; }
        public int IdTipoFondeo { get; set; } 
        public string? DistribucionFondeador { get; set; }

    }

}
