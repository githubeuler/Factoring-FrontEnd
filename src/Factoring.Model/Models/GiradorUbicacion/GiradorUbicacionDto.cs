namespace Factoring.Model.Models.GiradorUbicacion
{
    public class UbicacionGiradorInsertDto
    {
        public string FormatoUbigeo { get; set; }
        public string Direccion { get; set; }
        public int IdTipoDireccion { get; set; }
        public int IdGirador { get; set; }
    }


    public class UbicacionGiradorListDto
    {
        public int nIdGiradorDireccion { get; set; }
        public string cFormatoUbigeo { get; set; }
        public string cDireccion { get; set; }
    }

    public class UbicacionGiradorDeleteDto
    {
        public int IdGiradorDireccion { get; set; }
        public string UsuarioActualizacion { get; set; }
    }
}
