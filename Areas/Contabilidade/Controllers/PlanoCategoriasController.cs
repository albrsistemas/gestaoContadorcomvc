using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    //[FiltroAutenticacao]
    //[FiltroContabilidade]
    [Authorize(Roles = "Contabilidade")]
    public class PlanoCategoriasController : Controller
    {
        [FiltroAutorizacao(permissao = "planoCategoriasList")]
        public ActionResult Index()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            TempData["user"] = user;

            PlanoCategorias plano = new PlanoCategorias();

            return View(plano.listaPlanoCategorias(user.usuario_conta_id, user.usuario_id));
        }

        [FiltroAutorizacao(permissao = "planoCategoriasCreate")]
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
                PlanoCategorias plano = new PlanoCategorias();

                TempData["retornoPlanoCategorias"] = plano.cadastrarPlanoCategorias(user.usuario_conta_id, collection["pc_nome"], user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["retornoPlanoCategorias"] = "Erro ao executar a rotina. Tente novamente. Se persistir, entre em contato com o suporte";

                return RedirectToAction(nameof(Index));
            }
        }

        [FiltroAutorizacao(permissao = "planoCategoriasEdit")]
        public ActionResult Edit(int id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            PlanoCategorias plano = new PlanoCategorias();

            return View(plano.buscaPlanoCategorias(id, user.usuario_conta_id, user.usuario_id));
        }

        // POST: PlanoContasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int pc_id, IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                PlanoCategorias plano = new PlanoCategorias();

                TempData["retornoPlanoCategorias"] = plano.alterarPlanoCategorias(pc_id, collection["pc_nome"], user.usuario_conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["retornoPlanoCategorias"] = "Erro ao executar a rotina. Tente novamente. Se persistir, entre em contato com o suporte";

                return RedirectToAction(nameof(Index));
            }
        }

        [FiltroAutorizacao(permissao = "planoCategoriasDelete")]
        public ActionResult Delete(int id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            PlanoCategorias plano = new PlanoCategorias();

            return View(plano.buscaPlanoCategorias(id, user.usuario_conta_id, user.usuario_id));
        }

        // POST: PlanoContasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int pc_id, IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            try
            {
                PlanoCategorias plano = new PlanoCategorias();

                TempData["retornoPlanoCategorias"] = plano.deletarPlanoCategorias(pc_id, collection["pc_nome"], user.usuario_conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["retornoPlanoCategorias"] = "Erro ao executar a rotina. Tente novamente. Se persistir, entre em contato com o suporte";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
