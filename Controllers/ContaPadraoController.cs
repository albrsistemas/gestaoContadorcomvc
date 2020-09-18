using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    public class ContaPadraoController : Controller
    {
        // GET: ContaPadraoController
        public ActionResult Index()
        {
            ContaPadrao cp = new ContaPadrao();

            return View(cp.listContasPadrao());
        }

        // GET: ContaPadraoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContaPadraoController/Create
        public ActionResult Create()
        {
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
