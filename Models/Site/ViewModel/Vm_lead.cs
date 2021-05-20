using gestaoContadorcomvc.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site.ViewModel
{
    public class Vm_lead
    {
        public string status { get; set; }
        public IEnumerable<Lead> leads { get; set; }
        public Vm_usuario user { get; set; }
    }
}
