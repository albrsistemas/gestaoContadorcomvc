using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.Relatorios
{
    [Authorize]
    public class RpController : Controller
    {
        [Autoriza(permissao = "rpList")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
