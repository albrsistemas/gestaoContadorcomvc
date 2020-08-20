using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    public class ContasPagarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}