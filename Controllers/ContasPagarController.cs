﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class ContasPagarController : Controller
    {
        [Autoriza(permissao = "ContasPList")]
        public IActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_contas_pagar vm_cp = new Vm_contas_pagar();
            ContasPagar cp = new ContasPagar();

            vm_cp = cp.listaContasPagar(user.usuario_id, user.usuario_conta_id);
            vm_cp.user = user;

            Selects select = new Selects();
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Pagamento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoOpercao = select.getTipoOperacaoCCM().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(vm_cp);
        }
    }
}