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
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Log log = new Log();

            return View(log.logs(user.usuario_conta_id));
        }
    }
}
