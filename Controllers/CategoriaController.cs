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
        // GET: Categoria_v2Controller
        [FiltroAutorizacao(permissao = "categoriaList")]
        public ActionResult Index()
        {
            ViewData["bread"] = "Plano de Categorias";

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Categoria categoria = new Categoria();
            List<Vm_categoria> categorias = new List<Vm_categoria>();
            categorias = categoria.listaCategorias(user.usuario_conta_id, user.usuario_id);

            if(categorias.Count == 0)
            {
                categoria.startCategoria(user.usuario_conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }

            return View(categorias);
        } 


        // GET: Categoria_v2Controller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categoria_v2Controller/Create
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

        // GET: Categoria_v2Controller/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Categoria_v2Controller/Edit/5
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

        // GET: Categoria_v2Controller/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Categoria_v2Controller/Delete/5
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
                
        [FiltroAutorizacao(permissao = "categoriaCreate")]
        public ActionResult CreateGrupoCategoria(string escopo)
        {
            TempData["escopo"] = escopo;

            return View();
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGrupoCategoria(IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createGrupo"] = categoria.cadastrarCategoriaGrupo(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], user.usuario_conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao criar o grupo";

                return RedirectToAction(nameof(Index));
            }
        }

        //Verificar se classificação da categoria existe
        public IActionResult classificacaoExiste(string valor)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Categoria categoria = new Categoria();

            bool existe = categoria.classificacaoExiste(valor, user.usuario_conta_id);

            return Json(!existe);
        }


    }
}
