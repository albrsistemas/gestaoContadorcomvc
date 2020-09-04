using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.Menu
{    
    public class MenuController : Controller
    {
        [FiltroAutenticacao]
        public ActionResult menu()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            return PartialView(user);
        }        
    }
}