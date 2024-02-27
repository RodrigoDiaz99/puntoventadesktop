using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class ProductosPedidos
    {
        public int id { get; set; }
        public int cantidad{ get; set; }
        public int productos_id { get; set; }
        public int pedidos_id { get; set; }
        public bool lActivo { get; set; }
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    }
}
