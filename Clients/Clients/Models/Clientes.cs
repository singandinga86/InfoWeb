using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clients.Models
{
    [Table("CLIENTES")]
    public partial class Clientes
    {
        [Column("ID")]
        public long Id { get; set; }
        [Required]
        [Column("NOMBRE_COMPLETO", TypeName = "varchar(200)")]
        public string NombreCompleto { get; set; }
        [Column("IDENTIFICACION", TypeName = "numeric")]
        public decimal Identificacion { get; set; }
        [Column("TELEFONO", TypeName = "numeric")]
        public decimal? Telefono { get; set; }
    }
}
