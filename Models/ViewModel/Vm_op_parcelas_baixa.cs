using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_op_parcelas_baixa
    {
        public int baixa_id { get; set; }
        public int parcela_id { get; set; }
        public string referencia { get; set; }
        public DateTime vencimento { get; set; }
        public Decimal valor_parcela_original { get; set; }
        public Decimal saldo_parcela { get; set; }
        public DateTime data { get; set; }
        public Decimal valor { get; set; }
        public Decimal juros { get; set; }
        public Decimal multa { get; set; }
        public Decimal desconto { get; set; }
        public string obs { get; set; }
        public int contaCorrente { get; set; }                       
    }
}
