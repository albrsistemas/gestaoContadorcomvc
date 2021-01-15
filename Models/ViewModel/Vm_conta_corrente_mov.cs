using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_conta_corrente_mov
    {
        public Decimal abertura { get; set; }
        public DateTime data { get; set; }
        public string categoria { get; set; }
        public string memorando { get; set; }
        public string participante { get; set; }
        public Decimal valor { get; set; }
        public Decimal saldo { get; set; }
        public IEnumerable<Vm_conta_corrente_mov> conta_corrente_movimento { get; set; }
        public Vm_usuario user { get; set; }

    }
}
