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

namespace punto_venta
{
    /// <summary>
    /// Lógica de interacción para Product_View.xaml
    /// </summary>
    public partial class Product_View : Window
    {
        public Product_View()
        {
            InitializeComponent();



            using (var context = new DBConnection())
            {
                // Obtener todos los productos desde la base de datos
                var productosList = context.productos.ToList();
             
                // Configura la fuente de datos del DataGrid
                dataGridProductos.ItemsSource = productosList;
            }


        }
    }
}
