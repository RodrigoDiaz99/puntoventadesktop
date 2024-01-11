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

namespace punto_venta.views
{
    /// <summary>
    /// Lógica de interacción para UsuarioMembresia.xaml
    /// </summary>

    public partial class UsuarioMembresia : Window
    {
        private User usuarioSeleccionado;

        public UsuarioMembresia(User usuarioSeleccionado)
        {
            InitializeComponent();
            this.usuarioSeleccionado = usuarioSeleccionado;
            var nombre = usuarioSeleccionado.nombre + " " + usuarioSeleccionado.apellido_paterno + " " + usuarioSeleccionado.apellido_materno;
            cNombreCliente.Text = nombre;
        }

        private void btnBuscarMembresia_Click(object sender, RoutedEventArgs e)
        {
            // Obtener todos los productos desde la base de datos
            using (var context = new DBConnection())
            {
                string folioStr = cFolioCompra.Text;

                // Asegúrate de manejar posibles errores de conversión
                if (int.TryParse(folioStr, out int folio))
                {
                    var carrito = (from x in context.carritos
                                   where x.id == folio
                                   select x).FirstOrDefault();

                    if (carrito != null)
                    {
                        MessageBox.Show("No se encontraron resultados.");
                    }
                    else
                    {
                        btnActivar.IsEnabled = true;
                    }

                }
                else
                {
                    MessageBox.Show("Ingrese un folio válido.");
                }

            }
        }
    }
}
