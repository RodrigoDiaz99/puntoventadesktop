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

        public string? nombre { get; set; }
        public string? apellido_paterno { get; set; }
        public string? apellido_materno { get; set; }
        public string? usuario { get; set; }
        public string? codigo_usuario { get; set; }
        public string? email { get; set; }
        public string? telefono { get; set; }
        public string? telefono_contacto { get; set; }
        public DateTime? email_verified_at { get; set; }
        public string? password { get; set; }
        public string? huella_digital { get; set; }
        public string? foto_perfil { get; set; }
        public string? ocupacion { get; set; }
        public string? edad { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public bool? expediente { get; set; }
        public bool? cliente { get; set; }
        public bool? empleado { get; set; }
        public int? roles_id { get; set; }
        public string? remember_token { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
    }

}
