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
    public class RiopController : Controller
    {
        [Autoriza(permissao = "riopList")]
        public IActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            //Lista tipo operação
            List<Selects> ops = new List<Selects>();
            ops.Add(new Selects
            {
                value = "3",
                text = "Ambos"
            });
            ops.Add(new Selects
            {
                value = "1",
                text = "Compra"
            });
            ops.Add(new Selects
            {
                value = "2",
                text = "Venda"
            });            

            Selects select = new Selects();
            ViewBag.participantes = select.getParticipantes(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.centroCusto = select.getCentroCusto(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.produtos = select.getProdutos(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.operacao = ops.Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            Riop riop = new Riop();
            riop.lista = null;
            riop.user = user;

            return View(riop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Riop_filtro filtro)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            
            //Lista tipo operação
            List<Selects> ops = new List<Selects>();
            ops.Add(new Selects
            {
                value = "3",
                text = "Ambos"
            });
            ops.Add(new Selects
            {
                value = "1",
                text = "Compra"
            });
            ops.Add(new Selects
            {
                value = "2",
                text = "Venda"
            });            

            Selects select = new Selects();  
            if(filtro.participante != null && filtro.participante.Length > 0)
            {
                ViewBag.participantes = select.getParticipantes(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = filtro.participante.Contains(Convert.ToInt32(c.value)) });
            }
            else
            {
                ViewBag.participantes = select.getParticipantes(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            }

            if (filtro.item != null && filtro.item.Length > 0)
            {
                ViewBag.produtos = select.getProdutos(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = filtro.item.Contains(Convert.ToInt32(c.value)) });
            }
            else
            {
                ViewBag.produtos = select.getProdutos(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            }

            if (filtro.centro_custo != null && filtro.centro_custo.Length > 0)
            {
                ViewBag.centroCusto = select.getCentroCusto(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = filtro.centro_custo.Contains(Convert.ToInt32(c.value)) });
            }
            else
            {
                ViewBag.centroCusto = select.getCentroCusto(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            }

            ViewBag.operacao = ops.Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == filtro.operacao_tipo });            

            Riop riop = new Riop();
            riop = riop.create(filtro, user.conta.conta_id);            
            riop.user = user;

            return View(riop);
        }
    }
}
