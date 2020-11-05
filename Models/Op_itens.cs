using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_itens
    {
        public int op_item_id { get; set; }

        [Display(Name = "Código Empresa")]
        public string op_item_codigo { get; set; }

        [Display(Name = "Nome")]
        public string op_item_nome { get; set; }

        [Display(Name = "Unidade")]
        public string op_item_unidade { get; set; }

        [Display(Name = "Preço")]
        public Decimal op_item_preco { get; set; }

        [Display(Name = "GTIN")]
        public string op_item_gtin_ean { get; set; }

        public string op_item_gtin_ean_trib { get; set; }

        [Display(Name = "Observações")]
        public string op_item_obs { get; set; }

        [Display(Name = "Quantidade")]
        public Decimal op_item_qtd { get; set; }

        [Display(Name = "Frete")]
        public Decimal op_item_frete { get; set; }

        [Display(Name = "Seguros")]
        public Decimal op_item_seguros { get; set; }

        [Display(Name = "Desp. Acessória")]
        public Decimal op_item_desp_aces { get; set; }

        [Display(Name = "Desconto")]
        public Decimal op_item_desconto { get; set; }
        public int op_item_op_id { get; set; }

        [Display(Name = "IPI")]
        public Decimal op_item_vlr_ipi { get; set; }

        [Display(Name = "ICMS_ST")]
        public Decimal op_item_vlr_icms_st { get; set; }

        [Display(Name = "Código do Fornecedor")]
        public string op_item_cod_fornecedor { get; set; } //Código do produto no fornecedor
        public string controleEdit { get; set; } //Campo criado para controle edição. Não consta no banco de dados.
    }
}
