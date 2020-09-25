using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Components
{
    public class Menu : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");            

            TempData["user"] = user;            

            return View();
        }
    }
}
