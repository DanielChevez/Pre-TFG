using Microsoft.AspNetCore.Http;
using Medical_Record_Models.DTO;
using System.Text.Json.Nodes;
using Medical_Record_Models.Tables;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface IUsersRepository
    {

        public Task<bool> CreateUser(Usuario user);

        public Task<bool> UpdateUser(Usuario user);
        public Task<UserDTO> FindUser(string username);

        public Task<JsonArray> Delete(string username);
        public Task<List<UserDTO>> GetUsers(Pagina obj, HttpContext http);        
        
        
        public Task<bool> VerifyUser(Usuario user);


        public bool changeStatus(string UserName);

        public Task<List<int>> getAuthorizedActions(string Username);

    }
}
