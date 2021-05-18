using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.Site;
using gestaoContadorcomvc.Models.Site.ViewModel;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers.Site
{
    [Authorize]
    public class Lead_atendentesController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Lead_atendentes atendente = new Lead_atendentes();
            Vm_lead_atendentes lista = new Vm_lead_atendentes();            
            lista = atendente.list_lead_atendentes(user.conta.conta_id);
            lista.user = user;

            return Json(JsonConvert.SerializeObject(lista));
        }

        // POST: Lead_atendentesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Lead_atendentes atendente = new Lead_atendentes();

                retorno = atendente.create(collection["lead_atendentes_nome"], collection["lead_atendentes_celular"], collection["lead_atendentes_email"], user.conta.conta_id, Convert.ToBoolean(collection["lead_atendentes_atende_fila_um"]), Convert.ToBoolean(collection["lead_atendentes_atende_fila_dois"]));

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao processar o cadastro do atendente!!";
                }
                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Busca(int id)
        {
            Lead_atendentes atendente = new Lead_atendentes();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                atendente = atendente.busca(user.conta.conta_id, id);

                return Json(JsonConvert.SerializeObject(atendente));
            }
            catch
            {
                atendente.lead_atendentes_id = 0;

                return Json(JsonConvert.SerializeObject(atendente));
            }
        }

        // POST: Lead_atendentesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int lead_atendentes_id, IFormCollection collection)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Lead_atendentes atendente = new Lead_atendentes();

                retorno = atendente.edit(lead_atendentes_id, collection["lead_atendentes_nome"], collection["lead_atendentes_celular"], collection["lead_atendentes_email"], user.conta.conta_id, Convert.ToBoolean(collection["lead_atendentes_atende_fila_um"]), Convert.ToBoolean(collection["lead_atendentes_atende_fila_dois"]));

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao processar a alteração do atendente!!";
                }
                return Json(JsonConvert.SerializeObject(retorno));
            }
        }        

        // POST: Lead_atendentesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int lead_atendentes_id, IFormCollection collection)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Lead_atendentes atendente = new Lead_atendentes();

                retorno = atendente.delete(user.conta.conta_id, lead_atendentes_id);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao processar a exclusão do atendente!!";
                }
                return Json(JsonConvert.SerializeObject(retorno));
            }
        }
    }
}
