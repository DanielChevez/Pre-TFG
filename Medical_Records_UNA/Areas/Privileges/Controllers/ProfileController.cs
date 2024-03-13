using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Medical_Records_Utilities.Extensions;
using System.Text.RegularExpressions;
using System.Text.Json.Nodes;
using Medical_Record_Models.ModelsGeneric;
using Microsoft.AspNetCore.Mvc.Rendering;
using Medical_Record_Models.ViewModels;
using System.Security.Claims;
using Medical_Records_UNA.Extensions;
using Medical_Record_Models.Tables;

namespace Medical_Records_UNA.Areas.Privileges.Controllers
{
    [Area("Privileges")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IUsuarioPerfilRepository _userProfile;


        public ProfileController(IProfileRepository profileRepository, IUsuarioPerfilRepository userProfile, IUsersRepository usersRepository)
        {
            _profileRepository = profileRepository;
            _userProfile = userProfile;
            _usersRepository = usersRepository;
        }

        int PagActual = 0;
        const int DEFAULT_NUMBER_PAGE = 1;

        public List<ProfileDTO> lstProfiles = new();



        // GET: ProfileController
        [ActionsFilter(9)]
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

                lstProfiles = await _profileRepository.GetProfiles(objPag, context);
                PagActual = 0;

                HttpContext.Session.SetObject("ProfileList", lstProfiles);
                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");
                ViewBag.PalabrapalabraBuscar = "";
            }
            return View();
        }



        #region CREATE


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateProfile([FromBody] ProfileDTO profile)
        {
            var result = false;
            try
            {
                if (profile != null)
                {
                    if (ValidateProfileData(profile))
                    {
                        result = await _profileRepository.Create(profile);
                    }
                }

                return Json(new { result });
            }
            catch
            {
                return Json(new { result });
            }
        }

        #endregion CREAR

        #region Edit
        [ActionsFilter(8)]
        public async Task<ActionResult> Edit(int IdPerfil)
        {
            if (IdPerfil > 0)
            {
                ProfileDTO profile = await _profileRepository.Find(IdPerfil);

                if (profile != null)
                {
                    try
                    {
                        List<UsuarioPerfil> usersProfile = new();
                        int PagActual = 0;
                        //IdPerfil = 1;



                        //var profile = this.Find_Data_Profile(IdPerfil);
                        var objPag = new Pagina
                        {
                            PaginaActual = 0,
                            NumPagina = 0,
                            CantRegistros = 2,
                            palabraBuscar = "",
                        };

                        usersProfile = await _userProfile.Users_Profile(objPag, HttpContext, IdPerfil);

                        //var ddlUsers = await _userProfile.optionsUsersProfile(IdPerfil);
                        List<SelectListItem> ddlUsers = await getOptionsUser(IdPerfil);


                        HttpContext.Session.SetObject("editProfile", profile);


                        //HttpContext.Session.SetObject("lstUsersProfile", usersProfile);

                        ViewBag.lstUser = ddlUsers;
                        ViewBag.lstUsersProfile = usersProfile;

                        ViewBag.PagActualUP = PagActual + 1;
                        ViewBag.totalPagUP = HttpContext.Session.GetObject<int>("TotalPagUP");

                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return Redirect("/Profiles/");
                    }
                }
            }

            return Redirect("/Profiles/");
        }

        #endregion Edit

        #region UPDATE
        [HttpPost]
        [ActionsFilter(8)]
        public async Task<JsonResult> UpdateProfile([FromBody] ProfileDTO profile)
        {

            var result = false;
            try
            {
                if (profile != null)
                {
                    if (ValidateProfileData(profile))
                    {
                        result = await _profileRepository.Update(profile);
                    }
                }

                return Json(new { result });
            }
            catch
            {
                return Json(new { result });
            }
        }

        #endregion UPDATE


        #region DELETE
        [ActionsFilter(14)]
        public async Task<JsonResult> DeleteProfile(int id)
        {
            var result = new JsonArray();
            if (id > 0)
            {
                result = await _profileRepository.Delete(id);

            }
            return Json(new { result });
        }

        #endregion DELETE


        #region CHANGE STATUS OF PROFILE 
        [ActionsFilter(8)]
        public async Task<JsonResult> ChangeStatus(int IdPerfil)
        {

            var result = false;
            try
            {
                if (IdPerfil > 0)
                {
                    result = await _profileRepository.ChangeStatus(IdPerfil);

                }
                return Json(new { result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { result });
            }


        }
        #endregion CHANGE STATUS OF PROFILE 


        #region FIND_PROFILE
        [HttpPost]
        public JsonResult Find_Data_Profile(int IdPerfil)
        {
            var profile = new ProfileDTO();
            try
            {
                if (IdPerfil > 0)
                {
                    List<ProfileDTO> lst = HttpContext.Session.GetObject<List<ProfileDTO>>("ProfileList");
                    profile = lst.Find(p => p.IdPerfil == IdPerfil);
                }
                return Json(new { profile });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { profile });
            }

        }
        #endregion FIND_PROFILE

        #region PARTIAL_VIEWS
        [ActionsFilter(9)]
        public async Task<PartialViewResult> _ShowProfiles([FromBody] Pagina obj)
        {
            int totalPag = HttpContext.Session.GetObject<int>("TotalPag");
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
                CantRegistros = 10,
                palabraBuscar = obj.palabraBuscar,
            };


            lstProfiles = await _profileRepository.GetProfiles(objPag, HttpContext);

            HttpContext.Session.SetObject("ProfileList", lstProfiles);

            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");

            return PartialView("_ShowProfiles");

        }


        [ActionsFilter(9)]
        public async Task<PartialViewResult> _TableProfiles([FromBody] Pagina obj)
        {
            int totalPag = HttpContext.Session.GetObject<int>("TotalPag");
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
                CantRegistros = 10,
                palabraBuscar = obj.palabraBuscar,
            };


            lstProfiles = await _profileRepository.GetProfiles(objPag, HttpContext);

            HttpContext.Session.SetObject("ProfileList", lstProfiles);

            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");

            return PartialView("_TableProfiles");

        }


        [ActionsFilter(16)]
        public async Task<PartialViewResult> _UsersProfile([FromBody] Pagina obj)
        {
            List<UsuarioPerfil> lstUsersProfile = new List<UsuarioPerfil>();
            try
            {
                int totalPag = HttpContext.Session.GetObject<int>("TotalPagUP");
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


                lstUsersProfile = await _userProfile.Users_Profile(objPag, HttpContext, int.Parse(obj.Usuario));


                //ViewBag.palabraBuscarUP = obj.palabraBuscar;
                //ViewBag.CantRegistrosUP = obj.CantRegistros;


                ViewBag.PagActualUP = obj.NumPagina + 1;
                ViewBag.totalPagUP = HttpContext.Session.GetObject<int>("TotalPagUP");

                return PartialView("_UsersProfile", lstUsersProfile);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return PartialView("_UsersProfile", lstUsersProfile);
        }
        [ActionsFilter(15)]
        public async Task<PartialViewResult> _SelectOpcUser([FromBody] ProfileDTO profile)
        {
            var options = new List<SelectListItem>();
            if (profile.IdPerfil > 0)
            {
                options = await getOptionsUser(Convert.ToInt32(profile.IdPerfil));
            }
            return PartialView("_SelectOpcUser", options);
        }
        #endregion PARTIAL_VIEW
        [ActionsFilter(15)]
        public async Task<JsonResult> agregarUsuarioPerfil([FromBody] UsuarioPerfilDTO obj)
        {
            var result = false;
            if (obj.Usuario != null || obj.IdPerfil > 0)
            {
                try
                {
                    result = await _userProfile.Create_User_Profile(obj.Usuario, obj.IdPerfil);
                    return Json(new { result });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return Json(new { result });
                }
            }
            return Json(new { result });
        }

        [ActionsFilter(17)]
        public async Task<JsonResult> eliminarUsuarioPerfil([FromBody] UsuarioPerfilDTO obj)
        {
            var result = false;
            try
            {
                result = await _userProfile.Delete_User_Profile(obj.Usuario, obj.IdPerfil);
                return Json(new { result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { result });
            }
        }







        #region TREE


        [ActionsFilter(18)]
        [HttpPost]
        public async Task<JsonResult> SaveActions([FromBody] ProfileDTO profile)
        {

            // Procesa la lista de enteros según sea necesario

            var result = false;


            var claimUser = HttpContext.User;


            if (claimUser.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claimsPrincipal = User;
                var username = claimsPrincipal.FindFirst(ClaimTypes.Actor)?.Value;

                try
                {
                    if (profile.IdPerfil != 0)
                    {
                        result = await _userProfile.SaveCheckNodes(profile.Actions, Convert.ToInt32(profile.IdPerfil));

                        if (result)
                        {
                            List<int> lstActions = await _usersRepository.getAuthorizedActions(username);
                            HttpContext.Session.SetObject("ActionsOfUser", lstActions);
                        }
                    }
                    return Json(new { result });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return Json(new { result });
                }
            }
            else
            {
                return Json(new { result });
            }
        }
        [ActionsFilter(18)]
        public async Task<JsonResult> Tree(int id)
        {

            var result = new List<ArbolPrivilegiosViewModel>();

            var claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                try
                {

                    result = await _userProfile.DrawTree(id);

                    return Json(new { result });

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message.ToString());
                    return Json(new { result });
                }
            }
            else
            {
                return Json(new { result });
            }

        }


        public async Task<JsonResult> VerifyPassword(string password)
        {
            var result = false;
            string pattern = @"\[a-zA-Z0-9_]";
            try
            {
                //if (Regex.IsMatch(password, pattern))
                //{
                ClaimsPrincipal claimsPrincipal = User;

                var username = claimsPrincipal.FindFirst(ClaimTypes.Actor)?.Value;
                if (ValidatePassword(password))
                {
                    result = await _userProfile.VerifyPassword(username, password);
                    return Json(new { result });
                }
                // }

                return Json(new { result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new
                {
                    result
                });
            }

        }


        //public async void ActionsTree(int id)
        //{
        //    var result = new List<ArbolPrivilegiosViewModel>();

        //    var claimUser = HttpContext.User;
        //    if (claimUser.Identity.IsAuthenticated)
        //    {

        //        try
        //        {

        //            result = await _userProfile.DrawTree(id);

        //            HttpContext.Session.SetObject("Arbol", result);


        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Write(ex.Message.ToString());

        //        }
        //    }

        //}

        #endregion TREE
        public async Task<List<SelectListItem>> getOptionsUser(int IdPerfil)
        {

            var lstOpc = await _userProfile.optionsUsersProfile(IdPerfil);

            List<SelectListItem> options = lstOpc.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.NombreUsuario.ToString(),
                    Selected = true
                };
            });

            return options;
        }
        private bool ValidateProfileData(ProfileDTO profile)
        {
            // Validar las expresiones regulares usando Regex.IsMatch
            if (!Regex.IsMatch(profile.Nombre, @"^[a-zA-Z]{1,25}$"))
            {
                // El nombre de perfil no cumple con la expresión regular
                return false;
            }

            if (!Regex.IsMatch(profile.Descripcion, @"^[a-zA-Z\s]{1,100}$"))
            {
                // La descripción de perfil no cumple con la expresión regular
                return false;
            }

            // Si es necesario, realizar más validaciones aquí

            // Todos los datos cumplen con las expresiones regulares
            return true;
        }

        public bool ValidatePassword(string Password)
        {

            string pattern = @"^[a-zA-Z0-9!@#$%^&*()\-_=+{}\[\]|\\;:'""<>,.?\/]*$";

            if (Regex.IsMatch(Password, pattern))
            {
                return true;
            }
            else
            {
                Console.WriteLine("El input contiene caracteres especiales no permitidos.");
                return false;
            }

        }

    }
}
