
namespace Factoring.Model.Models.Adquiriente
{
    public class AdquirienteReporte
    {
        public AdquirienteReporte()
        {
            Facturas = new List<FacturasAdquirienteReporte>();
        }
        public int IdAdquiriente { get; set; }
        public string RazonSocial { get; set; }
        public int CantidadGiradores { get; set; }
        public List<FacturasAdquirienteReporte> Facturas { get; set; }
    }

    public class FacturasAdquirienteReporte
    {
        public int CantidadFacturas { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }
    }
    public class AdquirienteRequestDatatableDto
    {
        public int Pageno { get; set; }
        public string FilterRuc { get; set; }
        public string FilterRazon { get; set; }
        public int FilterIdPais { get; set; }
        public string FilterFecCrea { get; set; }
        public int FilterIdSector { get; set; }
        public int FilterIdGrupoEconomico { get; set; }
        public int PageSize { get; set; }
        public string Sorting { get; set; }
        public string SortOrder { get; set; }
    }

    public class AdquirienteResponseDatatableDto
    {
        public int nIdAdquiriente { get; set; }
        public string cRegUnicoEmpresa { get; set; }
        public string cRazonSocial { get; set; }
        public string cNombreSector { get; set; }
        public DateTime dFechaCreacion { get; set; }
        public string cNombrePais { get; set; }
        public string nEstado { get; set; }
        public string NombreEstado { get; set; }
        public int TotalRecords { get; set; }
    }

    public class AdquirienteResponseComentariosLista
    {
        public DateTime dFechaCreacion { get; set; }
        public string cUsuarioCreador { get; set; }
        public string cComentario { get; set; }
        public string nNroOperacion { get; set; }
    }

    public class AdquirienteResponseListaDto
    {
        public int nIdAdquiriente { get; set; }
        public string cRegUnicoEmpresa { get; set; }
        public string cRazonSocial { get; set; }


    }

    public class AdquirienteGetByIdDto
    {
        public int nIdAdquiriente { get; set; }
        public int nIdPais { get; set; }
        public string cNombrePais { get; set; }
        public string cRegUnicoEmpresa { get; set; }
        public string cFormatoUbigeo { get; set; }
        public string cRazonSocial { get; set; }
        public int nIdSector { get; set; }
        public string cNombreSector { get; set; }
        public int nIdGrupoEconomico { get; set; }
        public string cNombreGrupoEconomico { get; set; }
        public int nEstado { get; set; }
        public string NombreEstado { get; set; }
        public List<string> FormatoUbigeoPais { get; set; }
    }

    public class AdquirienteDeleteDto
    {
        public int IdAdquiriente { get; set; }
        public string UsuarioActualizacion { get; set; }
    }

    public class AdquirienteInsertDto
    {
        public int IdPais { get; set; }
        public string RegUnicoEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public int IdSector { get; set; }
        public int IdGrupoEconomico { get; set; }
        public string UsuarioCreador { get; set; }
    }

    public class AdquirienteUpdateDto
    {
        public int IdAdquiriente { get; set; }
        //public int IdPais { get; set; }
        public string? RegUnicoEmpresa { get; set; }
        public string? RazonSocial { get; set; }
        //public int IdSector { get; set; }
        //public int IdGrupoEconomico { get; set; }
        public string? UsuarioActualizacion { get; set; }
        public string? FechaInicioActividad { get; set; }
        public int? IdActividadEconomica { get; set; }
        public string? FechaFirmaContrato { get; set; }
        public string? Antecedente { get; set; }
    }
}