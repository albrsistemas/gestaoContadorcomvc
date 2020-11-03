using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_contabilizacao
    {
        public int opc_id { get; set; }
        public int opc_op_id { get; set; }
        public int opc_cliente_id { get; set; }
        public int opc_contador_id { get; set; }
        public string opc_status { get; set; }
    }
}
