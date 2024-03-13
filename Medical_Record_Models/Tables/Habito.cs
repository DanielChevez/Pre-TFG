using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Habito
    {
        [Key]
        public int Id_Habito { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string Frecuencia { get; set; }
        public string Estado { get; set; }
    }
}
