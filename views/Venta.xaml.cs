using punto_venta.models;
using punto_venta.views;
using punto_venta.views.Caja;
using punto_venta.views.Membresias;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Printing;
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
    /// 
    public partial class Venta : Window
    {
        private StringBuilder codigoBarras = new StringBuilder();

        public ObservableCollection<CarritoModel> objetoVenta;
        public double? sumPrecioUnitario;
        public string formattedSum;
        public int userId;
        public int cortes_caja_id;
        public Venta(bool sesionActiva, int userId, int cortes_caja_id, string cUsuario)
        {
            InitializeComponent();
            gridAccionesCajaAbierta.Visibility = Visibility.Visible;
            gridAccionesCajaCerrada.Visibility = Visibility.Hidden;
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtUsuario.Text = cUsuario;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            objetoVenta = new ObservableCollection<CarritoModel>();
            dataGridProductosVenta.ItemsSource = objetoVenta;
            formattedSum = "";
            this.userId = userId;
            this.cortes_caja_id = cortes_caja_id;
            if (sesionActiva)
            {
                // Mostrar ventana de aviso
                MessageBox.Show("La última vez que se cerró la aplicación, no se realizó un corte de caja. Se continuará con la sesión.", "Atención", MessageBoxButton.OK, MessageBoxImage.Question);
            }
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

        private void btnAperturarCaja_Click(object sender, RoutedEventArgs e)
        {
            AperturaCaja aperturaCaja = new AperturaCaja(userId);
            aperturaCaja.ShowDialog();
            this.Close();
        }

        private void btnVerRealizarCorte(object sender, RoutedEventArgs e)
        {
            CerrarCaja cerrarCaja = new CerrarCaja(this, userId, cortes_caja_id);
            cerrarCaja.ShowDialog();
        }

        private void btnValidar_membresias(object sender, RoutedEventArgs e)
        {
            ValidarMembresias validarMembresias = new ValidarMembresias();
            validarMembresias.ShowDialog();
        }

        private void btnCobrar_Click(object sender, RoutedEventArgs e)
        {
            Pagos pago = new Pagos(this, cortes_caja_id);
            pago.ShowDialog();
        }

        private void verificarCarrito()
        {
            btnCobrar.IsEnabled = (objetoVenta.Count > 0);
        }

        public void reiniciarVenta()
        {
            objetoVenta.Clear();
            sumPrecioUnitario = objetoVenta.Sum(item => item.Subtotal);
            CollectionViewSource.GetDefaultView(dataGridProductosVenta.ItemsSource).Refresh();

            // Formatear y mostrar en tus controles
            formattedSum = sumPrecioUnitario.HasValue ? sumPrecioUnitario.Value.ToString("C") : "N/A";
            iImporte.Text = formattedSum;
            iSubtotal.Text = formattedSum;
            iTotal.Text = formattedSum;
            verificarCarrito();
        }

        public void agregarProducto(CarritoModel producto)
        {
            // Supongamos que 'producto' es el nuevo producto que deseas agregar

            // Buscar si ya existe un producto con el mismo internal_id y esMembresia
            CarritoModel existingProduct = objetoVenta.FirstOrDefault(item => item.internal_id == producto.internal_id && item.esMembresia == producto.esMembresia);
            CarritoModel validarMembresia = objetoVenta.FirstOrDefault(item => item.esMembresia == true);

            if (validarMembresia != null && producto.esMembresia == true)
            {
                MessageBox.Show("Solo puede agregar una membresía a la vez.", "Atención", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }

            if (existingProduct != null)
            {
                if (existingProduct.esMembresia != true)
                {
                    // El producto ya existe, simplemente incrementar la cantidad
                    existingProduct.CantidadCarrito += producto.CantidadCarrito; // Sumar 1 si la cantidad es nula
                    existingProduct.Subtotal += producto.Subtotal; // Sumar 1 si la cantidad es nula
                }
                else
                {
                    MessageBox.Show("Solo puede agregar una membresía a la vez.", "Atención", MessageBoxButton.OK, MessageBoxImage.Question);
                }

            }
            else
            {
                // El producto no existe, agregarlo a la colección
                objetoVenta.Add(producto);
            }

            // Recalcular la suma de PrecioUnitario después de la posible modificación
            sumPrecioUnitario = objetoVenta.Sum(item => item.Subtotal);
            CollectionViewSource.GetDefaultView(dataGridProductosVenta.ItemsSource).Refresh();

            // Formatear y mostrar en tus controles
            formattedSum = sumPrecioUnitario.HasValue ? sumPrecioUnitario.Value.ToString("C") : "N/A";
            iImporte.Text = formattedSum;
            iSubtotal.Text = formattedSum;
            iTotal.Text = formattedSum;
            verificarCarrito();
        }



        private void escanearProducto(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string textoCapturado = codigoBarras.ToString();
                codigoBarras.Clear();
                obtenerProductoEscaneado(textoCapturado);
            }
            else
            {
                string caracter = e.Key.ToString();
                if (caracter.Length == 1)
                {
                    codigoBarras.Append(caracter);
                }
                else if (caracter.StartsWith("D") && caracter.Length == 2 && char.IsDigit(caracter[1]))
                {
                    codigoBarras.Append(caracter[1]);
                }
            }
        }

        private void obtenerProductoEscaneado(string codigoBarras)
        {
            using (var context = new DBConnection())
            {
                try
                {
                    Productos producto = null;

                    producto = context.productos
                        .Where(u => u.codigo_barras == codigoBarras)
                        .FirstOrDefault();

                    if (producto == null)
                    {
                        MessageBox.Show("No se encontraron resultados.");
                        return;
                    }

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

                    this.agregarProducto(productoSeleccionado);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los resultados: " + ex.Message);
                }
            }
        }

        private void verTickets_Click(object sender, RoutedEventArgs e)
        {
            Tickets ventana = new Tickets();
            ventana.ShowDialog();
        }
    }
}
