using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.ModelsGeneric
{
    [Keyless]
    public class UsuarioPerfil
    {
        
        public String NombreUsuario { get; set; }
        public string Nombre { get; set; }
        
        
        
    
    }
}
