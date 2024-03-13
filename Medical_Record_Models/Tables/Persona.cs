using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Persona
    {
        [Key]
        public string Id_Persona { get; set; }

        [StringLength(25)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public DateTime Fecha_Nacimiento { get; set; }

        public int? Edad { get; set; }

        [Required]
        [StringLength(50)]
        public string Sexo { get; set; }

        [MaxLength]
        public string Direccion { get; set; }

        [StringLength(100)]
        public string Lugar_Origen { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }


    }
}
