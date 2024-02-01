using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using punto_venta.models;
using System;
using System.ComponentModel.DataAnnotations;

namespace punto_venta
{
    public class DBConnection : DbContext
    {
   
        public DbSet<Productos> productos { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Corte_Caja> corte_cajas { get; set; }
        public DbSet<Membresia> tipo_membresias { get; set; }
        public DbSet<Carritos> carritos { get; set; }
        public DbSet<vouchers> vouchers{ get; set; }
        public DbSet<carrito_has_productos> carrito_has_productos { get; set; }
        public DbSet<usuario_membresias> usuario_membresias { get; set; }
        public DbSet<Ventas> ventas{ get; set; }

        public bool IsConnectionSuccessful()
        {
            try
            {
                const string connectionString = "Server=sql902.main-hosting.eu;Database=u379902634_gym_desarrollo;Uid=u379902634_gymsystem;Pwd=Sondeo23.;";

                var optionsBuilder = new DbContextOptionsBuilder<DBConnection>()
                    .UseMySql(connectionString,
                        new MySqlServerVersion(new Version(8, 0, 25))); // Especifica la versión de MySQL que estás utilizando

                using (var context = new DBConnection())
                {
                    // No es necesario hacer ninguna operación en la base de datos aquí.
                    // Solo estamos probando la conexión.
                    context.Database.OpenConnection();
                }
                return true; // Si llegamos aquí, la conexión fue exitosa.
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que pueda ocurrir al intentar abrir la conexión.
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                return false; // La conexión no fue exitosa.
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "Server=sql902.main-hosting.eu;Database=u379902634_gym_desarrollo;Uid=u379902634_gymsystem;Pwd=Sondeo23.;";
            optionsBuilder.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 25)));

          
        }
    

    }
   
}
