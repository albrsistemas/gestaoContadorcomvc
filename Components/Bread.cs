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
        public IViewComponentResult Invoke(string area)
        {
            string retorno = "";

            string openNav = "fechado";

            string pagina = HttpContext.Request.GetDisplayUrl();

            Conta conta = new Conta();

            var cliente = HttpContext.Session.GetInt32("cliente_id");

            //Contabilidade
            if (area.Equals("Contabilidade"))
            {
                retorno = "Contabilidade";

                if (cliente != null && cliente > 0)
                {
                    conta = conta.buscarConta(Convert.ToInt32(cliente));
                    retorno = conta.conta_email;
                }
                else
                {
                    retorno = "Nenhum cliente selecionado";
                }
            }

            //Empresa
            if (area.Equals("Empresa"))
            {
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
                if (pagina.Contains("Configuracoes/Index"))
                {
                    retorno = "Configurações";
                    openNav = "aberto";
                }
                if (pagina.Contains("Configuracoes/Contabilidade"))
                {
                    retorno = "Configurações Contábeis";
                    openNav = "aberto";
                }
            }

            ViewData["openNav"] = openNav;

            ViewData["area"] = area;

            ViewData["bread"] = retorno;

            return View();
        }
    }
}
