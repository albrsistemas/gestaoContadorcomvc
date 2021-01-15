using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_detalhamento_parcela
    {
        public ContasPagar parcela { get; set; }
        public List<parcelas_relacionadas> parcelas_referenciadas { get; set; }        
        public List<baixas> baixas { get; set; }
        public Vm_usuario user { get; set; }
    }

    public class parcelas_relacionadas
    {
        public DateTime vencimento { get; set; }
        public string referencia { get; set; }
        public Decimal valor { get; set; }
    }

    public class baixas
    {
        public DateTime data { get; set; }
        public Decimal valor { get; set; }
        public Decimal acrescimos { get; set; }
        public Decimal descontos { get; set; }
        public string conta_corrente { get; set; }
    }
}
