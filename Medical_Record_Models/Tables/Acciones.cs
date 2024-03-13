using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    [Keyless]
    public class Acciones
    {
        public int IdAccion { get; set; }
        public string? Alias { get; set; }
        public string? Descripcion { get; set; }
        public string? Tipo { get; set; }
    }
}
