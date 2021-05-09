using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;

namespace gestaoContadorcomvc.Components
{
    public class Site_Servicos : ViewComponent
    {
        public IViewComponentResult Invoke()
        {  
            return View();
        }
    }
}
