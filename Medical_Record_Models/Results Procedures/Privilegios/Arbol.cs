using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.ModelsGeneric
{
    [Keyless]
    public class Arbol
    {
        public int IdAccion { get; set; }
        public int Padre { get; set; }
        public int NumeroHermano { get; set; }
        public string Descripcion { get; set; }
        public bool check { get; set; }
    }
}
