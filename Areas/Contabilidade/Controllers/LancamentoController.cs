using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [Authorize(Roles = "Contabilidade")]
    public class LancamentoController : Controller
    {
        // GET: LancamentoController
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            Lancamento lancamento = new Lancamento();
            vm_lancamento vm_lanc = new vm_lancamento();
            vm_lanc.lancamentos = lancamento.listarLancamentosLimit(user.usuario_id, contexto.conta_id, user.usuario_conta_id, 25);
            vm_lanc.user = user;

            return View(vm_lanc);
        }        

        // GET: LancamentoController/Create
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            Config_contador_cliente config = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_config = new vm_ConfigContadorCliente();
            vm_config = config.buscaCCC(user.usuario_id, contexto.conta_id, user.usuario_conta_id);

            Lancamento lancamento = new Lancamento();
            vm_lancamento vm_lanc = new vm_lancamento();
            vm_lanc.lancamentos = lancamento.listarLancamentosLimit(user.usuario_id, contexto.conta_id, user.usuario_conta_id, 3);
            vm_lanc.user = user;
            vm_lanc.vm_config = vm_config;

            if(config.ccc_planoContasVigente != "0")
            {
                Selects select = new Selects();
                ViewBag.planoContas = select.getPlanoContas(Convert.ToInt32(vm_config.ccc_planoContasVigente)).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            }            

            return View(vm_lanc);
        }

        // POST: LancamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {            
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["novoLancamento"] = "Erro no preenchimento das informações do formulário!";

                    return RedirectToAction(nameof(Create));
                }

                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
                Conta contexto = new Conta();
                contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

                Lancamento lancamento = new Lancamento();
                TempData["novoLancamento"] = lancamento.cadastrarLancamento(user.usuario_id, contexto.conta_id, user.usuario_conta_id, "Lançamento Contábil", Convert.ToDateTime(collection["lancamento_data"]), collection["lancamento_valor"], collection["lancamento_debito_conta_id"], collection["lancamento_credito_conta_id"], collection["lancamento_historico"]);

                Config_contador_cliente config = new Config_contador_cliente();
                vm_ConfigContadorCliente vm_config = new vm_ConfigContadorCliente();
                vm_config = config.buscaCCC(user.usuario_id, contexto.conta_id, user.usuario_conta_id);

                vm_lancamento vm_lanc = new vm_lancamento();
                vm_lanc.lancamentos = lancamento.listarLancamentosLimit(user.usuario_id, contexto.conta_id, user.usuario_conta_id, 3);
                vm_lanc.user = user;
                vm_lanc.vm_config = vm_config;

                if (config.ccc_planoContasVigente != "0")
                {
                    Selects select = new Selects();
                    ViewBag.planoContas = select.getPlanoContas(Convert.ToInt32(vm_config.ccc_planoContasVigente)).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
                }

                return View(vm_lanc);
            }
            catch
            {
                TempData["novoLancamento"] = "Erro ao registrar o lançamento contábil. Tente novamente. Se persistir entre em contato com o suporte!";

                return RedirectToAction(nameof(Create));
            }
        }

        // GET: LancamentoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LancamentoController/Edit/5
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

        // GET: LancamentoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LancamentoController/Delete/5
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
