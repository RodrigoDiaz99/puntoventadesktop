using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class Productos
    {
        [Key] // Esto indica que Id es la clave primaria
        public int id { get; set; }
        [DisplayName("Producto")]
        public string? nombre_producto { get; set; }
        [DisplayName("Código de Barras")]
        public string codigo_barras { get; set; }
        public bool? inventario { get; set; }
        [DisplayName("Disponible")]
        public int? cantidad_producto { get; set; }
        public int? alerta_minima { get; set; }
        public int? alerta_maxima { get; set; }
        [DisplayName("Precio de venta")]
        public double? precio_venta { get; set; }
        public int? users_id { get; set; }
        public string? estatus { get; set; }
        public int? proveedores_id { get; set; }
        public int? categoria_productos_id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    }
}
