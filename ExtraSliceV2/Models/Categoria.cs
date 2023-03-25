using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExtraSliceV2.Models
{
    [Table("categorias")]
    public class Categoria
    {
        [Key]
        [Column("id")]
        public int IdCategoria { get; set; }
        [Column("nombre_categoria")]
        public string NombreCategoria { get; set; }
        [Column("imagen")]
        public string Imagen { get; set; }
    }
}
