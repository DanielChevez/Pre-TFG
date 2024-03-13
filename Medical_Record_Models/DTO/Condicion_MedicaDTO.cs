using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class Condicion_MedicaDTO
    {
        
        public int Id_Condicion_Medica { get; set; }
        [StringLength(100)]
       
        public string Nombre { get; set; }
        [MaxLength]
        public string Descripcion { get; set; }
       
        [MaxLength]
       
        public string Recomendacion { get; set; }
    }
}
