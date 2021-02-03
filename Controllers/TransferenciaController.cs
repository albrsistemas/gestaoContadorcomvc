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
    public class TransferenciaController : Controller
    {
        [Autoriza(permissao = "CCMCreate")]
        public ActionResult Create(string dataInicio, string dataFim, int contacorrente_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Selects select = new Selects();
            ViewBag.ccorrente_de = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.ccorrente_para = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            TempData["dataInicio"] = Convert.ToDateTime(dataInicio);
            TempData["dataFim"] = Convert.ToDateTime(dataFim);
            TempData["contacorrente_id"] = contacorrente_id;

            Vm_transferencia vm_transfer = new Vm_transferencia();
            DateTime today = DateTime.Today;
            vm_transfer.data = today;

            return View(vm_transfer);
        }

        // POST: TransferenciaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DateTime data, Decimal valor, int ccorrente_de, int ccorrente_para, string dataInicio, string dataFim, int contacorrente_id, string memorando)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Fluxo_caixa fc = new Fluxo_caixa();
                TempData["msgCCM"] = fc.transferencia(user.usuario_id, user.usuario_conta_id, data, valor, ccorrente_de, ccorrente_para, memorando);

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
            catch
            {
                TempData["msgCCM"] = "Erro ao efeturar a transferência. Tente novamente, se persistir, entre em contato com o suporte!";

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
        }

        [Autoriza(permissao = "CCMEdit")]
        public ActionResult Edit(int ccm_id, string dataInicio, string dataFim, int contacorrente_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Fluxo_caixa fc = new Fluxo_caixa();
            Vm_transferencia vm_transf = new Vm_transferencia();
            vm_transf = fc.buscaTransferencia(user.usuario_id, user.usuario_conta_id, ccm_id);

            TempData["dataInicio"] = Convert.ToDateTime(dataInicio);
            TempData["dataFim"] = Convert.ToDateTime(dataFim);
            TempData["contacorrente_id"] = contacorrente_id;

            Selects select = new Selects();
            ViewBag.ccorrente_de = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_transf.ccorrente_de.ToString() });
            ViewBag.ccorrente_para = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_transf.ccorrente_para.ToString() });

            return View(vm_transf);
        }

        // POST: TransferenciaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int ccm_id, IFormCollection collection, string dataInicio, string dataFim, int contacorrente_id, DateTime data, Decimal valor, int ccorrente_de, int ccorrente_para, string memorando)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Fluxo_caixa fc = new Fluxo_caixa();
                Vm_transferencia vm_transf = new Vm_transferencia();

                retorno = fc.alteraTransferencia(user.usuario_id, user.usuario_conta_id, ccm_id, data, valor, ccorrente_de, ccorrente_para, memorando);

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
            catch
            {
                TempData["msgCCM"] = "Erro ao alterar a transferência. Tente novamente, se persistir, entre em contato com o suporte!";

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
        }

        [Autoriza(permissao = "CCMDelete")]
        public ActionResult Delete(int ccm_id, string dataInicio, string dataFim, int contacorrente_id)
        {   
            TempData["dataInicio"] = Convert.ToDateTime(dataInicio);
            TempData["dataFim"] = Convert.ToDateTime(dataFim);
            TempData["contacorrente_id"] = contacorrente_id;

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Fluxo_caixa fc = new Fluxo_caixa();

                TempData["msgCCM"] = fc.excluirTransferencia(user.usuario_id, user.usuario_conta_id, ccm_id);

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
            catch
            {
                TempData["msgCCM"] = "Erro ao excluir a transferência. Tente novamente, se persistir, entre em contato com o suporte!";

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
        }
    }
}
