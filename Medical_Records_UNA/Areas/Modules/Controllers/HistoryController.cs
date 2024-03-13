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
using Medical_Record_Data.Repository.Repository;

namespace Medical_Records_UNA.Areas.Modules.Controllers
{
    [Area("Modules")]
    [Authorize]
    public class HistoryController : Controller
    {
        
        private readonly IDiseaseRepository _diseaseRepository;

        public HistoryController( IDiseaseRepository diseaseRepository)
        { 
            _diseaseRepository = diseaseRepository;

        }
        public ActionResult Index() { return View(); }
        public async Task<PartialViewResult> _HistoryModule([FromBody] FolderDTO obj)
        {
            var diseasesByFamily = new List<DiseaseVM>();
            var diseasesByPathological = new List<DiseaseVM>();
            if (obj.Id_Paciente > 0)
            {
                diseasesByFamily = await _diseaseRepository.getDiseasByTypes(obj.Id_Paciente);
                diseasesByPathological = await _diseaseRepository.GetPathologicalDiseases(obj.Id_Paciente);

                this.HttpContext.Session.SetObject("PathologicalDiseases", diseasesByPathological);
                //this.HttpContext.Session.SetObject("DiseaseOfPatient", diseasesOfPatient);

            }

            return PartialView("_HistoryModule", diseasesByFamily);
        }

    }
}
