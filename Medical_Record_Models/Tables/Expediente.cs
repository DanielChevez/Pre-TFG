using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Expediente
    {
        [Key]
        public int Id_Expediente { get; set; }

        [Required]
        public int Id_Paciente { get; set; }

        [StringLength(100)]
        public string Numero_Expediente { get; set; }
    }
}
