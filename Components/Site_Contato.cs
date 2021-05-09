using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Components
{
    public class Site_Contato : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
