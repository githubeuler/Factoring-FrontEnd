using Microsoft.AspNetCore.Mvc;

namespace Factoring.WebMvc.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
