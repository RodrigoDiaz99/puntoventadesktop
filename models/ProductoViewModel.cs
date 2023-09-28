using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{
    internal class ProductoViewModel
    {
        [DisplayName("Identificador")]
        public int Id { get; set; }
        [DisplayName("Nombre Producto")]
        public string? NombreProducto { get; set; }
        [DisplayName("Código de Barras")]
        public string? CodigoBarras { get; set; }
        [DisplayName("Precio Unitario")]
        public double? PrecioUnitario { get; set; }
        public int? Cantidad { get; set; }
        [DisplayName("Total")]
        public double? TotalProducto { get; set; }


    }
}
