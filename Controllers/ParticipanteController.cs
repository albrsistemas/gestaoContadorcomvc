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
    [Authorize]
    public class ParticipanteController : Controller
    {
        [Autoriza(permissao = "participanteList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Participante participante = new Participante();
            Vm_participante vm_part = new Vm_participante();

            vm_part.user = user;
            vm_part.participantes = participante.listaParticipantes(user.usuario_id, user.usuario_conta_id);

            return View(vm_part);
        }
                

        // GET: ParticipanteController/Create
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            ViewBag.tipoPessoa = select.getPlanoContas(plano_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.regimeTributario = select.getPlanoContas(plano_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.icmsContribuinte = select.getPlanoContas(plano_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.ufIbge = select.getPlanoContas(plano_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View();
        }

        // POST: ParticipanteController/Create
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

        // GET: ParticipanteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ParticipanteController/Edit/5
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

        // GET: ParticipanteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ParticipanteController/Delete/5
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
