using Org.BouncyCastle.Crypto.Generators;

using System;
using BCrypt.Net;

using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace punto_venta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
         

            string inputTexto = txtUsuario.Text;
            string inputPassword = txtPassword.Password;

            using (var context = new DBConnection())
            {
                try
                {
                    var resultados = context.users.Where(u => u.id>0).ToList();

                    var password = resultados[0].password;
                  
                    bool isMatch = BCrypt.Net.BCrypt.Verify(inputPassword, password);
                    var txt = isMatch;
                    if (isMatch)
                    {
                        var userId = resultados[0].id;
                        var resultadoCorte = context.corte_cajas
                            .Where(u => u.lActivo==true)
                            .Where(u=>u.users_id==userId)
                            .ToList();
                        if (resultadoCorte.Count > 0)
                        {
                            Venta ventanaPuntoVenta = new Venta();
                            ventanaPuntoVenta.Show();
                            this.Close();
                        }
                        else
                        {
                            CorteCaja ventanaCorteCaja = new CorteCaja(userId);
                            ventanaCorteCaja.Show();
                            this.Close();
                        }
                       
                       
                    }
                    else
                    {
                        MessageBox.Show("Contraseña Incorrecta" );
                    }

                    // Resto del código para procesar los resultados
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los resultados: " + ex.Message);
                   
                }

              
         
            }

        
       
           
        }
      
    }
}
