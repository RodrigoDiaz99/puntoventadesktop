using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class usuario_membresias
    {
        public int id { get; set; }
        public int users_id { get; set; }
        public DateOnly fecha_inicio { get; set; }
        public DateOnly fecha_expiracion { get; set; }

        public int carritos_id { get; set; }
        public int tipo_membresias_id { get; set; }
        public bool estatus_membresia { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
