using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_contasFinanceiras
    {
        public Operacao op { get; set; }
        public ContasFinanceiras cf { get; set; }
        public Op_parcelas parcelas { get; set; }
        public Op_participante participante { get; set; }
        public Op_nf nf { get; set; }
    }
}
