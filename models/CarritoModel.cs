using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class CarritoModel
    {
        [DisplayName("internal_id")]
        public int internal_id { get; set; }
        [DisplayName("Producto")]
        public string? NombreProducto { get; set; }
        [DisplayName("Código de Barras")]
        public string? CodigoBarras { get; set; }
        [DisplayName("Precio Unitario")]
        public double? PrecioUnitario { get; set; }
        [DisplayName("Subtotal")]
        public double? Subtotal { get; set; }
        [DisplayName("Cantidad")]
        public double? CantidadCarrito { get; set; }
        public bool? esMembresia { get; set; }
    }
}
