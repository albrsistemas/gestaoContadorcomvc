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
            return View();
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
        public ActionResult Create(IFormCollection collection, Vm_contasFinanceiras cf)
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
    }
}
