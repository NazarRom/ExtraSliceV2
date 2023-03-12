using Microsoft.AspNetCore.Mvc;

namespace ExtraSliceV2.Controllers
{
    public class CartaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Restaurante()
        {
            return View();
        }
    }
}
