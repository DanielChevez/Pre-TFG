using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    [Keyless]
    public class Parentesco
    {

        public string IdAccion { get; set; }
        public string Padre { get; set; }
        public string NumeroHermano { get; set; }
    }
}
