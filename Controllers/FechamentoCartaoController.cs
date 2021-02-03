using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class FechamentoCartaoController : Controller
    {   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fechamento_cartao fc)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            string retorno = "";

            try
            {
                Fechamento_cartao fc_existe = new Fechamento_cartao();

                fc_existe = fc_existe.buscaPorRef(fc.fc_forma_pagamento,fc.fc_referencia);

                if(fc_existe == null)
                {
                    //cadastrar nova fatura
                    retorno = fc.cadastraFechamentoCartao(user.usuario_conta_id, user.usuario_id, fc,false,0,0);
                }
                else
                {
                    //somar fechamento a fatura anterior fechada
                    Fechamento_cartao fc_atualizada = new Fechamento_cartao();
                    fc_atualizada = fc_existe;
                    fc_atualizada.fc_qtd_parcelas = fc_atualizada.fc_qtd_parcelas + fc.fc_qtd_parcelas;
                    fc_atualizada.fc_valor_total = fc_atualizada.fc_valor_total + fc.fc_valor_total;
                    fc_atualizada.fc_seguro_cartao = fc_atualizada.fc_seguro_cartao + fc.fc_seguro_cartao;
                    fc_atualizada.fc_abatimentos_cartao = fc_atualizada.fc_abatimentos_cartao + fc.fc_abatimentos_cartao;
                    fc_atualizada.fc_acrescimos_cartao = fc_atualizada.fc_acrescimos_cartao + fc.fc_acrescimos_cartao;
                    fc_atualizada.fc_tarifas_bancarias = fc_atualizada.fc_tarifas_bancarias + fc.fc_tarifas_bancarias;
                    fc_atualizada.fc_vencimento = fc.fc_vencimento;
                    fc_atualizada.fc_matriz_parcelas_text += ", " + fc.fc_matriz_parcelas_text;
                    fc_atualizada.fc_forma_pgto_boleto_fatura = fc.fc_forma_pgto_boleto_fatura;

                    retorno = fc.cadastraFechamentoCartao(user.usuario_conta_id, user.usuario_id, fc_atualizada, true, fc_atualizada.fc_op_id, fc_atualizada.fc_id);
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao gravar a fatura do cartão. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult fc_existe(int fc_forma_pagamento, string fc_referencia)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            string retorno = "";

            try
            {
                Fechamento_cartao fc_existe = new Fechamento_cartao();
                
                fc_existe = fc_existe.buscaPorRef(fc_forma_pagamento, fc_referencia);

                return Json(JsonConvert.SerializeObject(fc_existe));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao consultar cartão";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }



    }
}
