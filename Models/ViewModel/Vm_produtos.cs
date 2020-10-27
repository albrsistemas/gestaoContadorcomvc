using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_produtos
    {
        public int produtos_id { get; set; }

        [Display(Name = "Código")]
        public string produtos_codigo { get; set; }

        [Display(Name = "Nome")]
        public string produtos_nome { get; set; }

        [Display(Name = "Data Cadastro")]
        public DateTime produtos_dataCriacao { get; set; }

        [Display(Name = "Formato")]
        public string produtos_formato { get; set; }

        [Display(Name = "Status")]
        public string produtos_status { get; set; }

        [Display(Name = "Unidade")]
        public string produtos_unidade { get; set; }

        [Display(Name = "Preço Venda")]
        public Decimal produtos_preco_venda { get; set; }

        [Display(Name = "GTIN/EAN")]
        public string produtos_gtin_ean { get; set; }

        [Display(Name = "GTIN/EAN Tributário")]
        public string produtos_gtin_ean_trib { get; set; }

        [Display(Name = "Estoque Mínimo")]
        public Decimal produtos_estoque_min { get; set; }

        [Display(Name = "Estoque Máximo")]
        public Decimal produtos_estoque_max { get; set; }

        [Display(Name = "Saldo Inicial Estoque")]
        public Decimal produtos_estoque_qtd_inicial { get; set; }

        [Display(Name = "Preço Compra")]
        public Decimal produtos_estoque_preco_compra { get; set; }

        [Display(Name = "Custo Compra")]
        public Decimal produtos_estoque_custo_compra { get; set; }

        [Display(Name = "Observações")]
        public string produtos_obs { get; set; }

        [Display(Name = "Origem")]
        public int produtos_origem { get; set; }

        [Display(Name = "NCM")]
        public string produtos_ncm { get; set; }

        [Display(Name = "CEST")]
        public string produtos_cest { get; set; }

        [Display(Name = "Tipo Item")]
        public string produtos_tipo_item { get; set; }

        [Display(Name = "% Tributos")]
        public Decimal produtos_perc_tributos { get; set; }

        public int produtos_conta_id { get; set; }

        //atributos de controle
        public IEnumerable<Vm_produtos> produtos { get; set; }
        public Vm_usuario user { get; set; }
    }
}
