using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    public class Vm_UsuarioController : Controller
    {
        // GET: Vm_UsuarioController
        public ActionResult Index()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Vm_usuario usuario = new Vm_usuario();

            List<Vm_usuario> lista = new List<Vm_usuario>();
            lista = usuario.listaVmUsuario(user.usuario_conta_id, user.usuario_id);

            return View(lista);
        }

        public IActionResult users()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Vm_usuario usuario = new Vm_usuario();

            List<Vm_usuario> lista = new List<Vm_usuario>();
            lista = usuario.listaVmUsuario(user.usuario_conta_id, user.usuario_id);

            return View(lista);
        }

        // GET: Vm_UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Vm_UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vm_UsuarioController/Create
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

        // GET: Vm_UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Vm_UsuarioController/Edit/5
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

        // GET: Vm_UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vm_UsuarioController/Delete/5
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
