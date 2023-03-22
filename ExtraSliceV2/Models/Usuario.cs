using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExtraSliceV2.Models
{
    [Table("clientes")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int IdUser { get; set; }
        [Column("nombre_cliente")]
        public string Nombre_cliente { get; set; }
        [Column("direccion")]
        public string Direccion { get; set; }
        [Column("telefono")]
        public string Telefono { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("passcifrado")]
        public byte[] PasswordCifrado { get; set; }
        [Column("pass")]
        public string Password { get; set; }
        [Column("salt")]
        public string Salt { get; set; }
    }
}
