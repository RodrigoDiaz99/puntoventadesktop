using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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

namespace punto_venta
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class AsignarMembresias : Window
    {
        public AsignarMembresias()
        {
            InitializeComponent();
            using (var context = new DBConnection())
            {
                // Obtener todos los productos desde la base de datos
                var clientes = (from x in context.users
                                  where x.cliente == true
                                  select x).ToList();

                // Configura la fuente de datos del DataGrid
                dataGridClientes.ItemsSource = clientes;

            }

        }

        private void Asignar(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Error al obtener los resultados: ");

        }

    }
}
