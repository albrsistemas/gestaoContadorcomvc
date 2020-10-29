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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class FormaPagamentoController : Controller
    {
        [Autoriza(permissao = "fpList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            FormaPagamento fp = new FormaPagamento();
            Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();

            vm_fp.formasPagamento = fp.listFormasPagamento(user.usuario_conta_id, user.usuario_id);
            vm_fp.user = user;

            return View(vm_fp);
        }


        [Autoriza(permissao = "fpCreate")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            ViewBag.meioPgto = select.getMeioPagamento().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "01" });
            ViewBag.Identificacao = select.getTipoFormaPgto().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Pagamento" });
            ViewBag.ccorrente = select.getContasCorrente(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.IntegracaoCartao = select.getIntegracaoCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.bandeiraCartao = select.getBandeirasCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View();
        }

        // POST: FormaPagamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            FormaPagamento fp = new FormaPagamento();
            Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();

            Selects select = new Selects();
            ViewBag.meioPgto = select.getMeioPagamento().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_meio_pgto_nfe"] });
            ViewBag.Identificacao = select.getTipoFormaPgto().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_identificacao"] });
            ViewBag.ccorrente = select.getContasCorrente(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_vinc_conta_corrente"] });
            ViewBag.IntegracaoCartao = select.getIntegracaoCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_tipo_integracao_nfe"] });
            ViewBag.bandeiraCartao = select.getBandeirasCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_bandeira_cartao"] });

            vm_fp.fp_nome = d["fp_nome"];
            vm_fp.fp_status = d["fp_status"];
            vm_fp.fp_identificacao = d["fp_identificacao"];
            vm_fp.fp_meio_pgto_nfe = d["fp_meio_pgto_nfe"];
            vm_fp.fp_baixa_automatica = Convert.ToBoolean(d["fp_baixa_automatica"]);
            vm_fp.fp_vinc_conta_corrente = d["fp_vinc_conta_corrente"];
            vm_fp.fp_tipo_integracao_nfe = d["fp_tipo_integracao_nfe"];
            vm_fp.fp_bandeira_cartao = d["fp_bandeira_cartao"];
            vm_fp.fp_cnpj_credenciadora_cartao = d["fp_cnpj_credenciadora_cartao"];
            vm_fp.fp_dia_fechamento_cartao = Convert.ToInt32(d["fp_dia_fechamento_cartao"]);
            vm_fp.fp_dia_vencimento_cartao = Convert.ToInt32(d["fp_dia_vencimento_cartao"]);

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["msgFp"] = "Formulário com informação incorreta (Erro)!";

                    return View(vm_fp);
                }

                TempData["msgFp"] = fp.cadastraFormaPagamento(user.usuario_conta_id, user.usuario_id, vm_fp.fp_nome, vm_fp.fp_meio_pgto_nfe, vm_fp.fp_baixa_automatica, vm_fp.fp_vinc_conta_corrente, vm_fp.fp_identificacao, vm_fp.fp_tipo_integracao_nfe, vm_fp.fp_bandeira_cartao, vm_fp.fp_cnpj_credenciadora_cartao, vm_fp.fp_dia_fechamento_cartao, vm_fp.fp_dia_vencimento_cartao);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgFp"] = "Erro na gravação dos dados da forma de pagamento!";

                return View(vm_fp);
            }
        }

        [Autoriza(permissao = "fpEdit")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FormaPagamentoController/Edit/5
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

        [Autoriza(permissao = "fpDelete")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FormaPagamentoController/Delete/5
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
    }
}
