using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_forma_pagamento
    {
        public int fp_id { get; set; }
        public string fp_nome { get; set; }
        public string fp_meio_pgto_nfe { get; set; }
        public bool fp_baixa_automatica { get; set; }
        public string fp_vinc_conta_corrente { get; set; }
        public string fp_identificacao { get; set; }
        public string fp_tipo_integracao_nfe { get; set; }
        public string fp_bandeira_cartao { get; set; }
        public string fp_cnpj_credenciadora_cartao { get; set; }
        public int fp_conta_id { get; set; }
        public DateTime fp_dataCriacao { get; set; }
        public string fp_status { get; set; }
        //Atributos de controle
        public IEnumerable<Vm_forma_pagamento> formasPagamento { get; set; }
        public Vm_usuario user { get; set; }
    }
}
