using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    public class OperacaoController : Controller
    {
        // GET: OperacaoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OperacaoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OperacaoController/Create
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();            
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });            
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });

            return View();
        }

        // POST: OperacaoController/Create
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

        // GET: OperacaoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OperacaoController/Edit/5
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

        // GET: OperacaoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OperacaoController/Delete/5
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
