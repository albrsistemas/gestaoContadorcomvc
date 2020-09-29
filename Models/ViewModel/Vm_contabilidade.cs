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
        public int cc_conta_id { get; set; }
        public int cc_conta_id_contador { get; set; }
        public string cc_dctoContador { get; set; }
        public string cc_nomeContador { get; set; }
        public string cc_termo { get; set; }
        public DateTime cc_dataVinculacao { get; set; }
        public DateTime cc_dataDesvinculacao { get; set; }
        public Usuario usuario { get; set; }                
        public Conta conta { get; set; }


    }
}
