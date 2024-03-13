using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.ModelsGeneric
{
    [Keyless]
    public class OpcionesusuariosPerfil
    {
        public string NombreUsuario { get; set; }
        public string Nombre { get;set; }
        public string Tipo { get; set; }
    }
}
