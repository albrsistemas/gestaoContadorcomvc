using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Site;
using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Components
{
    public class Site_Whatsapp : ViewComponent
    {
        public IViewComponentResult Invoke(string user_id)
        {
            Lead_atendentes l = new Lead_atendentes();
            l = l.busca_proximo_atendente(Convert.ToInt32(user_id), 2);

            Codificacao c = new Codificacao();
            var codigo = c.EncodeToBase64(l.lead_atendentes_id.ToString() + "," + user_id);

            string w_link = "https://api.whatsapp.com/send?phone=55" + l.lead_atendentes_celular + "&text=Ol%C3%A1%2C%20tudo%20bem%3F%20Gostaria%20de%20conversar%20sobre%20os%20servi%C3%A7os%20da%20Contadorcomvoc%C3%AA";

            TempData["codigo"] = codigo;
            TempData["link"] = w_link;
            TempData["nome"] = l.lead_atendentes_nome;

            return View();
        }
    }
}
