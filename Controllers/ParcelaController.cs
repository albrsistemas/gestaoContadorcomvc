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
    public class ParcelaController : Controller
    {
        [Autoriza(permissao = "ContasPList")]
        public ActionResult Index(int parcela_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Op_parcelas p = new Op_parcelas();
            Vm_detalhamento_parcela vm_dp = new Vm_detalhamento_parcela();
            vm_dp = p.detalhamentoParcelas(user.usuario_id, user.usuario_conta_id, parcela_id);
            vm_dp.user = user;

            return View(vm_dp);
        }

        [Autoriza(permissao = "ContasPList")]        
        public ActionResult Delete(int parcela_id)
        {
            string retorno = "";
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Op_parcelas p = new Op_parcelas();

                retorno = p.deleteParcelaCartaoCredito(user.usuario_id, user.usuario_conta_id, parcela_id);

                TempData["msgCP"] = retorno;

                return RedirectToAction("Index", "ContasPagar");
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao excluir a fatura do cartão. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                TempData["msgCP"] = retorno;

                return RedirectToAction("Index", "ContasPagar");
            }
        }
    }
}
