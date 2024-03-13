using Medical_Record_Models.DTO;
using Medical_Record_Models.ModelsGeneric;
using Medical_Record_Models.Results_Procedures;
using Medical_Record_Models.Tables;
using Medical_Record_Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace Medical_Record_Data.Repository.IRepository
{
    public interface IUsuarioPerfilRepository
    {
        public Task<List<Medical_Record_Models.ModelsGeneric.UsuarioPerfil>> Users_Profile(Pagina obj,HttpContext http,int idPerfil);
        public Task<List<Medical_Record_Models.ModelsGeneric.Perfil_Usuario>> Profiles_User(Pagina obj, HttpContext http, string user);
        public Task<List<OpcionesusuariosPerfil>> optionsUsersProfile(int IdPerfil);

        public Task<List<OpcionesPerfilesDelUsuario>> optionsProfilesUser(string User);
        public Task<bool> Create_User_Profile(string User, int IdPerfil);

        public Task<bool> Delete_User_Profile(string User, int IdPerfil);

       
        public Task<List<ArbolPrivilegiosViewModel>> DrawTree( int IdPerfil);
        public Task<bool> SaveCheckNodes(List<int> checkedIds, int IdPerfil);

        public Task<bool> VerifyPassword(string User,string Password);

        public Task<UserDTO> FindUser(string username);

    }
}
