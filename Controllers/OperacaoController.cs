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
    public class OperacaoController : Controller
    {
        [Autoriza(permissao = "operacaoList")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: OperacaoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Autoriza(permissao = "operacaoCreate")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            //ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });            
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id,true,false,true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });
            ViewBag.operacoes = select.getOperacoes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "OutrosGastos" });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id,"Pagamento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "OutrosGastos" });

            return View();
        }

        // POST: OperacaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Vm_operacao op)
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

        [Autoriza(permissao = "operacaoEdit")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OperacaoController/Edit/5
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

        [Autoriza(permissao = "operacaoDelete")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OperacaoController/Delete/5
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
        public ActionResult GerarFormaPgto(string op_tipo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            string identificacao = "";
            if(op_tipo == "Compra" || op_tipo == "ServicoTomado" || op_tipo == "OutrasDespesas")
            {
                identificacao = "Pagamento";
            }
            if (op_tipo == "Venda" || op_tipo == "ServicoPrestado" || op_tipo == "OutrasReceitas")
            {
                identificacao = "Recebimento";
            }

            Selects select = new Selects();
            List<Selects> lista = new List<Selects>();
            lista = select.getFormaPgto(user.usuario_conta_id, identificacao);

            return Json(JsonConvert.SerializeObject(lista));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GerarCategorias(string op_tipo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            bool entradas = false;
            bool saidas = false;
            if (op_tipo == "Compra" || op_tipo == "ServicoTomado" || op_tipo == "OutrasDespesas")
            {
                saidas = true;
            }
            if (op_tipo == "Venda" || op_tipo == "ServicoPrestado" || op_tipo == "OutrasReceitas")
            {
                entradas = true;
            }

            Selects select = new Selects();
            List<Selects> lista = new List<Selects>();
            lista = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, entradas, saidas);

            return Json(JsonConvert.SerializeObject(lista));
        }
    }
}
