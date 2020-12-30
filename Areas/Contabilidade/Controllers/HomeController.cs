using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
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
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_home home = new Vm_home();
            home.user = user;

            return View(home);
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
