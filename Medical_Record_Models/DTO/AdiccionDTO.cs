using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class AdiccionDTO
    {
        
        public int Id_Adiccion { get; set; }
        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string Nombre { get; set; }
        [MaxLength]
        public string Descripcion { get; set; }
        
        [StringLength(20)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,20}$")]
        public string Gravedad { get; set; }
    }
}
