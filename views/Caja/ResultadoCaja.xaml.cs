using iText.Html2pdf;
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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Html2pdf;
using punto_venta.models;
using System.Collections.ObjectModel;
using iText.Kernel.Geom;

namespace punto_venta.views.Caja
{
    public partial class ResultadoCaja : Window
    {
        private double dEfectivoInicial;
        private double dEfectivoEntrante;
        private double dEfectivoSaliente;
        private double dEfectivoFinal;
        private double dEfectivoFaltante;
        private double dEfectivoSobrante;
        private int id_corte_cajas;
        private int userId;
        private User usuario;
        public ResultadoCaja(int userId, int id_corte_cajas, double efectivoInicial, double efectivoEntrante, double efectivoSaliente, double efectivoFinal, double efectivoFaltante, double efectivoSobrante)
        {
            InitializeComponent();

            dEfectivoInicial = efectivoInicial;
            dEfectivoEntrante = efectivoEntrante;
            dEfectivoSaliente = efectivoSaliente;
            dEfectivoFinal = efectivoFinal;
            dEfectivoFaltante = efectivoFaltante;
            dEfectivoSobrante = efectivoSobrante;
            this.id_corte_cajas = id_corte_cajas;
            this.userId = userId;
            this.efectivoInicial.Text = dEfectivoInicial.ToString();
            this.efectivoEntrante.Text = dEfectivoEntrante.ToString();
            this.efectivoSaliente.Text = dEfectivoSaliente.ToString();
            this.efectivoFinal.Text = dEfectivoFinal.ToString();
            this.efectivoFaltante.Text = dEfectivoFaltante.ToString();
            this.efectivoSobrante.Text = dEfectivoSobrante.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var context = new DBConnection())
                {
                     usuario = (from x in context.users
                                   where x.id == userId
                                   select x).FirstOrDefault();

                    if (usuario == null)
                    {
                        throw new Exception("Ocurrió un problema al generar la información.");
                    }

                    userId = usuario.id;
                }
                string htmlTemplate = Properties.Resources.CierreCaja;
                string ticketHtml = htmlTemplate.Replace("@dEfectivoInicial", dEfectivoInicial.ToString());
                ticketHtml = ticketHtml.Replace("@Ticket", id_corte_cajas.ToString());
                ticketHtml = ticketHtml.Replace("@Usuario", usuario.usuario);
                ticketHtml = ticketHtml.Replace("@FechaVenta", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                ticketHtml = ticketHtml.Replace("@dEfectivoEntrante", dEfectivoEntrante.ToString());
                ticketHtml = ticketHtml.Replace("@dEfectivoSaliente", dEfectivoSaliente.ToString());
                ticketHtml = ticketHtml.Replace("@dEfectivoFinal", dEfectivoFinal.ToString());
                ticketHtml = ticketHtml.Replace("@dEfectivoFaltante", dEfectivoFaltante.ToString());
                ticketHtml = ticketHtml.Replace("@dEfectivoSobrante", dEfectivoSobrante.ToString());

                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, new PageSize(380, 420));

                HtmlConverter.ConvertToPdf(ticketHtml, pdf, new ConverterProperties());

                string pdfFilePath = id_corte_cajas + "_CIERRE" + ".pdf";
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
}
