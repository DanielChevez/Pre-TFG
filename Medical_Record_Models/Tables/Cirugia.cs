using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Cirugia
    {
        [Key]
        public int Id_Cirugia { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_Realizada { get; set; }
    }
}
