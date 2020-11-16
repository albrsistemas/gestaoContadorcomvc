using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_contas_pagar
    {
        public IEnumerable<ContasPagar> contasPagar { get; set; }        
        public Vm_usuario user { get; set; }
    }
}
