using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    public class ParcelaController : Controller
    {
        // GET: ParcelaController
        public ActionResult Index(int parcela_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Op_parcelas p = new Op_parcelas();
            Vm_detalhamento_parcela vm_dp = new Vm_detalhamento_parcela();
            vm_dp = p.detalhamentoParcelas(user.usuario_id, user.usuario_conta_id, parcela_id);
            vm_dp.user = user;

            return View(vm_dp);
        }

        // GET: ParcelaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ParcelaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParcelaController/Create
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

        // GET: ParcelaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ParcelaController/Edit/5
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

        // GET: ParcelaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParcelaController/Delete/5
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
