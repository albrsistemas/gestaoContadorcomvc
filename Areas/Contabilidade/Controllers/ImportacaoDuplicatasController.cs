using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models.SCI;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [Authorize(Roles = "Contabilidade")]
    public class ImportacaoDuplicatasController : Controller
    {
        public IActionResult Create()
        {
            DateTime h = DateTime.Now;
            SCI_IDs ids = new SCI_IDs();
            ids.fd = new Filtros_Duplicatas();
            ids.fd.data_inicial = h.AddDays(-30);
            ids.fd.data_final = h;

            return View(ids);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SCI_IDs ids)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            ImportacaoDuplicatas id = new ImportacaoDuplicatas();
            ids = id.create(ids.fd, user.usuario_conta_id, user.usuario_id);

            return View(ids);
        }
    }
}
