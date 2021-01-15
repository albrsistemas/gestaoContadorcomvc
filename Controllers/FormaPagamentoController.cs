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
using Newtonsoft.Json;

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

            vm_fp.formasPagamento = fp.listFormasPagamentoViewIndex(user.usuario_conta_id, user.usuario_id);
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
            ViewBag.ccorrente = select.getContasCorrenteTipo(user.usuario_conta_id, "Caixa").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.IntegracaoCartao = select.getIntegracaoCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.bandeiraCartao = select.getBandeirasCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View();
        }

        // POST: FormaPagamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d, bool fp_baixa_automatica)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            FormaPagamento fp = new FormaPagamento();
            Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();

            Selects select = new Selects();
            ViewBag.meioPgto = select.getMeioPagamento().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_meio_pgto_nfe"] });
            ViewBag.Identificacao = select.getTipoFormaPgto().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_identificacao"] });
            //ViewBag.ccorrente = select.getContasCorrente(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_vinc_conta_corrente"] });
            ViewBag.IntegracaoCartao = select.getIntegracaoCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_tipo_integracao_nfe"] });
            ViewBag.bandeiraCartao = select.getBandeirasCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["fp_bandeira_cartao"] });

            vm_fp.fp_nome = d["fp_nome"];
            vm_fp.fp_status = d["fp_status"];
            vm_fp.fp_identificacao = d["fp_identificacao"];
            vm_fp.fp_meio_pgto_nfe = d["fp_meio_pgto_nfe"];
            vm_fp.fp_baixa_automatica = fp_baixa_automatica;
            vm_fp.fp_vinc_conta_corrente = d["fp_vinc_conta_corrente"];


            if ((vm_fp.fp_meio_pgto_nfe == "03" || vm_fp.fp_meio_pgto_nfe == "04") && vm_fp.fp_identificacao == "Recebimento")
            {
                vm_fp.fp_tipo_integracao_nfe = d["fp_tipo_integracao_nfe"];
                vm_fp.fp_bandeira_cartao = d["fp_bandeira_cartao"];
                vm_fp.fp_cnpj_credenciadora_cartao = d["fp_cnpj_credenciadora_cartao"];
            }
            else
            {
                vm_fp.fp_tipo_integracao_nfe = "0";
                vm_fp.fp_bandeira_cartao = "0";
                vm_fp.fp_cnpj_credenciadora_cartao = "Não se aplica";
            }

            if (vm_fp.fp_identificacao == "Pagamento" && vm_fp.fp_meio_pgto_nfe == "03")
            {
                vm_fp.fp_dia_fechamento_cartao = Convert.ToInt32(d["fp_dia_fechamento_cartao"]);
                vm_fp.fp_dia_vencimento_cartao = Convert.ToInt32(d["fp_dia_vencimento_cartao"]);
            }
            else
            {
                vm_fp.fp_dia_fechamento_cartao = 0;
                vm_fp.fp_dia_vencimento_cartao = 0;
            }

            

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
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            FormaPagamento fp = new FormaPagamento();
            Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();

            vm_fp = fp.buscaFormasPagamento(user.usuario_conta_id, user.usuario_id, id);

            Selects select = new Selects();
            ViewBag.meioPgto = select.getMeioPagamento().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_fp.fp_meio_pgto_nfe });
            ViewBag.Identificacao = select.getTipoFormaPgto().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_fp.fp_identificacao });
            if (vm_fp.ccorrente_tipo.Equals("0"))
            {
                Selects select_2 = new Selects();
                List<Selects> lista = new List<Selects>();
                select_2.value = "0";
                select_2.text = "Não se aplica";
                lista.Add(select_2);

                ViewBag.ccorrente = lista.Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_fp.fp_vinc_conta_corrente });
            }
            else
            {
                ViewBag.ccorrente = select.getContasCorrenteTipo(user.usuario_conta_id, vm_fp.ccorrente_tipo).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_fp.fp_vinc_conta_corrente });
            }
            ViewBag.IntegracaoCartao = select.getIntegracaoCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_fp.fp_tipo_integracao_nfe });
            ViewBag.bandeiraCartao = select.getBandeirasCartao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_fp.fp_bandeira_cartao });
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_fp.fp_status });

            vm_fp.user = user;

            return View(vm_fp);
        }

        // POST: FormaPagamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection d, bool fp_baixa_automatica)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            FormaPagamento fp = new FormaPagamento();
            Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();

            vm_fp.fp_nome = d["fp_nome"];
            vm_fp.fp_status = d["fp_status"];
            vm_fp.fp_identificacao = d["fp_identificacao"];
            vm_fp.fp_meio_pgto_nfe = d["fp_meio_pgto_nfe"];
            vm_fp.fp_baixa_automatica = fp_baixa_automatica;
            vm_fp.fp_vinc_conta_corrente = d["fp_vinc_conta_corrente"];


            if ((vm_fp.fp_meio_pgto_nfe == "03" || vm_fp.fp_meio_pgto_nfe == "04") && vm_fp.fp_identificacao == "Recebimento")
            {
                vm_fp.fp_tipo_integracao_nfe = d["fp_tipo_integracao_nfe"];
                vm_fp.fp_bandeira_cartao = d["fp_bandeira_cartao"];
                vm_fp.fp_cnpj_credenciadora_cartao = d["fp_cnpj_credenciadora_cartao"];
            }
            else
            {
                vm_fp.fp_tipo_integracao_nfe = "0";
                vm_fp.fp_bandeira_cartao = "0";
                vm_fp.fp_cnpj_credenciadora_cartao = "Não se aplica";
            }

            if (vm_fp.fp_identificacao == "Pagamento" && vm_fp.fp_meio_pgto_nfe == "03")
            {
                vm_fp.fp_dia_fechamento_cartao = Convert.ToInt32(d["fp_dia_fechamento_cartao"]);
                vm_fp.fp_dia_vencimento_cartao = Convert.ToInt32(d["fp_dia_vencimento_cartao"]);
            }
            else
            {
                vm_fp.fp_dia_fechamento_cartao = 0;
                vm_fp.fp_dia_vencimento_cartao = 0;
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["msgFp"] = "Formulário com informação incorreta (Erro)!";

                    return View(vm_fp);
                }

                TempData["msgFp"] = fp.alteraFormaPagamento(user.usuario_conta_id,user.usuario_id,vm_fp.fp_nome,vm_fp.fp_meio_pgto_nfe,fp_baixa_automatica,vm_fp.fp_vinc_conta_corrente,vm_fp.fp_identificacao,vm_fp.fp_tipo_integracao_nfe,vm_fp.fp_bandeira_cartao,vm_fp.fp_cnpj_credenciadora_cartao,vm_fp.fp_dia_fechamento_cartao,vm_fp.fp_dia_vencimento_cartao,vm_fp.fp_status,id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgFp"] = "Erro na gravação dos dados da forma de pagamento!";

                return RedirectToAction("Edit", new { id = id });
            }
        }

        [Autoriza(permissao = "fpDelete")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            FormaPagamento fp = new FormaPagamento();
            Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();

            vm_fp = fp.buscaFormasPagamento(user.usuario_conta_id, user.usuario_id, id);
            vm_fp.user = user;

            return View(vm_fp);
        }

        // POST: FormaPagamentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int fp_id, IFormCollection collection)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                FormaPagamento fp = new FormaPagamento();
                Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();

                TempData["msgFp"] = fp.deletaFormaPagamento(user.usuario_conta_id, user.usuario_id, fp_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgFp"] = "Falha na exclusão da forma de pagamento - Erro!";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult gerarSelectMeioPgto(string identificacao, string meioPgto)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            List<Selects> lista = new List<Selects>();

            if((identificacao.Equals("Pagamento") || identificacao.Equals("Recebimento")) && meioPgto.Equals("01"))
            {
                lista = select.getContasCorrenteTipo(user.usuario_conta_id, "Caixa");
            }

            if (identificacao.Equals("Pagamento") && (meioPgto.Equals("02") || meioPgto.Equals("04") || meioPgto.Equals("99")))
            {
                lista = select.getContasCorrenteTipo(user.usuario_conta_id, "Banco");
            }

            if (identificacao.Equals("Recebimento") && (meioPgto.Equals("02") || meioPgto.Equals("05")))
            {
                lista.Clear();
                select.value = "0";
                select.text = "Não se aplica";
                lista.Add(select);
            }

            if (identificacao.Equals("Pagamento") && (meioPgto.Equals("03") || meioPgto.Equals("05") || meioPgto.Equals("15")))
            {
                lista.Clear();
                select.value = "0";
                select.text = "Não se aplica";
                lista.Add(select);
            }

            if (identificacao.Equals("Recebimento") && (meioPgto.Equals("03") || meioPgto.Equals("04")))
            {
                lista = select.getContasCorrenteTipo(user.usuario_conta_id, "Maquininha de Cartão");
            }

            if (identificacao.Equals("Recebimento") && (meioPgto.Equals("15") || meioPgto.Equals("99")))
            {
                lista = select.getContasCorrenteTipo(user.usuario_conta_id, "Banco");
            }

            return Json(JsonConvert.SerializeObject(lista));
        }
    }
}
