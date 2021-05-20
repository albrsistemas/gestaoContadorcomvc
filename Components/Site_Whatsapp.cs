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

            TempData["msgParticipante"] = codigo;

            return View();
        }
    }
}
