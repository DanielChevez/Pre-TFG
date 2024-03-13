using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.ViewModels
{
    public class ArbolPrivilegiosViewModel
    {
        public int idAccion { get; set; }
        public int Padre { get; set; }
        public int NumeroHermano { get; set; }
        public string text { get; set; }
        public bool @checked { get; set; }
 
        public bool hasChildren { get; set; }
        public virtual List<ArbolPrivilegiosViewModel> children { get; set; }
    }
}
