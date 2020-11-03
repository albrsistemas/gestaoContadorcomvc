using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_totais
    {
        public int op_totais_id { get; set; }
        public Decimal op_totais_preco_itens { get; set; }
        public Decimal op_totais_frete { get; set; }
        public Decimal op_totais_seguro { get; set; }
        public Decimal op_totais_desp_aces { get; set; }
        public Decimal op_totais_desconto { get; set; }
        public int op_totais_itens { get; set; }
        public Decimal op_totais_qtd_itens { get; set; }
        public int op_totais_op_id { get; set; }
        public Decimal op_totais_retencoes { get; set; }
        public Decimal op_totais_total_op { get; set; }
        public Decimal op_totais_ipi { get; set; }
        public Decimal op_totais_icms_st { get; set; }
        public Decimal op_totais_saldoLiquidacao { get; set; }
    }
}
