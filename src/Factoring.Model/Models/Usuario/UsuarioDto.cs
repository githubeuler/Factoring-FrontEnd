namespace Factoring.Model.Models.Usuario
{
    public class UsuarioSingleDto
    {
        public int IdUsuario { get; set; }
        public string? CodigoUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public int IdPais { get; set; }
        public string? Pais { get; set; }
        public int IdRol { get; set; }
        public string? Rol { get; set; }
        public int IdEstado { get; set; }
        public string? Estado { get; set; }
    }

    public class UsuarioResponseDataTableDto
    {
        public int nIdUsuario { get; set; }
        public string? cCodigoUsuario { get; set; }
        public string? cNombreUsuario { get; set; }
        public string? cFechaRegistro { get; set; }
        public string? cFechaCese { get; set; }
        public string? cCorreo { get; set; }
        public string? cActivo { get; set; }
        public string? cNombrePais { get; set; }
        public string? cNombreRol { get; set; }
        public int nEditar { get; set; }
        public int TotalRecords { get; set; }
    }

    public class UsuarioRequestDataTableDto
    {
        public int Pageno { get; set; }
        public string? FilterCodigoUsuario { get; set; }
        public string? FilterNombreUsuario { get; set; }
        public int FilterActivo { get; set; }
        public int FilterIdPais { get; set; }
        public string? Usuario { get; set; }
        public int PageSize { get; set; }
        public string? Sorting { get; set; }
        public string? SortOrder { get; set; }
    }

    public class UsuarioRegistroRequestDto
    {
        public int IdUsuario { get; set; }
        public string? CodigoUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Password { get; set; }
        public int IdPais { get; set; }
        public int IdRol { get; set; }
        public string? UsuarioCreador { get; set; }
    }
    public class UsuarioUpdateRequestDto
    {
        public int IdUsuario { get; set; }
        public string? CodigoUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        //public string? Password { get; set; }
        public int IdPais { get; set; }
        public int IdRol { get; set; }
        public int Activo { get; set; }
        public string? UsuarioModificacion { get; set; }

    }

    
}
