using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_forma_pagamento
    {
        public int fp_id { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string fp_nome { get; set; }

        [Display(Name = "Meio de Pagamento")]
        public string fp_meio_pgto_nfe { get; set; }
        
        public bool fp_baixa_automatica { get; set; }

        [Display(Name = "Conta Corrente Vinculada")]
        public string fp_vinc_conta_corrente { get; set; }

        [Display(Name = "Tipo")]
        public string fp_identificacao { get; set; }

        [Display(Name = "Integração da maquinhinha de cartão")]
        public string fp_tipo_integracao_nfe { get; set; }

        [Display(Name = "Bandeira da maquinhinha de cartão")]
        public string fp_bandeira_cartao { get; set; }

        [Display(Name = "CNPJ operadora da maquinhinha de cartão")]
        public string fp_cnpj_credenciadora_cartao { get; set; }
        public int fp_conta_id { get; set; }
        public DateTime fp_dataCriacao { get; set; }

        [Display(Name = "Status")]
        public string fp_status { get; set; }

        [Display(Name = "Dia de fechamento fatura cartão")]
        [Required(ErrorMessage = "O dia de fechamento é obrigatório")]
        [Range(1,28, ErrorMessage = "Informe um dia no intervalo de 1 a 28")]
        public int fp_dia_fechamento_cartao { get; set; }
                
        [Display(Name = "Dia de vencimento fatura cartão")]
        [Required(ErrorMessage = "O dia de vencimento é obrigatório")]
        [Range(1, 28, ErrorMessage = "Informe um dia no intervalo de 1 a 28")]
        public int fp_dia_vencimento_cartao { get; set; }
        //Atributos de controle
        public IEnumerable<Vm_forma_pagamento> formasPagamento { get; set; }
        public Vm_usuario user { get; set; }
        public string destino { get; set; }
        public string aplicavel { get; set; }
        public string meioPgto { get; set; }
    }
}
