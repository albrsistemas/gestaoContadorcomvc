using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_contas_receber
    {
        public cp_filterCR filter { get; set; }
        public IEnumerable<ContasReceber> contasReceber { get; set; }
        public Vm_usuario user { get; set; }
    }
}
