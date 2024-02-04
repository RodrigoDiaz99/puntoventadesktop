using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using punto_venta.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Lógica de interacción para Tickets.xaml
    /// </summary>
    public partial class Tickets : Window
    {
        public Tickets()
        {
            InitializeComponent();
        }

        private void btnBuscar_Folio(object sender, RoutedEventArgs e)
        {
            try
            {
                int folio;
                if (int.TryParse(txtFolioTicket.Text, out folio))
                {
                    using (var context = new DBConnection())
                    {
                        List<vouchers> voucher = context.vouchers.Where(v => v.id == folio).ToList();
                        if (voucher == null)
                        {
                            MessageBox.Show("No se encontró el ticket.");
                            return;
                        }
                        else
                        {
                            dataGridTickets.ItemsSource = voucher;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un folio válido.");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error desconocido. Por favor, intente de nuevo.");

            }

        }

        private void btnBuscar_Fecha(object sender, RoutedEventArgs e)
        {
            try
            {
                int folio;
                string fecha_inicio = dt_fecha_inicial.Text;
                string fecha_final = dt_fecha_final.Text;
                if (fecha_inicio != "" || fecha_final != "")
                {
                    using (var context = new DBConnection())
                    {
                        DateTime fechaInicio = DateTime.Parse(fecha_inicio);
                        DateTime fechaFinal = DateTime.Parse(fecha_final).AddDays(1).AddSeconds(-1);
                        if (fechaFinal < fechaInicio)
                        {
                            MessageBox.Show("Por favor, ingrese un rango de fechas válido.");
                            return;
                        }

                        List<vouchers> voucher = context.vouchers
                            .Where(v => v.created_at >= fechaInicio && v.created_at <= fechaFinal)
                            .ToList();
                        if (voucher == null)
                        {
                            MessageBox.Show("No se encontró el ticket.");
                            return;
                        }
                        else
                        {
                            dataGridTickets.ItemsSource = voucher;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un rango de fechas válido.");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error desconocido. Por favor, intente de nuevo.");

            }
        }

        private void imprimirTicket(object sender, RoutedEventArgs e)
        {

            try
            {
                vouchers ticket = (vouchers)dataGridTickets.SelectedItem;

                using (var context = new DBConnection())
                {
                    Carritos actualcarrito = context.carritos.Where(c => c.id == ticket.carritos_id).FirstOrDefault();
                    User u = context.users.Where(u => u.id == actualcarrito.users_id).FirstOrDefault();
                    vouchers v = context.vouchers.Where(v => v.id == ticket.id).FirstOrDefault();
                    var productos_carrito = (from c in context.carrito_has_productos
                                             join p in context.productos
                                             on c.productos_id equals p.id
                                             where c.carritos_id == ticket.carritos_id
                                             select new { CarritoProducto = c, Producto = p }).ToList();


                    string productoRow = "";
                    foreach (var carrito in productos_carrito)
                    {

                        productoRow += "<tr>";
                        productoRow += "<td>" + carrito.CarritoProducto.cantidad + "x" + carrito.Producto.nombre_producto + " </td>";
                        productoRow += "<td>" + carrito.CarritoProducto.precio_unitario + "</td>";
                        productoRow += "<td>" + carrito.CarritoProducto.cantidad * carrito.CarritoProducto.precio_unitario + "</td>";
                        productoRow += "</tr>";

                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        string htmlTemplate = Properties.Resources.Ticket;
                        string ticketHtml = htmlTemplate.Replace("@Usuario", u.usuario);
                        ticketHtml = ticketHtml.Replace("@FechaVenta", v.created_at.ToString("dd/MM/yyyy HH:mm:ss"));
                        ticketHtml = ticketHtml.Replace("@TotalVenta", v.precio_total.ToString());
                        ticketHtml = ticketHtml.Replace("@Ticket", v.id.ToString("D6"));
                        ticketHtml = ticketHtml.Replace("@ListaProductos", productoRow);
                        ticketHtml = ticketHtml.Replace("@PagoEfectivo", v.cantidad_pagada_efectivo.ToString());
                        ticketHtml = ticketHtml.Replace("@PagoTarjeta", v.cantidad_pagada_tarjeta.ToString());
                        ticketHtml = ticketHtml.Replace("@CambioEfectivo", v.cambio_efectivo.ToString());
                        ticketHtml = ticketHtml.Replace("@CantidadArticulos", v.cantidad.ToString());
                        PdfWriter writer = new PdfWriter(ms);
                        int fila_producto = 0; //
                        PdfDocument pdf = new PdfDocument(writer);
                        Document document = new Document(pdf, new PageSize(380, 420 + (v.cantidad * 10)));


                        HtmlConverter.ConvertToPdf(ticketHtml, pdf, new ConverterProperties());

                        string pdfFilePath = v.id + ".pdf";
                        File.WriteAllBytes(pdfFilePath, ms.ToArray());

                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = pdfFilePath,
                            UseShellExecute = true
                        };
                        System.Diagnostics.Process.Start(psi);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Ocurrió un error: {ex.Message} at {ex.StackTrace.ToString()}", "Error");

            }
        }
    }
}
