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
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

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
                List<Selects> cc = new List<Selects>();
                cc = select.getContasCorrenteConta_id(user.conta.conta_id);
                ViewBag.ccorrente = cc.Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == cc[0].value });
                ViewBag.tipoOpercao = select.getTipoOperacaoCCM().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });

                DateTime today = DateTime.Today;
                //TempData["dataInicio"] = today.AddDays(-30).ToShortDateString();
                DateTime ini = new DateTime(today.Year, today.Month, 1);
                TempData["dataInicio"] = ini.ToShortDateString();
                TempData["dataFim"] = today.ToShortDateString();
                if(cc.Count == 0)
                {
                    TempData["contacorrente_id"] = 0;
                }
                else
                {
                    TempData["contacorrente_id"] = cc[0].value;
                }                
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
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

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

        [Autoriza(permissao = "CCMCreate")]
        public IActionResult Create(string dataInicio, string dataFim, int contacorrente_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            //Selects select = new Selects();
            //ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });

            Vm_ccm ccm = new Vm_ccm();
            filtro f = new filtro();
            f.contacorrente_id = contacorrente_id;
            f.dataInicio = Convert.ToDateTime(dataInicio);
            f.dataFim = Convert.ToDateTime(dataFim);
            ccm.filtro = f;
            Ccm_nf nf = new Ccm_nf();
            nf.ccm_nf = false;
            ccm.nf = nf;

            return View(ccm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string dataInicio, string dataFim, int contacorrente_id, DateTime data, DateTime ccm_data_competencia, Decimal valor, string memorando, int categoria_id, int participante_id, int ccorrente_id, bool ccm_nf, DateTime ccm_nf_data_emissao, Decimal ccm_nf_valor, string ccm_nf_serie, string ccm_nf_numero, string ccm_nf_chave, Decimal ccm_valor_principal, Decimal ccm_multa, Decimal ccm_juros, Decimal ccm_desconto)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

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

                retorno = ccm.cadastrarCCM(user.usuario_id, user.usuario_conta_id, data, ccm_data_competencia, valor, memorando, categoria_id, participante_id, ccorrente_id, ccm_nf, ccm_nf_data_emissao, ccm_nf_valor, ccm_nf_serie, ccm_nf_numero, ccm_nf_chave, ccm_valor_principal, ccm_multa, ccm_juros, ccm_desconto);

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

        [Autoriza(permissao = "CCMEdit")]
        public IActionResult Edit(string dataInicio, string dataFim, int contacorrente_id, int ccm_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Conta_corrente_mov conta_mov = new Conta_corrente_mov();
            Vm_ccm ccm = new Vm_ccm();
            ccm = conta_mov.buscaCCM(user.usuario_id, user.usuario_conta_id, ccm_id);
            filtro f = new filtro();
            f.contacorrente_id = contacorrente_id;
            f.dataInicio = Convert.ToDateTime(dataInicio);
            f.dataFim = Convert.ToDateTime(dataFim);
            ccm.filtro = f;            

            Selects select = new Selects();
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == ccm.ccm_contra_partida_id.ToString() });

            return View(ccm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string dataInicio, string dataFim, int contacorrente_id, DateTime data, DateTime ccm_data_competencia, Decimal valor, string memorando, int categoria_id, int participante_id, int ccorrente_id, bool ccm_nf, DateTime ccm_nf_data_emissao, Decimal ccm_nf_valor, string ccm_nf_serie, string ccm_nf_numero, string ccm_nf_chave, int ccm_id, Decimal ccm_valor_principal, Decimal ccm_multa, Decimal ccm_juros, Decimal ccm_desconto)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

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

                retorno = ccm.alterarCCM(user.usuario_id, user.usuario_conta_id, data, ccm_data_competencia, valor, memorando, categoria_id, participante_id, ccorrente_id, ccm_nf, ccm_nf_data_emissao, ccm_nf_valor, ccm_nf_serie, ccm_nf_numero, ccm_nf_chave, ccm_id, ccm_valor_principal, ccm_multa, ccm_juros, ccm_desconto);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao alterar o lançamento. Tente novamente. Se persistir, entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [Autoriza(permissao = "CCMDelete")]
        public ActionResult Delete(int ccm_id, string dataInicio, string dataFim, int contacorrente_id)
        {
            string retorno = "";

            TempData["dataInicio"] = Convert.ToDateTime(dataInicio);
            TempData["dataFim"] = Convert.ToDateTime(dataFim);
            TempData["contacorrente_id"] = contacorrente_id;

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Fluxo_caixa fc = new Fluxo_caixa();
                Conta_corrente_mov ccm = new Conta_corrente_mov();

                retorno =  ccm.excluirCCM(user.usuario_id, user.usuario_conta_id, ccm_id);

                TempData["msgCCM"] = retorno;

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
            catch
            {
                if(retorno == "")
                {
                    TempData["msgCCM"] = "Erro ao excluir . Tente novamente, se persistir, entre em contato com o suporte!";
                }
                else
                {
                    TempData["msgCCM"] = retorno;
                }

                return RedirectToAction("Index", "ContaCorrenteMov", new { dataInicio = Convert.ToDateTime(dataInicio), dataFim = Convert.ToDateTime(dataFim), contacorrente_id = contacorrente_id });
            }
        }
    }
}
