using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class Membresia
    {
        [Key] // Esto indica que Id es la clave primaria
        public int id { get; set; }
        public string? nombre_membresia { get; set; }
        public double precio { get; set; }
        public string? descripcion_membresia { get; set; }
        public int? dias_membresia { get; set; }
       
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
