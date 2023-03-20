using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExtraSliceV2.Models
{
    [Table("productos")]
    public class Producto
    {
        [Key]
        [Column("id")]
        public int IdProducto { get; set; }
        [Column("nombre_producto")]
        public string Nombre_producto { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("id_restaurante")]
        public int IdRestaurante { get; set; }
    }
}
