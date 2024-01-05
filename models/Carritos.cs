using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class Carritos
    {
        public int id { get; set; }
        [DisplayName("Producto")]
        public int? users_id { get; set; }
        public string? numero_venta { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    }
}
