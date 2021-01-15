using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_totais
    {
        public int op_totais_id { get; set; }        
        public Decimal op_totais_preco_itens { get; set; } //Colocar direto na view

        [Display(Name = "Frete")]
        public Decimal op_totais_frete { get; set; }

        [Display(Name = "Seguros")]
        public Decimal op_totais_seguro { get; set; }

        [Display(Name = "Despesas Acessórias")]
        public Decimal op_totais_desp_aces { get; set; }

        [Display(Name = "Desconto")]
        public Decimal op_totais_desconto { get; set; }                
        public int op_totais_itens { get; set; }

        [Display(Name = "Quantidade total de itens")]
        public Decimal op_totais_qtd_itens { get; set; }
        public int op_totais_op_id { get; set; }
        public Decimal op_totais_retencoes { get; set; }
        public Decimal op_totais_total_op { get; set; } //Colocar direto na view
        public Decimal op_totais_ipi { get; set; }
        public Decimal op_totais_icms_st { get; set; }
        public Decimal op_totais_saldoLiquidacao { get; set; }
        public Decimal op_totais_preco_servicos { get; set; }
    }
}
