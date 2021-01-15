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
    public class ServicoPrestadoController : Controller
    {
        [Autoriza(permissao = "servicoPList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Operacao operacao = new Operacao();
            Vm_operacao op = new Vm_operacao();
            op.operacoes = operacao.listaOperacao(user.usuario_id, user.usuario_conta_id, "ServicoPrestado");
            op.user = user;

            return View(op);
        }

        [Autoriza(permissao = "servicoPCreate")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Selects select = new Selects();
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, true, false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.modFrete = select.getModFrete().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.statusServico = select.getStatusServico().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });

            return View();
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Vm_operacao op)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            string retorno = "";

            try
            {
                op.operacao.op_tipo = "ServicoPrestado";
                Operacao operacao = new Operacao();
                retorno = operacao.cadastraOperacao(user.usuario_id, user.usuario_conta_id, op);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [Autoriza(permissao = "servicoPEdit")]
        public ActionResult Details(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Operacao op = new Operacao();
            Vm_operacao vm_op = new Vm_operacao();

            vm_op = op.buscaOperacao(user.usuario_id, user.usuario_conta_id, id);
            vm_op.user = user;

            Selects select = new Selects();
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_part_tipo });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_uf_ibge_codigo.ToString() });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_paisesIBGE_codigo.ToString() });
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true,true,false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.operacao.op_categoria_id.ToString() });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.modFrete = select.getModFrete().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.transportador.op_transportador_modalidade_frete });
            ViewBag.statusServico = select.getStatusServico().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.servico.op_servico_status });

            return View(vm_op);
        }

        [Autoriza(permissao = "servicoPEdit")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Operacao op = new Operacao();
            Vm_operacao vm_op = new Vm_operacao();

            vm_op = op.buscaOperacao(user.usuario_id, user.usuario_conta_id, id);
            vm_op.user = user;


            Selects select = new Selects();
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_part_tipo });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_uf_ibge_codigo.ToString() });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.participante.op_paisesIBGE_codigo.ToString() });
            ViewBag.categoria = select.getCategoriasClienteComEscopo(user.usuario_conta_id, true, true, false).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.operacao.op_categoria_id.ToString() });
            ViewBag.formaPgto = select.getFormaPgto(user.usuario_conta_id, "Recebimento").Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.modFrete = select.getModFrete().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.transportador.op_transportador_modalidade_frete });
            ViewBag.statusServico = select.getStatusServico().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_op.servico.op_servico_status });

            if (vm_op.operacao.possui_parcelas)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return View(vm_op);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Vm_operacao op)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
                
                Operacao operacao = new Operacao();
                retorno = operacao.alterarOperacao(user.usuario_id, user.usuario_conta_id, op);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                retorno = "Erro. Ocorreu uma falha na alteração da operação!";

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

    }
}
