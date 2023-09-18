﻿using Org.BouncyCastle.Crypto.Generators;

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
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(inputPassword);
            using (var context = new DBConnection())
            {
                try
                {
                    var resultados = context.users.Where(u => u.id>0).ToList();

                    var nombre = resultados[0].nombre;

                    Console.WriteLine("nombre: " + nombre);

                    // Resto del código para procesar los resultados
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener los resultados: " + ex.Message);
                }

              
            /*    foreach (var resultado in resultados)
                {
                    Console.WriteLine("Contraseña encriptada: " + resultados);
                    Console.WriteLine("Contraseña encriptada: " + resultados);
                }*/
            }

            bool isMatch = BCrypt.Net.BCrypt.Verify("123456", passwordHash);
       
            Console.WriteLine("Contraseña encriptada: " + passwordHash);

        }
      
    }
}
