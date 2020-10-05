using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    public class ContaPadraoController : Controller
    {
        // GET: ContaPadraoController
        public ActionResult Index(string pattern)
        {
            ContaPadrao cp = new ContaPadrao();

            ViewData["bread"] = "PLano de contas padrão";

            List<Vm_ContaPadrao> contas = new List<Vm_ContaPadrao>();

            //if(pattern == null)
            //{
            //    contas = cp.listContasPadrao("01");
            //}
            //else
            //{
            //    contas = cp.listContasPadrao(pattern);
            //}

            return View(contas);
        }

        // GET: ContaPadraoController/Create
        public ActionResult Create()
        {
            Selects select = new Selects();
            ViewBag.grupoContas = select.getGrupoContas().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Selected = c.value == "Ativo" }).ToList();

            return View();
        }

        // POST: ContaPadraoController/Create
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

        // GET: ContaPadraoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContaPadraoController/Edit/5
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

        // GET: ContaPadraoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContaPadraoController/Delete/5
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
