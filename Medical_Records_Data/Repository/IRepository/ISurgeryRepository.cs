using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Medical_Record_Models.DTO;
using System.Text.Json.Nodes;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface ISurgeryRepository
    {
        public Task<JsonArray> Create_Update_Surgery();
        public Task<JsonArray> Add_Pathological_Surgey();
    }
}
