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
    public class ProfileRepository : IProfileRepository
    {
        private readonly DataBaseConnection _context;

        public ProfileRepository(DataBaseConnection context) { _context = context; }



        //=========================================
        //================= CRUD ==================
        //=========================================
        public async Task<bool> Create(ProfileDTO profile)
        {
            try
            {

                var result = new SqlParameter("@Resultado", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;


                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.SP_P_CrearPerfil @Nombre={profile.Nombre}, @Descripcion={profile.Descripcion},@Activo={profile.Activo}, @Resultado={result} OUTPUT");
                var res = (bool)result.Value;
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }


        }
        public async Task<bool> Update(ProfileDTO profile)
        {
            try
            {

                var result = new SqlParameter("@Resultado", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;
                var active = Convert.ToByte(profile.Activo);

                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC SP_P_Modificar_Perfil @IdPerfil={profile.IdPerfil},@Nombre={profile.Nombre},@Descripcion={profile.Descripcion},@Activo={active},@Resultado={result} OUTPUT");

                var res = (bool)result.Value;
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public async Task<JsonArray> Delete(int IdPerfil)
        {
            JsonArray jsonArray = new();
            var resultSP = false;
            var msgSP = "Ocurrio un Error al Elminar el Usuario";
            try
            {
                var res = new SqlParameter("@Resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output }; // Tamaño de 100
                var idParam = new SqlParameter("@IdPerfil", SqlDbType.Int) { Value = IdPerfil }; // Tamaño de 70


                if (IdPerfil > 0)
                {
                    await _context.Database.
                        ExecuteSqlInterpolatedAsync($@"EXEC SP_P_Eliminar_Perfil @IdPerfil={idParam},@Resultado={res} OUTPUT,@Mensaje={msg} OUTPUT");

                    jsonArray.Add(res.Value);
                    jsonArray.Add(msg.Value);
                }
                return jsonArray;

            }
            catch (Exception ex)
            {
                jsonArray.Add(resultSP);
                jsonArray.Add(msgSP);
                Console.WriteLine(ex.ToString());
                return jsonArray;

            }

        }
        public async Task<List<ProfileDTO>> GetProfiles(Pagina obj, HttpContext http)
        {


            List<ProfileDTO> profilesDTO = new();


            SqlParameter paramPagina = new SqlParameter("@NPag", obj.NumPagina);
            SqlParameter paramCantReg = new SqlParameter("@CantReg", obj.CantRegistros);
            SqlParameter paramPalabraBusc = new SqlParameter("@palabraBusc", obj.palabraBuscar);
            SqlParameter paramTotalPag = new SqlParameter("@TotalPag", SqlDbType.Int);
            paramTotalPag.Direction = ParameterDirection.Output;

            var profilesDB = await _context.Perfil.FromSqlRaw("EXECUTE dbo.SP_C_Mostrar_Perfiles @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                                                        paramPagina, paramCantReg, paramPalabraBusc, paramTotalPag).ToListAsync();
            if (paramTotalPag.Value is null || (int)paramTotalPag.Value == 0)
            {

                http.Session.SetObject("TotalPag", 0);
            }
            else if ((int)paramTotalPag.Value > 0)
            {
                http.Session.SetObject("TotalPag", paramTotalPag.Value);
            }

            foreach (var p in profilesDB)
            {
                var profile = new ProfileDTO
                {

                    IdPerfil = p.IdPerfil,
                    Descripcion = p.Descripcion,
                    Activo = p.Activo,
                    Nombre = p.Nombre

                };

                profilesDTO.Add(profile);
            }

            return profilesDTO;
        }

        public async Task<ProfileDTO> Find(int IdPerfil)
        {

            var profileDTO = await _context.Perfil.Where(p => p.IdPerfil == IdPerfil).FirstOrDefaultAsync();
            if (profileDTO != null)
            {
                ProfileDTO profile = new ProfileDTO
                {
                    IdPerfil = profileDTO.IdPerfil,
                    Descripcion = profileDTO.Descripcion,
                    Nombre = profileDTO.Nombre,
                    Activo = profileDTO.Activo

                };
                return profile;
            }
            return null;
        }
        public async Task<bool> ChangeStatus(int IdProfile)
        {

            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;

            try
            {
                if (IdProfile > 0)
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.SP_P_ModificarEstadoPerfil @IdPerfil={IdProfile}, @Resultado={result} OUTPUT");
                }
                return (bool)result.Value;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }
    }
}
