﻿using punto_venta.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
        private int tipo_membresias_id;
        private int users_id;
        private int carritos_id;
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
                    var voucher = (from x in context.vouchers
                                   where x.id == folio
                                   select x).FirstOrDefault();

                    if (voucher == null)
                    {
                        cResultado.Text = "No se encontraron resultados del ticket.";
                        cResultado.Foreground = Brushes.Red;
                        return;
                    }

                    var carrito = (from x in context.carritos
                                   where x.id == voucher.carritos_id
                                   select x).FirstOrDefault();

                    if (carrito == null)
                    {
                        cResultado.Text = "No se encontraron resultados del carrito.";
                        cResultado.Foreground = Brushes.Red;
                        return;
                    }

                    var existeMembresia = (from x in context.carrito_has_productos
                                           where x.carritos_id == carrito.id
                                           && x.lMembresia == true
                                           select x).FirstOrDefault();

                    if (existeMembresia == null)
                    {
                        cResultado.Text = "El ticket no tiene comprado una membresía";
                        cResultado.Foreground = Brushes.Red;
                        return;
                    }

                    var membresia = (from x in context.tipo_membresias
                                     where x.id == existeMembresia.productos_id
                                     select x).FirstOrDefault();

                    if (membresia == null)
                    {
                        cResultado.Text = "El ticket no tiene comprado una membresía";
                        cResultado.Foreground = Brushes.Red;
                        return;
                    }
                    else
                    {
                        tipo_membresias_id = membresia.id;
                        users_id = usuarioSeleccionado.id;
                        carritos_id = carrito.id;
                        cResultado.Text = $"Se va a asociar la membresía: {membresia.nombre_membresia}.";
                        cResultado.Foreground = Brushes.Green;
                        btnActivar.IsEnabled = true;
                    }

                }
                else
                {
                    MessageBox.Show("Ingrese un folio válido.");
                }

            }
        }

        private void btnActivar_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DBConnection())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string fecha_inicio = "01/01/2023";
                        DateOnly fecha_inicio_date;
                        DateOnly fecha_expiracion_date;

                        if (DateOnly.TryParse(fecha_inicio, out DateOnly output))
                        {
                            fecha_inicio_date = output;
                        }
                        else
                        {
                            MessageBox.Show("Error de fecha.");
                        }

                        if (DateOnly.TryParse(fecha_inicio, out DateOnly output2))
                        {
                            fecha_expiracion_date = output;
                        }
                        else
                        {
                            MessageBox.Show("Error de fecha.");
                        }

                        usuario_membresias usuario_membresia = new usuario_membresias()
                        {
                            users_id = users_id,
                            fecha_inicio = fecha_inicio_date,
                            fecha_expiracion = fecha_expiracion_date,
                            carritos_id = carritos_id,
                            tipo_membresias_id = tipo_membresias_id,
                        };

                        context.usuario_membresias.Add(usuario_membresia);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Windows.Forms.MessageBox.Show($"Error: {ex.Message} at {ex.ToString()}", "Error");
                    }
                }
            }
        }
    }
}