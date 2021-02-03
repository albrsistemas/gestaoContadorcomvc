using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class ContasReceberController : Controller
    {
        [Autoriza(permissao = "ContasRList")]
        public IActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_contas_receber vm_cr = new Vm_contas_receber();
            ContasReceber cr = new ContasReceber();

            vm_cr = cr.listaContasReceber(user.usuario_id, user.usuario_conta_id, 0, 0, 0, 1, 0, "", "");
            vm_cr.user = user;
            cp_filterCR filter = new cp_filterCR();
            filter.contexto = false;
            vm_cr.filter = filter;

            Selects select = new Selects();
            ViewBag.formaPgto = select.getFormaPgtoContas(user.usuario_conta_id, "Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            ViewBag.formaPgto_cartao = select.getFormaPgto_boletoBancario(user.usuario_conta_id, "Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoOpercao = select.getTipoOperacao("Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            ViewBag.situacao = select.getSituacaoContas().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.vencimento = select.getVencimentoContas().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });

            return View(vm_cr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(cp_filterCR filter)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_contas_receber vm_cr = new Vm_contas_receber();
            ContasReceber cr = new ContasReceber();

            //verificando as datas se estão nulas
            if (filter.dataInicial != null && filter.dataFinal != null)
            {
                filter.dataInicial = filter.dataInicial.Substring(6, 4) + "/" + filter.dataInicial.Substring(3, 2) + "/" + filter.dataInicial.Substring(0, 2);
                filter.dataFinal = filter.dataFinal.Substring(6, 4) + "/" + filter.dataFinal.Substring(3, 2) + "/" + filter.dataFinal.Substring(0, 2);
            }

            if (filter.dataInicial == null || filter.dataFinal == null)
            {
                filter.dataInicial = "";
                filter.dataFinal = "";
            }


            vm_cr = cr.listaContasReceber(user.usuario_id, user.usuario_conta_id, filter.fornecedor_id, filter.formaPgto, filter.vencimento, filter.situacao, filter.operacao, filter.dataInicial, filter.dataFinal);
            vm_cr.user = user;
            filter.contexto = true;
            vm_cr.filter = filter;

            Selects select = new Selects();
            ViewBag.formaPgto = select.getFormaPgtoContas(user.usuario_conta_id, "Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == filter.formaPgto.ToString() });
            ViewBag.formaPgto_cartao = select.getFormaPgto_boletoBancario(user.usuario_conta_id, "Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoOpercao = select.getTipoOperacao("Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == filter.operacao.ToString() });
            ViewBag.situacao = select.getSituacaoContas().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == filter.situacao.ToString() });
            ViewBag.vencimento = select.getVencimentoContas().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == filter.vencimento.ToString() });

            return View(vm_cr);
        }

    }
}
