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
    public class ContasFinanceirasController : Controller
    {
        [Autoriza(permissao = "contasFList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            ContasFinanceiras cf = new ContasFinanceiras();
            Vm_contasFinanceiras vmcf = new Vm_contasFinanceiras();
            vmcf = cf.listaCF(user.usuario_id, user.usuario_conta_id);
            vmcf.user = user;

            return View(vmcf);
        }

        [Autoriza(permissao = "contasFCreate")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            ViewBag.tipoCtaFinaceira = select.getTipoCtaFinanceira().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Parcelada" });
            ViewBag.recorrencias = select.getRecorrencias().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Unica" });
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id,false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });            
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });

            Vm_contasFinanceiras vmcf = new Vm_contasFinanceiras();

            return View(vmcf);
        }

        // POST: ContasFinanceirasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Vm_contasFinanceiras vmcf)
        {
            string retorno = "";
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Selects select = new Selects();
                ViewBag.tipoCtaFinaceira = select.getTipoCtaFinanceira().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Parcelada" });
                ViewBag.recorrencias = select.getRecorrencias().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Unica" });
                ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });
                ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
                ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
                ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
                ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });

                ContasFinanceiras cf = new ContasFinanceiras();

                retorno = cf.cadastrarContaFinanceira(user.usuario_id, user.usuario_conta_id, vmcf);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao cadastrar a conta financeira. Tente novamente. Se persistir entre em contato com o suporte!";
                }
                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [Autoriza(permissao = "contasFEdit")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            ContasFinanceiras cf = new ContasFinanceiras();
            Vm_contasFinanceiras vmcf = new Vm_contasFinanceiras();
            vmcf = cf.buscaCF(id, user.usuario_conta_id);

            Selects select = new Selects();
            ViewBag.tipoCtaFinaceira = select.getTipoCtaFinanceira().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.cf.cf_tipo });

            if (vmcf.cf.cf_tipo == "Realizar")
            {
                List<Selects> selects = new List<Selects>();
                selects.Add(new Selects
                {
                    value = "0",
                    text = "Não se Aplica",
                    disabled = true
                });
                ViewBag.formaPgto = selects.Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            }
            else
            {
                ViewBag.formaPgto = select.getFormaPgtoGeral(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.cf.cf_forma_pgto.ToString() });
            }

            ViewBag.recorrencias = select.getRecorrencias().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.cf.cf_recorrencia });
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.cf.cf_status });           
            if (vmcf.op.op_escopo_caixa == "S")
            {
                ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, false, false, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.cf.cf_categoria_id.ToString() });
            }
            else
            {
                ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, false, true, false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.cf.cf_categoria_id.ToString() });
            }
            
            
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.nf.op_nf_tipo.ToString() });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.participante.op_uf_ibge_codigo.ToString() });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.participante.op_paisesIBGE_codigo.ToString() });

            return View(vmcf);
        }

        // POST: ContasFinanceirasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Vm_contasFinanceiras vmcf)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                ContasFinanceiras cf = new ContasFinanceiras();

                retorno = cf.alterarContaFinanceira(user.usuario_id, user.usuario_conta_id, vmcf);

                TempData["msgCF"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao cadastrar a conta financeira. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                TempData["msgCF"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "contasFDelete")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContasFinanceirasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GerarSelectFormaPagamento(int categoria_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            List<Selects> lista = new List<Selects>();
            lista = select.getFormaPgto_categoria_id(user.usuario_conta_id, categoria_id);

            return Json(JsonConvert.SerializeObject(lista));
        }

        [Autoriza(permissao = "contasFCreate")]
        public ActionResult CFR_realizacao(int parcela_id, string contexto)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            ContasFinanceiras cf = new ContasFinanceiras();
            Vm_contasFinanceiras vmcf = new Vm_contasFinanceiras();
            vmcf = cf.gerarCFR(parcela_id, user.usuario_conta_id);

            Selects select = new Selects();
            ViewBag.tipoCtaFinaceira = select.getTipoCtaFinanceira().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Realizada" });
            ViewBag.recorrencias = select.getRecorrencias().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Unica" });
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.cf.cf_categoria_id.ToString() });
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.nf.op_nf_tipo.ToString() });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.participante.op_uf_ibge_codigo.ToString() });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.participante.op_paisesIBGE_codigo.ToString() });

            TempData["parcela_id"] = parcela_id;

            TempData["contexto"] = contexto;

            return View(vmcf);
        }

        // POST: ContasFinanceirasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CFR_realizacao(IFormCollection collection, Vm_contasFinanceiras vmcf, int parcela_id)
        {
            string retorno = "";
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Selects select = new Selects();
                ViewBag.tipoCtaFinaceira = select.getTipoCtaFinanceira().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Parcelada" });
                ViewBag.recorrencias = select.getRecorrencias().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Unica" });
                ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });
                ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
                ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
                ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
                ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });

                ContasFinanceiras cf = new ContasFinanceiras();

                retorno = cf.cadastrarContaFinanceiraCFR(user.usuario_id, user.usuario_conta_id, vmcf, parcela_id);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao cadastrar a conta financeira. Tente novamente. Se persistir entre em contato com o suporte!";
                }
                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

    }
}
