using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Filtros;

namespace gestaoContadorcomvc.Controllers
{   
    [FiltroAutenticacao]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
        {
            //Teste
            //Registro registro = new Registro();
            //registro.conta = new Conta();
            //registro.conta.conta_dcto = "01050932000101";
            //registro.conta.conta_tipo = "Contabilidade";
            //registro.usuario = new Usuario();
            //registro.usuario.usuario_dcto = "22598091892";
            //registro.usuario.usuario_nome = "Ariel Estevam";
            //registro.usuario.usuario_user = "ariel";
            //registro.usuario.usuario_senha = "1234";           

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
