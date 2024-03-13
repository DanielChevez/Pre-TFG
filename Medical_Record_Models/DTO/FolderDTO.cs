using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class FolderDTO
    {
        public int Id_Expediente { get; set; }
        public string? Numero_Expediente { get; set; }
        public int Id_Paciente { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public string Fase { get; set; }
        public string Condicion_Rehabilitacion { get; set; }
    }
}
