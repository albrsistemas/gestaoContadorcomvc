using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.Site
{
    public class LeadController : Controller
    {
        // GET: LeadController
        public ActionResult Index()
        {
            return View();
        }

        // GET: LeadController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LeadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeadController/Create
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

        // GET: LeadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeadController/Edit/5
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

        // GET: LeadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeadController/Delete/5
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
