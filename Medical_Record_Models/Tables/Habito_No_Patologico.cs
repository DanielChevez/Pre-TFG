using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Habito_No_Patologico
    {

        [Key]
        public int ID_HABITO_NO_PATOLOGICO { get; set; }

        [ForeignKey("ID_HABITO")]
        public int Id_Habito { get; set; }

        [ForeignKey("ID_HABITO")]
        public int Id_No_Patologico { get; set; }

    }
}
