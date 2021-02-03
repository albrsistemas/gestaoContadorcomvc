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
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class OperacaoController : Controller
    {
        [Autoriza(permissao = "operacaoList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Operacao op = new Operacao();
            Vm_operacao_index vm_op = new Vm_operacao_index();
            vm_op.lista = op.index(user.usuario_conta_id);
            vm_op.user = user;

            Selects select = new Selects();
            //ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });            
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, false, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });
            ViewBag.operacoes = select.getOperacoes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "OutrasDespesas" });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Pagamento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(vm_op);
        }

        // GET: OperacaoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Autoriza(permissao = "operacaoCreate")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Selects select = new Selects();
            //ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });            
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id,true,false,true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });
            ViewBag.operacoes = select.getOperacoes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "OutrasDespesas" });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id,"Pagamento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View();
        }

        // POST: OperacaoController/Create
        [HttpPost]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Vm_operacao op)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Operacao operacao = new Operacao();
                retorno = operacao.create(user.usuario_id, user.usuario_conta_id, op);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao cadastrar a operação. Tente novamente, se persistir, entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [Autoriza(permissao = "operacaoEdit")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Operacao op = new Operacao();
            Vm_operacao vm_op = new Vm_operacao();

            vm_op = op.buscaOperacao(user.usuario_id, user.usuario_conta_id, id);            

            Selects select = new Selects();
            //ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });            
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, false, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.operacao.op_categoria_id.ToString() });
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_part_tipo });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_uf_ibge_codigo.ToString() });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_paisesIBGE_codigo.ToString() });
            ViewBag.operacoes = select.getOperacoes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.text == vm_op.operacao.op_tipo });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Pagamento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.nf.op_nf_tipo.ToString() });

            return Json(JsonConvert.SerializeObject(vm_op));
        }

        // POST: OperacaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, Vm_operacao op)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Operacao operacao = new Operacao();

                if (operacao.baixasPorOperacao(id) > 0)
                {
                    retorno = "Erro. Operação possui baixas e não pode ser alterada. Primeiro deve-se excluir as baixas no movimento de conta corrente!";
                }
                else
                {
                    retorno = operacao.alterarOperacao(user.usuario_id, user.usuario_conta_id, op);
                }                

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao cadastrar a operação. Tente novamente, se persistir, entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [Autoriza(permissao = "operacaoDelete")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Operacao op = new Operacao();

            if (op.baixasPorOperacao(id) > 0)
            {
                TempData["msgOP"] = "Erro. Operação possui baixas e não pode ser excluído. Primeiro deve-se excluir as baixas no movimento de conta corrente!";

                return RedirectToAction(nameof(Index));
            }
            else
            {   
                Vm_operacao vm_op = new Vm_operacao();
                vm_op = op.buscaOperacao(user.usuario_id, user.usuario_conta_id, id);
                
                return View(vm_op);
            }
        }

        // POST: OperacaoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int op_id, IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Operacao op = new Operacao();

            string retorno = "";

            try
            {
                retorno = op.delete(user.usuario_id, user.usuario_conta_id, op_id);

                TempData["msgOP"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Ocorreu um erro no processamento da solicitação de exclusão!";
                }

                TempData["msgOP"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GerarFormaPgto(string op_tipo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            string identificacao = "";
            if(op_tipo == "Compra" || op_tipo == "ServicoTomado" || op_tipo == "OutrasDespesas")
            {
                identificacao = "Pagamento";
            }
            if (op_tipo == "Venda" || op_tipo == "ServicoPrestado" || op_tipo == "OutrasReceitas")
            {
                identificacao = "Recebimento";
            }

            Selects select = new Selects();
            List<Selects> lista = new List<Selects>();
            lista = select.getFormaPgto(user.usuario_conta_id, identificacao);

            return Json(JsonConvert.SerializeObject(lista));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GerarCategorias(string op_tipo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            bool entradas = false;
            bool saidas = false;
            if (op_tipo == "Compra" || op_tipo == "ServicoTomado" || op_tipo == "OutrasDespesas")
            {
                saidas = true;
            }
            if (op_tipo == "Venda" || op_tipo == "ServicoPrestado" || op_tipo == "OutrasReceitas")
            {
                entradas = true;
            }

            Selects select = new Selects();
            List<Selects> lista = new List<Selects>();
            lista = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, entradas, saidas);

            return Json(JsonConvert.SerializeObject(lista));
        }
    }
}
