using Medical_Record_Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Authorization;

using System.Text.Json.Nodes;

using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.DTO;
using Medical_Record_Models.Tables;
using Medical_Records_Utilities.Extensions;
using Medical_Record_Models.ViewModels;
namespace Medical_Records_UNA.Areas.Modules.Controllers
{
    [Area("Modules")]
    [Authorize]
    public class Patient_InformationController : Controller
    {

        private readonly IFolder_Repository _folderRepository;
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IPatientRepository _patient_Repository;
        public Patient_InformationController(IPatientRepository patient_Repository, IFolder_Repository folder_Repository, IDiseaseRepository diseaseRepository)
        {
            _folderRepository = folder_Repository;
            _diseaseRepository = diseaseRepository;
            _patient_Repository = patient_Repository;
        }

        public async Task<PartialViewResult> _InformationModule([FromBody] FolderDTO Folder)
        {

            if (Folder.Id_Paciente > 0)
            {

                var fold = FindOnList(Folder.Id_Paciente);
                All_Personal_informationDTO information = await _patient_Repository.GetInformation(Folder.Id_Paciente);


                return PartialView("_InformationModule", information);
            }
            return PartialView("_InformationModule");
        }

        #region CRUD
        public async Task<JsonResult> Add_Update_Information_Personal([FromBody] Personal_informationDTO Information)
        {
            JsonArray result = new JsonArray();
            try
            {

                DateTime fechaActual = DateTime.Now;

                Information.Edad = CalcularEdad(Information.Fecha_Nacimiento, fechaActual);


                //TRUE = ACUTALIZAR
                //Information.Accion = true;
                if (ModelState.IsValid)
                {
                    result = await _patient_Repository.Add_Update_Information(Information);
                    return Json(new { result });
                }
                else
                {
                    var msg = "Error al actualizar, Información no Valida";
                    var res = false;
                    result.Add(msg);
                    result.Add(res);

                    return Json(new { result });
                }



            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return Json(null);
            }

        }
        #endregion

        #region FIND_ON_lIST
        public FolderDTO FindOnList(int Id_Paciente)
        {
            if (Id_Paciente > 0)
            {
                var list = HttpContext.Session.GetObject<List<FolderDTO>>("FolderList");

                var folder = list.Where(f => f.Id_Paciente == Id_Paciente).FirstOrDefault();
                if (folder != null)
                    return folder;
            }
            return null;
        }
        #endregion FIND_ON_lIST


        static int CalcularEdad(DateTime fechaNacimiento, DateTime fechaActual)
        {
            int edad = fechaActual.Year - fechaNacimiento.Year;

            // Restar un año si la fecha de nacimiento aún no ha ocurrido este año
            if (fechaActual.Month < fechaNacimiento.Month || (fechaActual.Month == fechaNacimiento.Month && fechaActual.Day < fechaNacimiento.Day))
            {
                edad--;
            }

            return edad;
        }

    }
}
