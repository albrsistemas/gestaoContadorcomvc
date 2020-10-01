using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [FiltroAutenticacao]
    [FiltroContabilidade]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(string controller, string action)
        {
            Error erro = new Error();
            erro.controller = controller;
            erro.action = action;

            return View(erro);
        }
    }
}
