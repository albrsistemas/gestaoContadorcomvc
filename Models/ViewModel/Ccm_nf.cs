using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Ccm_nf
    {
        public bool ccm_nf { get; set; }
        public int ccm_nf_id { get; set; }
        public DateTime ccm_nf_data_emissao { get; set; }
        public Decimal ccm_nf_valor { get; set; }
        public string ccm_nf_serie { get; set; }
        public string ccm_nf_numero { get; set; }
        public string ccm_nf_chave { get; set; }
        public int ccm_nf_ccm_id { get; set; }
        public int ccm_nf_conta_id { get; set; }

    }
}
