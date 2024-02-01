using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class Ventas
    {
        [Key] // Esto indica que Id es la clave primaria
        public int id { get; set; }
        public int vouchers_id { get; set; }
        public string estatus { get; set; }
        public double monto_recibido { get; set; }
        public double cambio { get; set; } 
        public int creado_por { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    }
}
