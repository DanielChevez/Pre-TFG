using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Medical_Records_UNA.Extensions;
using Medical_Records_Utilities.Extensions;
using Medical_Record_Data.Repository.IRepository;
using Medical_Record_Models.Tables;

namespace Medical_Records_UNA.Controllers
{
    public class AccessController : Controller
    {
    

        private readonly IUsersRepository _AccessRepository;

        public class LoginCredentials
        {
            public string NombreUsuario { get; set; }
            public string Contrasena { get; set; }

        }

        int PagActual = 0;
        const int DEFAULT_NUMBER_PAGE = 1;


        public AccessController(IUsersRepository accessRepository)
        {
            _AccessRepository = accessRepository;
        }

        // GET: AccessController
        public ActionResult Index()
        {

            var claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }




        [HttpPost]

        public async Task<JsonResult> AccessVerify([FromBody] LoginCredentials pCredentials)
        {
            string result = "0";

            try
            {

                if (ModelState.IsValid)
                { 


               
                   Usuario _user = new  (){ 
                      
                       NombreUsuario= pCredentials.NombreUsuario,
                       Contrasena= pCredentials.Contrasena
                    };
                      var verifyAccess =  await _AccessRepository.VerifyUser(_user);


                    if (verifyAccess)                                       
                    {

                        var User = await _AccessRepository.FindUser(pCredentials.NombreUsuario);



                         List<int> lstActions = await _AccessRepository.getAuthorizedActions(_user.NombreUsuario);
                        HttpContext.Session.SetObject("ActionsOfUser", lstActions);
                        
                        
                        //List<int> actions = HttpContext.Session.GetObject<List<int>>("ActionsOfUser");

                        List<Claim> claims = new() {

                            new Claim(ClaimTypes.Name, User.Nombre),
                            new Claim(ClaimTypes.Actor, User.NombreUsuario),
                            new Claim(ClaimTypes.Email, User.CorreoElectronico),

                        };
                        //foreach (var rol in VerifyUser.Roles)
                        //{
                        //    claims.Add(new Claim(ClaimTypes.Role, rol));
                        //}

                        ClaimsIdentity claimsIdentity = new(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));


                        ViewBag.Username = pCredentials.NombreUsuario;
                        ViewBag.Email = pCredentials.NombreUsuario;
                        ViewData["Validation Message"] = "Usuario no encontrado.";

                        result = "1";
                    }
                }
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

        //[Authorize(Roles = "Admin,Asitente,Profe")]
        


        [ActionsFilter(5)]

        // GET: AccessController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccessController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccessController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccessController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccessController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccessController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccessController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
