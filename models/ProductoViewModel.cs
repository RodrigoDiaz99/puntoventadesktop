using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    public class ProductoViewModel
    {
        [DisplayName("Identificador")]
        public int Id { get; set; }
        [DisplayName("Producto")]
        public string? NombreProducto { get; set; }
        [DisplayName("Código de Barras")]
        public string? CodigoBarras { get; set; }
        [DisplayName("Precio Unitario")]
        public double PrecioUnitario { get; set; }
        [DisplayName("Disponible")]
        public string Cantidad { get; set; }
        public bool? esMembresia { get; set; }

       
    }
}
