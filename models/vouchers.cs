using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class vouchers
    {
        public int id { get; set; }
        public int carritos_id { get; set; }
        public int corte_cajas_id { get; set; }
        public int cantidad { get; set; }
        public double precio_total { get; set; }
        public string vendedor { get; set; }
        public double cantidad_pagada { get; set; }
        public double cantidad_pagada_efectivo { get; set; }
        public double cantidad_pagada_tarjeta { get; set; }
         public double cambio_efectivo { get; set; }
        public string estatus { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
