using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_transferencia
    {
        public int ccm_id { get; set; }
        public DateTime data { get; set; }
        public Decimal valor { get; set; }
        public string memorando { get; set; }
        public int ccorrente_de { get; set; }
        public int ccorrente_para { get; set; }
    }
}
