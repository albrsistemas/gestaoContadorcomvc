using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using System;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {      
        public IActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_home home = new Vm_home();
            home.user = user;

            return View(home);
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
