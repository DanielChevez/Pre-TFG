using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Paciente
    {
        [Key]
        public int Id_Paciente { get; set; }
        
        public string Id_Persona { get; set; }

        [Required]
        [StringLength(25)]
        public string Cedula { get; set; }

        [StringLength(100)]
        public string Profesion { get; set; }

        [StringLength(50)]
        public string Estado_Civil { get; set; }

        [StringLength(10)]
        public string Fase { get; set; }

       
        
        
    }
}
