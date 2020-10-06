using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
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
    public class CCOController : Controller
    {
        // GET: CCOController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CCOController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CCOController/Create
        public ActionResult Create(int plano_id, int categoria_id)
        {
            Categoria categoria = new Categoria();
            Vm_categoria vm_Categoria = new Vm_categoria();
            vm_Categoria = categoria.buscaCategoria(categoria_id);

            vm_categoria_contaonline vm_cco = new vm_categoria_contaonline();
            vm_cco.cco_plano_id = plano_id;
            vm_cco.cco_categoria_id = categoria_id;
            vm_cco.categoria_nome = vm_Categoria.categoria_nome;


            Selects select = new Selects();
            ViewBag.planoContas = select.getPlanoContas(plano_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(vm_cco);
        }

        // POST: CCOController/Create
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

        // GET: CCOController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CCOController/Edit/5
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

        // GET: CCOController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CCOController/Delete/5
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
