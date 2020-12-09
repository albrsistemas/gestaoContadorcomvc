﻿using gestaoContadorcomvc.Models.Autenticacao;
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
                //Usuário
                if (pagina.Contains("Usuario/Index"))
                {
                    retorno = "Usuário";
                }
                if (pagina.Contains("Usuario/Create"))
                {
                    retorno = "Incluir Usuário";
                }
                if (pagina.Contains("Usuario/Edit"))
                {
                    retorno = "Alterar Usuário";
                }

                //Categorias
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
                //Produtos
                if (pagina.Contains("Produtos/Index"))
                {
                    retorno = "Produtos";
                }
                if (pagina.Contains("Produtos/Create"))
                {
                    retorno = "Incluir Produto";
                }
                if (pagina.Contains("Produtos/Edit"))
                {
                    retorno = "Alterar Produto";
                }
                //ContaCorrente
                if (pagina.Contains("ContaCorrente/Index"))
                {
                    retorno = "Conta Corrente";
                }
                if (pagina.Contains("ContaCorrente/Create"))
                {
                    retorno = "Incluir Conta Corrente";
                }
                if (pagina.Contains("ContaCorrente/Edit"))
                {
                    retorno = "Alterar Conta Corrente";
                }
                //Forma de pagamento
                if (pagina.Contains("FormaPagamento/Index"))
                {
                    retorno = "Forma de Pagamento";
                }
                if (pagina.Contains("FormaPagamento/Create"))
                {
                    retorno = "Incluir Forma de Pagamento";
                }
                if (pagina.Contains("FormaPagamento/Edit"))
                {
                    retorno = "Alterar Forma de Pagamento";
                }
                //Compras
                if (pagina.Contains("Compra/Index"))
                {
                    retorno = "Compras";
                }
                if (pagina.Contains("Compra/Create"))
                {
                    retorno = "Incluir Compra";
                }
                if (pagina.Contains("Compra/Edit"))
                {
                    retorno = "Alterar Compra";
                }
                if (pagina.Contains("Compra/Details"))
                {
                    retorno = "Detelhes Compra";
                }
                //Contas a pagar
                if (pagina.Contains("ContasPagar/Index"))
                {
                    retorno = "Contas a Pagar";
                    openNav = "aberto";
                }
                //Contas a receber
                if (pagina.Contains("ContasReceber/Index"))
                {
                    retorno = "Contas a Receber";
                }
                //Conta Corrente
                if (pagina.Contains("ContaCorrenteMov/Index"))
                {
                    retorno = "Conta Corrente Movimento";
                }
                //Venda
                if (pagina.Contains("Venda/Index"))
                {
                    retorno = "Vendas";
                }
                if (pagina.Contains("Venda/Create"))
                {
                    retorno = "Incluir Venda";
                }
                if (pagina.Contains("Venda/Edit"))
                {
                    retorno = "Alterar Venda";
                }
                if (pagina.Contains("Venda/Details"))
                {
                    retorno = "Detelhes Venda";
                }
                //Serviço Prestado
                if (pagina.Contains("ServicoPrestado/Index"))
                {
                    retorno = "Prestação de Serviços";
                }
                if (pagina.Contains("ServicoPrestado/Create"))
                {
                    retorno = "Incluir Serviço Prestado";
                }
                if (pagina.Contains("ServicoPrestado/Edit"))
                {
                    retorno = "Alterar Serviço Prestado";
                }
                if (pagina.Contains("ServicoPrestado/Details"))
                {
                    retorno = "Detelhes Serviço Prestado";
                }
                //Log
                if (pagina.Contains("Log/Index"))
                {
                    retorno = "LOG";
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
