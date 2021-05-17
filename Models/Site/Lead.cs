using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site
{
    public class Lead
    {
        public int lead_id { get; set; }
        public string lead_nome { get; set; }
        public string lead_celular { get; set; }
        public string lead_email { get; set; }
        public int lead_conta_id { get; set; }
        public string lead_tipo { get; set; }
        public string lead_situacao { get; set; }
        public int lead_lead_atendentes_id { get; set; }
        public IEnumerable<Lead_contato> contatos { get; set; }
    }
}
