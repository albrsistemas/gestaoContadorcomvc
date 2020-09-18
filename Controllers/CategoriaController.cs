using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    [FiltroAutenticacao]
    public class CategoriaController : Controller
    {
        // GET: Categoria
        [FiltroAutorizacao(permissao = "categoriaList")]
        public ActionResult Index()
        {
            ViewData["bread"] = "Categoria";

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            ContaPadrao contasPadrao = new ContaPadrao();
            Vm_categoria categoria = new Vm_categoria();

            categoria = contasPadrao.listaCategorias(user.usuario_conta_id, user.usuario_id, 0, user);

            return View(categoria);
        }

        // GET: Categoria/Create
        [FiltroAutorizacao(permissao = "categoriaCreate")]
        public ActionResult Create(string id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            ContaPadrao contaPadrao = new ContaPadrao();
            Vm_categoria categoria = new Vm_categoria();

            categoria = contaPadrao.buscaCategoria(id, user.usuario_conta_id, user.usuario_id);

            categoria.contaPadrao_descricao = "";
            categoria.contaPadrao_apelido = "";

            return View(categoria);
        }

        // POST: Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection dados)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            try
            {
                ContaPadrao contaPadrao = new ContaPadrao();
                Vm_categoria categoriaPai = new Vm_categoria();

                categoriaPai = contaPadrao.buscaCategoria(dados["categoriaPai_id"], user.usuario_conta_id, user.usuario_id);

                string retorno = contaPadrao.criarCategoriaCliente(categoriaPai, dados["contaPadrao_descricao"], dados["contaPadrao_apelido"], user.usuario_conta_id, user.usuario_id);

                TempData["novaCategoria"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Categoria/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Categoria/Edit/5
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

        // GET: Categoria/Delete/5
        [FiltroAutorizacao(permissao = "categoriaDelete")]
        public ActionResult Delete(string id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            ContaPadrao contaPadrao = new ContaPadrao();

            Vm_categoria categoria = new Vm_categoria();

            categoria = contaPadrao.buscaCategoria(id, user.usuario_conta_id, user.usuario_id);

            return View(categoria);
        }

        // POST: Categoria/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            try
            {
                ContaPadrao contaPadrao = new ContaPadrao();

                TempData["deleteCategoria"] = contaPadrao.deletaCategoria(user.usuario_conta_id, user.usuario_id, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [FiltroAutorizacao(permissao = "categoriaCreate")]
        public ActionResult CreateCxBanco()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");                       

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCxBanco(IFormCollection dados)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            try
            {
                ContaPadrao contaPadrao = new ContaPadrao();
                string retorno = contaPadrao.vincularBanco(dados["contaPadrao_codigoBanco"], user.usuario_conta_id, user.usuario_id);

                TempData["novaCategoria"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}
