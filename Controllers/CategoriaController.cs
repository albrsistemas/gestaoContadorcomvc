using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
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
    public class CategoriaController : Controller
    {
        // GET: Categoria_v2Controller
        [Autoriza(permissao = "categoriaList")]
        public ActionResult Index()
        {
            ViewData["bread"] = "Categorias";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Categoria categoria = new Categoria();
            Vm_categoria categorias = new Vm_categoria();
            categorias.categorias = categoria.listaCategorias(user.usuario_conta_id, user.usuario_id, null, null,"Não");

            categorias.user = user;

            Config_contador_cliente cco = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_cco = new vm_ConfigContadorCliente();
            vm_cco = cco.buscaCCC(user.usuario_id, user.usuario_conta_id_original, user.conta.contador_id);

            categorias.cco = vm_cco;

            return View(categorias);
        }

        [Autoriza(permissao = "categoriaCreate")]
        public ActionResult CreateGrupoCategoria(string escopo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Config_contador_cliente cco = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_cco = new vm_ConfigContadorCliente();
            vm_cco = cco.buscaCCC(user.usuario_id, user.usuario_conta_id_original, user.conta.contador_id);

            if (vm_cco.ccc_pref_novaCategoria)
            {
                TempData["createGrupo"] = "Erro, a função de criar categorias está bloqueada pelo contador!";

                return RedirectToAction(nameof(Index));
            }

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
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                if (categoria.classificacaoExistePorPlano(collection["categoria_classificacao"], user.usuario_conta_id, "Não"))
                {
                    TempData["createCategoria"] = "Erro. Classifcação já existe!";

                    return RedirectToAction(nameof(Index));
                }

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
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Config_contador_cliente cco = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_cco = new vm_ConfigContadorCliente();
            vm_cco = cco.buscaCCC(user.usuario_id, user.usuario_conta_id_original, user.conta.contador_id);

            if (vm_cco.ccc_pref_novaCategoria)
            {
                TempData["createGrupo"] = "Erro, a função de criar categorias está bloqueada pelo contador!";

                return RedirectToAction(nameof(Index));
            }

            TempData["grupoCategoria"] = grupo;

            TempData["escopo"] = escopo;

            return View();
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, bool categoria_categoria_fiscal, bool categoria_categoria_tributo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                if(categoria.validaFormulario(collection))
                {
                    TempData["createCategoria"] = "Erro. Nome e Classificação são obrigatórios.";

                    return RedirectToAction(nameof(Index));
                }

                if(categoria.classificacaoExistePorPlano(collection["categoria_classificacao"], user.usuario_conta_id, "Não"))
                {
                    TempData["createCategoria"] = "Erro. Classifcação já existe!";

                    return RedirectToAction(nameof(Index));
                }

                TempData["createCategoria"] = categoria.cadastrarCategoria(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], user.usuario_conta_id, user.usuario_id, collection["categoria_conta_contabil"], "Não", "0", categoria_categoria_fiscal, categoria_categoria_tributo);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["createCategoria"] = "Erro ao criar a categoria. Tente novamente, se persistir, entre em contato com o suporte!";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Categoria_v2Controller/Edit/5
        [Autoriza(permissao = "categoriaEdit")]
        public ActionResult Edit(int id, string tipo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Selects select = new Selects();
            ViewBag.categoria_padrao = select.getCategoriaPadrao().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });

            Config_contador_cliente cco = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_cco = new vm_ConfigContadorCliente();
            vm_cco = cco.buscaCCC(user.usuario_id, user.usuario_conta_id_original, user.conta.contador_id);

            if (vm_cco.ccc_pref_novaCategoria)
            {
                TempData["createGrupo"] = "Erro, a função de alterar categorias está bloqueada pelo contador!";

                return RedirectToAction(nameof(Index));
            }

            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);

            TempData["classificacao"] = vm_categoria.categoria_classificacao;

            return View(vm_categoria);
        }

        // POST: Categoria_v2Controller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int categoria_id, IFormCollection collection, bool categoria_categoria_fiscal, bool categoria_categoria_tributo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["editCategoria"] = categoria.alterarNomeCategoria(collection["categoria_nome"], collection["categoria_conta_contabil"], categoria_id, user.usuario_conta_id, user.usuario_id, categoria_categoria_fiscal, categoria_categoria_tributo);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["createCategoria"] = "Erro ao alterar a categoria. Tente novamente, se persistir, entre em contato com o suporte!";

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "categoriaDelete")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Config_contador_cliente cco = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_cco = new vm_ConfigContadorCliente();
            vm_cco = cco.buscaCCC(user.usuario_id, user.usuario_conta_id_original, user.conta.contador_id);

            if (vm_cco.ccc_pref_novaCategoria)
            {
                TempData["createGrupo"] = "Erro, a função de apagar categorias está bloqueada pelo contador!";

                return RedirectToAction(nameof(Index));
            }            

            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);            

            if (vm_categoria.categoria_tipo == "Sintetica" && (categoria.quatidadeRegistroGrupo(vm_categoria.categoria_classificacao, user.usuario_conta_id, "Não") > 0))
            {
                TempData["RestricaoDelete"] = "Grupo não pode ser excluído, pois há categorias ativas atreladas ao grupo!";
            }           
            else
            {
                if (categoria.verificaCategoriaFoiUsada(id, user.conta.conta_id) > 0)
                {
                    TempData["RestricaoDelete"] = "Categoria não pode ser apagada, pois está sendo utilizada nos lançamentos de operação, conta corrente movimento ou no cadastro de participante!";
                }
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
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            try
            {
                Categoria categoria = new Categoria();
                if(categoria.verificaCategoriaFoiUsada(categoria_id, user.conta.conta_id) > 0)
                {
                    TempData["deleteCategoria"] = "Erro. A categoria " + categoria_nome + " não pode ser apagada, pois já está sendo usada no cadastro de participante ou operação ou conta corrente movimento!";
                }
                else
                {
                    TempData["deleteCategoria"] = categoria.deletarCategoria(categoria_id, categoria_nome, user.usuario_conta_id, user.usuario_id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //Verificar se classificação da categoria existe
        public IActionResult classificacaoExiste(string categoria_classificacao, int pc_id, int planoContas_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Categoria categoria = new Categoria();

            bool existe = categoria.classificacaoExiste(categoria_classificacao, user.usuario_conta_id);

            return Json(!existe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GerarCategorias(string  termo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Categoria c = new Categoria();
            List<Vm_categoria> lista = new List<Vm_categoria>();
            lista = c.listaCategoriasPorTermo(user.usuario_conta_id, termo);

            return Json(JsonConvert.SerializeObject(lista));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DefinirPadraoCategoria(string padrao, int categoria_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Categoria c = new Categoria();
            Vm_categoria v = new Vm_categoria();
            v = c.buscaCategoria(categoria_id);

            string retorno = "";

            if(v.categoria_conta_id != user.conta.conta_id) //se for diferente está sendo acessado pelo contador
            {
                retorno = c.definirPadraoCategoria(user.usuario_id, Convert.ToInt32(user.usuario_ultimoCliente), padrao, categoria_id);
            }
            else
            {
                retorno = c.definirPadraoCategoria(user.usuario_id, user.usuario_conta_id, padrao, categoria_id);
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListaCategorias_ajax(IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Categoria c = new Categoria();
            List<Vm_categoria> vm = new List<Vm_categoria>();
            vm = c.listaCategorias_dados_basicos(user.conta.conta_id);

            return Json(JsonConvert.SerializeObject(vm));
        }
    }
}
