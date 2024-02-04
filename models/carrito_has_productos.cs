using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class carrito_has_productos
    {
        public int? id { get; set; }
        public int? carritos_id { get; set; }
        public int? productos_id { get; set; }
        public int? cantidad { get; set; }
        public bool? lMembresia { get; set; }
        public bool? lPedido { get; set; }
        public double? precio_unitario { get; set; }
        public double? precio_total { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
