using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Filtros
{
    public class Autoriza : Attribute, IActionFilter
    {
        public string permissao { get; set; }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (permissao == null)
            {
                permissao = "";
            }

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            //user = usuario.BuscaUsuario(Convert.ToInt32(context.HttpContext.User.Identity.Name));
            user = usuario.BuscaUsuario(context.HttpContext.User.Identity.Name);

            if (user == null || user.usuario_id == 0)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Conta" }, { "action", "login" } });
            }

            if (user.conta.conta_tipo.ToUpper().Equals("CLIENTE"))
            {
                if(user.Role.ToUpper() != "ADM")
                {  

                    if (!(bool)user._permissoes.GetType().GetProperty(permissao).GetValue(user._permissoes))
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "AcessoNegado" } });
                    }
                }
            }

            if (user.conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
            {
                if (user.Role.ToUpper() != "ADM")
                {
                    if (!context.RouteData.Values.Keys.Contains("area"))
                    {
                        if (user._permissoes.area_empresa_contador == false)
                        {
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "AcessoNegado" } });
                        }                       
                    }
                    else
                    {
                        if (!(bool)user._permissoes.GetType().GetProperty(permissao).GetValue(user._permissoes))
                        {
                            context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "AcessoNegadoContador" } });
                        }
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
