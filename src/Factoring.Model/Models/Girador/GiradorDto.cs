using System.ComponentModel.DataAnnotations;

namespace Factoring.Model.Models.Girador
{
    public class GiradorReporte
    {
        public GiradorReporte()
        {
            Facturas = new List<FacturasGiradorReporte>();
        }
        public int IdGirador { get; set; }
        public string RazonSocial { get; set; }
        public int CantidadAquirientes { get; set; }
        public List<FacturasGiradorReporte> Facturas { get; set; }
    }

    public class FacturasGiradorReporte
    {
        public int CantidadFacturas { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }
    }
    public class GiradorSingleDto
    {
        public int nIdGirador { get; set; }
        public int nIdPais { get; set; }
        public string cNombrePais { get; set; }
        public string cRegUnicoEmpresa { get; set; }
        public string cRazonSocial { get; set; }
        public int nIdSector { get; set; }
        public string cNombreSector { get; set; }
        public decimal nVenta { get; set; }
        public decimal nCompra { get; set; }
        public int nIdGrupoEconomico { get; set; }
        public string cNombreGrupoEconomico { get; set; }
        public int nEstado { get; set; }
        public string NombreEstado { get; set; }
        public List<string> FormatoUbigeoPais { get; set; }
        public int nIdActividadEconomica { get; set; }
        public string dFechaInicioActividad { get; set; }
        public string dFechaFirmaContrato { get; set; }
        public string cAntecedente { get; set; }

    }
    public class GiradorResponseComentariosLista
    {
        public DateTime dFechaCreacion { get; set; }
        public string cUsuarioCreador { get; set; }
        public string cComentario { get; set; }
        public string nNroOperacion { get; set; }
    }
    public class GiradorUpdateDto
    {
        [Required]
        public int IdGirador { get; set; }

        [Required]
        public int IdPais { get; set; }

        [Required]
        public string RegUnicoEmpresa { get; set; }

        [Required]
        public string RazonSocial { get; set; }

        [Required]
        public int IdSector { get; set; }

        [Required]
        public decimal Venta { get; set; }

        [Required]
        public decimal Compra { get; set; }

        [Required]
        public int IdGrupoEconomico { get; set; }

        public string UsuarioActualizacion { get; set; }

        [Required]
        public int IdActividadEconomica { get; set; }
        [Required]
        public string FechaInicioActividad { get; set; }
        [Required]
        public string FechaFirmaContrato { get; set; }
        [Required]
        public string Antecedente { get; set; }

    }

    public class GiradorCreateDto
    {
        [Required]
        public int IdPais { get; set; }

        [Required]
        public string RegUnicoEmpresa { get; set; }

        [Required]
        public string RazonSocial { get; set; }

        [Required]
        public int IdSector { get; set; }

        [Required]
        public decimal Venta { get; set; }

        [Required]
        public decimal Compra { get; set; }

        [Required]
        public int IdGrupoEconomico { get; set; }

        public string UsuarioCreador { get; set; }

        public int IdActividadEconomica { get; set; }

        public string FechaInicioActividad { get; set; }
    }

    public class GiradorRequestDatatableDto
    {
        public int Pageno { get; set; }
        public string FilterRuc { get; set; }
        public string FilterRazon { get; set; }
        public int FilterIdPais { get; set; }
        public string FilterFecCrea { get; set; }
        public int FilterIdSector { get; set; }
        public int FilterIdGrupoEconomico { get; set; }
        public string Usuario { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
    }

    public class GiradorResponseDatatableDto
    {
        public int nIdGirador { get; set; }
        public string cRegUnicoEmpresa { get; set; }
        public string cRazonSocial { get; set; }
        public string cNombreSector { get; set; }
        public DateTime dFechaCreacion { get; set; }
        public string cNombrePais { get; set; }
        public string nEstado { get; set; }
        public string NombreEstado { get; set; }
        public int TotalRecords { get; set; }
    }

    public class GiradorResponseListaDto
    {
        public int nIdGirador { get; set; }
        public string cRegUnicoEmpresa { get; set; }
        public string cRazonSocial { get; set; }
    }


    public class GiradorEvaluacionUpdateDto
    {
        [Required]
        public int IdGirador { get; set; }
        public string UsuarioEvaluacion { get; set; }
        public int Estado { get; set; }
    }

    public class GiradorInsertComentarioDto
    {
        [Required]
        public int IdGirador { get; set; }
        [Required]
        public string Comentario { get; set; }
        public string Usuario { get; set; }
    }

    public class GiradorFileNameEntidad
    {

        public int IdDocumento { get; set; }
        public int IdTipo { get; set; }
        public string FileName { get; set; }
        public string Ruta { get; set; }
    }
}