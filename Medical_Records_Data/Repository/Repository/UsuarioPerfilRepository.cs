using Medical_Record_Data.Repository.IRepository;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Medical_Records_Utilities.Extensions;

using Medical_Record_Models.ViewModels;
using Medical_Record_Models.ModelsGeneric;
using Medical_Record_Models.Results_Procedures;
using Medical_Record_Models.DTO;
using Medical_Record_Models.Tables;

namespace Medical_Record_Data.Repository.Repository
{
    public class UsuarioPerfilRepository : IUsuarioPerfilRepository
    {
        private readonly DataBaseConnection _context;

        public UsuarioPerfilRepository(DataBaseConnection context)
        {
            _context = context;

        }


        //=========================================
        //=============== USER BY =================
        //================PROFILES==================
        //=========================================

        public async Task<List<Perfil_Usuario>> Profiles_User(Pagina obj, HttpContext http, string user)
        {

            SqlParameter paramPagina = new SqlParameter("@NPag", obj.NumPagina);
            SqlParameter paramCantReg = new SqlParameter("@CantReg", obj.CantRegistros);
            SqlParameter paramPalabraBusc = new SqlParameter("@palabraBusc", obj.palabraBuscar);
            SqlParameter paramTotalPag = new SqlParameter("@TotalPag", SqlDbType.Int);
            paramTotalPag.Direction = ParameterDirection.Output;


            List<Perfil_Usuario> profiles = new();
            //var profiles_userDB = await _context.PerfilUsuario.FromSqlRaw($@"EXECUTE dbo.SP_P_PerfilesDelUsuario @Usuario={user}, @NPag={paramPagina},@CantReg={paramCantReg}, @palabraBusc={paramPalabraBusc},@TotalPag={paramTotalPag} OUTPUT").ToListAsync();

            var profiles_userDB = await _context.PerfilUsuario
                .FromSqlRaw("EXECUTE dbo.SP_P_PerfilesDelUsuario @Usuario, @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                new SqlParameter("@Usuario", user), paramPagina, paramCantReg, paramPalabraBusc, paramTotalPag)
                .ToListAsync();

            if (paramTotalPag.Value is null || (int)paramTotalPag.Value == 0)
            {

                http.Session.SetObject("TotalPagPU", 0);
            }
            else if ((int)paramTotalPag.Value > 0)
            {
                http.Session.SetObject("TotalPagPU", paramTotalPag.Value);
            }


            foreach (var p in profiles_userDB)
            {

                var profile = new Perfil_Usuario
                {
                    IdPerfil = p.IdPerfil,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion
                };


                profiles.Add(profile);
            }
            return profiles;

        }


        public async Task<List<UsuarioPerfil>> Users_Profile(Pagina obj, HttpContext http, int idPerfil)
        {
            try
            {
                SqlParameter paramPagina = new SqlParameter("@NPag", obj.NumPagina);
                SqlParameter paramCantReg = new SqlParameter("@CantReg", obj.CantRegistros);
                SqlParameter paramPalabraBusc = new SqlParameter("@palabraBusc", obj.palabraBuscar);
                SqlParameter paramTotalPag = new SqlParameter("@TotalPag", SqlDbType.Int);
                paramTotalPag.Direction = ParameterDirection.Output;

                List<UsuarioPerfil> profiles = new();

                var profiles_userDB = await _context.UsuarioPerfil
                    .FromSqlRaw("EXEC dbo.SP_P_UsuariosDelPerfil @IdPerfil, @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                        new SqlParameter("@IdPerfil", idPerfil),
                        paramPagina,
                        paramCantReg,
                        paramPalabraBusc,
                        paramTotalPag)
                    .ToListAsync();

                int totalPaginas = (int)paramTotalPag.Value;
                http.Session.SetObject("TotalPagUP", totalPaginas);

                profiles.AddRange(profiles_userDB.Select(p => new UsuarioPerfil
                {
                    Nombre = p.Nombre,
                    NombreUsuario = p.NombreUsuario
                }));

                return profiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        //public async Task<List<UsuarioPerfil>> Users_Profile(Pagina obj, HttpContext http, int idPerfil)
        //{


        //    SqlParameter paramPagina = new SqlParameter("@NPag", obj.NumPagina);
        //    SqlParameter paramCantReg = new SqlParameter("@CantReg", obj.CantRegistros);
        //    SqlParameter paramPalabraBusc = new SqlParameter("@palabraBusc", obj.palabraBuscar);

        //    List<UsuarioPerfil> profiles = new();


        //   SqlParameter paramTotalPag = new SqlParameter("@TotalPag", SqlDbType.Int);
        //    paramTotalPag.Direction = ParameterDirection.Output;

        //    var profiles_userDB = await _context.UsuarioPerfil.FromSqlRaw("EXECUTE dbo.SP_P_UsuariosDelPerfil @IdPerfil, @NPag,@CantReg,@palabraBusc,@TotalPag OUTPUT",
        //        new SqlParameter("@IdPerfil", idPerfil), paramPagina, paramCantReg, paramPalabraBusc, paramTotalPag).ToListAsync();

        //    int totalPag = (int)paramTotalPag.Value;

        //    if (paramTotalPag.Value is null || (int)paramTotalPag.Value == 0)
        //    {
        //        http.Session.SetObject("TotalPag", 0);
        //    }
        //    else if ((int)paramTotalPag.Value > 0)
        //    {
        //        http.Session.SetObject("TotalPag", paramTotalPag.Value);
        //    }

        //    foreach (var p in profiles_userDB)
        //    {

        //        var profile = new UsuarioPerfil
        //        {

        //            Nombre = p.Nombre,
        //            NombreUsuario = p.NombreUsuario
        //        };


        //        profiles.Add(profile);
        //    }
        //    return profiles;

        //}

        public async Task<List<OpcionesusuariosPerfil>> optionsUsersProfile(int IdPerfil)
        {
            try
            {

                //SqlParameter idPerfil = new SqlParameter("@IdPerfil", IdPerfil);

                var lst = await _context.optionsUsersProfile.FromSqlRaw("SELECT * FROM dbo.f_opcionesUsuariosPerfil(@IdPerfil)", new SqlParameter("@IdPerfil", IdPerfil)).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        public async Task<List<OpcionesPerfilesDelUsuario>> optionsProfilesUser(string User)
        {

            try
            {
                SqlParameter user = new SqlParameter("@usuario", User);
                // var lst = await _context.optionsProfilesUser.FromSqlRaw("SELECT * FROM dbo.f_opcionesPerfilesUsuario(@usuario), user").ToListAsync();
                var lst = await _context.optionsProfilesUser.FromSqlRaw("SELECT * FROM dbo.f_opcionesPerfilesUsuario({0})", User).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;

            }
            return null;
        }
        public async Task<bool> Create_User_Profile(string User, int IdPerfil)
        {
            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;
            var resultSp = false;
            try
            {

                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC SP_P_CrearUsuarioPerfil @Usuario={User},@IdPerfil={IdPerfil},@Resultado={result} OUTPUT");
                resultSp = (bool)result.Value;
                return resultSp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> Delete_User_Profile(string User, int IdPerfil)
        {
            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;
            var resultSp = false;
            try
            {

                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC SP_P_EliminarUsuarioPerfil @Usuario={User},@IdPerfil={IdPerfil},@Resultado={result} OUTPUT");
                resultSp = (bool)result.Value;
                return resultSp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<List<ArbolPrivilegiosViewModel>> DrawTree(int IdPerfil)
        {

            List<ArbolPrivilegiosViewModel> lstActions = new List<ArbolPrivilegiosViewModel>();
            try
            {
                var Actions = await _context.Arbol.FromSqlRaw("EXEC dbo.SP_P_DibujarArbol @idPerfil", new SqlParameter("@idPerfil", IdPerfil)).ToListAsync();


                lstActions = Actions.Where(l => l.Padre == 0).OrderBy(l => l.NumeroHermano)
                    .Select(l => new ArbolPrivilegiosViewModel
                    {
                        idAccion = l.IdAccion,
                        @checked = l.check,
                        text = l.Descripcion,
                        children = GetChildren(Actions, l.IdAccion)
                    }).ToList();



                return lstActions;


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return lstActions;
            }

        }
        public List<ArbolPrivilegiosViewModel> GetChildren(List<Arbol> arbols, int parentId)
        {
            return arbols.Where(l => l.Padre == parentId).OrderBy(l => l.NumeroHermano)
                .Select(l => new ArbolPrivilegiosViewModel
                {
                    idAccion = l.IdAccion,
                    @checked = l.check,
                    text = l.Descripcion,
                    children = GetChildren(arbols, l.IdAccion)
                }).ToList();

        }


        public async Task<bool> SaveCheckNodes(List<int> checkedIds, int IdPerfil)
        {
            SqlParameter idPerfil = new SqlParameter("@idPerfil", IdPerfil);

            var idCheck = "";
            try
            {

                var Arbol = await _context.Arbol.FromSqlRaw("EXEC dbo.SP_P_DibujarArbol @idPerfil", new SqlParameter("@idPerfil", IdPerfil)).ToListAsync();

                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC SP_P_EliminarAll_AccionPerfil @idPerfil={idPerfil}");



                foreach (var arbol in Arbol)
                {
                    arbol.check = checkedIds.Contains(arbol.IdAccion);
                    if (arbol.check == true)
                    {

                        idCheck += arbol.IdAccion.ToString() + ",";

                    }
                }
                idCheck = idCheck.Remove(idCheck.Length - 1);
                var seLogr = await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC SP_P_GuardarCheck @idAccion={idCheck},@idPerfil={idPerfil}");

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> VerifyPassword(string User, string Password)
        {
            try
            {
                SqlParameter user = new SqlParameter("@usuario", SqlDbType.VarChar, 70) { Value = User };
                SqlParameter password = new SqlParameter("@contrasenaIngresada", SqlDbType.VarChar, 100) { Value = Password };
                // SqlParameter msj = new SqlParameter("@msj", SqlDbType.VarChar,40) { Direction = ParameterDirection.Output };

                var result = new SqlParameter("@tienePermiso", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;
                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.SP_P_VerificarContrasenaAdmin @usuario={user},@contrasenaIngresada={password},@tienePermiso={result} OUTPUT");

                var resultSp = (bool)result.Value;
                return resultSp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }




        public async Task<UserDTO> FindUser(string username)
        {
            if (!string.IsNullOrWhiteSpace(username)
)
            {


                SqlParameter user = new SqlParameter("@Usuario", username);
                //var usuarios = await _context.Usuario.FromSqlRaw("SELECT NombreUsuario,Nombre,Identification,CorreoElectronico,Telefono,WhatsApp,Activo,Tipo FROM USUARIO WHERE NombreUsuario = {0}", username).ToListAsync();


                var userD = await _context.FindUser
                    .FromSqlRaw("EXEC SP_C_Buscar_Usuario @Usuario", user)
                    .ToListAsync();

                foreach (var item in userD)
                {
                    if (item.NombreUsuario == username)
                    {
                        UserDTO userDTO = new UserDTO
                        {
                            NombreUsuario = item.NombreUsuario,
                            Nombre = item.Nombre,
                            Identification = item.Identification,
                            CorreoElectronico = item.CorreoElectronico,
                            Telefono = item.Telefono,
                            whatsApp = item.WhatsApp,
                            Tipo = item.Tipo,
                            //Activo = item.Activo,
                        };

                        return userDTO;

                    }

                }

                //if (userDB != null)
                //{
                //    UserDTO userDTO = new UserDTO
                //    {
                //        NombreUsuario = userDB.NombreUsuario,
                //        Nombre = userDB.Nombre,
                //        Identification = userDB.Identification,
                //        CorreoElectronico = userDB.CorreoElectronico,
                //        Telefono = userDB.Telefono,
                //        whatsApp = userDB.whatsApp,
                //        Tipo = userDB.Tipo,
                //        Activo = userDB.Activo,
                //    };

                //    return userDTO;
                //}
            }
            return null;
        }
    }
}
