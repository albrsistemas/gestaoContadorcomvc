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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [FiltroAutenticacao]
    [FiltroContabilidade]
    public class CategoriaController : Controller
    {
        // GET: CategoriaController
        public ActionResult Index()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Conta conta = new Conta();
            conta = conta.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));
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

                categorias = categoria.listaCategorias(conta.conta_id, user.usuario_id, user.usuario_conta_id.ToString(), vm_config.ccc_planoContasVigente);
            }
            else
            {
                categorias = categoria.listaCategorias(conta.conta_id, user.usuario_id, null, null);
            }

            return View(categorias);
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
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createGrupo"] = categoria.cadastrarCategoriaGrupo(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], contexto.conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao criar o grupo";

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: CategoriaController/Create
        [FiltroAutorizacao(permissao = "categoriaCreate")]
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
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));


            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createCategoria"] = categoria.cadastrarCategoria(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], contexto.conta_id, user.usuario_id, collection["categoria_conta_contabil"]);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Edit/5
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
        public ActionResult Edit(int categoria_id, IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["editCategoria"] = categoria.alterarNomeCategoria(collection["categoria_nome"], collection["categoria_conta_contabil"], categoria_id, contexto.conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);

            int retorno = 0;

            if (vm_categoria.categoria_tipo == "Sintetica")
            {
                retorno = categoria.quatidadeRegistroGrupo(vm_categoria.categoria_classificacao, user.usuario_conta_id);
            }

            if (retorno > 1)
            {
                TempData["RestricaoDelete"] = "Grupo não pode ser excluído, pois há categorias ativas atreladas ao grupo!";
            }

            return View(vm_categoria);
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int categoria_id, string categoria_nome)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));

            try
            {
                Categoria categoria = new Categoria();

                TempData["deleteCategoria"] = categoria.deletarCategoria(categoria_id, categoria_nome, contexto.conta_id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
