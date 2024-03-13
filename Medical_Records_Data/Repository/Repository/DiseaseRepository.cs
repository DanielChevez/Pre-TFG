using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.DTO;
using Medical_Record_Models.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Text.Json.Nodes;
using Medical_Records_Utilities.Extensions;
using System.Runtime.Serialization.Formatters;
using Medical_Record_Models.ViewModels;
using Microsoft.Identity.Client;

namespace Medical_Record_Data.Repository.Repository
{
    public class DiseaseRepository : IDiseaseRepository
    {

        private readonly DataBaseConnection _context;

        public DiseaseRepository(DataBaseConnection context) { _context = context; }



        public async Task<JsonArray> Create_Update_Disease(DiseaseDTO Disease)
        {
            var resultsDB = new JsonArray();
            try
            {


                var result = new SqlParameter("@Resultado", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;

                var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msg.Direction = ParameterDirection.Output;

                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.SP_P_AgregarActualizarEnfermedad
                   @Id = {Disease.Id_Enfermedad},
                   @Nombre = {Disease.Nombre},
                   @Descripcion = {Disease.Descripcion},
                   @Estado = {Disease.Estado},
                   @Recomendacion = {Disease.Recomendacion},
                   @Tipo = {Disease.Tipo},
                  @Mensaje = {msg} OUTPUT,
                   @Resultado = {result} OUTPUT
                ");

                var msgDb = (string)msg.Value;
                var res = (bool)result.Value;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);
                return resultsDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return resultsDB;
            }

        }
        public async Task<List<DiseaseDTO>> GetDiseases(Pagina obj, HttpContext http)
        {
            List<DiseaseDTO> diseases = new List<DiseaseDTO>();
            SqlParameter paramPagina = new SqlParameter("@NPag", obj.NumPagina);
            SqlParameter paramCantReg = new SqlParameter("@CantReg", obj.CantRegistros);
            SqlParameter paramPalabraBusc = new SqlParameter("@palabraBusc", obj.palabraBuscar);
            SqlParameter paramTotalPag = new SqlParameter("@TotalPag", SqlDbType.Int);
            paramTotalPag.Direction = ParameterDirection.Output;

            var diseaseDb = await _context.Enfermedad.FromSqlRaw("EXECUTE dbo.SP_C_Mostrar_Enfermedades @NPag, @CantReg, @palabraBusc, @TotalPag OUTPUT",
                paramPagina, paramCantReg, paramPalabraBusc, paramTotalPag)
                .ToListAsync();



            if (paramTotalPag.Value is null || (int)paramTotalPag.Value == 0)
            {

                http.Session.SetObject("TotalPag", 0);
            }
            else if ((int)paramTotalPag.Value > 0)
            {
                http.Session.SetObject("TotalPag", paramTotalPag.Value);
            }


            foreach (var d in diseaseDb)
            {
                var disease = new DiseaseDTO
                {
                    Id_Enfermedad = d.Id_Enfermedad,
                    Nombre = d.Nombre,
                    Descripcion = d.Descripcion,
                    Recomendacion = d.Recomendacion,
                    Estado = d.Estado,
                    Tipo = d.Tipo
                };

                diseases.Add(disease);

            }
            return diseases;

        }

        public async Task<DiseaseDTO> FindDisease(int id)
        {

            var disease = await _context.Enfermedad.Where(d => d.Id_Enfermedad == id).FirstOrDefaultAsync();

            if (disease != null)
            {
                return new DiseaseDTO
                {
                    Id_Enfermedad = disease.Id_Enfermedad,
                    Nombre = disease.Nombre,
                    Descripcion = disease.Descripcion,
                    Recomendacion = disease.Recomendacion,
                    Tipo = disease.Tipo,
                    Estado = disease.Estado
                };
            }
            return null;
        }

