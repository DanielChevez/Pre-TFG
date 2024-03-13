using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Condicion_Medica
    {
        [Key]
        public int Id_Condicion { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Recomendacion { get; set; }
        public string Tipo { get; set; }
    }
}
