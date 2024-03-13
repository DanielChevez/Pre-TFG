using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical_Record_Models.Tables
{

    //    [Table("USUARIO")]
    public class Usuario
    {

        [Key]

        public string? NombreUsuario { get; set; }

        public string? Nombre { get; set; }

        public string? Identification { get; set; }

        public string? CorreoElectronico { get; set; }

        public string Telefono { get; set; }

        public string WhatsApp { get; set; }

        public string? Tipo { get; set; }

        public bool Activo { get; set; }

        public string? Contrasena { get; set; }

    }
}
