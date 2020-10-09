using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    //[FiltroAutenticacao]
    //[FiltroContabilidade]
    [Authorize(Roles = "Contabilidade")]
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
            if(ccc.ccc_planoContasVigente == "0")
            {
                ccc.ccc_liberaPlano = "Sim";
            }
            else
            {
                //Pesquisa lançamentos e atribui sim, se retorno for não há lançamentos sem encerramnto para o plano. Do contrário não;
                ccc.ccc_liberaPlano = "Sim"; //Por enquanto não existe lançamento, então vai ficar padrão 'sim'.
            }

            Selects select = new Selects();

            if(ccc.ccc_id == 0)
            {
                ViewBag.planosContador = select.getPlanosContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.text == "Selecione um plano de contas" }).ToList();
            }
            else
            {
                ViewBag.planosContador = select.getPlanosContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == ccc.ccc_planoContasVigente.ToString() }).ToList();
            }

            return View(ccc);
        }

        [FiltroAutorizacao(permissao = "configClienteCreate")]
        public ActionResult Create()
        {
            //Usuário logado / contexto conta selecionada pelo contador
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Selects select = new Selects();
           
            ViewBag.planosContador = select.getPlanosContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.text == "Selecione um plano de contas" }).ToList();

            return View();
        }

        // POST: ConfigContadorClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bool ccc_pref_contabilizacao, string ccc_planoContasVigente, bool ccc_pref_novaCategoria, bool ccc_pref_editCategoria, bool ccc_pref_deleteCategoria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }

                if(ccc_planoContasVigente == null)
                {
                    ccc_planoContasVigente = "0";
                }

                var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
                Conta contexto = new Conta();
                contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));

                Config_contador_cliente config = new Config_contador_cliente();

                TempData["retornoConfig"] = config.cadastrarConfiguracoes(user.usuario_conta_id, user.usuario_id, user.usuario_conta_id, contexto.conta_id, ccc_pref_contabilizacao, ccc_planoContasVigente, ccc_pref_novaCategoria, ccc_pref_editCategoria, ccc_pref_deleteCategoria);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["retornoConfig"] = "Erro! - Houve uma falha na gravação da informação. Tente novamenteo. Se persistir entre em contato como o suporte";

                return RedirectToAction(nameof(Index));
            }
        }


        [FiltroAutorizacao(permissao = "configClienteEdit")]
        public ActionResult Edit()
        {
            //Usuário logado / contexto conta selecionada pelo contador
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));           

            //Criar objeto de configurações para enviar para a view
            Config_contador_cliente config = new Config_contador_cliente();
            vm_ConfigContadorCliente ccc = new vm_ConfigContadorCliente();
            ccc = config.buscaCCC(user.usuario_id, contexto.conta_id, user.usuario_conta_id);

            //Verificação provisória
            //Verificar se existe lançamentos em aberto no plano de contas vigente a atribuir ao objeto configurações 's' ou 'n'
            if (ccc.ccc_planoContasVigente == "0")
            {
                ccc.ccc_liberaPlano = "Sim";
            }
            else
            {
                //Pesquisa lançamentos e atribui sim, se retorno for não há lançamentos sem encerramnto para o plano. Do contrário não;
                ccc.ccc_liberaPlano = "Sim"; //Por enquanto não existe lançamento, então vai ficar padrão 'sim'.
            }

            Selects select = new Selects();

            if (ccc.ccc_id == 0)
            {
                ViewBag.planosContador = select.getPlanosContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.text == "Selecione um plano de contas" }).ToList();
            }
            else
            {
                ViewBag.planosContador = select.getPlanosContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == ccc.ccc_planoContasVigente.ToString() }).ToList();
            }

            return View(ccc);
        }

        // POST: ConfigContadorClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(bool ccc_pref_contabilizacao, string ccc_planoContasVigente, bool ccc_pref_novaCategoria, bool ccc_pref_editCategoria, bool ccc_pref_deleteCategoria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (ccc_planoContasVigente == null)
                {
                    ccc_planoContasVigente = "0";
                }

                var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
                Conta contexto = new Conta();
                contexto = contexto.contextoCliente(Convert.ToInt32(HttpContext.Session.GetInt32("cliente_selecionado")));

                Config_contador_cliente config = new Config_contador_cliente();

                TempData["retornoConfig"] = config.alterarConfiguracoes(user.usuario_conta_id, user.usuario_id, user.usuario_conta_id, contexto.conta_id, ccc_pref_contabilizacao, ccc_planoContasVigente, ccc_pref_novaCategoria, ccc_pref_editCategoria, ccc_pref_deleteCategoria);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["retornoConfig"] = "Erro! - Houve uma falha na gravação da informação. Tente novamenteo. Se persistir entre em contato como o suporte";

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
