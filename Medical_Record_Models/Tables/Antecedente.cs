using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Antecedente
    {
        [Key]
        public int Id_Antecedente { get; set;}
        public int Id_Paciente { get; set;}
    }
}
