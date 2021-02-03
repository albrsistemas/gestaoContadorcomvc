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

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class ContaCorrenteController : Controller
    {
        [Autoriza(permissao = "ccorrenteList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            ContaCorrente ccorrente = new ContaCorrente();
            Vm_conta_corrente vm_corrente = new Vm_conta_corrente();

            vm_corrente.contas_corrente = ccorrente.listContaCorrete(user.usuario_conta_id, user.usuario_id);
            vm_corrente.user = user;

            return View(vm_corrente);
        }

        [Autoriza(permissao = "ccorrenteCreate")]
        public ActionResult Create()
        {
            Selects select = new Selects();
            ViewBag.tipoContaCorrente = select.getTiposContaCorrente().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });

            Vm_conta_corrente vm_ccorrente = new Vm_conta_corrente();
            vm_ccorrente.ccorrente_saldo_abertura = 0;

            return View(vm_ccorrente);
        }

        // POST: ContaCorrenteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d)
        {
            ContaCorrente ccorrente = new ContaCorrente();
            Vm_conta_corrente vm_corrente = new Vm_conta_corrente();

            Selects select = new Selects();
            ViewBag.tipoContaCorrente = select.getTiposContaCorrente().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);


                vm_corrente.ccorrente_nome = d["ccorrente_nome"];
                vm_corrente.ccorrente_tipo = d["ccorrente_tipo"];
                vm_corrente.ccorrente_saldo_abertura = Convert.ToDecimal(d["ccorrente_saldo_abertura"]);
                vm_corrente.ccorrente_masc_contabil = d["ccorrente_masc_contabil"];

                if (!ModelState.IsValid)
                {
                    TempData["msgCCorrente"] = "Formulário com informação incorreta (Erro)!";

                    return View(vm_corrente);
                }

                TempData["msgCCorrente"] = ccorrente.cadastraContaCorrente(user.usuario_conta_id, user.usuario_id, vm_corrente.ccorrente_nome, vm_corrente.ccorrente_tipo, vm_corrente.ccorrente_masc_contabil, vm_corrente.ccorrente_saldo_abertura);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgCCorrente"] = "Erro na gravação dos dados da conta corrente!";

                return View(vm_corrente);
            }
        }

        [Autoriza(permissao = "ccorrenteEdit")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            ContaCorrente ccorrente = new ContaCorrente();
            Vm_conta_corrente vm_corrente = new Vm_conta_corrente();

            vm_corrente = ccorrente.buscaContaCorrete(user.usuario_conta_id, user.usuario_id, id);

            Selects select = new Selects();
            ViewBag.tipoContaCorrente = select.getTiposContaCorrente().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_corrente.ccorrente_tipo });
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_corrente.ccorrente_status });


            return View(vm_corrente);
        }

        // POST: ContaCorrenteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ccorrente_id, IFormCollection d)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            ContaCorrente ccorrente = new ContaCorrente();
            Vm_conta_corrente vm_corrente = new Vm_conta_corrente();

            Selects select = new Selects();
            ViewBag.tipoContaCorrente = select.getTiposContaCorrente().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == d["ccorrente_status"] });
                        
            vm_corrente.ccorrente_nome = d["ccorrente_nome"];
            vm_corrente.ccorrente_tipo = d["ccorrente_tipo"];
            vm_corrente.ccorrente_saldo_abertura = Convert.ToDecimal(d["ccorrente_saldo_abertura"]);
            vm_corrente.ccorrente_masc_contabil = d["ccorrente_masc_contabil"];
            vm_corrente.ccorrente_status = d["ccorrente_status"];
            vm_corrente.ccorrente_id = ccorrente_id;


            if (!ModelState.IsValid)
            {
                TempData["msgCCorrente"] = "Formulário com informação incorreta (Erro)!";

                return View(vm_corrente);
            }

            TempData["msgCCorrente"] = ccorrente.alteraContaCorrente(user.usuario_conta_id,user.usuario_id,vm_corrente.ccorrente_nome,vm_corrente.ccorrente_tipo,vm_corrente.ccorrente_masc_contabil,vm_corrente.ccorrente_saldo_abertura,vm_corrente.ccorrente_status,ccorrente_id); ;

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgCCorrente"] = "Erro na gravação dos dados da conta corrente!";

                return View(vm_corrente);
            }
        }

        [Autoriza(permissao = "ccorrenteDelete")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            ContaCorrente ccorrente = new ContaCorrente();
            Vm_conta_corrente vm_corrente = new Vm_conta_corrente();

            vm_corrente = ccorrente.buscaContaCorrete(user.usuario_conta_id, user.usuario_id, id);        


            return View(vm_corrente);
        }

        // POST: ContaCorrenteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int ccorrente_id, IFormCollection collection)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                ContaCorrente ccorrente = new ContaCorrente();
                Vm_conta_corrente vm_corrente = new Vm_conta_corrente();

                TempData["msgCCorrente"] = ccorrente.deleteContaCorrente(user.usuario_conta_id, user.usuario_id, ccorrente_id);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgProdutos"] = "Erro ao tentar apagar a conta corrente!";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
