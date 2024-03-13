using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Record_Models.Tables
{
    public class Pagina
    {
        public string Usuario { get; set; }
        public int PaginaActual { set; get; }
        public int NumPagina { set; get; }
        public int CantRegistros { set; get; }
        public string palabraBuscar { set; get; }

        public int totalPaginas { set; get; }

        // S -> Pagina Siguiete  |  A -> Pagina Anterior

        public char accion { set; get; }

        //Constructor
        public Pagina()
        {

            CantRegistros = 1;
            accion = 'A';

        }
    }
}
