﻿using System;
using System.Linq;
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
    public class BaixaController : Controller
    {

        [Autoriza(permissao = "baixaCreate")]
        public ActionResult Create(int parcela_id, string contexto)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();
            Op_parcelas_baixa b = new Op_parcelas_baixa();
            vm_baixa = b.buscaDados_para_Baixa(user.usuario_conta_id, user.usuario_id, parcela_id);

            Selects select = new Selects();
            ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            DateTime today = DateTime.Today;
            vm_baixa.data = today;
            vm_baixa.contexto = contexto;

            return View(vm_baixa);
        }

        // POST: BaixaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Decimal valor, Decimal juros, Decimal multa, Decimal desconto, string obs, int contacorrente_id, DateTime data, int parcela_id, string contexto)
        {
            string retorno = "";
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();
                Op_parcelas_baixa b = new Op_parcelas_baixa();

                retorno = b.cadastrarBaixa(user.usuario_id, user.usuario_conta_id, parcela_id, contacorrente_id, data, valor, obs, juros, multa, desconto);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao gravar a baixa. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [Autoriza(permissao = "baixaEdit")]
        public ActionResult Edit(int baixa_id, string local, DateTime dataInicio, DateTime dataFim, int contacorrente_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();
            Op_parcelas_baixa b = new Op_parcelas_baixa();

            vm_baixa = b.buscaBaixa(user.usuario_conta_id, user.usuario_id, baixa_id);

            Selects select = new Selects();
            ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_baixa.contaCorrente.ToString() });

            TempData["local"] = local;
            TempData["dataInicio"] = dataInicio.ToShortDateString();
            TempData["dataFim"] = dataFim.ToShortDateString();
            TempData["contacorrente_id"] = contacorrente_id;

            vm_baixa.user = user;

            return View(vm_baixa);
        }

        // POST: BaixaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection, int baixa_id, Decimal valor, Decimal juros, Decimal multa, Decimal desconto, string obs, int contacorrente_id, DateTime data, int parcela_id, string contexto)
        {
            string retorno = "";
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();
                Op_parcelas_baixa b = new Op_parcelas_baixa();

                retorno = b.alterarBaixa(user.usuario_id, user.usuario_conta_id, baixa_id, contacorrente_id, data, valor, obs, juros, multa, desconto);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao gravar a alteração da baixa. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }
       

        // POST: BaixaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int baixa_id)
        {
            string retorno = "";
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();
                Op_parcelas_baixa b = new Op_parcelas_baixa();

                retorno = b.excluirBaixa(user.usuario_id, user.usuario_conta_id, baixa_id);

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao realizar a exclusão da baixa. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }
    }
}
