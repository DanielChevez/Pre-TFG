using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class DiseaseDTO
    {

        public int? Id_Enfermedad { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string Nombre { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]

        public string Estado { get; set; }
        [MaxLength]
        public string Descripcion { get; set; }

        [MaxLength]
        public string Recomendacion { get; set; }

        [StringLength(200)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,200}$")]
        public string? Tipo { get; set; }
    }
}
