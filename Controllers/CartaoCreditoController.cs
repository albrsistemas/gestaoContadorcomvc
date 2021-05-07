using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class CartaoCreditoController : Controller
    {
        [Autoriza(permissao = "cartaoCreditoList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            FormaPagamento fp = new FormaPagamento();
            Vm_cartao_credito vmcc = new Vm_cartao_credito();
            vmcc.cartoes = new List<FormaPagamento>();            
            vmcc.cartoes = fp.listFormasPagamentoPorMeioPgto(user.conta.conta_id, user.usuario_id, 03);

            Selects select = new Selects();
            ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(vmcc);
        }

        [Autoriza(permissao = "cartaoCreditoList")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Details(string contexto, FaturaCartaoCredito fcc)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            
            Vm_FaturaCartaoCredito vm_fcc = new Vm_FaturaCartaoCredito();

            vm_fcc = fcc.buscaFaturaCartao(user.conta.conta_id, user.usuario_id, contexto, fcc.fcc_id, fcc.fcc_data_corte, fcc.fcc_data_vencimento, fcc.fcc_forma_pagamento_id);
            vm_fcc.user = user;

            return Json(JsonConvert.SerializeObject(vm_fcc));
        }

        [Autoriza(permissao = "cartaoCreditoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult alocacaoCompetencia(FaturaCartaoCredito fcc, DateTime competencia)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);                

                retorno = fcc.alocacaoCompetencia(user.conta.conta_id, user.usuario_id, fcc, competencia);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro no processamento da solicitação.";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        //Ajustes na composição dos valores das parcelas da operação
        [Autoriza(permissao = "operacaoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjusteParcelasOperacao(string mcc_tipo, int mcc_tipo_id, IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            FaturaCartaoCredito f = new FaturaCartaoCredito();
            AjustaParcelasCartao apc = new AjustaParcelasCartao();
            apc = f.ajustaParcelasCartao_pesquisa(user.conta.conta_id, user.usuario_id, mcc_tipo, mcc_tipo_id);

            return Json(JsonConvert.SerializeObject(apc));
        }

        [Autoriza(permissao = "operacaoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjusteParcelasOperacaoGravar(AjustaParcelasCartao apc)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            FaturaCartaoCredito f = new FaturaCartaoCredito();
            List<string> retorno = new List<string>();
            try
            {
                retorno = f.ajustaParcelasCartao_gravar(apc, user.usuario_id, user.conta.conta_id);
            }
            catch
            {
                retorno.Add("Catch-->  Ocorreu um erro no precossamento da solicitação.");                
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }

        [Autoriza(permissao = "cartaoCreditoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult fechar_abrir_cartao(FaturaCartaoCredito fcc, string contexto)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            
            FaturaCartaoCredito f = new FaturaCartaoCredito();
            string retorno = "";
            try
            {
                retorno = f.fechar_abrir_cartao(user.conta.conta_id, user.usuario_id, contexto, fcc);
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro no processamento da solicitação de " + contexto + " a fatura do cartão de crédito!";
                }
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }

        [Autoriza(permissao = "cartaoCreditoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult PagamentoFatura(IFormCollection d, Decimal valorPgto, DateTime dataPgto, int conta_corrente_Pgto, FaturaCartaoCredito fcc)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            FaturaCartaoCredito f = new FaturaCartaoCredito();

            string retorno = "";

            try
            {
                retorno = f.PagamentoFatura(user.conta.conta_id, user.usuario_id, valorPgto, dataPgto, conta_corrente_Pgto, fcc);
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Catch-- > Ocorreu um erro no precossamento da solicitação.";
                }
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }

        [Autoriza(permissao = "cartaoCreditoDelete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeletePagamento(int mcc_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            FaturaCartaoCredito f = new FaturaCartaoCredito();

            string retorno = "";

            try
            {
                retorno = f.deletePagamento(user.conta.conta_id, user.usuario_id, mcc_id);
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Catch-- > Ocorreu um erro no precossamento da solicitação.";
                }
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }

        [Autoriza(permissao = "cartaoCreditoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult edit_datas_cartao(DateTime data_fechamento, DateTime data_vencimento, int fcc_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);


            FaturaCartaoCredito f = new FaturaCartaoCredito();
            string retorno = "";
            try
            {
                retorno = f.edit_datas_cartao(data_fechamento, data_vencimento, fcc_id, user.conta.conta_id);
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao tentar alterar a data!";
                }
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }
    }
}
