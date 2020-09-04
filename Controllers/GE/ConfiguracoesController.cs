using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.GE
{
    public class ConfiguracoesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["bread"] = "Configurações";

            return View();
        }
    }
}