using ExtraSliceV2.Extensions;
using ExtraSliceV2.Models;
using ExtraSliceV2.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExtraSliceV2.Controllers
{
    public class ManageController : Controller
    {
        private RepositoryRestaurante repo;
        public ManageController(RepositoryRestaurante repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            Usuario usuario = await this.repo.ExisteUsuario(email,password);

            if(usuario != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUser.ToString());
                identity.AddClaim(claimId);

                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre_cliente.ToString());
                identity.AddClaim(claimName);

                Claim claimDirect = new Claim("Direccion", usuario.Direccion.ToString());
                identity.AddClaim(claimDirect);

                Claim claimTelef = new Claim(ClaimTypes.MobilePhone, usuario.Telefono.ToString());
                identity.AddClaim(claimTelef);

                Claim claimEmail = new Claim(ClaimTypes.Email, usuario.Email.ToString());
                identity.AddClaim(claimEmail);

                ClaimsPrincipal usePrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, usePrincipal);
                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();
                return RedirectToAction(action, controller);

            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("IdProductos");
            return RedirectToAction("Index", "Carta");
        }
    }
}
