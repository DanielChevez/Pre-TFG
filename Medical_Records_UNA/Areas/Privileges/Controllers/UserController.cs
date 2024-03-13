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
    public class UserController : Controller
    {
        private readonly IUsersRepository _userRepository;
        private readonly IUsuarioPerfilRepository _userProfile;
        public UserController(IUsersRepository userRepository, IUsuarioPerfilRepository usuarioPerfil)
        {
            _userRepository = userRepository;
            _userProfile = usuarioPerfil;
        }

        int PagActual = 0;
        const int DEFAULT_NUMBER_PAGE = 1;
        public List<UserDTO> lstUsers = new();


        [ActionsFilter(6)]

        public async Task<IActionResult> Index()
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

                lstUsers = await _userRepository.GetUsers(objPag, context);


                HttpContext.Session.SetObject("UsersList", lstUsers);
                ViewBag.PagActual = PagActual + 1;
                ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");
                ViewBag.PalabrapalabraBuscar = "";

            }

            return View();
        }


        #region METODOS CRUD

        [ActionsFilter(5)]
        public async Task<ActionResult> Edit(string Username)
        {
            if (Username != null && Username != "")
            {
                //var us = await _userProfile.FindUser(Username);
                //var list = HttpContext.Session.GetObject<List<UserDTO>>("UsersList");
                //UserDTO userDTO = list.Where(u=> u.NombreUsuario == Username).FirstOrDefault();

                UserDTO userDTO = await _userRepository.FindUser(Username);

                if (userDTO != null)
                {
                    try
                    {
                        List<Perfil_Usuario> profilesUser = new();
                        int PagActual = 0;
                        var objPag = new Pagina
                        {
                            PaginaActual = 0,
                            NumPagina = 0,
                            CantRegistros = 2,
                            palabraBuscar = "",
                        };
                        profilesUser = await _userProfile.Profiles_User(objPag, HttpContext, Username);
                        var lstProfile = await _userProfile.optionsProfilesUser(Username);

                        List<SelectListItem> choiceProfiles = lstProfile.ConvertAll(d =>
                        {
                            return new SelectListItem()
                            {
                                Text = d.Descripcion.ToString(),
                                Value = d.IdPerfil.ToString(),
                                Selected = false,
                            };
                        });
                        HttpContext.Session.SetObject("editUser", userDTO);

                        ViewBag.PagActualPU = PagActual + 1;
                        ViewBag.totalPagPU = HttpContext.Session.GetObject<int>("TotalPagPU");

                        ViewBag.profiles = profilesUser;
                        ViewBag.opcProfiles = choiceProfiles;
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return Redirect("/Users/");
                    }
                }

            }

            return Redirect("/Users/");

        }


        [ActionsFilter(4)]
        [HttpPost]

        public async Task<JsonResult> CreateUser([FromBody] UserDTO user)
        {
            var result = false;
            try
            {

                if (ModelState.IsValid)
                {

                    Usuario _user = new()
                    {
                        NombreUsuario = user.NombreUsuario,
                        Nombre = user.Nombre,
                        Identification = user.Identification,
                        CorreoElectronico = user.CorreoElectronico,
                        Telefono = user.Telefono,
                        WhatsApp = user.whatsApp,
                        Tipo = user.Tipo,
                        Activo = user.Activo,
                        Contrasena = user.Contrasena,
                    };

                    result = await _userRepository.CreateUser(_user);
                    return Json(new { result });

                }
                return Json(new { result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { result });
            }
        }
        [HttpPost]

        [ActionsFilter(5)]
        public async Task<JsonResult> UpdateUser([FromBody] UserDTO user)
        {
            var result = false;
            try
            {

                if (ModelState.IsValid)
                {

                    Usuario _user = new()
                    {
                        NombreUsuario = user.NombreUsuario,
                        Nombre = user.Nombre,
                        Identification = user.Identification,
                        CorreoElectronico = user.CorreoElectronico,
                        Telefono = user.Telefono,
                        WhatsApp = user.whatsApp,
                        Tipo = user.Tipo,
                        Activo = user.Activo,
                        Contrasena = user.Contrasena,
                    };

                    result = await _userRepository.UpdateUser(_user);
                    return Json(new { result });

                }
                return Json(new { result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { result });
            }
        }


        [ActionsFilter(10)]

        [HttpPost]
        public async Task<JsonResult> DeleteUser(string UserName)
        {
            var result = new JsonArray();
            if (UserName != null)
            {
                result = await _userRepository.Delete(UserName);

            }
            return Json(new { result });
        }



        [HttpPost]
        public async Task<JsonResult> FindUser([FromBody] UserDTO pUser)
        {
            var res = 0;
            try
            {

                if (pUser.NombreUsuario != null)
                {
                    var list = HttpContext.Session.GetObject<List<UserDTO>>("UsersList");
                    UserDTO userDTO = new UserDTO();

                    userDTO = list.Where(u => u.NombreUsuario == pUser.NombreUsuario).FirstOrDefault();


                    if (userDTO != null)
                    {

                        return Json(new { userDTO });

                    }

                    var user = await _userRepository.FindUser(pUser.NombreUsuario);
                    return Json(new { user });


                }
                return Json(new { res });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { res });
            }
        }


        [ActionsFilter(5)]
        public JsonResult ChangeStatus(string userName)
        {
            if (userName == null) return Json(new { result = '0' });

            var result = _userRepository.changeStatus(userName);

            return Json(new { result });

        }
        #endregion CRUD



        #region PARTIAL VIEWS



        [HttpPost]

        //==========================================

        //    RETURN TO VIEW OF TABLE USER

        //==========================================

        [ActionsFilter(6)]
        public async Task<PartialViewResult> _ShowUsers([FromBody] Pagina obj)
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


            lstUsers = await _userRepository.GetUsers(objPag, HttpContext);

            HttpContext.Session.SetObject("UsersList", lstUsers);

            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");

            return PartialView("_ShowUsers");
        }

        [ActionsFilter(6)]
        [HttpPost]
        public async Task<PartialViewResult> _TableUsers([FromBody] Pagina obj)
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
                CantRegistros = obj.CantRegistros,
                palabraBuscar = obj.palabraBuscar,
            };


            lstUsers = await _userRepository.GetUsers(objPag, HttpContext);

            HttpContext.Session.SetObject("UsersList", lstUsers);

            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = HttpContext.Session.GetObject<int>("TotalPag");

            return PartialView("_TableUsers");
        }


        [ActionsFilter(13)]
        public async Task<PartialViewResult> _ProfilesUser([FromBody] Pagina obj)
        {
            List<Perfil_Usuario> lstProfiles = new List<Perfil_Usuario>();
            try
            {
                int totalPag = HttpContext.Session.GetObject<int>("TotalPagPU");
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
                //ClaimsPrincipal claimsPrincipal = User;
                //string actor = claimsPrincipal.FindFirst(ClaimTypes.Actor)?.Value;


                lstProfiles = await _userProfile.Profiles_User(objPag, HttpContext, obj.Usuario);



                ViewBag.PagActualPU = obj.NumPagina + 1;
                ViewBag.totalPagPU = HttpContext.Session.GetObject<int>("TotalPagPU");
                return PartialView("_ProfilesUser", lstProfiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return PartialView("_ProfilesUser", lstProfiles);
            }

        }
        [ActionsFilter(11)]
        public async Task<PartialViewResult> _SelectOpcProfile([FromBody] UserDTO userDTO)
        {
            if (userDTO.NombreUsuario != null || userDTO.NombreUsuario != "")
            {
                List<SelectListItem> choiceProfiles = new List<SelectListItem>();
                try
                {
                    var lstProfile = await _userProfile.optionsProfilesUser(userDTO.NombreUsuario);

                    choiceProfiles = lstProfile.ConvertAll(d =>
                      {
                          return new SelectListItem()
                          {
                              Text = d.Descripcion.ToString(),
                              Value = d.IdPerfil.ToString(),
                              Selected = false,
                          };
                      });
                    return PartialView("_SelectOpcProfile", choiceProfiles);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }
            }
            return PartialView("_SelectOpcProfile");


        }
        #endregion

        [ActionsFilter(11)]
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

        [ActionsFilter(12)]
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

        public JsonResult GetUserData(string UserName)
        {
            UserDTO user = new();
            List<UserDTO> list = HttpContext.Session.GetObject<List<UserDTO>>("UsersList");
            if (UserName != null)
            {

                user = list.Find(u => u.NombreUsuario == UserName);


            }

            return Json(new { user });

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
