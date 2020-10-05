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

                categorias = categoria.listaCategorias(conta.conta_id, user.usuario_id, user.usuario_conta_id.ToString(), vm_config.ccc_planoContasVigente);
            }
            else
            {
                categorias = categoria.listaCategorias(conta.conta_id, user.usuario_id, null, null);
            }

            return View(categorias);
        }

        // GET: CategoriaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaController/Create
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

        // GET: CategoriaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoriaController/Edit/5
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

        // GET: CategoriaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoriaController/Delete/5
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
    }
}
