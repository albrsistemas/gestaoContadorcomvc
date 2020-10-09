using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    //[FiltroAutenticacao]
    //[FiltroContabilidade]
    [Authorize(Roles = "Contabilidade")]    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //List<string> roles = new List<string>();

            //string user = "";

            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    foreach(Claim ci in HttpContext.User.Claims)
            //    {
            //        roles.Add(ci.Value);
            //    }

            //    user = HttpContext.User.Identity.Name;
            //}

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
