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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void btnVer_Productos(object sender, RoutedEventArgs e)
        {
            Product_View popup = new Product_View();
            popup.ShowDialog(); // Esto muestra la ventana emergente como un cuadro de diálogo modal
        }
    }
}
