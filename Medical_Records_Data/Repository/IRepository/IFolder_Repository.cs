using Medical_Record_Models.DTO;
using Medical_Record_Models.Results_Procedures.Folder;
using Medical_Record_Models.Tables;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface IFolder_Repository
    {
        public Task<List<FolderDTO>> GetFolders(Pagina obj, HttpContext http);
       
        //public Task<Show_Family_Info> Get_Family_Info(int id);
    }
}
