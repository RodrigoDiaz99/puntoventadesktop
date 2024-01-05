using punto_venta.models;
using punto_venta.views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private ObservableCollection<CarritoModel> venta;
        public double? sumPrecioUnitario;
        public string formattedSum;
        public Venta()
        {
            InitializeComponent();
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            venta = new ObservableCollection<CarritoModel>();
            dataGridProductosVenta.ItemsSource = venta;
            formattedSum = "";

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            txtHora.Text = DateTime.Now.ToString("HH:mm:ss");
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

        private void btnAsignar_Membresias(object sender, RoutedEventArgs e)
        {
            AsignarMembresias asignarMembresias = new AsignarMembresias();
            asignarMembresias.ShowDialog();

        }

        private void btnVerAgregarProductos(object sender, RoutedEventArgs e)
        {
            AgregarProductos agregarProductos = new AgregarProductos(this);
            agregarProductos.ShowDialog();
        }

        private void btnVerRealizarCorte(object sender, RoutedEventArgs e)
        {
            CorteCaja corteCaja = new CorteCaja(1);
            corteCaja.ShowDialog();
        }

        private void btnValidar_membresias(object sender, RoutedEventArgs e)
        {

        }

        private void btnCobrar_Click(object sender, RoutedEventArgs e)
        {
            Pago pago = new Pago(this);
            pago.ShowDialog();
        }

        public void agregarProducto(CarritoModel producto)
        {
            // Supongamos que 'producto' es el nuevo producto que deseas agregar

            // Buscar si ya existe un producto con el mismo internal_id y esMembresia
            CarritoModel existingProduct = venta.FirstOrDefault(item => item.internal_id == producto.internal_id && item.esMembresia == producto.esMembresia);

            if (existingProduct != null)
            {
                if (existingProduct.esMembresia != true)
                {
                    // El producto ya existe, simplemente incrementar la cantidad
                    existingProduct.CantidadCarrito += producto.CantidadCarrito ?? 1; // Sumar 1 si la cantidad es nula
                    existingProduct.Subtotal += producto.Subtotal ?? 1; // Sumar 1 si la cantidad es nula
                }
                else
                {
                    MessageBox.Show("Solo puede agregar una membresía a la vez.", "Atención", MessageBoxButton.OK, MessageBoxImage.Question);
                }

            }
            else
            {
                // El producto no existe, agregarlo a la colección
                venta.Add(producto);
            }

            // Recalcular la suma de PrecioUnitario después de la posible modificación
            sumPrecioUnitario = venta.Sum(item => item.Subtotal);
            CollectionViewSource.GetDefaultView(dataGridProductosVenta.ItemsSource).Refresh();

            // Formatear y mostrar en tus controles
            formattedSum = sumPrecioUnitario.HasValue ? sumPrecioUnitario.Value.ToString("C") : "N/A";
            iImporte.Text = formattedSum;
            iSubtotal.Text = formattedSum;
            iTotal.Text = formattedSum;
        }
    }
}
