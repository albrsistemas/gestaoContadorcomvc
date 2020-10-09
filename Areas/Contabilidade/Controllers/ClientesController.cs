using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    //[FiltroAutenticacao]
    //[FiltroContabilidade]
    [Authorize(Roles = "Contabilidade")]
    public class ClientesController : Controller
    {
        // GET: ClientesController         
        public ActionResult Index(int id)
        {

            HttpContext.Session.SetInt32("cliente_id", id);


            return View();
        }

        // GET: ClientesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientesController/Create
        public ActionResult Create()
        {
            var cliente = HttpContext.Session.GetInt32("cliente_id");

            return View();
        }

        // POST: ClientesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ClientesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClientesController/Edit/5
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

        // GET: ClientesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClientesController/Delete/5
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

        // GET: ClientesController/Edit/5
        public ActionResult SelectCliente(string url)
        {
            TempData["url"] = url;

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Selects select = new Selects();

            var cliente_selecionado = HttpContext.Session.GetInt32("cliente_selecionado");

            ViewBag.empresasContador = select.getEmpresasContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Selected = c.value == cliente_selecionado.ToString()}).ToList();

            return View();
        }

        // POST: ClientesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectCliente(int cliente_id, string url)
        {
            string[] urlFatiada = new string[10];
            urlFatiada = url.Split("/");

            try
            {
                HttpContext.Session.SetInt32("cliente_selecionado", cliente_id);

                return RedirectToAction(urlFatiada[5], urlFatiada[4], new { area = "Contabilidade" });
            }
            catch
            {
                return View();
            }
        }
    }
}
