using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models.SCI;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    public class ImportacaoDuplicatasController : Controller
    {
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Filtros_Duplicatas fd)
        {
            ImportacaoDuplicatas id = new ImportacaoDuplicatas();
            SCI_IDs ids = new SCI_IDs();



            return View(ids);
        }
    }
}
