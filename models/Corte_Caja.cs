using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class Corte_Caja
    {
        [Key] // Esto indica que Id es la clave primaria
        public int id { get; set; }
        public int? users_id { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public DateTime? fecha_final { get; set; }
        public double? cantidad_inicial { get; set; }
        public double? cantidad_final { get; set; }
        public double? total_venta { get; set; }
        public double? diferencia { get; set; }
        public bool? lActivo { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }
}
