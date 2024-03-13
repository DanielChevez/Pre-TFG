using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Condicion_Patologica
    {
        [Key]
        [Column("ID_CONDICION")]
        public int IdCondicion { get; set; }

        [Column("ID_PATOLOGICO")]
        public int IdPatologico { get; set; }

    }
}
