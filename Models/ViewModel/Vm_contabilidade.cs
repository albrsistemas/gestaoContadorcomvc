using gestaoContadorcomvc.Models.Autenticacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_contabilidade
    {
        public int cc_id { get; set; }
        public string cc_cnpj { get; set; }
        public string cc_nome { get; set; }
        public string cc_termo { get; set; }
        public DateTime cc_dataVinculacao { get; set; }
        public DateTime cc_dataDesvinculacao { get; set; }
        public string cc_nomeUsuario { get; set; }
        public string cc_dctoUsuario { get; set; }
        public int cc_conta_id { get; set; }

        public Conta conta { get; set; }


    }
}
