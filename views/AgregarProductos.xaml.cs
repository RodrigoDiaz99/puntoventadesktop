using Microsoft.EntityFrameworkCore.Metadata.Internal;
using punto_venta.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace punto_venta.views
{
    public partial class AgregarProductos : Window
    {
        private Venta venta; // Variable para almacenar la referencia a la ventana Venta

        public AgregarProductos(Venta venta)
        {
            InitializeComponent();
            this.venta = venta;
        }

        private void btnBuscar_Producto(object sender, RoutedEventArgs e)
        {
            string inputBuscar = txtCodigoProducto.Text;
            // List<Productos> resultadosViewModel = new List<Productos>();

            using (var context = new DBConnection())
            {
                try
                {
                    List<Productos> resultados = null;

                    if (int.TryParse(inputBuscar, out int id))
                    {
                        // Buscar por ID
                        resultados = context.productos
                            .Where(u => u.id == id || u.codigo_barras == inputBuscar)
                            .ToList();
                    }
                    else
                    {
                        // Buscar por nombre o código de barras
                        resultados = context.productos
                            .Where(u => u.nombre_producto.Contains(inputBuscar) || u.codigo_barras == inputBuscar)
                            .ToList();
                    }

                    // Membresías

                    List<Membresia> membresias = null;

                    if (int.TryParse(inputBuscar, out int id2))
                    {

                    }
                    else
                    {
                        // Buscar por nombre o código de barras
                        membresias = context.tipo_membresias
                            .Where(u => u.nombre_membresia.Contains(inputBuscar))
                            .ToList();
                    }

                    List<ProductoViewModel> todo = new List<ProductoViewModel>();

                    foreach (var resultado in resultados)
                    {
                        ProductoViewModel producto = new ProductoViewModel
                        {
                            Id = resultado.id,
                            NombreProducto = resultado.nombre_producto,
                            CodigoBarras = resultado.codigo_barras,
                            PrecioUnitario = (double)resultado.precio_venta,
                            Cantidad = resultado.cantidad_producto.ToString(),
                            esMembresia = false
                        };
                        todo.Add(producto);
                    }

                    // Recorre "membresias" y agrega los elementos a la lista
                    foreach (var membresia in membresias)
                    {
                        ProductoViewModel producto = new ProductoViewModel
                        {
                            Id = membresia.id,
                            NombreProducto = membresia.nombre_membresia,
                            CodigoBarras = "No aplica",
                            PrecioUnitario = membresia.precio,
                            Cantidad = "No aplica",
                            esMembresia = true,
                        };
                        todo.Add(producto);
                    }



                    if (resultados.Count == 0)
                    {
                        MessageBox.Show("No se encontraron resultados.");
                        return;
                    }

                    dataGridVenta.ItemsSource = todo;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un problema: " + ex.Message);
                }
            }
        }



        private void agregarProductoAVenta(object sender, RoutedEventArgs e)
        {
            ProductoViewModel producto = (ProductoViewModel)dataGridVenta.SelectedItem;
            CarritoModel productoSeleccionado = new CarritoModel
            {
                internal_id = producto.Id,
                NombreProducto = producto.NombreProducto,
                CodigoBarras = producto.CodigoBarras,
                PrecioUnitario = producto.PrecioUnitario,
                CantidadCarrito = 1,
                Subtotal = producto.PrecioUnitario,
                esMembresia = (bool)producto.esMembresia
            };

            if (productoSeleccionado != null)
            {
                venta.agregarProducto(productoSeleccionado);
            }
        }


    }

}
