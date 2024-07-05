namespace Factoring.Model.Models.ContactoGirador
{
    public class ContactoGiradorCreateDto
    {
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public int IdGirador { get; set; }
        public int IdTipoContacto { get; set; }
    }

    public class ContactoGiradorResponseListDto
    {
        public string nIdGiradorContacto { get; set; }
        public string cNombre { get; set; }
        public string cApellidoPaterno { get; set; }
        public string cApellidoMaterno { get; set; }
        public string cCelular { get; set; }
        public string cEmail { get; set; }
        public string nIdTipoContacto { get; set; }
        public string NombreTipoContacto { get; set; }
    }
}
