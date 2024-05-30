namespace Factoring.Model.Models.AdquirienteUbicacion
{
    public class UbicacionAdquirienteInsertDto
    {
        public string FormatoUbigeo { get; set; }
        public string Direccion { get; set; }
        public int IdTipoDireccion { get; set; }
        public int IdAdquiriente { get; set; }
    }


    public class UbicacionAdquirienteListDto
    {
        public int nIdAdquirienteDireccion { get; set; }
        public string cFormatoUbigeo { get; set; }
        public string cDireccion { get; set; }
    }

    public class UbicacionAdquirienteDeleteDto
    {
        public int IdAdquirienteDireccion { get; set; }
        public string UsuarioActualizacion { get; set; }
    }
}
