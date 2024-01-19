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

namespace punto_venta
{
    /// <summary>
    /// Lógica de interacción para CorteCaja.xaml
    /// </summary>
    public partial class AperturaCaja : Window
    {
        private int userId; // Declara una variable miembro para almacenar userId
        public AperturaCaja(int userId)
        {
            InitializeComponent();
            this.userId = userId; // Asigna el valor de userId al campo de la clase

        }

        private void btnDineroInicial(object sender, RoutedEventArgs e)
        {
            int userIdValue = this.userId;
            string inputTexto = txtDineroInicial.Text;
            try
            {
                if (int.TryParse(inputTexto, out int cantidadInicial))
                {
                    Corte_Caja nuevoCorte = new Corte_Caja()
                    {
                        users_id = userIdValue,
                        fecha_inicio = DateTime.Now, // Puedes ajustar esto según tus necesidades
                        cantidad_inicial = cantidadInicial,
                        lActivo = true, // Otra propiedad que quieras establecer
                        created_at = DateTime.Now, // Otra propiedad que quieras establecer
                    };
                    using (var context = new DBConnection())
                    {
                        context.corte_cajas.Add(nuevoCorte);
                        context.SaveChanges();
                        Venta ventanaPuntoVenta = new Venta(false, userId, nuevoCorte.id);
                        ventanaPuntoVenta.Show();
                        this.Close();

                    }
                }
                else
                {
                    MessageBox.Show("El valor en Dinero incial no es un número válido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los resultados: " + ex.Message);
            }
           
        }
    }
}
