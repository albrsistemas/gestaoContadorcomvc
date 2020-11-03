using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_itens
    {
        public int op_item_id { get; set; }
        public string op_item_codigo { get; set; }
        public string op_item_nome { get; set; }
        public string op_item_unidade { get; set; }
        public Decimal op_item_preco { get; set; }
        public string op_item_gtin_ean { get; set; }
        public string op_item_gtin_ean_trib { get; set; }
        public string op_item_obs { get; set; }
        public Decimal op_item_qtd { get; set; }
        public Decimal op_item_frete { get; set; }
        public Decimal op_item_seguros { get; set; }
        public Decimal op_item_desp_aces { get; set; }
        public Decimal op_item_desconto { get; set; }
        public int op_item_op_id { get; set; }
        public Decimal op_item_vlr_ipi { get; set; }
        public Decimal op_item_vlr_icms_st { get; set; }
        public string op_item_cod_fornecedor { get; set; } //Código do produto no fornecedor
    }
}
