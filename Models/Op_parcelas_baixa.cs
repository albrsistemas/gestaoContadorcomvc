using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_parcelas_baixa
    {
        public int oppb_id { get; set; }
        public DateTime oppb_data { get; set; }
        public DateTime oppb_dataCriacao { get; set; }
        public Decimal oppb_valor { get; set; }
        public string oppb_obs { get; set; }
        public int oppb_op_parcela_id { get; set; }
        public int oppb_op_id { get; set; }
    }
}
