using Medical_Record_Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Medical_Record_Data.Repository.Repository
{
    public class SurgeryRepository : ISurgeryRepository
    {
        private readonly DataBaseConnection _context;

        public SurgeryRepository(DataBaseConnection context) { _context = context; }

        public async Task<JsonArray> Create_Update_Surgery()
        {
            var resultDb = new JsonArray();

            try
            {
                return resultDb;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return resultDb;
            }



        }
        public async Task<JsonArray> Add_Pathological_Surgey()
        {
            var resultDb = new JsonArray();

            try
            {
                return resultDb;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return resultDb;
            }



        }
    }
}
