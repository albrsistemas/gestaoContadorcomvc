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

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]    
    [Authorize(Roles = "Contabilidade")]
    [Autoriza(permissao = "clienteCategoriasList")]
    public class CategoriaController : Controller
    {
        // GET: CategoriaController
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            Conta conta = new Conta();
            conta = conta.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));
            
            TempData["Cliente"] = conta.conta_nome;

            //Vericiar se cliente possui contabilidade on line. Se poitivo buscar o plano do cliente
            Config_contador_cliente config = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_config = new vm_ConfigContadorCliente();
            vm_config = config.buscaCCC(user.usuario_id, conta.conta_id, user.usuario_conta_id);


            Categoria categoria = new Categoria();
            List<Vm_categoria> categorias = new List<Vm_categoria>();

            //Verificando o plano de contas
            PlanoContas planoContas = new PlanoContas();
            if(vm_config.ccc_planoContasVigente != null)
            {
                planoContas = planoContas.buscaPlanoContas(Convert.ToInt32(vm_config.ccc_planoContasVigente), user.usuario_conta_id, user.usuario_id);
                TempData["planoCliente"] = planoContas.plano_nome;
                TempData["planoCliente_id"] = planoContas.plano_id;

                categorias = categoria.listaCategorias(conta.conta_id, user.usuario_id, user.usuario_conta_id.ToString(), vm_config.ccc_planoContasVigente,"Não");
            }
            else
            {
                categorias = categoria.listaCategorias(conta.conta_id, user.usuario_id, null, null,"Não");
            }

            Vm_categoria cats = new Vm_categoria();
            cats.categorias = categorias;
            cats.user = user;

            return View(cats);
        }

        [Autoriza(permissao = "clienteCategoriasCreate")]
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
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createGrupo"] = categoria.cadastrarCategoriaGrupo(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], contexto.conta_id, user.usuario_id, "Não", "0");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao criar o grupo";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: CategoriaController/Create
        [Autoriza(permissao = "clienteCategoriasCreate")]
        public ActionResult Create(string grupo, string escopo)
        {
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
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));


            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                if (categoria.validaFormulario(collection))
                {
                    TempData["createCategoria"] = "Erro. Nome e Classificação são obrigatórios.";

                    return RedirectToAction(nameof(Index));
                }

                TempData["createCategoria"] = categoria.cadastrarCategoria(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], contexto.conta_id, user.usuario_id, collection["categoria_conta_contabil"],"Não","0", categoria_categoria_fiscal, categoria_categoria_tributo);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Autoriza(permissao = "clienteCategoriasEdit")]
        public ActionResult Edit(int id, string tipo)
        {
            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);

            TempData["classificacao"] = vm_categoria.categoria_classificacao;

            return View(vm_categoria);
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int categoria_id, IFormCollection collection, bool categoria_categoria_fiscal, bool categoria_categoria_tributo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["editCategoria"] = categoria.alterarNomeCategoria(collection["categoria_nome"], collection["categoria_conta_contabil"], categoria_id, contexto.conta_id, user.usuario_id, categoria_categoria_fiscal, categoria_categoria_tributo);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Autoriza(permissao = "clienteCategoriasDelete")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);            

            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);

            if (vm_categoria.categoria_tipo == "Sintetica" && (categoria.quatidadeRegistroGrupo(vm_categoria.categoria_classificacao, user.usuario_conta_id, "Não") > 0))
            {
                TempData["RestricaoDelete"] = "Grupo não pode ser excluído, pois há categorias ativas atreladas ao grupo!";
            }
            else
            {
                if (categoria.verificaCategoriaFoiUsada(id, Convert.ToInt32(user.usuario_ultimoCliente)) > 0)
                {
                    TempData["RestricaoDelete"] = "Categoria não pode ser apagada, pois está sendo utilizada nos lançamentos de operação, conta corrente movimento ou no cadastro de participante!";
                }
            }

            return View(vm_categoria);
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int categoria_id, string categoria_nome)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            try
            {
                Categoria categoria = new Categoria();

                if (categoria.verificaCategoriaFoiUsada(categoria_id, Convert.ToInt32(user.usuario_ultimoCliente)) > 0)
                {
                    TempData["deleteCategoria"] = "Erro. A categoria " + categoria_nome + " não pode ser apagada, pois já está sendo usada no cadastro de participante ou operação ou conta corrente movimento!";
                }
                else
                {
                    TempData["deleteCategoria"] = categoria.deletarCategoria(categoria_id, categoria_nome, contexto.conta_id, user.usuario_id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Autoriza(permissao = "clienteCopiaPlano")]
        public ActionResult copiarPlanoCategorias()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            PlanoContas planoContas = new PlanoContas();

            Selects select = new Selects();
            ViewBag.getPlanosCategoriaContador = select.getPlanosCategoriaContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(contexto);
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult copiarPlanoCategorias(int pc_id, int cliente_id)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);                

                Categoria categoria = new Categoria();

                TempData["copiarPlanoCategorias"] = categoria.copiarPlanoCategorias(user.usuario_id, user.usuario_conta_id, pc_id, cliente_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}