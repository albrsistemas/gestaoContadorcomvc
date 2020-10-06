using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.SoftwareHouse
{
    [FiltroAutenticacao]
    public class LogController : Controller
    {
        public IActionResult Index()
        {
            ViewData["bread"] = "Log";

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);
            TempData["conta_tipo"] = conta.conta_tipo;

            Log log = new Log();

            return View(log.logs(user.usuario_conta_id));
        }
    }
}
