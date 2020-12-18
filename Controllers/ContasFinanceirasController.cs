using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    public class ContasFinanceirasController : Controller
    {
        // GET: ContasFinanceirasController
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

        // GET: ContasFinanceirasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContasFinanceirasController/Create
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

        // GET: ContasFinanceirasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContasFinanceirasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ContasFinanceirasController/Delete/5
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

        public ActionResult CFR_realizacao(int parcela_id)
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
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.participante.op_uf_ibge_codigo.ToString() });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vmcf.participante.op_paisesIBGE_codigo.ToString() });

            TempData["parcela_id"] = parcela_id;

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
