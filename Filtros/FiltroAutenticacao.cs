using System;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace gestaoContadorcomvc.Filtros
{
    public class FiltroAutenticacao : Attribute, IActionFilter
    {        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Código :  antes que a action executa 
            var user = context.HttpContext.Session.GetObjectFromJson<Usuario>("user");
            
            if (user == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary{{ "controller", "Conta" },{ "action", "login" }});
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Codigo  : depois que a action executa 
        }
    }
}
