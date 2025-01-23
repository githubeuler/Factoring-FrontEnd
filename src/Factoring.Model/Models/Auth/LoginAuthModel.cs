namespace Factoring.Model.Models.Auth
{
    public class LoginAuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class ChangeAuthModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
    public class ResetPasswordModel
    {
        public int IdUsuario { get; set; }
        public string CodigoUsuario { get; set; }
    }
}
