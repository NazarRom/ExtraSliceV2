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

        public Restaurante FindRestaurante(int idrestaunrante)
        {
            return this.context.Restaurantes.FirstOrDefault(x => x.IdRestaurante == idrestaunrante);
        }

        public List<Producto> FindProductos(int idrestaurante)
        {
            var consulta = from data in this.context.Productos
                           where data.IdRestaurante == idrestaurante
                           select data;
            return consulta.ToList();
        }
    }
}
