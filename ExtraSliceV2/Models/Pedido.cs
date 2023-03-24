using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExtraSliceV2.Models
{
    [Table("pedidos")]
    public class Pedido
    {
        [Key]
        [Column("Key")]
        public int IdPedido { get; set; }
        [Column("fecha_hora")]
        public DateTime Fecha_hora { get; set; }
        [Column("id_cliente")]
        public int IdCliente { get; set; }
    }
}
