using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]    
    [Authorize(Roles = "Contabilidade")]
    public class ContaContabilController : Controller
    {
        [Autoriza(permissao = "planoContasList")]
        public ActionResult Index(int plano_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));            

            PlanoContas plano = new PlanoContas();

            plano = plano.buscaPlanoContas(plano_id, user.usuario_conta_id, user.usuario_id);

            if(plano.plano_conta_id != user.usuario_conta_id)
            {
                return RedirectToAction("AcessoNegadoContador", "Home");
            }

            ContaContabil conta = new ContaContabil();            
            vm_ContaContabil contaContabil = new vm_ContaContabil();

            contaContabil.contasContabeis = conta.listaContasContabeisPorPlano(user.usuario_conta_id, plano_id, user.usuario_id);
            contaContabil.user = user;
            contaContabil.plano = plano;

            return View(contaContabil);
        }

        [Autoriza(permissao = "planoContasCreate")]
        public ActionResult Create(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            PlanoContas plano = new PlanoContas();

            plano = plano.buscaPlanoContas(id, user.usuario_conta_id, user.usuario_id);

            TempData["plano"] = plano;

            Selects select = new Selects();
            ViewBag.ccontabil_natureza = select.getNaturezaContabil().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });

            return View();
        }

        // POST: ContaContabilController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["retornoContasContabeis"] = "Algumas informações no formulário pode conter erro!";

                    return RedirectToAction("Index", new { plano_id = Convert.ToInt32(collection["plano_id"]) });
                }

                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                ContaContabil ccontabil = new ContaContabil();

                TempData["retornoContasContabeis"] = ccontabil.cadastrarContaContabil(user.usuario_conta_id, user.usuario_id, collection["plano_id"], collection["ccontabil_classificacao"], collection["ccontabil_nivel"], collection["ccontabil_nome"], collection["ccontabil_apelido"], collection["ccontabil_natureza"]);

                return RedirectToAction("Index", new { plano_id = Convert.ToInt32(collection["plano_id"]) });
            }
            catch
            {
                return RedirectToAction("Index", new { controller = "ContaContabil", action = "Index" });
            }
        }

        [Autoriza(permissao = "planoContasEdit")]
        public ActionResult Edit(int ccontabil_id, int plano_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            PlanoContas plano = new PlanoContas();

            plano = plano.buscaPlanoContas(plano_id, user.usuario_conta_id, user.usuario_id);

            TempData["plano"] = plano;

            ContaContabil contaContabil = new ContaContabil();
            vm_ContaContabil conta = new vm_ContaContabil();
            conta = contaContabil.buscaContaContabeisPorID(user.usuario_conta_id, user.usuario_id, ccontabil_id);

            Selects select = new Selects();
            ViewBag.ccontabil_natureza = select.getNaturezaContabil().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == conta.ccontabil_natureza.ToString() });

            return View(conta);
        }

        // POST: ContaContabilController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["retornoContasContabeis"] = "Algumas informações no formulário pode conter erro!";

                    return RedirectToAction("Index", new { plano_id = Convert.ToInt32(collection["plano_id"]) });
                }

                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                ContaContabil contaContabil = new ContaContabil();

                TempData["retornoContasContabeis"] = contaContabil.editarContaContabil(user.usuario_conta_id, user.usuario_id, collection["plano_id"], collection["ccontabil_id"], collection["ccontabil_nome"], collection["ccontabil_apelido"], collection["ccontabil_natureza"]);

                return RedirectToAction("Index", new { plano_id = Convert.ToInt32(collection["plano_id"]) });
            }
            catch
            {
                return RedirectToAction("Index", new { controller = "ContaContabil", action = "Index" });
            }
        }

        [Autoriza(permissao = "planoContasDelete")]
        public ActionResult Delete(int ccontabil_id, int plano_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            PlanoContas plano = new PlanoContas();

            plano = plano.buscaPlanoContas(plano_id, user.usuario_conta_id, user.usuario_id);

            TempData["plano"] = plano;

            ContaContabil contaContabil = new ContaContabil();
            vm_ContaContabil conta = new vm_ContaContabil();
            conta = contaContabil.buscaContaContabeisPorID(user.usuario_conta_id, user.usuario_id, ccontabil_id);

            return View(conta);
        }

        // POST: ContaContabilController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                ContaContabil contaContabil = new ContaContabil();

                TempData["retornoContasContabeis"] = contaContabil.deleteContaContabil(user.usuario_conta_id, user.usuario_id, collection["plano_id"], collection["ccontabil_id"], collection["ccontabil_nome"]);

                return RedirectToAction("Index", new { plano_id = Convert.ToInt32(collection["plano_id"]) });
            }
            catch
            {
                return RedirectToAction("Index", new { controller = "ContaContabil", action = "Index" });
            }
        }
        
        public ActionResult consultaContasContabeis(string termo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            Config_contador_cliente config = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_config = new vm_ConfigContadorCliente();
            vm_config = config.buscaCCC(user.usuario_id, contexto.conta_id, user.usuario_conta_id);

            if(vm_config.ccc_planoContasVigente != "0")
            {
                ContaContabil contaContabil = new ContaContabil();
                vm_ContaContabil conta = new vm_ContaContabil();                
                conta.contasContabeis = contaContabil.listaContas(Convert.ToInt32(vm_config.ccc_planoContasVigente),termo);

                return Json(JsonConvert.SerializeObject(conta.contasContabeis));
            }
            else
            {
                return Json("Cliente não possui plano de contas configurado!");
            }
        }
    }
}
