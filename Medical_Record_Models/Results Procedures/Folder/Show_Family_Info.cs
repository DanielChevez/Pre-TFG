using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Results_Procedures.Folder
{
    [Keyless]
    public class Show_Family_Info
    {
        public int Id_Paciente { get; set; }
        public string? Conyuge { get; set; }
        public string? Nombre_Hijo { get; set; }
        public string? Persona_a_cargo { get; set; }
        public string? Telefono_emergencia { get; set; }

        public string? Nombre_Contacto { get; set; }
        public string? Parentesco { get; set; }

    }
}
