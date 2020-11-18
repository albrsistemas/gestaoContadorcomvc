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
    public class ContaCorrenteMovController : Controller
    {
        [Autoriza(permissao = "CCMList")]
        public ActionResult Index(DateTime dataInicio, DateTime dataFim, int contacorrente_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();            

            Vm_fluxo_caixa vm_fc = new Vm_fluxo_caixa();            
            Fluxo_caixa fc = new Fluxo_caixa();

            if(contacorrente_id > 0)
            {
                vm_fc = fc.fluxoCaixa(user.usuario_id, user.usuario_conta_id, contacorrente_id, dataInicio, dataFim);
                TempData["dataInicio"] = dataInicio.ToShortDateString();
                TempData["dataFim"] = dataFim.ToShortDateString();
                TempData["contacorrente_id"] = contacorrente_id;
                ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == contacorrente_id.ToString() });
            }
            else
            {
                List<Fluxo_caixa> lista = new List<Fluxo_caixa>();
                vm_fc.fluxo = lista;
                DateTime today = DateTime.Today;
                TempData["dataInicio"] = today.AddDays(-30).ToShortDateString();
                TempData["dataFim"] = today.ToShortDateString();
                ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            }            
            vm_fc.user = user;

            TempData["msg"] = "Selecione uma conta corrente e período para gerar fluxo de caixa!";

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
                ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == contacorrente_id.ToString() });
                TempData["dataInicio"] = dataInicio.ToShortDateString();
                TempData["dataFim"] = dataFim.ToShortDateString();
                TempData["contacorrente_id"] = contacorrente_id;
                TempData["msg"] = "Não há lançamentos para esta conta corrente neste período!";

                return View(vm_fc);
            }
            catch
            {
                return RedirectToAction(nameof(Index));                
            }
        }
    }
}
