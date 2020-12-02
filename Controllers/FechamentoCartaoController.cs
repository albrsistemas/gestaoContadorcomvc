using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    public class FechamentoCartaoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fechamento_cartao fc)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            string retorno = "";

            try
            {
                retorno = fc.cadastraFechamentoCartao(user.usuario_conta_id, user.usuario_id, fc);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao gravar a fatura do cartão. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }
    }
}
