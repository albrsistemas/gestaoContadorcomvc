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
            vm_op.user = user;

            //Filtro
            OperacaoFiltro filtro = new OperacaoFiltro();
            DateTime data = DateTime.Today;
            //DateTime com o primeiro dia do mês
            filtro.data_inicio = new DateTime(data.Year, data.Month, 1);
            //DateTime com o último dia do mês
            filtro.data_fim = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            vm_op.filtro = filtro;

            vm_op.lista = op.index(user.usuario_conta_id, filtro);


            Selects select = new Selects();
            //ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });            
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, false, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });
            ViewBag.operacoes = select.getOperacoes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "OutrasDespesas" });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Pagamento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.centroCusto = select.getCentroCusto(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.origem = select.getOrigemMercadoria().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.cstICMS = select.getCstICMS().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View(vm_op);
        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        public ActionResult Index(OperacaoFiltro filtro)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);            

            Operacao op = new Operacao();
            Vm_operacao_index vm_op = new Vm_operacao_index();
            vm_op.lista = op.index(user.usuario_conta_id, filtro);
            vm_op.user = user;
            vm_op.filtro = filtro;

            Selects select = new Selects();
            //ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Ativo" });            
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, false, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });
            ViewBag.operacoes = select.getOperacoes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "OutrasDespesas" });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Pagamento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.tipoNF = select.getTipoNF().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.centroCusto = select.getCentroCusto(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.origem = select.getOrigemMercadoria().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.cstICMS = select.getCstICMS().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

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
            ViewBag.centroCusto = select.getCentroCusto(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.origem = select.getOrigemMercadoria().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.cstICMS = select.getCstICMS().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
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

                if (operacao.verificaPossuiParcelasComFaturaFechada(op.parcelas, user.conta.conta_id))
                {
                    retorno = "Erro. Operação possui parcela(s) em que a competência recai em fatura de cartão de crédito com situação 'Fechada'. Para incluir a operação todas as parcelas no cartão de crédito precisa estar em fatura aberta.";

                    return Json(JsonConvert.SerializeObject(retorno));
                }

                int retorno_create_op = operacao.create(user.usuario_id, user.usuario_conta_id, op);

                if(retorno_create_op == 0)
                {
                    retorno = "Erro ao cadastrar a operação. Tente novamente. Se persistir, entre em contato com o suporte!";
                }

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
            ViewBag.centroCusto = select.getCentroCusto(user.conta.conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.origem = select.getOrigemMercadoria().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.cstICMS = select.getCstICMS().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            //Verificando se parcela possui baixas ou parcela em fatura fechada e retornando para usuário aviso que não poderá ser alterada a operação.
            if (op.baixasPorOperacao(id) > 0)
            {
                vm_op.mensagem = "Operação não pode ser alterada pois possui parcela(s) com baixa. Para alterar exclua as baixas.";
                
            }

            if (op.verificaFaturaCartaoFechada(id, user.conta.conta_id))
            {
                vm_op.mensagem += "Operação não pode ser alterada pois existem parcelas em cartão de crédito com fatura fechada. Para alterar reabra a fatura do cartão!";
            }

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

                if (operacao.baixasPorOperacao(op.operacao.op_id) > 0)
                {
                    retorno = "Erro. Operação possui baixas e não pode ser alterada. Primeiro deve-se excluir as baixas no movimento de conta corrente!";
                    
                    return Json(JsonConvert.SerializeObject(retorno));
                }

                if (operacao.verificaFaturaCartaoFechada(op.operacao.op_id, user.conta.conta_id))
                {
                    retorno = "Erro. Operação não pode ser alterada pois existem parcelas em cartão de crédito com fatura fechada. Para alterar reabra a fatura do cartão!";

                    return Json(JsonConvert.SerializeObject(retorno));
                }

                retorno = operacao.alterarOperacao(user.usuario_id, user.usuario_conta_id, op);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao alterar a operação. Tente novamente, se persistir, entre em contato com o suporte!";
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

            Vm_operacao vm_op = new Vm_operacao();

            Operacao op = new Operacao();

            if (op.baixasPorOperacao(id) > 0)
            {
                TempData["msgOP"] = "Erro. Operação possui baixas e não pode ser excluído. Primeiro deve-se excluir as baixas no movimento de conta corrente!";

                return View(vm_op);
            }

            if (op.verificaFaturaCartaoFechada(id, user.conta.conta_id))
            {
                TempData["msgOP"] = "Erro. Operação possui parcelas no cartão com fatura fechada. Para excluir deve-se abrir a fatura do cartão!";

                return View(vm_op);
            }

            
            vm_op = op.buscaOperacao(user.usuario_id, user.usuario_conta_id, id);

            return View(vm_op);
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
                if (op.baixasPorOperacao(op_id) > 0)
                {
                    TempData["msgOP"] = "Erro. Operação possui baixas e não pode ser excluído. Primeiro deve-se excluir as baixas no movimento de conta corrente!";

                    return RedirectToAction(nameof(Index));
                }

                if (op.verificaFaturaCartaoFechada(op_id, user.conta.conta_id))
                {
                    TempData["msgOP"] = "Erro. Operação possui parcelas no cartão com fatura fechada. Para excluir deve-se abrir a fatura do cartão!";

                    return RedirectToAction(nameof(Index));
                }

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

        //Ajustes na composição dos valores das parcelas da operação
        [Autoriza(permissao = "operacaoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjusteParcelasOperacao(int parcela_id, IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Operacao op = new Operacao();
            Vm_ajuste_parcelas_operacao vm = new Vm_ajuste_parcelas_operacao();
            vm = op.ajuste_Parcelas_Operacao(user.usuario_id, user.conta.conta_id, parcela_id);

            return Json(JsonConvert.SerializeObject(vm));
        }

        [Autoriza(permissao = "operacaoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjusteParcelasOperacaoGravar(Vm_ajuste_parcelas_operacao apo)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Operacao op = new Operacao();
            string retorno = "";
            try
            {
                retorno = op.ajuste_Parcelas_Operacao_gravar(apo, user.usuario_id, user.conta.conta_id);
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro no processamento da solicitação.";
                }
            }
            

            return Json(JsonConvert.SerializeObject(retorno));
        }

    }
}
