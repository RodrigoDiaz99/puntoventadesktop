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
                dataGridClientes.ItemsSource = getMembresias("");
            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string terminoBusqueda = cBusquedaMembresia.Text;



                dataGridClientes.ItemsSource = getMembresias(terminoBusqueda);

            }
            catch (Exception ex) { }
        }

        private List<dataGridMembresias> getMembresias(string terminoBusqueda)
        {
            using (var context = new DBConnection())
            {
                var query = from Users in context.users
                            join UsuarioMembresias in (from um in context.usuario_membresias
                                                       where um.estatus_membresia == true
                                                       group um by um.users_id into grupo
                                                       select new
                                                       {
                                                           id = grupo.Max(um => um.id),
                                                           users_id = grupo.Max(um => um.users_id),
                                                           fecha_inicio = grupo.Max(um => um.fecha_inicio),
                                                           fecha_expiracion = grupo.Max(um => um.fecha_expiracion),
                                                           tipo_membresias_id = grupo.Max(um => um.tipo_membresias_id),
                                                           estatus_membresia = grupo.Max(um => um.estatus_membresia),
                                                       }) on Users.id equals UsuarioMembresias.users_id into UsuarioMembresiasJoin
                            from um in UsuarioMembresiasJoin.DefaultIfEmpty()
                            join TipoMembresia in context.tipo_membresias on um.tipo_membresias_id equals TipoMembresia.id into TipoMembresiasJoin
                            from tm in TipoMembresiasJoin.DefaultIfEmpty()
                            where string.IsNullOrEmpty(terminoBusqueda) ||
                                Users.nombre.Contains(terminoBusqueda) ||
                                Users.apellido_paterno.Contains(terminoBusqueda) ||
                                Users.apellido_materno.Contains(terminoBusqueda) ||
                                Users.usuario.Contains(terminoBusqueda)
                            select new
                            {
                                nombre = Users.nombre,
                                apellido_paterno = Users.apellido_paterno,
                                apellido_materno = Users.apellido_materno,
                                usuario = Users.usuario,
                                codigo_usuario = Users.codigo_usuario != null ? Users.codigo_usuario : "N/A",
                                fecha_inicio = um.fecha_inicio,
                                fecha_expiracion = um.fecha_expiracion,
                                estatus_membresia = um.estatus_membresia,
                                nombre_membresia = tm.nombre_membresia
                            };

                resultadosBusqueda = query.Select(fila => new dataGridMembresias
                {
                    usuario = fila.usuario,
                    nombre = fila.nombre,
                    nombre_completo = fila.nombre + " " + fila.apellido_paterno + " " + fila.apellido_materno,
                    codigo_usuario = fila.codigo_usuario,
                    apellido_paterno = fila.apellido_paterno,
                    apellido_materno = fila.apellido_materno,
                    nombre_membresia = fila.nombre_membresia ?? "N/A",
                    estatus_membresia = fila.estatus_membresia == true ? "ACTIVO" : "INACTIVO",
                    fecha_inicio = fila.fecha_inicio.ToString("yyyy-MM-dd") ?? "N/A",
                    fecha_expiracion = fila.fecha_expiracion.ToString("yyyy-MM-dd") ?? "N/A"

                }).ToList();
                return resultadosBusqueda;
            }
        }


        private class dataGridMembresias
        {
            //Usuarios
            public string usuario { get; set; }
            public string codigo_usuario { get; set; }
            public string nombre { get; set; }
            public string nombre_completo { get; set; }
            public string apellido_paterno { get; set; }
            public string apellido_materno { get; set; }
            //Membresias
            public string nombre_membresia { get; set; }
            public string fecha_inicio { get; set; }
            public string fecha_expiracion { get; set; }
            public string estatus_membresia { get; set; }

        }
    }
}
