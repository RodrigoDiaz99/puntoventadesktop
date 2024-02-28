using punto_venta.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace punto_venta.views
{
    /// <summary>
    /// Lógica de interacción para CobrarPedido.xaml
    /// </summary>
    public partial class CobrarPedido : Window
    {
        private Venta venta; // Variable para almacenar la referencia a la ventana Venta
        public int id_pedido;

        public CobrarPedido(Venta venta)
        {
            InitializeComponent();
            this.venta = venta;
            dataGridPedidos.ItemsSource = getAllPedidos();
        }

        private List<Pedidos> getAllPedidos()
        {
            using (var context = new DBConnection())
            {
                List<Pedidos> allPedidos = context.pedidos
                    .Where(pedido => pedido.cobrado == false)
                            .ToList();
                return allPedidos;
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            string inputBuscar = cBuscarLinea.Text;
            // List<Productos> resultadosViewModel = new List<Productos>();

            using (var context = new DBConnection())
            {
                try
                {
                    List<Pedidos> pedidos = context.pedidos
                            .Where(u => u.linea_referencia.Contains(inputBuscar))
                              .Where(pedido => pedido.cobrado == false)
                            .ToList();

                    List<ProductoViewModel> todo = new List<ProductoViewModel>();


                    if (pedidos.Count == 0)
                    {
                        MessageBox.Show("No se encontraron resultados.");
                        return;
                    }

                    dataGridPedidos.ItemsSource = pedidos;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show($"Ocurrió un problema: {ex.Message} at {ex.ToString()}", "Error");
                }
            }
        }

        private void seleccionarPedido(object sender, RoutedEventArgs e)
        {
            try
            {
                venta.reiniciarVenta();
                Pedidos pedidoSeleccionado = (Pedidos)dataGridPedidos.SelectedItem;

                using (var context = new DBConnection())
                {
                    List<ProductosPedidos> productos_ids = context.productos_pedidos
                         .Where(productos_pedidos => productos_pedidos.pedidos_id.Equals(pedidoSeleccionado.id))
                         .ToList();
                    List<Productos> productos = new List<Productos>();

                    foreach (var producto_id in productos_ids)
                    {
                        Productos producto = context.productos.Where(producto => producto.id.Equals(producto_id.productos_id)).FirstOrDefault();
                        for (int i = 1; i <= producto_id.cantidad; i++)
                        {
                            productos.Add(producto);
                        }
                    }

                    foreach (var producto in productos)
                    {

                        CarritoModel productoSeleccionado = new CarritoModel
                        {
                            internal_id = producto.id,
                            NombreProducto = producto.nombre_producto,
                            CodigoBarras = producto.codigo_barras,
                            PrecioUnitario = (double)producto.precio_venta,
                            CantidadCarrito = 1,
                            Subtotal = (double)producto.precio_venta,
                            esMembresia = false
                        };

                        if (productoSeleccionado != null)
                        {
                            venta.agregarProducto(productoSeleccionado);
                        }

                    }

                    this.Close();
                    Pagos ventana = new Pagos(venta, venta.cortes_caja_id, pedidoSeleccionado.id);
                    ventana.ShowDialog();

                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ocurrió un error desconocido.", "Error");

            }
        }


    }
}
