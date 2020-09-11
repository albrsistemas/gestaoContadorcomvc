using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Filtros
{
    public class FiltroAutorizacao : Attribute, IActionFilter
    {
        public string permissao { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var user = context.HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if(user.Role.ToUpper() != "ADM")
            {
                if (user == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Conta" }, { "action", "login" } });
                }

                if (permissao.ToUpper() == "ADM" && user.Role.ToUpper() != "ADM")
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "AcessoNegado" } });
                }
                else
                {
                    if (!user.permissoes.Contains(permissao))
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "AcessoNegado" } });
                    }
                }
            }           

            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
