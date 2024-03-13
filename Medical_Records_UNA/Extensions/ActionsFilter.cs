using Medical_Record_Data;
using Medical_Record_Models;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using Medical_Record_Data;
using Medical_Records_Utilities.Extensions;

namespace Medical_Records_UNA.Extensions
{
    public class ActionsFilterAttribute : TypeFilterAttribute
    {
        public int NumeroDeAccion { get; set; } // Propiedad para almacenar el número de acción


        public ActionsFilterAttribute(int numeroDeAccion) : base(typeof(NumberActionsFilterImpl))
        {
            Arguments = new object[] { numeroDeAccion }; // Pasar el número de acción como argumento al constructor interno
        }

        private class NumberActionsFilterImpl : IAuthorizationFilter
        {
            private readonly int _numeroDeAccion;
            private static readonly Dictionary<int, string> listaAcciones = new() { };
            public NumberActionsFilterImpl(int numeroDeAccion)
            {
                _numeroDeAccion = numeroDeAccion;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {

                try
                {

                    
                    var claimUser = context.HttpContext.User;
                    if (claimUser.Identity.IsAuthenticated)
                    {
                        
                        List<int> actionsAllow = context.HttpContext.Session.GetObject<List<int>>("ActionsOfUser");
                        if (!(actionsAllow.Contains(_numeroDeAccion)))
                        {
                            // Verificar si el número de acción solicitado está en la lista de números de acción permitidos para el usuario
                            if (actionsAllow.Contains(_numeroDeAccion))
                            {
                                string url = listaAcciones[_numeroDeAccion];
                                context.Result = new ForbidResult();
                            }
                            else
                            {
                                context.Result = new RedirectResult("/Home/Index");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    context.Result = new RedirectResult("/Home/Index");
                }
            }

            // Propiedad para obtener el número de acción permitido
            public int NumeroDeAccion => _numeroDeAccion;
        }
    }
}
