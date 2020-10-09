using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC.Multiplier;
using System;

namespace gestaoContadorcomvc.Controllers.GE
{
    [FiltroAutorizacao]
    public class ConfiguracoesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["bread"] = "Configurações";

            return View();
        }

        // GET: Categoria_v2Controller/Create
        //[FiltroAutorizacao(permissao = "ADM")]
        [Authorize(Roles = "adm")]
        public ActionResult Contabilidade()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);

            ContaContabilidade contaContabilidade = new ContaContabilidade();
            Vm_contabilidade contabilidade = new Vm_contabilidade();

            if(conta.conta_contador > 0)
            {
                contabilidade = contaContabilidade.buscarContabilidade(conta.conta_contador);
            }
            else
            {
                contabilidade = null;
            }

            return View(contabilidade);
        }

        // GET: Categoria_v2Controller/Create   
        [FiltroAutorizacao(permissao = "ADM")]
        public ActionResult AddContabilidade(string dctoContador)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Conta empresa = new Conta();
            empresa = empresa.buscarConta(user.usuario_conta_id);

            Conta contador = new Conta();
            contador = contador.buscarContaPorDcto(dctoContador);

            Vm_contabilidade contabilidade = new Vm_contabilidade();

            if (contador.conta_id == 0)
            {   
                contabilidade.cc_termo = "Erro";

                return View(contabilidade);
            }
            
            contabilidade.cc_conta_id_contador = contador.conta_id;
            contabilidade.cc_conta_id = empresa.conta_id;
            contabilidade.cc_dctoContador = contador.conta_dcto;
            contabilidade.cc_nomeContador = contador.conta_nome;

            //Produção do termo
            string termo = "Prezado cliente, |" +
                           "Ao aceitar o compartilhamento de informações contábeis o contador designado por você no campo" +
                            "'Meu Contador', terá acesso a todas as suas informações com execessão a manipulação de usuários, senhas e atribuição de contabilidade.|" +
                            "A contabilidade poderá visualizar suas categorias, caixa e bancos, relatórios, participantes, produtos, compras, vendas, serviços, " +
                            "controle de cartão de crédito, contas a pagar e receber.|" +
                            "Além de visuazar, a contabilidade poderá em alguns casos fazer alterações pertinentes a contabilização das informações tais como " +
                            "alteração da categoria informada nos cadastros e lançamentos e " +
                            "atribuir configurações extras para melhor desempenho da escrituração contábel, tais como " +
                            "atribuição da estrutura DRE(demonstrativo de resultado do exercício) nas categorias e atribuição de conta contábil nas categorias.|" +
                            "Além disso a contabiliade poderá restringir algumas funções do sistema para melhor controle das configurações contábeis, tais como, " +
                            "cadastro de nova categoria, novo caixa/banco e novo cartão de crédito.|" +
                            "Fique tranquilo, ao desvincular o contador a contabilidade perderá acesso as todas as suas informações.|" +
                            "Não nos reponsabilizamos pelo uso da informação fora do domínio do sistema.|" + 
                            "Eu " + user.usuario_nome.ToUpper() + ", usuário administrador(a) da conta: " + empresa.conta_nome.ToUpper() + ", registrada no sitema no código: " + empresa.conta_id + 
                            ", CNPJ/CPF número: " + empresa.conta_dcto +
                            ", ciente das informações, autorizo o compartilhamento de informações da minha empresa com a contabilidade de CNPJ número: " + contador.conta_dcto + ".";
            
            contabilidade.cc_termo = termo;            
            
            return View(contabilidade);
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContabilidade(IFormCollection dados)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                ContaContabilidade contaContabilidade = new ContaContabilidade();
                
                TempData["vincularContador"] = contaContabilidade.vincularContabilidade(user.usuario_id, Convert.ToInt32(dados["cc_conta_id"]), Convert.ToInt32(dados["cc_conta_id_contador"]), dados["cc_dctoContador"], dados["cc_nomeContador"], dados["cc_termo"]);

                return RedirectToAction(nameof(Contabilidade));
            }
            catch
            {
                return View();
            }
        }

        // GET: Categoria_v2Controller/Create    
        [FiltroAutorizacao(permissao = "ADM")]
        public ActionResult DeleteContabilidade()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Conta empresa = new Conta();
            empresa = empresa.buscarConta(user.usuario_conta_id);

            Vm_contabilidade contabilidade = new Vm_contabilidade();
            ContaContabilidade contaContabilidade = new ContaContabilidade();
            contabilidade = contaContabilidade.buscarContabilidade(empresa.conta_contador);                        

            return View(contabilidade);
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteContabilidade(IFormCollection dados)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                ContaContabilidade contaContabilidade = new ContaContabilidade();

                TempData["desvincularContador"] = contaContabilidade.desvincularContabilidade(user.usuario_conta_id, user.usuario_id, Convert.ToInt32(dados["cc_id"]),dados["cc_nomeContador"]);

                return RedirectToAction(nameof(Contabilidade));
            }
            catch
            {
                return View();
            }
        }

    }
}