using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Conta_corrente_mov
    {
        public int ccm_id { get; set; }
        public int ccm_conta_id { get; set; }
        public int ccm_ccorrente_id { get; set; }
        public string ccm_movimento { get; set; }
        public string ccm_contra_partida_tipo { get; set; }
        public int ccm_contra_partida_id { get; set; }
        public DateTime ccm_data { get; set; }
        public DateTime ccm_dataCriacao { get; set; }
        public Decimal ccm_valor { get; set; }
    }
}
