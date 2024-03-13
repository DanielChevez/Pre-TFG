using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.ViewModels
{
    [Keyless]
    public  class DiseaseCheckList
    {
        public int Id_Enfermedad { get; set; }
        public string? Nombre { get; set; }
        public string? Tipo { get; set; }
        public bool Activo { get; set; }
        
        public string? Parentesco { get; set; }

        
    }
}
