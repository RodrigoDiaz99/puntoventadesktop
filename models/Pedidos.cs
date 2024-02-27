using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace punto_venta.models
{
    public class Pedidos
    {
        [Key] 
        public int id { get; set; }
        public string numero_orden { get; set; }
        public string linea_referencia { get; set; }
        public string? comentarios { get; set; }
        public string estatus { get; set; }
        public bool? cobrado { get; set; }
        public int users_id { get; set; }
        public double precio { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    }
}
