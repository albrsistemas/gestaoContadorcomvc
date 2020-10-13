using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]    
    [Authorize(Roles = "Contabilidade")]
    public class PlanoContasController : Controller
    {
        [Autoriza(permissao = "planoContasList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
            Conta conta = new Conta();
            conta = conta.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            TempData["user"] = user;

            PlanoContas plano = new PlanoContas();
            plano.planosConta = plano.listaPlanoContas(user.usuario_conta_id, user.usuario_id);
            plano.user = user;

            return View(plano);
        }

        [Autoriza(permissao = "planoContasCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanoContasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                PlanoContas plano = new PlanoContas();

                TempData["retornoPlanoContas"] = plano.cadastrarPlanoContas(user.usuario_conta_id, collection["plano_nome"], user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index", new { controller = "ContaContabil", action = "Index" });
            }
        }

        [Autoriza(permissao = "planoContasEdit")]
        public ActionResult Edit(int id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            PlanoContas plano = new PlanoContas();

            return View(plano.buscaPlanoContas(id,user.usuario_conta_id,user.usuario_id));
        }

        // POST: PlanoContasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int plano_id, IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                PlanoContas plano = new PlanoContas();

                TempData["retornoPlanoContas"] = plano.alterarPlanoContas(plano_id, collection["plano_nome"], user.usuario_conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index", new { controller = "ContaContabil", action = "Index" });
            }
        }

        [Autoriza(permissao = "planoContasDelete")]
        public ActionResult Delete(int id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            PlanoContas plano = new PlanoContas();

            return View(plano.buscaPlanoContas(id, user.usuario_conta_id, user.usuario_id));
        }

        // POST: PlanoContasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int plano_id, IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            try
            {
                PlanoContas plano = new PlanoContas();

                TempData["retornoPlanoContas"] = plano.deletarPlanoContas(plano_id, collection["plano_nome"], user.usuario_conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index", new { controller = "ContaContabil", action = "Index" });
            }
        }
    }
}
