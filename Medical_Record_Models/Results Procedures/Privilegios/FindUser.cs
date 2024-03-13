using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Results_Procedures
{
    [Keyless]
    public class FindUser
    {
        public string? NombreUsuario { get; set; }
        
        public string? Nombre { get; set; }
        
        public string? Identification { get; set; }
        
        public string? CorreoElectronico { get; set; }
        
        public string? Telefono { get; set; }
        
        public string WhatsApp { get; set; }
                
        public bool Activo { get; set; }
        public string? Tipo { get; set; }
    }
}
