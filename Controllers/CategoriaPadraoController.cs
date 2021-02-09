using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    public class CategoriaPadraoController : Controller
    {
        // GET: CategoriaPadraoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriaPadraoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriaPadraoController/Create
        public ActionResult Create()
        {
            Selects select = new Selects();
            ViewBag.categoria_padrao = select.getCategoriaPadrao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });

            return View();
        }

        // POST: CategoriaPadraoController/Create
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

        // GET: CategoriaPadraoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoriaPadraoController/Edit/5
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

        // GET: CategoriaPadraoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriaPadraoController/Delete/5
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
