using ExtraSliceV2.Data;
using ExtraSliceV2.Helpers;
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

         private int GetMaxIdusuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.Usuarios.Max(z => z.IdUser) + 1; 
            }
        }

        public async Task RegisterUser(string nombre, string direccion, string telefono, string email,  string pass)
        {
            Usuario user = new Usuario();
            user.IdUser = this.GetMaxIdusuario();
            user.Nombre_cliente = nombre;
            user.Direccion = direccion;
            user.Telefono = telefono;
            user.Email = email;
            user.Salt = HelperCryptograhy.GenerateSalt();
            user.PasswordCifrado = HelperCryptograhy.EncryptPassword(pass, user.Salt);
            user.Password = pass;
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }


        public async Task<Usuario> ExisteUsuario(string email, string password)
        {
            var consulta = this.context.Usuarios.Where(x => x.Email == email && x.Password == password);
            return consulta.FirstOrDefault();
        }
    }
}
