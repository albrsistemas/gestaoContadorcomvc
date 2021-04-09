using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.Relatorios;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers.Relatorios
{
    [Authorize]
    public class RpController : Controller
    {
        [Autoriza(permissao = "rpList")]
        public IActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Rp_filtro filtro = new Rp_filtro();
            DateTime hoje = DateTime.Today;
            filtro.ano = hoje.Year.ToString();
            filtro.ignorar_zerados = true;
            filtro.ocultar_nomes = false;

            Relatorio_participante rp = new Relatorio_participante();            
            rp.filtro = filtro;
            
            Selects select = new Selects();
            ViewBag.tipos_participante = select.getTipoParticipante(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.categorias = select.getCategoriasCliente(user.conta.conta_id,false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(rp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Rp_filtro filtro)
        {
            Rp rp = new Rp();
            List<Rp> rps = new List<Rp>();
            Relatorio_participante relatorio_p = new Relatorio_participante();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
                
                relatorio_p = rp.create(user.conta.conta_id, filtro.ano, filtro.ignorar_zerados, filtro.ocultar_nomes, filtro.tipos_participante, filtro.categorias);
                relatorio_p.filtro = filtro;
                relatorio_p.retorno = "Sucesso";

                return Json(JsonConvert.SerializeObject(relatorio_p));
            }
            catch
            {
                relatorio_p.filtro = filtro;
                relatorio_p.retorno = "Erro (catch)";

                return Json(JsonConvert.SerializeObject(relatorio_p));
            }
        }

    }
}
