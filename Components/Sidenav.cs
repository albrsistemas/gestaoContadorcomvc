using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Components
{
    public class Sidenav : ViewComponent
    {
        public IViewComponentResult Invoke(string escopo)
        {
            TempData["escopo"] = escopo;           

            return View();
        }
    }
}
