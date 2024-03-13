using Medical_Record_Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface IMedical_Conditions
    {
        public Task<JsonArray> Add_Habit(HabitoDTO obj);
        public Task<JsonArray> Delete_Habit(int idHabit);
    }
}
