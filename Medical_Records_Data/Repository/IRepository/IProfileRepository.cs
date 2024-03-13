using System.Text.Json.Nodes;
using Medical_Record_Models.DTO;
using Medical_Record_Models.Tables;
using Microsoft.AspNetCore.Http;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface IProfileRepository
    {

        public Task<bool> Create(ProfileDTO profile);
        public Task<bool> Update(ProfileDTO profile);

        public Task<JsonArray> Delete(int IdPerfil);

        public Task<List<ProfileDTO>> GetProfiles(Pagina obj, HttpContext http);

        public Task<ProfileDTO> Find(int IdPerfil);
        public Task<bool> ChangeStatus(int IdPerfil);
    }
}
