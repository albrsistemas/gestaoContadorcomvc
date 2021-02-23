using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class MovimentosCartaoCredito
    {
        public int mcc_id { get; set; }
        public string mcc_tipo { get; set; }
        public int mcc_tipo_id { get; set; }
        public string mcc_movimento { get; set; }
        public DateTime mcc_data { get; set; }
        public string mcc_descricao { get; set; }
        public Decimal mcc_valor { get; set; }
        public int mcc_fcc_id { get; set; }        
    }
}
