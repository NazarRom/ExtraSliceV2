using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExtraSliceV2.Models
{
    [Table("restaurantes")]
    public class Restaurante
    {
        [Key]
        [Column("id")]
        public int IdRestaurante { get; set; }
        [Column("nombre_restaurante")]
        public string Nombre_restaurante { get; set; }

        [Column("direccion")]
        public string Direccion { get; set; }

        [Column("telefono")]
        public int Telefono { get; set; }

        [Column("id_categoria")]
        public int Id_categoria { get; set; }

        [Column("imagen")]
        public string Imagen { get; set; }


    }
}
