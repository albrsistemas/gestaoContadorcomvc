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

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        // GET: Categoria_v2Controller
        [Autoriza(permissao = "categoriaList")]
        public ActionResult Index()
        {
            ViewData["bread"] = "Categorias";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Categoria categoria = new Categoria();
            Vm_categoria categorias = new Vm_categoria();
            categorias.categorias = categoria.listaCategorias(user.usuario_conta_id, user.usuario_id, null, null,"Não");

            categorias.user = user;

            return View(categorias);
        }

        [Autoriza(permissao = "categoriaCreate")]
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
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createGrupo"] = categoria.cadastrarCategoriaGrupo(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], user.usuario_conta_id, user.usuario_id, "Não", "0");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao criar o grupo";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Categoria_v2Controller/Create
        [Autoriza(permissao = "categoriaCreate")]
        public ActionResult Create(string grupo, string escopo)
        {
            TempData["grupoCategoria"] = grupo;

            TempData["escopo"] = escopo;

            return View();
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createCategoria"] = categoria.cadastrarCategoria(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], user.usuario_conta_id, user.usuario_id, collection["categoria_conta_contabil"], "Não", "0");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Categoria_v2Controller/Edit/5
        [Autoriza(permissao = "categoriaEdit")]
        public ActionResult Edit(int id, string tipo)
        {
            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);

            TempData["classificacao"] = vm_categoria.categoria_classificacao;

            return View(vm_categoria);
        }

        // POST: Categoria_v2Controller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int categoria_id, IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["editCategoria"] = categoria.alterarNomeCategoria(collection["categoria_nome"], collection["categoria_conta_contabil"], categoria_id, user.usuario_conta_id, user.usuario_id);

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
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);

            int retorno = 0;

            if (vm_categoria.categoria_tipo == "Sintetica")
            {
                retorno = categoria.quatidadeRegistroGrupo(vm_categoria.categoria_classificacao, user.usuario_conta_id);
            }

            if(retorno > 1)
            {
                TempData["RestricaoDelete"] = "Grupo não pode ser excluído, pois há categorias ativas atreladas ao grupo!";
            }                       

            return View(vm_categoria);
        }

        // POST: Categoria_v2Controller/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int categoria_id, string categoria_nome)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            try
            {
                Categoria categoria = new Categoria();

                TempData["deleteCategoria"] = categoria.deletarCategoria(categoria_id, categoria_nome, user.usuario_conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //Verificar se classificação da categoria existe
        public IActionResult classificacaoExiste(string categoria_classificacao)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Categoria categoria = new Categoria();

            bool existe = categoria.classificacaoExiste(categoria_classificacao, user.usuario_conta_id);

            return Json(!existe);
        }


    }
}
