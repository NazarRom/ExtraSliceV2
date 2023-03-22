using ExtraSliceV2.Data;
using ExtraSliceV2.Models;

namespace ExtraSliceV2.Repositories
{
    public class RepositoryRestaurante
    {
        private RestauranteContext context;
        public RepositoryRestaurante(RestauranteContext context)
        {
            this.context = context;
        }

        public List<Restaurante> GetRestaurantes()
        {
            var consulta = from data in this.context.Restaurantes
                           select data;
            return consulta.ToList();
        }
        public List<Producto> GetProductos()
        {
            var consulta = from data in this.context.Productos
                           select data;
            return consulta.ToList();
        }

        public List<Producto> GetProductosSession(List<int> ids)
        {
            var consulta = from datos in this.context.Productos
                           where ids.Contains(datos.IdProducto)
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            return consulta.ToList();
        }


        public List<Producto> FindProductos(int idrestaurante)
        {
            var consulta = from data in this.context.Productos
                           where data.IdRestaurante == idrestaurante
                           select data;
            return consulta.ToList();
        }

        public Restaurante FindRestaurante(int idrestaunrante)
        {
            return this.context.Restaurantes.FirstOrDefault(x => x.IdRestaurante == idrestaunrante);
        }

        public Producto FindProducto(int idproducto)
        {
            return this.context.Productos.FirstOrDefault(x => x.IdProducto == idproducto);
        }

        //public RestauranteProductos RestProduct (int idrestaurante)
        //{
        //    var consultaRest = this.context.Restaurantes.FirstOrDefault(x => x.IdRestaurante == idrestaurante);

        //    var consultaProd = from data in this.context.Productos
        //                       where data.IdRestaurante == idrestaurante
        //                       select data;

        //    RestauranteProductos restauranteProductos = new RestauranteProductos
        //    {
        //        Restaurante = consultaRest,
        //        Productos = consultaProd.ToList(),

        //    };
        //    return restauranteProductos;
        //}
    }
}
