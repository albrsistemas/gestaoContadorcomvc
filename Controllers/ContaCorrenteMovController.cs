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
            TempData["dataInicio"] = today;
            TempData["dataFim"] = today.AddDays(-30);

            return View();
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

                Conta_corrente_mov ccm = new Conta_corrente_mov();
                Vm_conta_corrente_mov vm_ccm = new Vm_conta_corrente_mov();
                vm_ccm.user = user;
                vm_ccm.conta_corrente_movimento = ccm.listaCCM(user.usuario_id, user.usuario_conta_id, contacorrente_id, dataInicio, dataFim);

                Selects select = new Selects();
                ViewBag.ccorrente = select.getContasCorrente(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == contacorrente_id.ToString() });
                TempData["dataInicio"] = dataInicio;
                TempData["dataFim"] = dataFim;

                return View(vm_ccm);
            }
            catch
            {
                return RedirectToAction(nameof(Index));                
            }
        }
    }
}
