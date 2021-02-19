using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_ajuste_parcelas_operacao
    {
        public List<Op_parcelas> parcelas { get; set; }
        public Operacao operacao { get; set; }
        public Op_totais operacao_totais { get; set; }        
    }
}
