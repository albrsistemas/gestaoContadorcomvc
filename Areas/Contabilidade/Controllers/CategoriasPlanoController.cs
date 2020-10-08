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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [FiltroAutenticacao]
    [FiltroContabilidade]
    public class CategoriasPlanoController : Controller
    {  
        public ActionResult SelectPlano(int pc_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            PlanoContas planoContas = new PlanoContas();

            TempData["pc_id"] = pc_id;

            Selects select = new Selects();
            ViewBag.planosContasContador = select.getPlanosContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(planoContas.listaPlanoContas(user.usuario_conta_id, user.usuario_id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectPlano(int pc_id, int planoContas_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if(pc_id > 0 && planoContas_id > 0)
            {
                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = pc_id, @planoContas_id = planoContas_id });
            }            

            return RedirectToAction("Index", "PlanoCategorias");
        }



        // GET: CategoriasPlanoController
        public ActionResult Index(int planoContas_id, int pc_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            //Verificando o plano de contas
            PlanoContas planoContas = new PlanoContas();
            planoContas = planoContas.buscaPlanoContas(planoContas_id, user.usuario_conta_id, user.usuario_id);           
            TempData["plano_nome"] = planoContas.plano_nome;
            TempData["plano_id"] = planoContas_id;

            PlanoCategorias pc = new PlanoCategorias();
            pc = pc.buscaPlanoCategorias(pc_id, user.usuario_conta_id, user.usuario_id);
            TempData["pc_nome"] = pc.pc_nome;
            TempData["pc_id"] = pc.pc_id;


            Categoria categoria = new Categoria();
            List<Vm_categoria> categorias = new List<Vm_categoria>();
            categorias = categoria.listaCategorias(user.usuario_conta_id, user.usuario_id, user.usuario_conta_id.ToString(), planoContas_id.ToString(), pc_id.ToString());

            return View(categorias);
        }

        [FiltroAutorizacao(permissao = "categoriaCreate")]
        public ActionResult Create(string grupo, string escopo, string planoCategorias_id, string planoContas_id)
        {
            TempData["grupoCategoria"] = grupo;
            TempData["escopo"] = escopo;
            TempData["planoCategorias_id"] = planoCategorias_id;
            TempData["planoContas_id"] = planoContas_id;

            return View();
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                TempData["createGrupo"] = "Erro nos dados informados!";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createCategoria"] = categoria.cadastrarCategoria(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], user.usuario_conta_id, user.usuario_id, collection["categoria_conta_contabil"], collection["planoCategorias_id"], "0");

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao criar a categoria!";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
        }

        // GET: CategoriasPlanoController/Edit/5
        [FiltroAutorizacao(permissao = "categoriaEdit")]
        public ActionResult Edit(int id, string tipo, string planoCategorias_id, string planoContas_id)
        {
            Categoria categoria = new Categoria();

            Vm_categoria vm_categoria = new Vm_categoria();

            vm_categoria = categoria.buscaCategoria(id);

            //TempData["classificacao"] = vm_categoria.categoria_classificacao;
            TempData["planoCategorias_id"] = planoCategorias_id;
            TempData["planoContas_id"] = planoContas_id;

            return View(vm_categoria);
        }

        // POST: Categoria_v2Controller/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int categoria_id, IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                TempData["createGrupo"] = "Erro nos dados informados!";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["editCategoria"] = categoria.alterarNomeCategoria(collection["categoria_nome"], collection["categoria_conta_contabil"], categoria_id, user.usuario_conta_id, user.usuario_id);

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao alterar a categoria!";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
        }

        // GET: CategoriasPlanoController/Delete/5
        public ActionResult Delete(int id, string planoCategorias_id, string planoContas_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            TempData["planoCategorias_id"] = planoCategorias_id;
            TempData["planoContas_id"] = planoContas_id;

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

        // POST: Categoria_v2Controller/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int categoria_id, string categoria_nome, string planoCategorias_id, string planoContas_id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            try
            {
                Categoria categoria = new Categoria();

                TempData["deleteCategoria"] = categoria.deletarCategoria(categoria_id, categoria_nome, user.usuario_conta_id, user.usuario_id);

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = planoCategorias_id, @planoContas_id = planoContas_id });
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao alterar a categoria!";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = planoCategorias_id, @planoContas_id = planoContas_id });
            }
        }

        [FiltroAutorizacao(permissao = "categoriaCreate")]
        public ActionResult CreateGrupoCategoria(string escopo, string planoCategorias_id, string planoContas_id)
        {
            TempData["escopo"] = escopo;
            TempData["planoCategorias_id"] = planoCategorias_id;
            TempData["planoContas_id"] = planoContas_id;

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
                TempData["createGrupo"] = "Erro ao criar o grupo";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }

            try
            {
                Categoria categoria = new Categoria();

                TempData["createGrupo"] = categoria.cadastrarCategoriaGrupo(collection["categoria_classificacao"], collection["categoria_nome"], collection["escopo"], user.usuario_conta_id, user.usuario_id, collection["planoCategorias_id"], "0");

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
            catch
            {
                TempData["createGrupo"] = "Erro ao criar o grupo";

                return RedirectToAction("Index", "CategoriasPlano", new { @pc_id = collection["planoCategorias_id"], @planoContas_id = collection["planoContas_id"] });
            }
        }
    }
}
