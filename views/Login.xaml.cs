using Org.BouncyCastle.Crypto.Generators;

using System;
using BCrypt.Net;

using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;
using punto_venta.models;

namespace punto_venta
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            btnLogin.Content = "Iniciar sesión";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {

            btnLogin.Content = "Iniciando sesión...";
            btnLogin.IsEnabled = false;

            string inputTexto = txtUsuario.Text;
            string inputPassword = txtPassword.Password;

            using (var context = new DBConnection())
            {
                try
                {

                    User usuario = await Task.Run(() => context.users.Where(u => u.usuario == inputTexto).FirstOrDefault());
                    if (usuario == null)
                    {
                        MessageBox.Show("El usuario o contraseña son incorrectos. Verifique los datos.");
                        return;
                    }

                    var password = usuario.password;

                    bool isMatch = await Task.Run(() => BCrypt.Net.BCrypt.Verify(inputPassword, password));

                    if (isMatch)
                    {
                        var userId = usuario.id;
                        var resultadoCorte = context.corte_cajas
                            .Where(u => u.lActivo == true)
                            .Where(u => u.users_id == userId)
                            .FirstOrDefault();

                        if (resultadoCorte != null)
                        {
                            Venta ventanaPuntoVenta = new Venta(true, userId, resultadoCorte.id, usuario.usuario);
                            ventanaPuntoVenta.Show();
                            this.Close();
                        }
                        else
                        {
                            AperturaCaja ventanaCorteCaja = new AperturaCaja(userId);
                            ventanaCorteCaja.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("El usuario o contraseña son incorrectos. Verifique los datos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un problema: " + ex.Message);
                }
                finally
                {
                    btnLogin.IsEnabled = true;
                    btnLogin.Content = "Iniciar sesión";
                }
            }
        }


        private void txtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
