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
using System.Windows.Threading;

namespace punto_venta
{
    /// <summary>
    /// Lógica de interacción para Venta.xaml
    /// </summary>
    public partial class Venta : Window
    {
        public Venta()
        {
            InitializeComponent();
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            txtHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

       

        private void btnVer_Productos(object sender, RoutedEventArgs e)
        {
            Product_View popup = new Product_View();
            popup.ShowDialog(); // Esto muestra la ventana emergente como un cuadro de diálogo modal
        }

        private void btnVer_Membresias(object sender, RoutedEventArgs e)
        {
            membresias_vistas popup = new membresias_vistas();
            popup.ShowDialog();
        }

        private void btnVer_Usuarios(object sender, RoutedEventArgs e)
        {
            Usuarios_vistas popup = new Usuarios_vistas();
            popup.ShowDialog();
        }

        private void btnValidar_membresias(object sender, RoutedEventArgs e)
        {
            

        }

        private void btnBuscar_Producto(object sender, RoutedEventArgs e)
        {
            string inputBuscar = txtCodigoProducto.Text;
            List<ProductoViewModel> resultadosViewModel = new List<ProductoViewModel>();

            using (var context = new DBConnection())
            {
                try
                {
                    List<Productos> resultados = null;

                    if (int.TryParse(inputBuscar, out int id))
                    {
                        // Buscar por ID
                        resultados = context.productos
                            .Where(u => u.id == id)
                            .ToList();
                    }
                    else
                    {
                        // Buscar por nombre o código de barras
                        resultados = context.productos
                            .Where(u => u.nombre_producto.Contains(inputBuscar) || u.codigo_barras == inputBuscar)
                            .ToList();
                    }

                    if (resultados.Count == 0)
                    {
                        MessageBox.Show("No se encontraron resultados.");
                        return;
                    }

                    foreach (var producto in resultados)
                    {
                        ProductoViewModel productoViewModel = resultadosViewModel.FirstOrDefault(p => p.Id == producto.id);

                        if (productoViewModel != null)
                        {
                            // Si el producto ya existe en resultadosViewModel, actualiza la cantidad y el total.
                            productoViewModel.Cantidad += 1;
                            productoViewModel.TotalProducto = productoViewModel.Cantidad * productoViewModel.PrecioUnitario;
                        }
                        else
                        {
                            // Si el producto no existe en resultadosViewModel, agrégalo.
                            productoViewModel = new ProductoViewModel
                            {
                                Id = producto.id,
                                NombreProducto = producto.nombre_producto,
                                CodigoBarras = producto.codigo_barras,
                                PrecioUnitario = producto.precio_venta,
                                Cantidad = 1, // Inicializamos con cantidad 1 por defecto
                                TotalProducto = producto.precio_venta
                            };
                            resultadosViewModel.Add(productoViewModel);
                        }
                    }


                    dataGridVenta.ItemsSource = resultadosViewModel;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los resultados: " + ex.Message);
                }
            }
        }

    }
}
