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

namespace punto_venta.views.Membresias
{
    /// <summary>
    /// Lógica de interacción para ValidarMembresias.xaml
    /// </summary>
    public partial class ValidarMembresias : Window
    {
        private List<dataGridMembresias> resultadosBusqueda = new List<dataGridMembresias>();

        public ValidarMembresias()
        {
            InitializeComponent();
            using (var context = new DBConnection())
            {
                List<User> clientes = context.users.ToList();
                dataGridClientes.ItemsSource = clientes;
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new DBConnection())
                {
                    string terminoBusqueda = cBusquedaMembresia.Text;

                    var query = (from User in context.users
                                 join UsuarioMembresia in
                                     (from um in context.usuario_membresias
                                      where um.estatus_membresia == true
                                      group um by um.users_id into umGroup
                                      select new
                                      {
                                          id = umGroup.Max(x => x.id),
                                          users_id = umGroup.Key,
                                          fecha_inicio = umGroup.First().fecha_inicio,
                                          fecha_expiracion = umGroup.First().fecha_expiracion,
                                          tipo_membresias_id = umGroup.First().tipo_membresias_id
                                      }) on User.id equals UsuarioMembresia.users_id into umJoin
                                 from um in umJoin.DefaultIfEmpty()
                                 join tipo_membresias in context.tipo_membresias on (um != null ? um.tipo_membresias_id : (int?)null) equals tipo_membresias.id into tipoMembresiasJoin
                                 from tipo_membresias in tipoMembresiasJoin.DefaultIfEmpty()
                                 where string.IsNullOrEmpty(terminoBusqueda) ||
                                       User.nombre.Contains(terminoBusqueda) ||
                                       User.apellido_paterno.Contains(terminoBusqueda) ||
                                       User.apellido_materno.Contains(terminoBusqueda) ||
                                       User.usuario.Contains(terminoBusqueda)
                                 select new
                                 {
                                     User.nombre,
                                     User.apellido_paterno,
                                     User.apellido_materno,
                                     User.usuario,
                                     codigo_usuario = User.codigo_usuario != null ? User.codigo_usuario : "N/A",
                                     fecha_inicio = um != null ? um.fecha_inicio.ToString("yyyy-MM-dd") : "N/A",
                                     fecha_expiracion = um != null ? um.fecha_expiracion.ToString("yyyy-MM-dd") : "N/A",
                                     tipo_membresias_id = um != null ? um.tipo_membresias_id : (int?)null,
                                     nombre_membresia = tipo_membresias != null ? tipo_membresias.nombre_membresia : null
                                 }).ToList();

                    resultadosBusqueda = query.Select(fila => new dataGridMembresias
                    {
                        usuario = fila.usuario,
                        nombre = fila.nombre,
                        codigo_usuario = fila.codigo_usuario,
                        apellido_paterno = fila.apellido_paterno,
                        apellido_materno = fila.apellido_materno,
                        nombre_membresia = fila.nombre_membresia,
                        estatus_membresia = "ACTIVO",
                        fecha_inicio = fila.fecha_inicio,
                        fecha_expiracion = fila.fecha_expiracion
                    }).ToList();

                    dataGridClientes.ItemsSource = resultadosBusqueda;
                }

            }
            catch (Exception ex) { }
        }

        private class dataGridMembresias
        {
            //Usuarios
            public string usuario { get; set; }
            public string codigo_usuario { get; set; }
            public string nombre { get; set; }
            public string apellido_paterno { get; set; }
            public string apellido_materno { get; set; }
            //Membresias
            public string? nombre_membresia { get; set; }
            public string? fecha_inicio { get; set; }
            public string? fecha_expiracion { get; set; }
            public string? estatus_membresia { get; set; }

        }
    }
}
