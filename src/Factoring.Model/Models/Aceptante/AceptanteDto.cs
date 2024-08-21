using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factoring.Model.Models.Aceptante
{
    public class AceptanteReporte
    {
        public AceptanteReporte()
        {
            Facturas = new List<FacturasAceptanteReporte>();
        }
        public int IdAdquiriente { get; set; }
        public string RazonSocial { get; set; }
        public int CantidadGiradores { get; set; }
        public List<FacturasAceptanteReporte> Facturas { get; set; }
    }

    public class FacturasAceptanteReporte
    {
        public int CantidadFacturas { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }
    }
    public class AceptanteRequestDatatableDto
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

    public class AceptanteResponseDatatableDto
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

    public class AceptanteResponseComentariosLista
    {
        public DateTime dFechaCreacion { get; set; }
        public string cUsuarioCreador { get; set; }
        public string cComentario { get; set; }
        public string nNroOperacion { get; set; }
    }

    public class AceptanteResponseListaDto
    {
        public int nIdAceptante { get; set; }
        public string cRegUnicoEmpresa { get; set; }
        public string cRazonSocial { get; set; }
    }
    public class AceptanteGetByIdDto
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

        public string dFechaInicioActividad { get; set; }

        public int nIdActividadEconomica { get; set; }

        public string dFechaFirmaContrato { get; set; }

        public string cAntecedente { get; set; }


    }

    public class AceptanteDeleteDto
    {
        public int IdAceptante { get; set; }
        public string UsuarioActualizacion { get; set; }
    }

    public class AceptanteInsertDto
    {
        public int IdPais { get; set; }
        public string RegUnicoEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public int IdSector { get; set; }
        public int IdGrupoEconomico { get; set; }
        public string UsuarioCreador { get; set; }
    }
    public class AgregarDocumentosAceptante
    {
        public int IdAceptanteCabeceraDocumentos { get; set; }

        [Required]
        public int TipoDocumento { get; set; }

        [Required]
        public IFormFile FileDocumento { get; set; }
    }
    public class AceptanteUpdateDto
    {
        public int IdAceptante { get; set; }
        public int IdPais { get; set; }
        public string RegUnicoEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public int IdSector { get; set; }
        public int IdGrupoEconomico { get; set; }
        public string UsuarioActualizacion { get; set; }
    }
}