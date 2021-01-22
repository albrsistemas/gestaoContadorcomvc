using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_operacao_index
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public DateTime data { get; set; }
        public string participante { get; set; }
        public string categoria { get; set; }
        public string memorando { get; set; }
        public string documento { get; set; }
        public Decimal valor { get; set; }
        public IEnumerable<Vm_operacao_index> lista { get; set; }
        public Vm_usuario user { get; set; }

    }
}
