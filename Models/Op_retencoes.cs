using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_retencoes
    {
        public int op_ret_id { get; set; }
        public Decimal op_ret_pis { get; set; }
        public Decimal op_ret_cofins { get; set; }
        public Decimal op_ret_csll { get; set; }
        public Decimal op_ret_irrf { get; set; }
        public Decimal op_ret_inss { get; set; }
        public Decimal op_ret_issqn { get; set; }
        public int op_ret_op_id { get; set; }
    }
}
