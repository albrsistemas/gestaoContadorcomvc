using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
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

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            string retorno = "";

            string openNav = "fechado";

            string pagina = HttpContext.Request.GetDisplayUrl();

            var cliente = user.usuario_ultimoCliente;
            Conta contexto = new Conta();
            contexto = contexto.buscarConta(Convert.ToInt32(cliente));

            //Contabilidade
            if (area.Equals("Contabilidade"))
            {
                retorno = "Contabilidade";

                if (cliente != null && cliente != "0")
                {   
                    retorno = contexto.conta_nome;
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
                if (pagina.Contains("Participante/Index"))
                {
                    retorno = "Cliente/Fornecedor";                    
                }
                if (pagina.Contains("Participante/Create"))
                {
                    retorno = "Incluir Cliente/Fornecedor";
                }
                if (pagina.Contains("Participante/Edit"))
                {
                    retorno = "Alterar Cliente/Fornecedor";
                }
            }

            ViewData["openNav"] = openNav;

            ViewData["area"] = area;

            ViewData["bread"] = retorno;

            ViewData["userBread"] = user;

            return View();
        }
    }
}
