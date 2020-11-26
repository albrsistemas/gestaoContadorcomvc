using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace gestaoContadorcomvc.Controllers
{
    public class ContasReceberController : Controller
    {
        [Autoriza(permissao = "ContasRList")]
        public IActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_contas_receber vm_cp = new Vm_contas_receber();
            ContasReceber cp = new ContasReceber();

            vm_cp = cp.listaContasReceber(user.usuario_id, user.usuario_conta_id);
            vm_cp.user = user;

            return View(vm_cp);
        }
    }
}
