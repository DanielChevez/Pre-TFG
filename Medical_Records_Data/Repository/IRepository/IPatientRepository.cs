using Medical_Record_Models.DTO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface IPatientRepository
    {
        public Task<JsonArray> Add_Update_Information(Personal_informationDTO obj);
        public Task<JsonArray> Add_Update_InformationFamily(Family_informationDTO obj);

        public Task<All_Personal_informationDTO> GetInformation(int id);
    }
}
