using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Components
{
    public class MenuContabilidade : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);

            user.conta = conta;

            TempData["user"] = user;

            return View();
        }
    }
}