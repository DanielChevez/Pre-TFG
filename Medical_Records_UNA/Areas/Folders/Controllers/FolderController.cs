using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.DTO;
using Medical_Record_Models.Tables;
using Medical_Records_Utilities.Extensions;
namespace Medical_Records_UNA.Areas.Expediente.Controllers
{
    [Area("Folders")]
    [Authorize]
    public class FolderController : Controller
    {

        private readonly IFolder_Repository _folderRepository;
       
        public FolderController(IFolder_Repository folder_Repository)
        {
            _folderRepository = folder_Repository;
       
        }

        int PagActual = 0;
        const int DEFAULT_NUMBER_PAGE = 1;

        public List<FolderDTO> lstFolders = new List<FolderDTO>();
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
                lstFolders = await _folderRepository.GetFolders(objPag, context);
                PagActual = 0;

                this.HttpContext.Session.SetObject("FolderList", lstFolders);
                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");
                ViewBag.PalabrapalabraBuscar = "";

                

            }
            return View();
        }
        public async Task<ActionResult> View_Modules(int IdPaciente)
        {
            try
            {

                var folder = FindOnList(IdPaciente);
                if (folder != null)
                {


                    return View(folder);

                }
                RedirectToAction("/Index");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            return View();
        }


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

        #region PARTIAL_VIEWS
        //public async Task<PartialViewResult> _InformationModule([FromBody] FolderDTO Folder)
        //{

        //    if (Folder.Id_Paciente > 0)
        //    {
              
        //        var fold = FindOnList(Folder.Id_Paciente);
        //        All_Personal_informationDTO information =  await _folderRepository.GetInformation(Folder.Id_Paciente);

                
        //        return PartialView("_InformationModule",information);
        //    }
        //    return PartialView("_InformationModule");
        //}



        public async Task<PartialViewResult> _ShowFolders( [FromBody] Pagina obj) {
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


            lstFolders = await _folderRepository.GetFolders(objPag, HttpContext);

            this.HttpContext.Session.SetObject("FolderList", lstFolders);


            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");
            

            return PartialView("_ShowFolders");

        }


        public async Task<PartialViewResult> _TableFolders([FromBody] Pagina obj) {

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


            lstFolders = await _folderRepository.GetFolders(objPag, HttpContext);

            this.HttpContext.Session.SetObject("FolderList", lstFolders);


            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");

            return PartialView("_TableFolders");
        }
        #endregion PARTIAL_VIEWS

    }
}
