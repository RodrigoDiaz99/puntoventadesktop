using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;
using punto_venta.models;
using System;

namespace punto_venta
{
    public class DBConnection : DbContext
    {
        public DbSet<User> users { get; set; }

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
