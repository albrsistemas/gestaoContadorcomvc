using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    public class ParcelamentoFaturaCartaoCreditoController : Controller
    {
        [Autoriza(permissao = "cartaoCreditoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(int pfcc_fcc_id, Decimal pfcc_total_fatura, Decimal pfcc_valor_parcelado, int pfcc_numero_parcelas, Decimal pfcc_valor_parcela, Decimal pfcc_juros, int pfcc_categoria_id, DateTime pfcc_data_parcelamento, string[] competencias, string competencia, int fcc_forma_pagamento_id)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                ParcelamentoFaturaCartaoCredito p = new ParcelamentoFaturaCartaoCredito();

                retorno = p.criarParcelamento(user.conta.conta_id, user.usuario_id, pfcc_fcc_id, pfcc_total_fatura, pfcc_valor_parcelado, pfcc_numero_parcelas, pfcc_valor_parcela, pfcc_juros, pfcc_categoria_id, pfcc_data_parcelamento, competencias, competencia, fcc_forma_pagamento_id);
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao processar o parcelamento da fatura do cartão de crédito.";
                }
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }

        [Autoriza(permissao = "cartaoCreditoEdit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int pfcc_id)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                ParcelamentoFaturaCartaoCredito p = new ParcelamentoFaturaCartaoCredito();

                retorno = p.excluirParcelamento(user.conta.conta_id, pfcc_id);
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao processar a exclusão do parcelamento da fatura do cartão de crédito.";
                }
            }

            return Json(JsonConvert.SerializeObject(retorno));
        }
    }
}
