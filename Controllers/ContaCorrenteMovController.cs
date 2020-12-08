using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ContaCorrenteMovController : Controller
    {
        [Autoriza(permissao = "CCMList")]
        public ActionResult Index(DateTime dataInicio, DateTime dataFim, int contacorrente_id, int tipoOperacao, int nOperacao, int participante_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();            

            Vm_fluxo_caixa vm_fc = new Vm_fluxo_caixa();            
            Fluxo_caixa fc = new Fluxo_caixa();

            if(contacorrente_id > 0)
            {
                vm_fc = fc.fluxoCaixa(user.usuario_id, user.usuario_conta_id, contacorrente_id, dataInicio, dataFim,tipoOperacao,nOperacao,participante_id);
                TempData["dataInicio"] = dataInicio.ToShortDateString();
                TempData["dataFim"] = dataFim.ToShortDateString();
                TempData["contacorrente_id"] = contacorrente_id;
                ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == contacorrente_id.ToString() });
                ViewBag.tipoOpercao = select.getTipoOperacaoCCM().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            }
            else
            {
                List<Fluxo_caixa> lista = new List<Fluxo_caixa>();
                vm_fc.fluxo = lista;
                DateTime today = DateTime.Today;
                TempData["dataInicio"] = today.AddDays(-30).ToShortDateString();
                TempData["dataFim"] = today.ToShortDateString();
                TempData["contacorrente_id"] = 0;
                ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
                ViewBag.tipoOpercao = select.getTipoOperacaoCCM().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            }            
            vm_fc.user = user;

            TempData["msg"] = "Selecione uma conta corrente e período para gerar fluxo de caixa!";

            return View(vm_fc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(DateTime dataInicio, DateTime dataFim, int contacorrente_id, int tipoOperacao, int nOperacao, int participante_id, IFormCollection d)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Vm_fluxo_caixa vm_fc = new Vm_fluxo_caixa();
                Fluxo_caixa fc = new Fluxo_caixa();
                vm_fc = fc.fluxoCaixa(user.usuario_id, user.usuario_conta_id, contacorrente_id, dataInicio, dataFim, tipoOperacao, nOperacao, participante_id);
                vm_fc.user = user;

                Selects select = new Selects();
                ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == contacorrente_id.ToString() });                
                ViewBag.tipoOpercao = select.getTipoOperacaoCCM().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
                TempData["dataInicio"] = dataInicio.ToShortDateString();
                TempData["dataFim"] = dataFim.ToShortDateString();
                TempData["contacorrente_id"] = contacorrente_id;
                TempData["msg"] = "Não há lançamentos para esta conta corrente neste período!";

                return View(vm_fc);
            }
            catch
            {
                return RedirectToAction(nameof(Index));                
            }
        }

        public IActionResult Create(string dataInicio, string dataFim, int contacorrente_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });

            Vm_ccm ccm = new Vm_ccm();
            filtro f = new filtro();
            f.contacorrente_id = contacorrente_id;
            f.dataInicio = Convert.ToDateTime(dataInicio);
            f.dataFim = Convert.ToDateTime(dataFim);
            ccm.filtro = f;

            return View(ccm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string dataInicio, string dataFim, int contacorrente_id, DateTime data, Decimal valor, string memorando, int categoria_id, int participante_id, int ccorrente_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            string retorno = "";

            try
            {
                Conta_corrente_mov ccm = new Conta_corrente_mov();
                Vm_ccm vm_ccm = new Vm_ccm();
                filtro f = new filtro();
                f.contacorrente_id = contacorrente_id;
                f.dataInicio = Convert.ToDateTime(dataInicio);
                f.dataFim = Convert.ToDateTime(dataFim);
                vm_ccm.filtro = f;

                retorno = ccm.cadastrarCCM(user.usuario_id, user.usuario_conta_id, data, valor, memorando, categoria_id, participante_id, ccorrente_id);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch 
            {
                if(retorno == "")
                {
                    retorno = "Erro ao gravar o lançamento. Tente novamente. Se persistir, entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }



    }
}
