using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.DTO;
using Medical_Record_Models.Tables;
using Medical_Records_Utilities.Extensions;
using System.Text.Json.Nodes;
using Medical_Record_Models.ViewModels;

namespace Medical_Records_UNA.Areas.MedicalConditions.Controllers
{
    [Area("MedicalConditions")]
    [Authorize]
    public class DiseaseController : Controller
    {
        public readonly IDiseaseRepository _diseaseRepository;

        public DiseaseController(IDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
        }


        int PagActual = 0;
        const int DEFAULT_NUMBER_PAGE = 1;

        public List<DiseaseDTO> lstDiseases = new List<DiseaseDTO>();
        public async Task<ActionResult> Index()
        {
            var claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                HttpContext context = ControllerContext.HttpContext;
                var objPag = new Pagina
                {
                    PaginaActual = 0,
                    NumPagina = 0,
                    CantRegistros = 10,
                    palabraBuscar = "",
                };



                lstDiseases = await _diseaseRepository.GetDiseases(objPag, context);
                PagActual = 0;

                this.HttpContext.Session.SetObject("DiseasesLst", lstDiseases);
                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");
                ViewBag.PalabrapalabraBuscar = "";


            }
            return View();
        }
        public async Task<JsonResult> Create_Update_Disease([FromBody] DiseaseDTO Disease)
        {
            JsonArray result = new JsonArray();
            if (Disease != null)
            {
                if (ModelState.IsValid)
                {
                    result = await _diseaseRepository.Create_Update_Disease(Disease);
                    return Json(new { result });

                }
                else
                {
                    var msg = "Error al procesar la solicitud, Información no Valida";
                    var res = false;
                    result.Add(msg);
                    result.Add(res);

                    return Json(new { result });
                }
            }
            return Json(new { result });

        }

        public async Task<JsonResult> getDisease([FromBody] DiseaseDTO obj)
        {

            var disease = new DiseaseDTO();
            try
            {
                if (obj.Id_Enfermedad > 0)
                {
                    disease = await _diseaseRepository.FindDisease(Convert.ToInt32(obj.Id_Enfermedad));

                }
                return Json(new { disease });
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return Json(new { disease });
            }
        }

        public async Task<JsonResult> Delete([FromBody] DiseaseDTO obj)
        {
            var result = new JsonArray();
            try
            {
                if (obj.Id_Enfermedad > 0)
                {
                    result = await _diseaseRepository.Delete(Convert.ToInt32(obj.Id_Enfermedad));
                }
                return Json(new { result });
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return Json(new { result });
            }

        }


        public async Task<JsonResult> SaveFamilyDisease([FromBody] DiseaseVM obj)
        {
            var result = new JsonArray();
            try
            {
                if (obj != null && obj.Diseases != null)
                {
                    result = await _diseaseRepository.SaveDiseaseFamily(obj);
                }
                return Json(new { result });
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
                return Json(new { result });
            }
        }
        public async Task<JsonResult> SavePathologicalDisease([FromBody] DiseaseVM obj)
        {
            var result = new JsonArray();
            try
            {
                if (obj != null)
                {
                    result = await _diseaseRepository.SavePathologicalDiseases(obj);
                    var flag = (Boolean) result[1].AsValue(); 
                    if (flag == true)
                    {
                        var diseasesByPathological = new List<DiseaseVM>();

                        diseasesByPathological = await _diseaseRepository.GetPathologicalDiseases(obj.Id_Paciente);
                        this.HttpContext.Session.SetObject("PathologicalDiseases", diseasesByPathological);
                        
                    }
                    return Json(new { result });
                }
                return Json(new { result });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { result });
            }
        }
        public async Task<JsonResult> getTypes([FromBody] int id)
        {
            var Disease = new List<DiseaseVM>();
            try
            {
                Disease = await _diseaseRepository.getDiseasByTypes(id);
                return Json(new { Disease });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { Disease });
            }

        }
        //public async Task<JsonResult> Create([FromBody] DiseaseDTO Disease)
        //{
        //    JsonArray result = new JsonArray();
        //    if (Disease != null)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            result = await _diseaseRepository.Create_Update_Disease(Disease);
        //            return Json(new { result });

        //        }
        //        else
        //        {
        //            var msg = "Error al procesar la solicitud, Información no Valida";
        //            var res = false;
        //            result.Add(msg);
        //            result.Add(res);

        //            return Json(new { result });
        //        }
        //    }
        //    return Json(new { result });

        //}

        public async Task<PartialViewResult> _TableDiseases([FromBody] Pagina obj)
        {
            int totalPag = this.HttpContext.Session.GetObject<int>("TotalPag");
            //Si no busca viene nulo
            if (obj.palabraBuscar == null) obj.palabraBuscar = "";

            // Validacion si el usuario esta buscado o solo pasando de pagina
            if (obj.accion.Equals('S')) obj.NumPagina += 1; //Enviar al SP
            else if (obj.accion.Equals('N')) obj.NumPagina -= 1;

            // Restricciones para que no busque paginas que no existe
            if (obj.NumPagina > obj.totalPaginas - 1) obj.NumPagina = Convert.ToInt32(totalPag);
            else if (obj.NumPagina < 0) obj.NumPagina = 0;

            var objPag = new Pagina
            {
                NumPagina = obj.NumPagina,
                CantRegistros = obj.CantRegistros,
                palabraBuscar = obj.palabraBuscar,
            };



            lstDiseases = await _diseaseRepository.GetDiseases(objPag, HttpContext);
            this.HttpContext.Session.SetObject("DiseasesLst", lstDiseases);


            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = this.HttpContext.Session.GetObject<int>("TotalPag");

            return PartialView("_TableDiseases");
        }



        public async Task<PartialViewResult> _ShowDiseases([FromBody] Pagina obj)
        {

            int totalPag = this.HttpContext.Session.GetObject<int>("TotalPag");
            //Si no busca viene nulo
            if (obj.palabraBuscar == null) obj.palabraBuscar = "";

            // Validacion si el usuario esta buscado o solo pasando de pagina
            if (obj.accion.Equals('S')) obj.NumPagina += 1; //Enviar al SP
            else if (obj.accion.Equals('N')) obj.NumPagina -= 1;

            // Restricciones para que no busque paginas que no existe
            if (obj.NumPagina > obj.totalPaginas - 1) obj.NumPagina = Convert.ToInt32(totalPag);
            else if (obj.NumPagina < 0) obj.NumPagina = 0;

            var objPag = new Pagina
            {
                NumPagina = obj.NumPagina,
                CantRegistros = obj.CantRegistros,
                palabraBuscar = obj.palabraBuscar,
            };

            lstDiseases = await _diseaseRepository.GetDiseases(objPag, HttpContext);
            this.HttpContext.Session.SetObject("DiseasesLst", lstDiseases);


            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");

            return PartialView("_ShowDiseases");
        }
    }
}
