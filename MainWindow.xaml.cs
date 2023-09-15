using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            string hashedPassword = CalcularHash(inputPassword);

        }
        private string CalcularHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convierte la cadena de entrada en bytes
                byte[] bytes = Encoding.UTF8.GetBytes(input);

                // Calcula el hash y lo convierte a una cadena hexadecimal
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    stringBuilder.Append(b.ToString("x2")); // "x2" significa formato hexadecimal
                }

                return stringBuilder.ToString();
            }
        }
    }
}
