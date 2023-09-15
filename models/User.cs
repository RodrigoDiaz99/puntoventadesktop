using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace punto_venta.models
{

    public class User
    {
        [Key] // Esto indica que Id es la clave primaria
        public int id { get; set; }

        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Usuario { get; set; }
        public string CodigoUsuario { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string TelefonoContacto { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public string Password { get; set; }
        public string HuellaDigital { get; set; }
        public string FotoPerfil { get; set; }
        public string Ocupacion { get; set; }
        public int Edad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Expediente { get; set; }
        public bool Cliente { get; set; }
        public bool Empleado { get; set; }
        public int RolesId { get; set; }
        public string RememberToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

}
