using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Results_Procedures.Folder
{
    [Keyless]
    public class Show_Personal_Information
    {

        public int Id_Paciente { get; set; }
        public int Id_Persona { get; set; }
        public string Cedula { get; set; }
         
        public string Nombre { get; set; }

 
        public string Apellidos { get; set; }

    
        public string Email { get; set; }

        public DateTime Fecha_Nacimiento { get; set; }
        public int Edad { get; set; }     
        public string Sexo { get; set; }

        public string Direccion { get; set; }

        public string Lugar_origen { get; set; }

      
        public string Profesion { get; set; }
 
        public string Estado_Civil { get; set; }

     
        public string Condicion_Rehabilitacion { get; set; }

 
        public string? Numero_Expediente { get; set; }
       
        public string Fase { get; set; }
      
        public string Telefono { get; set; }
    }
}
