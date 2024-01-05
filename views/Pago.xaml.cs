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
using System.Text.RegularExpressions;
using System.Printing;

namespace punto_venta.views
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Pago : Window
    {
        private Venta venta;
        private double TotalPagar;
        private double Efectivo;
        private double Tarjeta;
        private double PagoTotal;
        private double Cambio;
        private double EfectivoTmp;
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

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

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

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


        public Pago(Venta venta)
        {
            InitializeComponent();
            this.venta = venta;
            TotalPagar = TotalPagar = (double)venta.sumPrecioUnitario;
            dTotalPagar.Text = venta.formattedSum;
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
            dTotalPagado.Text = PagoTotal.ToString();
            dCambio.Text = Cambio.ToString();


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
            dTotalPagado.Text = PagoTotal.ToString();
            dCambio.Text = Cambio.ToString();


        }
    }
}
