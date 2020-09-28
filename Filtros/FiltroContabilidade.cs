using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace gestaoContadorcomvc.Filtros
{
    public class FiltroContabilidade : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var user = context.HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);

            if (!conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "AcessoNegadoContabilidade" } });
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
