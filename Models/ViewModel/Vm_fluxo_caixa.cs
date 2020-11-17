using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_fluxo_caixa
    {
        public Decimal saldo_abertura { get; set; }
        public Decimal saldo_movimentos { get; set; }        
        public IEnumerable<Fluxo_caixa> fluxo { get; set; }
        public Vm_usuario user { get; set; }
    }
}
