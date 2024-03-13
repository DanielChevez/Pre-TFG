using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Medical_Record_Models.Tables
{
    public class Enfermedad
    {
        [Key]
        public int Id_Enfermedad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Recomendacion { get; set; }
        public string? Tipo { get; set; }

    }
}