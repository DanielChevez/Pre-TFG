using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Medical_Record_Models.Results_Procedures.Folder;

namespace Medical_Record_Data.Repository.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataBaseConnection _context;



        public PatientRepository(DataBaseConnection context)
        {
            _context = context;
        }
        public async Task<JsonArray> Add_Update_Information(Personal_informationDTO Information) {
            var resultsDB = new JsonArray();
            try
            {
                var result = new SqlParameter("@Resultado", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;

                var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar,100);
                msg.Direction = ParameterDirection.Output;


                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.Agregar_Actualizar_Persona 
                    @Id_Persona = {Information.Id_Persona},
                    @Cedula = {Information.Cedula},
                    @Nombre= {Information.Nombre},
                    @Apellidos = {Information.Apellidos},
         
                    @Email = {Information.Email},
                    @Fecha_Nacimiento = {Information.Fecha_Nacimiento},
                    @Edad = {Information.Edad},
                    @Sexo = {Information.Sexo},
                    @Direccion = {Information.Direccion},
                    @Lugar_Origen = {Information.Lugar_origen},
	                @Profesion = {Information.Profesion},
	                @Estado_Civil = {Information.Estado_Civil},
	                @Condicion_Rehabilitacion = {Information.Condicion_Rehabilitacion},
	                @Numero_expediente = {Information.Numero_Expediente},
	                @Fase = {Information.Fase},
                    @Telefono = {Information.Telefono},
	                @Accion = {Information.Accion},
	                @Mensaje = {msg} OUTPUT,
	                @Resultado = {result} OUTPUT");

                var msgDb = (string)msg.Value;
                var res = (bool)result.Value;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);

                return resultsDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public async Task<JsonArray> Add_Update_InformationFamily(Family_informationDTO Information) {
            var resultsDB = new JsonArray();
            try
            {
                var result = new SqlParameter("@Resultado", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;

                var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msg.Direction = ParameterDirection.Output;
                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.AgregarEditarInfoFamiliar 
                @Id_Paciente = {Information.Id_Paciente},                 
                @Conyuge = {Information.Conyuge},
                @Nombre_Hijo = {Information.Nombre_Hijo},
                @Persona_a_Cargo = {Information.Persona_a_cargo},
                @Telefono_Emergencia = {Information.Telefono_emergencia},
                @Nombre_Contacto = {Information.Nombre_Contacto},
                @Parentesco = {Information.Parentesco},
                @Mensaje = {msg} OUTPUT,
	            @Resultado = {result} OUTPUT");

                var msgDb = (string)msg.Value;
                var res = (bool)result.Value;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);

                return resultsDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<All_Personal_informationDTO> GetInformation(int id)
        {


            SqlParameter id_pa = new SqlParameter("@id_paciente", id);
            var personal_info = await _context.Show_Personal_Information.FromSqlRaw("EXECUTE dbo.SP_C_Buscar_Info_Paciente @id_paciente",
                id_pa)
               .ToListAsync();

            if (personal_info.Count() > 0)
            {
                var infoDb = personal_info.Where(info => info.Id_Paciente == id).FirstOrDefault();

                var info_family = await Get_Family_Info(id);

                if (info_family == null)
                {
                    info_family = new Show_Family_Info();
                }

                if (infoDb != null)
                {
                    var Personal_Information = new All_Personal_informationDTO
                    {
                        Id_Paciente = id,
                        Id_Persona = infoDb.Id_Persona,
                        Cedula = infoDb.Cedula,
                        Nombre = infoDb.Nombre,
                        Apellidos = infoDb.Apellidos,
                        Email = infoDb.Email,
                        Fecha_Nacimiento = infoDb.Fecha_Nacimiento,
                        Edad = infoDb.Edad,
                        Sexo = infoDb.Sexo,
                        Direccion = infoDb.Direccion,
                        Lugar_origen = infoDb.Lugar_origen,
                        Profesion = infoDb.Profesion,
                        Estado_Civil = infoDb.Estado_Civil,
                        Condicion_Rehabilitacion = infoDb.Condicion_Rehabilitacion,
                        Numero_Expediente = infoDb.Numero_Expediente,
                        Fase = infoDb.Fase,
                        Telefono = infoDb.Telefono,

                        Conyuge = info_family.Conyuge,
                        Nombre_Hijo = info_family.Nombre_Hijo,
                        Persona_a_cargo = info_family.Persona_a_cargo,
                        Telefono_emergencia = info_family.Telefono_emergencia,
                        Nombre_Contacto = info_family.Nombre_Contacto,
                        Parentesco = info_family.Parentesco

                    };



                    return Personal_Information;



                }
            }

            return null;
        }

        public async Task<Show_Family_Info> Get_Family_Info(int id)
        {

            SqlParameter id_pa = new SqlParameter("@id", id);
            var family_info = await _context.Show_Family_Info.FromSqlRaw("EXECUTE dbo.SP_C_Buscar_Info_familiar @id",
                id_pa)
               .ToListAsync();
            if (family_info.Count() > 0)
            {
                var info = family_info.Where(i => i.Id_Paciente == id).FirstOrDefault();
                return info;
            }
            return null;
        }
    }
}
