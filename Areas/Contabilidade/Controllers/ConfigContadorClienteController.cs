using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [FiltroAutenticacao]
    [FiltroContabilidade]
    public class ConfigContadorClienteController : Controller
    {
        // GET: ConfigContadorClienteController
        public ActionResult Index()
        {
            //Usuário logado / contexto conta selecionada pelo contador
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));
            TempData["Cliente"] = contexto.conta_nome;

            //Criar objeto de configurações para enviar para a view
            Config_contador_cliente config = new Config_contador_cliente();
            vm_ConfigContadorCliente ccc = new vm_ConfigContadorCliente();
            ccc = config.buscaCCC(user.usuario_id, contexto.conta_id, user.usuario_conta_id);

            //Verificar se existe lançamentos em aberto no plano de contas vigente a atribuir ao objeto configurações 's' ou 'n'
            if(ccc.ccc_planoContasVigente == 0)
            {
                ccc.ccc_liberaPlano = "S";
            }
            else
            {
                //Pesquisa lançamentos e atribui sim, se retorno for não há lançamentos sem encerramnto para o plano. Do contrário não;
                ccc.ccc_liberaPlano = "S"; //Por enquanto não existe lançamento, então vai ficar padrão 'sim'.
            }

            Selects select = new Selects();

            var cliente_selecionado = HttpContext.Session.GetInt32("cliente_selecionado");

            ViewBag.empresasContador = select.getEmpresasContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Selected = c.value == cliente_selecionado.ToString() }).ToList();


            return View(ccc);
        }

        // GET: ConfigContadorClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ConfigContadorClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConfigContadorClienteController/Create
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

        // GET: ConfigContadorClienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ConfigContadorClienteController/Edit/5
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

        // GET: ConfigContadorClienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConfigContadorClienteController/Delete/5
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
