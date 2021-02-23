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
            
            return View(vmcc);
        }

        [Autoriza(permissao = "cartaoCreditoDetails")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Details(string contexto, FaturaCartaoCredito fcc)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            
            Vm_FaturaCartaoCredito vm_fcc = new Vm_FaturaCartaoCredito();

            vm_fcc = fcc.buscaFaturaCartao(user.conta.conta_id, user.usuario_id, contexto, fcc.fcc_id, fcc.fcc_data_corte, fcc.fcc_data_vencimento, fcc.fcc_forma_pagamento_id);

            return Json(JsonConvert.SerializeObject(vm_fcc));
        }

        // GET: CartaoCreditoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartaoCreditoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartaoCreditoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartaoCreditoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartaoCreditoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartaoCreditoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

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

    }
}
