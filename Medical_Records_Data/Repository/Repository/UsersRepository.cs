using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.DTO;
using Microsoft.AspNetCore.Http;
using System.Data;
using Medical_Records_Utilities.Extensions;
using System.Text.Json.Nodes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Medical_Record_Models.Tables;

namespace Medical_Record_Data.Repository.Repository

{

    public class UsersRepository : IUsersRepository
    {
        private readonly DataBaseConnection _context;

        public UsersRepository(DataBaseConnection context) { _context = context; }

        //=========================================
        //================= CRUD ==================
        //=========================================

        #region CREATE

        public async Task<bool> CreateUser(Usuario user)
        {

            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;


            var active = Convert.ToByte(user.Activo);

            await _context.Database
               .ExecuteSqlInterpolatedAsync($@"EXEC SP_P_AgregarUsuario @Usuario={user.NombreUsuario}, @Nombre={user.Nombre}, @Tipo ={user.Tipo},
                @Activo={active}, @Contrasena={user.Contrasena}, @CorreoElectronico={user.CorreoElectronico},@Identificacion={user.Identification},@Telefono={user.Telefono},@whatsApp={user.WhatsApp}, @Resultado={result} OUTPUT");


            var res = (bool)result.Value;
            return res;
        }


        #endregion

        #region UPDATE

        public async Task<bool> UpdateUser(Usuario user)
        {
            try
            {
                var result = new SqlParameter("@Resultado", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;


                var active = Convert.ToByte(user.Activo);

                await _context.Database
                   .ExecuteSqlInterpolatedAsync($@"EXEC SP_P_Modificar_Usuario @Usuario={user.NombreUsuario}, @Nombre={user.Nombre}, @Tipo ={user.Tipo},
                @Activo={active}, @Contrasena={user.Contrasena}, @CorreoElectronico={user.CorreoElectronico},@Identificacion={user.Identification},@Telefono={user.Telefono},@whatsApp={user.WhatsApp}, @Resultado={result} OUTPUT");


                var res = (bool)result.Value;
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        #endregion

        #region FIND

        public async Task<UserDTO> FindUser(string username)
        {
            //if (!string.IsNullOrWhiteSpace(username))
            //{
            var userDB = await _context.Usuario.Where(u => u.NombreUsuario == username).FirstOrDefaultAsync();

            if (userDB != null)
            {
                UserDTO userDTO = new UserDTO
                {
                    NombreUsuario = userDB.NombreUsuario,
                    Nombre = userDB.Nombre,
                    Identification = userDB.Identification,
                    CorreoElectronico = userDB.CorreoElectronico,
                    Telefono = userDB.Telefono,
                    whatsApp = userDB.WhatsApp,
                    Tipo = userDB.Tipo,
                    Activo = userDB.Activo,
                };

                return userDTO;
            }


            return null;
        }
        #endregion

        #region DELETE

        public async Task<JsonArray> Delete(string UserName)
        {
            JsonArray jsonArray = new();
            var resultSP = false;
            var msgSP = "Ocurrio un Error al Elminar el Usuario";
            try
            {
                var res = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }; // Tamaño de 100
                var usuarioParam = new SqlParameter("@Usuario", SqlDbType.VarChar, 70) { Value = UserName }; // Tamaño de 70




                await _context.Database.
                    ExecuteSqlInterpolatedAsync($@"EXEC SP_P_Eliminar_Usuario @Usuario={usuarioParam},@Mensaje={msg} OUTPUT, @Resultado={res} OUTPUT");

                //     await _context.Database
                //.ExecuteSqlRawAsync("EXEC SP_P_Eliminar_Usuario @Usuario, @Mensaje OUTPUT, @Resultado OUTPUT",
                //    usuarioParam, msg, res);

                jsonArray.Add(res.Value);
                jsonArray.Add(msg.Value);

                return jsonArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                jsonArray.Add(resultSP);
                jsonArray.Add(msgSP);

                return jsonArray;

            }
        }
        #endregion

        #region SHOW_USERS

        public async Task<List<UserDTO>> GetUsers(Pagina obj, HttpContext http)

        {

            List<UserDTO> usersDTO = new();


            SqlParameter paramPagina = new SqlParameter("@NPag", obj.NumPagina);
            SqlParameter paramCantReg = new SqlParameter("@CantReg", obj.CantRegistros);
            SqlParameter paramPalabraBusc = new SqlParameter("@palabraBusc", obj.palabraBuscar);
            SqlParameter paramTotalPag = new SqlParameter("@TotalPag", SqlDbType.Int);
            paramTotalPag.Direction = ParameterDirection.Output;

            var usersDB = await _context.Usuario.FromSqlRaw("EXECUTE dbo.SP_C_Mostrar_Usuarios @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                                                        paramPagina, paramCantReg, paramPalabraBusc, paramTotalPag).ToListAsync();


            if (paramTotalPag.Value is null || (int)paramTotalPag.Value == 0)
            {

                http.Session.SetObject("TotalPag", 0);
            }
            else if ((int)paramTotalPag.Value > 0)
            {
                http.Session.SetObject("TotalPag", paramTotalPag.Value);
            }

            foreach (var u in usersDB)
            {
                var user = new UserDTO
                {
                    NombreUsuario = u.NombreUsuario,
                    Nombre = u.Nombre,
                    Identification = u.Identification,
                    CorreoElectronico = u.CorreoElectronico,
                    Telefono = u.Telefono,
                    whatsApp = u.WhatsApp,
                    Tipo = u.Tipo,
                    Activo = u.Activo,

                };
                usersDTO.Add(user);
            }
            return usersDTO;
        }

        #endregion


        #region CHANGE_STATUS

        public bool changeStatus(string UserName)
        {
            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;

            _context.Database.ExecuteSqlInterpolated(@$"EXEC SP_P_Modificar_Estado_Usuario @Usuario={UserName}, @Resultado ={result} OUTPUT");


            return (bool)result.Value;
        }


        #endregion

        //=========================================
        //=============== LOGIN ===================
        //=========================================
        #region LOGIN

        public async Task<bool> VerifyUser(Usuario user)
        {
            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;
            SqlParameter Username = new SqlParameter("@Usuario", user.NombreUsuario);
            SqlParameter Password = new SqlParameter("@Contrasena", user.Contrasena);
            var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar);
            msg.Direction = ParameterDirection.Output;

            await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC SP_P_Logeo @Usuario ={Username}, @Contrasena={Password}, @Resultado={result} OUTPUT");

            var resultDB = (bool)result.Value;
            //    msg; 

            //};

            return resultDB;
        }
        #endregion

        #region ACTIONS_USER

        public async Task<List<int>> getAuthorizedActions(string Username)
        {

            List<int> lstActions = new List<int>();

            var lst = await _context.GenericModels
            .FromSqlRaw($@"EXEC SP_C_AuthorizeUser @Usuario={Username}")
            .ToListAsync();
            foreach (var a in lst)
            {
                lstActions.Add(a.IdAccion);
            }

            return lstActions;
        }
        #endregion

    }
}
