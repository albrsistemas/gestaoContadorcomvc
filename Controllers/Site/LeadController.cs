using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.Site;
using gestaoContadorcomvc.Models.Site.ViewModel;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers.Site
{   
    public class LeadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Lead lead = new Lead();
            Vm_lead vm_Lead = new Vm_lead();
            vm_Lead = lead.list_leads(user.conta.conta_id);
            vm_Lead.user = user;

            return Json(JsonConvert.SerializeObject(vm_Lead));
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
