using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.Site
{
    public class SiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PrestadorServico()
        {
            return View();
        }

        public IActionResult AreaSaude()
        {
            return View();
        }

        public IActionResult MarketingPublicidade()
        {
            return View();
        }

        public IActionResult DesenvSistemas()
        {
            return View();
        }

        public IActionResult Academias()
        {
            return View();
        }

        public IActionResult Coaching()
        {
            return View();
        }

        public IActionResult EngenheirosArquitetos()
        {
            return View();
        }

        public IActionResult SeguradorasCorretoras()
        {
            return View();
        }

        public IActionResult Escolas()
        {
            return View();
        }

        public IActionResult Representantes()
        {
            return View();
        }
    }
}
