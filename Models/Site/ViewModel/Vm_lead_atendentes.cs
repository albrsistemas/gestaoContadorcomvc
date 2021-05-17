using gestaoContadorcomvc.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site.ViewModel
{
    public class Vm_lead_atendentes
    {
        public string status { get; set; }
        public IEnumerable<Lead_atendentes> atendentes { get; set; }
        public Vm_usuario user { get; set; }
    }
}
