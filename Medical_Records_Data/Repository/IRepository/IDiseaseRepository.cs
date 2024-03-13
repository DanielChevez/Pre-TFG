using Microsoft.AspNetCore.Http;
using Medical_Record_Models.DTO;
using System.Text.Json.Nodes;
using Medical_Record_Models.Tables;
using Medical_Record_Models.ViewModels;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface IDiseaseRepository
    {

        public Task<JsonArray> Create_Update_Disease(DiseaseDTO Disease);

        //public Task<DiseaseDTO> FindDisease(string Disease);

        public Task<JsonArray> Delete(int id);
        public Task<List<DiseaseDTO>> GetDiseases(Pagina obj, HttpContext http);
        public Task<DiseaseDTO> FindDisease(int id);

        public Task<List<DiseaseVM>> getDiseasByTypes(int id);

        public Task<JsonArray> SaveDiseaseFamily(DiseaseVM obj);
        public Task<JsonArray> SavePathologicalDiseases(DiseaseVM obj);
        public Task<List<DiseaseVM>> GetPathologicalDiseases(int IdPatient);
    }
}
