using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class Participante_tipoController : Controller
    {
        [Autoriza(permissao = "participanteList")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Participante_tipo pt = new Participante_tipo();
            List<Participante_tipo> pts = new List<Participante_tipo>();            
            pts = pt.index(user.conta.conta_id, user.usuario_id);

            return Json(JsonConvert.SerializeObject(pts));
        }

        [Autoriza(permissao = "participanteCreate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Participante_tipo pt = new Participante_tipo();

                retorno = pt.create(user.conta.conta_id, user.usuario_id, d["pt_nome"]);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao cadastrar o tipo de participante. Tente novamente, se persistir, entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }
        

        [Autoriza(permissao = "participanteEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Participante_tipo pt = new Participante_tipo();

                retorno = pt.edit(user.conta.conta_id, user.usuario_id, id, collection["pt_nome"]);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao alterar o tipo de participante. Tente novamente, se persistir, entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [Autoriza(permissao = "participanteDelete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Participante_tipo pt = new Participante_tipo();

                retorno = pt.delete(user.conta.conta_id, user.usuario_id, id);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao excluir o tipo de participante. Tente novamente, se persistir, entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }
    }
}
