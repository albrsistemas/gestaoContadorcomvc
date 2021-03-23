using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers.Relatorios
{
    public class RfmDetailsController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string classificacao, DateTime data_inicio, DateTime data_fim)
        {
            Vm_rfm_details rfmd = new Vm_rfm_details();
            List<Vm_rfm_details> rfmd_list = new List<Vm_rfm_details>();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                rfmd_list = rfmd.rfm_details(user.conta.conta_id, data_inicio, data_fim, classificacao);

                return Json(JsonConvert.SerializeObject(rfmd_list));
            }
            catch
            {
                return Json(JsonConvert.SerializeObject(rfmd_list));
            }
        }
    }
}
