using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.GE
{
    [FiltroAutorizacao]
    public class ConfiguracoesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["bread"] = "Configurações";

            return View();
        }

        // GET: Categoria_v2Controller/Create        
        public ActionResult Contabilidade()
        {            
            return View();
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contabilidade(IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}