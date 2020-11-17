﻿using System;
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
    public class ContaCorrenteMovController : Controller
    {
        // GET: ContaCorrenteMovController
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            ViewBag.ccorrente = select.getContasCorrente(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            DateTime today = DateTime.Today;
            TempData["dataInicio"] = today.AddDays(-30).ToShortDateString();
            TempData["dataFim"] = today.ToShortDateString();

            Vm_fluxo_caixa vm_fc = new Vm_fluxo_caixa();            
            Fluxo_caixa fc = new Fluxo_caixa();
            List<Fluxo_caixa> lista = new List<Fluxo_caixa>();
            vm_fc.fluxo = lista;
            vm_fc.user = user;

            return View(vm_fc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DateTime dataInicio, DateTime dataFim, int contacorrente_id, IFormCollection d)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Vm_fluxo_caixa vm_fc = new Vm_fluxo_caixa();
                Fluxo_caixa fc = new Fluxo_caixa();
                vm_fc = fc.fluxoCaixa(user.usuario_id, user.usuario_conta_id, contacorrente_id, dataInicio, dataFim);
                vm_fc.user = user;

                Selects select = new Selects();
                ViewBag.ccorrente = select.getContasCorrente(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == contacorrente_id.ToString() });
                TempData["dataInicio"] = dataInicio.ToShortDateString();
                TempData["dataFim"] = dataFim.ToShortDateString();

                return View(vm_fc);
            }
            catch
            {
                return RedirectToAction(nameof(Index));                
            }
        }
    }
}