        public async Task<JsonArray> Delete(int id)
        {
            var resultsDB = new JsonArray();
            var msgDb = "";
            var res = false;
            try
            {

                var result = new SqlParameter("@Resultado", SqlDbType.Bit);
                result.Direction = ParameterDirection.Output;

                var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
                msg.Direction = ParameterDirection.Output;

                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.SP_EliminarEnfermedad
                   @IdEnfermedad = {id},
                  @Mensaje = {msg} OUTPUT,
                   @Resultado = {result} OUTPUT
                ");

                msgDb = (string)msg.Value;
                res = (bool)result.Value;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);
                return resultsDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                msgDb = "Ocurrió un error al eliminar!";
                res = false;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);
                return resultsDB;
            }


        }

        public async Task<List<DiseaseVM>> getDiseasByTypes(int id)
        {
            //var diseaseDb = await _context.Enfermedad.FromSqlRaw("SELECT * FROM ENFERMEDAD").ToListAsync();

            var diseaseOfPastient = await _context.DiseaseCheckLists.FromSqlRaw($@"SELECT e.ID_ENFERMEDAD, e.NOMBRE,e.TIPO,h.PARENTESCO, CAST(CASE WHEN h.ID_ENFERMEDAD IS NOT NULL AND A.ID_PACIENTE = {id} THEN 1 ELSE 0 END AS BIT) AS ACTIVO FROM ENFERMEDAD as e LEFT JOIN HEREDO_FAMILIAR as h ON e.ID_ENFERMEDAD = h.ID_ENFERMEDAD left join Antecedente as A on h.ID_ANTECEDENTE = a.ID_ANTECEDENTE").ToListAsync();

            var types = diseaseOfPastient.Select(d => d.Tipo).Distinct().ToList();

            List<DiseaseVM> disease_vms = new List<DiseaseVM>();


            foreach (var g in types)
            {
                var diseaseByTypes = new DiseaseVM();

                var disease = diseaseOfPastient.Where(d => d.Tipo == g).ToList();

                diseaseByTypes.Tipo = g;
                diseaseByTypes.Diseases = disease;

                disease_vms.Add(diseaseByTypes);

            }


            return disease_vms;


        }
        public async Task<List<DiseaseVM>> GetPathologicalDiseases(int IdPatient)
        {


            var diseaseOfPastient = await _context.DiseaseCheckLists.FromSqlRaw($@"Select e.ID_ENFERMEDAD,e.NOMBRE,E.TIPO,'' AS 'PARENTESCO',CAST(CASE WHEN e.ID_ENFERMEDAD IS NOT NULL AND A.ID_PACIENTE = {IdPatient}
			THEN 1 ELSE 0 END AS BIT) AS ACTIVO 
			FROM ENFERMEDAD as e LEFT JOIN ENFERMEDAD_PATOLOGICA as p on e.ID_ENFERMEDAD = p.ID_ENFERMEDAD
			LEFT JOIN PATOLOGICO AS pa ON pa.ID_PATOLOGICO = P.ID_PATOLOGICO
			LEFT JOIN ANTECEDENTE AS A ON A.ID_ANTECEDENTE = PA.ID_ANTECEDENTE").ToListAsync();

            var types = diseaseOfPastient.Select(d => d.Tipo).Distinct().ToList();

            List<DiseaseVM> disease_vms = new List<DiseaseVM>();


            foreach (var g in types)
            {
                var diseaseByTypes = new DiseaseVM();

                var disease = diseaseOfPastient.Where(d => d.Tipo == g).ToList();

                diseaseByTypes.Tipo = g;
                diseaseByTypes.Diseases = disease;

                disease_vms.Add(diseaseByTypes);

            }


            return disease_vms;


        }

        public async Task<int> GetIdHistoryPatient(int id)
        {

            var idHistory = 0;
            try
            {
                var obj = await _context.Antecedente.Where(a => a.Id_Paciente == id).FirstOrDefaultAsync();
                idHistory = obj.Id_Antecedente;
                return idHistory;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return idHistory;
        }
        public async Task<JsonArray> SaveDiseaseFamily(DiseaseVM obj)
        {

            var resultsDB = new JsonArray();
            var msgDb = "";
            var res = false;


            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;

            var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
            msg.Direction = ParameterDirection.Output;

            try
            {
                var id_history = await GetIdHistoryPatient(obj.Id_Paciente);
                var values = this.ConcatDiseases(obj.Diseases);
                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.SP_P_Guardar_Heredo_Familiar
               
                @ENFERMEDADES = {values},
                @ID_ANTECEDENTE = {id_history},
               
                @Mensaje = {msg} OUTPUT,
                @Resultado = {result} OUTPUT
                ");

                msgDb = (string)msg.Value;
                res = (bool)result.Value;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);
                return resultsDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                msgDb = "Ocurrió un error al eliminar!";
                res = false;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);
                return resultsDB;
            }

        }
        public async Task<JsonArray> SavePathologicalDiseases(DiseaseVM obj)
        {
            var resultsDB = new JsonArray();
            var msgDb = "";
            var res = false;


            var result = new SqlParameter("@Resultado", SqlDbType.Bit);
            result.Direction = ParameterDirection.Output;

            var msg = new SqlParameter("@Mensaje", SqlDbType.VarChar, 100);
            msg.Direction = ParameterDirection.Output;

            try
            {
                var values = this.ConcatPathologicalDiseases(obj.Diseases);
                await _context.Database.ExecuteSqlInterpolatedAsync($@"EXEC dbo.SP_P_Guardar_Enfermedad_Patologica
               
                @ENFERMEDADES = {values},
                @ID_PACIENTE = {obj.Id_Paciente},
               
                @Mensaje = {msg} OUTPUT,
                @Resultado = {result} OUTPUT
                ");

                msgDb = (string)msg.Value;
                res = (bool)result.Value;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);
                return resultsDB;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                msgDb = "Ocurrió un error al eliminar!";
                res = false;

                resultsDB.Add(msgDb);
                resultsDB.Add(res);
                return resultsDB;
            }

        }
        public string ConcatDiseases(List<DiseaseCheckList> diseases)
        {
            string values = "";

            foreach (var d in diseases)
            {
                if (d.Activo)
                {
                    values += d.Id_Enfermedad.ToString() + "-" + d.Parentesco + ",";
                }
            }
            return values.TrimEnd(',');


        }
        public string ConcatPathologicalDiseases(List<DiseaseCheckList> diseases)
        {
            string values = "";

            foreach (var d in diseases)
            {
                if (d.Activo)
                {
                    values += d.Id_Enfermedad.ToString() + ",";
                }
            }
            return values.TrimEnd(',');

        }
    }
}
