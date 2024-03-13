using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class All_Personal_informationDTO
    {
        public int Id_Persona { get; set; }
        public int Id_Paciente { get; set; }
        [StringLength(25)]
        public string Cedula { get; set; }
        
        [StringLength(100)]
        public string Nombre { get; set; }
        
        [StringLength(100)]
        public string Apellidos { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime Fecha_Nacimiento { get; set; }
        public int Edad { get; set; }
        
        [StringLength(50)]
        public string Sexo { get; set; }
        
        [MaxLength]
        public string Direccion { get; set; }
        
        [MaxLength]
        public string Lugar_origen { get; set; }

        [StringLength(150)]
        public string Profesion { get; set; }
        
        [StringLength(50)]
        public string Estado_Civil { get; set; }

        [StringLength(50)]
        public string Condicion_Rehabilitacion { get; set; }
        
        [StringLength(100)]
        public string? Numero_Expediente { get; set; }
        [StringLength(50)]
        public string Fase { get; set; }
        [StringLength(20)]
        public string Telefono { get; set; }


        [StringLength(100)]
        public string? Conyuge { get; set; }

        [StringLength(100)]
        public string? Nombre_Hijo { get; set; }

        [StringLength(100)]
        public string? Persona_a_cargo { get; set; }


        [StringLength(20)]
        public string? Telefono_emergencia { get; set; }

        [StringLength(100)]
        public string? Nombre_Contacto { get; set; }


        [StringLength(100)]
        public string? Parentesco { get; set; }



        public bool Accion { get; set; }

        
        
        


    }
}
