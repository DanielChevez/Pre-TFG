using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.ViewModels
{
    public class DiseaseVM
    {
        public int Id_Paciente{ get; set; }
        public string? Tipo { get; set; }
        public List<DiseaseCheckList> Diseases { get; set; }
    }
}
