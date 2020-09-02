using System;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Mvc.Filters;

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
                context.HttpContext.Response.Redirect("/Conta/Login");
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Codigo  : depois que a action executa 
        }
    }
}
