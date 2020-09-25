using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Components
{
    public class Bread : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string retorno = "";

            string pagina = HttpContext.Request.GetDisplayUrl();

            Conta conta = new Conta();

            var cliente = HttpContext.Session.GetInt32("cliente_id");


            if (pagina.Contains("Contabilidade"))
            {
                retorno = "Contabilidade";

                if(cliente != null && cliente > 0)
                {
                    //Buscar conta e armazenar na ViewData["cliente"] para sre exibido no bread
                }

            }

            if (pagina.Contains("Home/Index"))
            {
                retorno = "Home Page";
            }

            if (pagina.Contains("Usuario/Index"))
            {
                retorno = "Usuário";
            }

            if (pagina.Contains("Categoria/Index"))
            {
                retorno = "Categorias";                
            }            

            ViewData["bread"] = retorno;

            return View();
        }
    }
}
