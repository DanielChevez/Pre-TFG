using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class HabitoDTO
    {
        
        public int Id_Habito { get; set; }
        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string Nombre { get; set; }
        [MaxLength]
        public string Descripcion { get; set; }
        [StringLength(50)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,50}$")]
        public string Categoria { get; set; }
        [StringLength(20)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,20}$")]
        public string Frecuencia { get; set; }
        [StringLength(20)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,20}$")]
        public string Estado { get; set; }
    }
}
