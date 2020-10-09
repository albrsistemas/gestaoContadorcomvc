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

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    //[FiltroAutenticacao]
    //[FiltroContabilidade]
    [Authorize(Roles = "Contabilidade")]
    public class CCOPlanoCategoriasController : Controller
    {
        public ActionResult Details(int id, int planoContas_id, int planoCategorias_id)
        {
            TempData["planoCategorias_id"] = planoCategorias_id;
            TempData["planoContas_id"] = planoContas_id;

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");           

            Categoria_contaonline cco = new Categoria_contaonline();
            vm_categoria_contaonline vm_cco = new vm_categoria_contaonline();
            vm_cco = cco.buscarVinculo(user.usuario_conta_id, user.usuario_id, id);

            return View(vm_cco);
        }

        // GET: CCOController/Create
        public ActionResult Create(int categoria_id, int planoContas_id, int planoCategorias_id)
        {
            TempData["planoCategorias_id"] = planoCategorias_id;
            TempData["planoContas_id"] = planoContas_id;

            Categoria categoria = new Categoria();
            Vm_categoria vm_Categoria = new Vm_categoria();
            vm_Categoria = categoria.buscaCategoria(categoria_id);

            vm_categoria_contaonline vm_cco = new vm_categoria_contaonline();
            vm_cco.cco_plano_id = planoContas_id;
            vm_cco.cco_categoria_id = categoria_id;
            vm_cco.categoria_nome = vm_Categoria.categoria_nome;

            Selects select = new Selects();
            ViewBag.planoContas = select.getPlanoContas(planoContas_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(vm_cco);
        }

        // POST: CCOController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");            

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["retornoCCO"] = "Erro! Há informações incorretas no formulário de vinculação de conta on line!";

                    return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
                }

                Categoria_contaonline cco = new Categoria_contaonline();

                TempData["retornoCCO"] = cco.vinculacaoCCO(user.usuario_id, user.usuario_conta_id, user.usuario_conta_id, collection["cco_plano_id"], collection["cco_ccontabil_id"], collection["cco_categoria_id"]);

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
            catch
            {
                TempData["retornoCCO"] = "Erro! Houve uma falha na vinculação da conta on line. Tente novamente. Se persistir entre em contato com o suporte!";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
        }

        // POST: CCOController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int cco_id, string cco_plano_id, string cco_categoria_id, string cco_ccontabil_id, int planoContas_id, int planoCategorias_id)
        {
            try
            {
                var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");                

                Categoria_contaonline cco = new Categoria_contaonline();

                TempData["retornoCCO"] = cco.desvinculacaoCCO(user.usuario_id, user.usuario_conta_id, user.usuario_conta_id, cco_id, cco_plano_id, cco_ccontabil_id, cco_categoria_id);

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = planoCategorias_id, @planoContas_id = planoContas_id });
            }
            catch
            {
                TempData["retornoCCO"] = "Erro! Houve uma falha na desvinculação da conta on line. Tente novamente. Se persistir entre em contato com o suporte!";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = planoCategorias_id, @planoContas_id = planoContas_id });
            }
        }
    }
}
