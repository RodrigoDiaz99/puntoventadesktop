using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Printing;
using System.Diagnostics;
using punto_venta.views.Pago;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Html2pdf;
using punto_venta.models;
using System.Collections.ObjectModel;
using iText.Kernel.Geom;
namespace punto_venta.views
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Pagos : Window
    {
        private Venta venta;
        private double TotalPagar;
        private double Efectivo;
        private double Tarjeta;
        private double PagoTotal;
        private double Cambio;
        private double EfectivoTmp;
        private int cortes_caja_id;
        private int userId;
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;

            // Verificar si el texto ingresado es un número o un punto decimal
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != ".")
            {
                e.Handled = true;
            }

            // Verificar si ya hay un punto decimal en el texto
            if (e.Text == "." && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = (System.Windows.Controls.TextBox)sender;

            // Permitir las teclas de borrar (Backspace y Delete)
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                return;
            }


            // Permitir las teclas de control como Ctrl+C, Ctrl+V, etc.
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl || e.Key == Key.C || e.Key == Key.V)
            {
                return;
            }

            // Prevenir la entrada de caracteres no válidos
            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Decimal)
            {
                e.Handled = true;
            }

            // Verificar la posición del punto decimal
            if (e.Key == Key.Decimal && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        public void validarPago()
        {
            btnFinalizarPago.IsEnabled = PagoTotal >= TotalPagar;
        }

        public void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
        public Pagos(Venta venta, int cortes_caja_id)
        {
            InitializeComponent();
            this.venta = venta;
            userId = venta.userId;
            TotalPagar = TotalPagar = (double)venta.sumPrecioUnitario;
            dTotalPagar.Text = venta.formattedSum;
            this.cortes_caja_id = cortes_caja_id;
            validarPago();

        }

        private void dEfectivo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(dEfectivo.Text))
            {
                Efectivo = 0;
            }
            else if (double.TryParse(dEfectivo.Text, out double efectivoValue))
            {
                Efectivo = efectivoValue;
            }

            PagoTotal = Efectivo + Tarjeta;
            EfectivoTmp = TotalPagar - Tarjeta;
            Cambio = Efectivo - EfectivoTmp;
            dTotalPagado.Text = PagoTotal.ToString("C");
            dCambio.Text = Cambio.ToString("C");
            validarPago();
        }

        private void dTarjeta_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(dTarjeta.Text))
            {
                Tarjeta = 0;
            }
            else if (double.TryParse(dTarjeta.Text, out double tarjetaValue))
            {
                Tarjeta = tarjetaValue;
            }

            PagoTotal = Efectivo + Tarjeta;
            EfectivoTmp = TotalPagar - Tarjeta;
            Cambio = Efectivo - EfectivoTmp;
            dTotalPagado.Text = PagoTotal.ToString("C");
            dCambio.Text = Cambio.ToString("C");
            validarPago();
        }

        private void btnFinalizarPago_Click(object sender, RoutedEventArgs e)
        {

            using (var context = new DBConnection())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        int voucher_id;

                        var usuario = (from x in context.users
                                       where x.id == userId
                                       select x).FirstOrDefault();

                        ObservableCollection<CarritoModel> objetoVenta = venta.objetoVenta;
                        int totalProductos = objetoVenta.Count;

                        Carritos nuevoCarrito = new Carritos
                        {
                            users_id = 1,
                            numero_venta = "000",
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now
                        };

                        context.carritos.Add(nuevoCarrito);
                        context.SaveChanges();
                        string productoRow = "";
                        foreach (CarritoModel carrito in objetoVenta)
                        {
                            carrito_has_productos carritoProductos = new carrito_has_productos
                            {
                                carritos_id = nuevoCarrito.id,
                                productos_id = carrito.internal_id,
                                cantidad = carrito.CantidadCarrito,
                                lMembresia = carrito.esMembresia,
                                lPedido = false,
                                created_at = DateTime.Now,
                                updated_at = DateTime.Now
                            };
                            context.carrito_has_productos.Add(carritoProductos);
                            context.SaveChanges();

                            // Sección para el ticket

                            productoRow += "<tr>";
                            productoRow += "<td>" + carrito.CantidadCarrito + "x" + carrito.NombreProducto + " </td>";
                            productoRow += "<td>" + carrito.PrecioUnitario + "</td>";
                            productoRow += "<td>" + carrito.CantidadCarrito * carrito.PrecioUnitario + "</td>";
                            productoRow += "</tr>";

                        }

                        vouchers voucher = new vouchers
                        {
                            carritos_id = nuevoCarrito.id,
                            corte_cajas_id = this.cortes_caja_id,
                            cantidad = 1,
                            precio_total = TotalPagar,
                            vendedor = usuario.usuario,
                            cantidad_pagada = PagoTotal,
                            cantidad_pagada_efectivo = Efectivo,
                            cantidad_pagada_tarjeta = Tarjeta,
                            cambio_efectivo = Cambio,
                            estatus = "PAGADO",
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now
                        };

                        context.vouchers.Add(voucher);
                        context.SaveChanges();
                        voucher_id = voucher.id;

                        Ventas nuevaVenta = new Ventas
                        {
                            vouchers_id = voucher_id,
                            estatus = "PAGADO",
                            monto_recibido = PagoTotal,
                            cambio = Cambio,
                            creado_por = usuario.id
                        };

                        context.ventas.Add(nuevaVenta);
                        context.SaveChanges();

                        bool isImprimirTicketChecked = lImprimirTicket.IsChecked ?? false;

                        if (isImprimirTicketChecked)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                string htmlTemplate = Properties.Resources.Ticket;
                                string ticketHtml = htmlTemplate.Replace("@Usuario", usuario.usuario);
                                ticketHtml = ticketHtml.Replace("@FechaVenta", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                ticketHtml = ticketHtml.Replace("@TotalVenta", TotalPagar.ToString());
                                ticketHtml = ticketHtml.Replace("@Ticket", voucher_id.ToString("D6"));
                                ticketHtml = ticketHtml.Replace("@ListaProductos", productoRow);
                                ticketHtml = ticketHtml.Replace("@PagoEfectivo", Efectivo.ToString());
                                ticketHtml = ticketHtml.Replace("@PagoTarjeta", Tarjeta.ToString());
                                ticketHtml = ticketHtml.Replace("@CambioEfectivo", Cambio.ToString());
                                ticketHtml = ticketHtml.Replace("@CantidadArticulos", totalProductos.ToString());
                                PdfWriter writer = new PdfWriter(ms);
                                int fila_producto = 0; //
                                PdfDocument pdf = new PdfDocument(writer);
                                Document document = new Document(pdf, new PageSize(380, 420 + (totalProductos * 10)));


                                HtmlConverter.ConvertToPdf(ticketHtml, pdf, new ConverterProperties());

                                string pdfFilePath = voucher_id + ".pdf";
                                File.WriteAllBytes(pdfFilePath, ms.ToArray());

                                ProcessStartInfo psi = new ProcessStartInfo
                                {
                                    FileName = pdfFilePath,
                                    UseShellExecute = true
                                };
                                System.Diagnostics.Process.Start(psi);
                            }
                        }

                        transaction.Commit();

                        //venta.reiniciarVenta();
                        //this.Close();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Windows.Forms.MessageBox.Show($"Error: {ex.Message} at {ex.ToString()}", "Error");
                    }
                }
            }
        }

        private void dTotalPagar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
