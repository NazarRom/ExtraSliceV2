using ExtraSliceV2.Models;
using ExtraSliceV2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExtraSliceV2.Controllers
{
    public class CartaController : Controller
    {
        private RepositoryRestaurante repo;
        public CartaController(RepositoryRestaurante repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Restaurante> restaurantes = this.repo.GetRestaurantes();
            return View(restaurantes);
        }

        public IActionResult Restaurante(int idrestaurante)
        {
            Restaurante restaurante = this.repo.FindRestaurante(idrestaurante);
            return View(restaurante);
        }

        public IActionResult _ListaProductos(int idrestaurante)
        {
            List<Producto> productos = this.repo.FindProductos(idrestaurante);
            return PartialView("_ListaProductos", productos);
        }
    }
}
