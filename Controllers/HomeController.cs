using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Services;

namespace gestaoContadorcomvc.Controllers
{   
    [FiltroAutenticacao]
    public class HomeController : Controller
    {      
        public IActionResult Index()
        {          

            return View();
        }

        public IActionResult AcessoNegado()
        {
            return View();
        }

        public IActionResult AcessoNegadoContabilidade()
        {
            return View();
        }

        public IActionResult AcessoNegadoContador()
        {
            return View();
        }

    }
}
