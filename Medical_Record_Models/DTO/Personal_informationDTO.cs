using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.DTO
{
    public class Personal_informationDTO
    {

        public int Id_Paciente { get; set; }
        public int Id_Persona{ get; set; }

        [RegularExpression(@"^[0-9]{1,25}$")]
        [StringLength(25)]
        public string Cedula { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string Nombre { get; set; }

        [StringLength(100)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string Apellidos { get; set; }

        [StringLength(100)]

        public string Email { get; set; }

        public DateTime Fecha_Nacimiento { get; set; }
        //[RegularExpression(@"^[0-9]{1,2}$")]
        //public int Edad { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^(?:Masculino|Femenino|Otro){1,50}$")]
        public string Sexo { get; set; }


        [MaxLength]
        
        public string Direccion { get; set; }

        [MaxLength]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{0,100}$")]
        public string Lugar_origen { get; set; }

        [StringLength(150)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,100}$")]
        public string Profesion { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s]{1,50}$")]
        public string Estado_Civil { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[áÁéÉíÍóÓúÚa-zA-Z\s]{1,50}$")]
        public string Condicion_Rehabilitacion { get; set; }


        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9-\s]{0,100}$")]
        public string? Numero_Expediente { get; set; }
        [StringLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9\s]{1,10}$")]
        public string Fase { get; set; }
        [StringLength(20)]
        //[RegularExpression(@"^\d{1,20-}$")]
        public string Telefono { get; set; }

        public int Edad { get; set; }

     
        public bool? Accion { get; set; }






    }
}
