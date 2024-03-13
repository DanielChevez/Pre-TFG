using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class UserDTO
    {
        public string NombreUsuario { get; set; }
        public string? Nombre { get; set; }
        public string? Identification { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public string whatsApp { get; set; }
        public string? Tipo { get; set; }
        public bool Activo { get; set; }
        public string Contrasena { get; set; }

    }
}
