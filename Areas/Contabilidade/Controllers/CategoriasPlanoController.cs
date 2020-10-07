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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [FiltroAutenticacao]
    [FiltroContabilidade]
    public class CategoriasPlanoController : Controller
    {  
        public ActionResult SelectPlano(int pc_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            PlanoContas planoContas = new PlanoContas();

            TempData["pc_id"] = pc_id;

            Selects select = new Selects();
            ViewBag.planosContasContador = select.getPlanosContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(planoContas.listaPlanoContas(user.usuario_conta_id, user.usuario_id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectPlano(int pc_id, int planoContas_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if(pc_id > 0 && planoContas_id > 0)
            {
                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = pc_id, @planoContas_id = planoContas_id });
            }            

            return RedirectToAction("Index", "PlanoCategorias");
        }



        // GET: CategoriasPlanoController
        public ActionResult Index(int planoContas_id, int pc_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            //Verificando o plano de contas
            PlanoContas planoContas = new PlanoContas();
            planoContas = planoContas.buscaPlanoContas(planoContas_id, user.usuario_conta_id, user.usuario_id);           
            TempData["plano_nome"] = planoContas.plano_nome;
            TempData["plano_id"] = planoContas.plano_id;

            PlanoCategorias pc = new PlanoCategorias();
            pc = pc.buscaPlanoCategorias(pc_id, user.usuario_conta_id, user.usuario_id);
            TempData["pc_nome"] = pc.pc_nome ;


            Categoria categoria = new Categoria();
            List<Vm_categoria> categorias = new List<Vm_categoria>();
            categorias = categoria.listaCategorias(user.usuario_conta_id, user.usuario_id, user.usuario_conta_id.ToString(), planoContas_id.ToString(), pc_id.ToString());

            if (categorias.Count == 0)
            {
                categoria.startCategoria(user.usuario_conta_id, user.usuario_id, pc_id.ToString());

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = pc_id, @planoContas_id = planoContas_id });
            }

            return View(categorias);
        }

        // GET: CategoriasPlanoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriasPlanoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriasPlanoController/Create
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

        // GET: CategoriasPlanoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoriasPlanoController/Edit/5
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

        // GET: CategoriasPlanoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriasPlanoController/Delete/5
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
