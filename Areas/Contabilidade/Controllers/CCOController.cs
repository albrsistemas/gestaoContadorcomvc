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
    public class CCOController : Controller
    {
        // GET: CCOController
        public ActionResult Index()
        {
            return View();
        }

        [Autoriza(permissao = "clienteCategoriasCreate")]
        public ActionResult Details(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            Categoria_contaonline cco = new Categoria_contaonline();
            vm_categoria_contaonline vm_cco = new vm_categoria_contaonline();
            vm_cco = cco.buscarVinculo(contexto.conta_id, user.usuario_id, id);                

            return View(vm_cco);
        }

        [Autoriza(permissao = "clienteCategoriasCreate")]
        public ActionResult Create(int plano_id, int categoria_id)
        {
            Categoria categoria = new Categoria();
            Vm_categoria vm_Categoria = new Vm_categoria();
            vm_Categoria = categoria.buscaCategoria(categoria_id);

            vm_categoria_contaonline vm_cco = new vm_categoria_contaonline();
            vm_cco.cco_plano_id = plano_id;
            vm_cco.cco_categoria_id = categoria_id;
            vm_cco.categoria_nome = vm_Categoria.categoria_nome;

            Selects select = new Selects();
            ViewBag.planoContas = select.getPlanoContas(plano_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(vm_cco);
        }

        // POST: CCOController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["retornoCCO"] = "Erro! Há informações incorretas no formulário de vinculação de conta on line!";

                    return RedirectToAction("Index", "Categoria", new { area = "Contabilidade" });
                }

                Categoria_contaonline cco = new Categoria_contaonline();

                TempData["retornoCCO"] = cco.vinculacaoCCO(user.usuario_id, contexto.conta_id, user.usuario_conta_id, collection["cco_plano_id"], collection["cco_ccontabil_id"], collection["cco_categoria_id"]);

                return RedirectToAction("Index", "Categoria", new { area = "Contabilidade" });
            }
            catch
            {
                TempData["retornoCCO"] = "Erro! Houve uma falha na vinculação da conta on line. Tente novamente. Se persistir entre em contato com o suporte!";

                return RedirectToAction("Index", "Categoria", new { area = "Contabilidade" });
            }
        }       

        // POST: CCOController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int cco_id, string cco_plano_id, string cco_categoria_id, string cco_ccontabil_id)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
                Conta contexto = new Conta();
                contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

                Categoria_contaonline cco = new Categoria_contaonline();

                TempData["retornoCCO"] = cco.desvinculacaoCCO(user.usuario_id,contexto.conta_id,user.usuario_conta_id, cco_id, cco_plano_id, cco_ccontabil_id, cco_categoria_id);

                return RedirectToAction("Index", "Categoria", new { area = "Contabilidade" });
            }
            catch
            {
                TempData["retornoCCO"] = "Erro! Houve uma falha na desvinculação da conta on line. Tente novamente. Se persistir entre em contato com o suporte!";

                return RedirectToAction("Index", "Categoria", new { area = "Contabilidade" });
            }
        }
    }
}
