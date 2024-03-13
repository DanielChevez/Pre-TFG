using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class Family_informationDTO
    {
    
        public int Id_Paciente { get; set; }
      
        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string? Conyuge { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string? Nombre_Hijo { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string? Persona_a_cargo { get; set; }


        [StringLength(20)]
        public string? Telefono_emergencia { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string? Nombre_Contacto { get; set; }


        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string? Parentesco { get; set; }


        
        
        


    }
}
