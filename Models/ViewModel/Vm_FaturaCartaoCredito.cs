using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_FaturaCartaoCredito
    {
        public int fcc_id { get; set; }
        public int fcc_conta_id { get; set; }
        public int fcc_forma_pagamento_id { get; set; }
        public string fcc_situacao { get; set; }
        public string fcc_competencia { get; set; }
        public DateTime fcc_data_corte { get; set; }
        public DateTime fcc_data_vencimento { get; set; }
        public List<MovimentosCartaoCredito> fcc_movimentos { get; set; }
        public ParcelamentoFaturaCartaoCredito parcelamento { get; set; }
        public string fcc_nome_cartao { get; set; }
        public Vm_usuario user { get; set; }
    }
}
