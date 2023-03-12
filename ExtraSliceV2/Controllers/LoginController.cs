using Microsoft.AspNetCore.Mvc;

namespace ExtraSliceV2.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Login(string email, string password)
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string algo)
        {
            return View();
        }


    }
}
