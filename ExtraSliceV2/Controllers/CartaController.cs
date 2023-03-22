using ExtraSliceV2.Extensions;
using ExtraSliceV2.Filters;
using ExtraSliceV2.Models;
using ExtraSliceV2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ExtraSliceV2.Controllers
{
    public class CartaController : Controller
    {
        private RepositoryRestaurante repo;
        private IMemoryCache memoryCache;
        public CartaController(RepositoryRestaurante repo, IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.memoryCache = memoryCache;
        }
        //miedo
        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            return View();
        }

        public IActionResult Index()
        {
            List<Restaurante> restaurantes = this.repo.GetRestaurantes();
            return View(restaurantes);
        }
        [AuthorizeUsuarios]
        public IActionResult CarritoProductos(int? ideliminar)
        {
            //tenemos una coleccion de ids y necesitamos
            //recuperamos los datos de session
            List<int> idsProductos = HttpContext.Session.GetObject<List<int>>("IdProductos");

            if (idsProductos == null)
            {
                ViewData["MENSAJE"] = "No hay productos";
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    //ELIMINAMOS EL ELEMENTO QUE NOS HAN SOLICITADO
                    idsProductos.Remove(ideliminar.Value);
                    if (idsProductos.Count == 0)
                    {
                        HttpContext.Session.Remove("IdProductos");
                    }
                    else
                    {
                        //DEBEMOS ACTUALIZAR DE NUEVO SESSION
                        HttpContext.Session.SetObject("IdProductos", idsProductos);
                    }
                }

                List<Producto> productosSession = this.repo.GetProductosSession(idsProductos);
                return View(productosSession);

            }
        }
        [AuthorizeUsuarios]
        public IActionResult Favoritos(int? ideliminar)
        {
            List<Producto> productosFavoritos;
            if (this.memoryCache.Get("FAVORITOS") == null)
            {
                productosFavoritos = new List<Producto>();
            }
            else
            {
                productosFavoritos = this.memoryCache.Get<List<Producto>>("FAVORITOS");
                if (ideliminar != null)
                {
                    //ELIMINAMOS EL ELEMENTO QUE NOS HAN SOLICITADO
                    Producto product = productosFavoritos.FirstOrDefault(p=>p.IdProducto == ideliminar);
                    productosFavoritos.Remove(product);
                    if (productosFavoritos.Count == 0)
                    {
                        this.memoryCache.Remove("FAVORITOS");
                       
                    }
                    else
                    {
                        //DEBEMOS ACTUALIZAR DE NUEVO SESSION
                        this.memoryCache.Set("FAVORITOS", productosFavoritos);
                    }
                    
                }
                

            }
            return View(productosFavoritos);
        }

        public IActionResult Restaurante(int idrestaurante, int? idproducto, int? idfavorito)
        {
            if (idfavorito != null)
            {
                List<Producto> productoFavoritos;
                if (this.memoryCache.Get("FAVORITOS") == null)
                {
                    productoFavoritos = new List<Producto>();
                }
                else
                {
                    productoFavoritos = this.memoryCache.Get<List<Producto>>("FAVORITOS");
                }
                //BUSCAMOS PRODUCTO EN BBDD PARA ALMACENARLO EN CACHE
                Producto producto = this.repo.FindProducto(idfavorito.Value);
                productoFavoritos.Add(producto);
                //ALMACENAMOS LOS DATOS EN CACHE
                this.memoryCache.Set("FAVORITOS", productoFavoritos);
            }


            if (idproducto != null)
            {
                List<int> idsProductos;
                if (HttpContext.Session.GetObject<List<int>>("IdProductos") == null)
                {
                    //creamos la lista para los ids
                    idsProductos = new List<int>();
                }
                else
                {
                    idsProductos = HttpContext.Session.GetObject<List<int>>("IdProductos");
                }
                idsProductos.Add(idproducto.Value);
                HttpContext.Session.SetObject("IdProductos", idsProductos);

            }
            //RestauranteProductos restauranteProductos = this.repo.RestProduct(idrestaurante.Value);
            RestauranteProductos restauranteProductos = new RestauranteProductos
            {
                Restaurante = this.repo.FindRestaurante(idrestaurante),
                Productos = this.repo.FindProductos(idrestaurante)
            };
            return View(restauranteProductos);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
           await this.repo.RegisterUser(usuario.Nombre_cliente, usuario.Direccion, usuario.Telefono, usuario.Email, usuario.Password);
            return View();
        }

    }
}
