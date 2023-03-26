using ExtraSliceV2.Data;
using ExtraSliceV2.Helpers;
using ExtraSliceV2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
#region procedure
//que pasa ahora
//create procedure sp_insert_cliente
//(@name nvarchar (50),
//@dire nvarchar(50),
//@tel int,
//@user nvarchar(50),
//@pass nvarchar(100),
//@passcif varbinary(500),
//@salt nvarchar(200))
//as
//declare @idmax int
//select @idmax = MAX(id) +1  from clientes
//insert into clientes values(@idmax, @name, @dire, @tel, @user, @passcif, @pass, @salt);
//GO

//create procedure sp_pedido_clientes
//(@fechahora datetime, @idcliente int)
//as
//declare @idmax int
//select @idmax = MAX(id) +1  from pedidos
//insert into pedidos values(@idmax, @fechahora, @idcliente)
//go

//create procedure sp_pedido_clientes
//(@fechahora datetime, @idcliente int)
//as
//declare @idmax int
//select @idmax = MAX(id) +1  from pedidos
//insert into pedidos values(@idmax, @fechahora, @idcliente)
//go

//create procedure sp_delete_last
//as
//declare @idmax int
//select @idmax = MAX(id) from pedidos
//delete from pedidos where id = @idmax
//go

//CREATE PROCEDURE sp_filtro_dinero
//    @dinero INT
//AS
//BEGIN
//    SELECT DISTINCT r.id, r.nombre_restaurante, r.direccion, r.telefono , r.id_categoria, r.imagen
//    FROM productos p
//    INNER JOIN restaurantes r ON p.id_restaurante = r.id
//    WHERE p.precio <= @dinero
//END


#endregion
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

        public async Task RegisterUser(string nombre, string direccion, string telefono, string email, string pass)
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

        public Usuario FindUsuario(int iduser)
        {
            return this.context.Usuarios.FirstOrDefault(x => x.IdUser == iduser);
        }

        //crearPedido
        public async Task CrearPedido(int idcliente)
        {
            string sql = "sp_pedido_clientes @fechahora, @idcliente";
            DateTime fecha = DateTime.Today;
            SqlParameter pamfecha = new SqlParameter("@fechahora", fecha);
            SqlParameter pamidcliente = new SqlParameter("@idcliente", idcliente);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamfecha, pamidcliente);

        }

        public async Task FinalizarPedido(int idcliente, List<int> idsproducto, List<int> cantidad)
        {
            await this.CrearPedido(idcliente);

            for (var i = 0; i < idsproducto.Count(); i++)
            {
                int id = idsproducto[i];
                int cant = cantidad[i];
                string sql = "sp_producto_pedidos @idproducto, @cantidad";
                SqlParameter pamidproducto = new SqlParameter("@idproducto", id);
                SqlParameter pamCantidad = new SqlParameter("cantidad", cant);
                await this.context.Database.ExecuteSqlRawAsync(sql, pamidproducto, pamCantidad);
            }


        }

        //eliminar pedido
        public async Task CancelarPedido()
        {
            string sql = "sp_delete_last";
            await this.context.Database.ExecuteSqlRawAsync(sql);

        }

        /*///////////////////////////////////////////////CATEGORIAS//////////*/
        public List<Categoria> GetAllCategorias()
        {
            return this.context.Categorias.ToList();
        }



        //VISTAS PARCIALES///////////////////////////////////////////////
        public List<Restaurante> FindRestauranteOnCategoria(int idcategoria)
        {
            var consulta = from data in this.context.Restaurantes
                           where data.Id_categoria == idcategoria
                           select data;
            return consulta.ToList();
        }

        public List<Restaurante> GetRestaurantesByDinero(int cantidad)
        {
            string sql = "sp_filtro_dinero @dinero";
            SqlParameter pamdinero = new SqlParameter("@dinero", cantidad);
            var consulta = this.context.Restaurantes.FromSqlRaw(sql, pamdinero);
            return consulta.ToList();
        }

        public Restaurante GetRestauranteByName(string name)
        {
            return this.context.Restaurantes.FirstOrDefault(z => z.Nombre_restaurante == name);
        }
    }
}
