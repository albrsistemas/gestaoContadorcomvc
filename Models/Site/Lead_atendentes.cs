using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site
{
    public class Lead_atendentes
    {
        public int lead_atendentes_id { get; set; }
        public string lead_atendentes_nome { get; set; }
        public string lead_atendentes_celular { get; set; }
        public string lead_atendentes_email { get; set; }
        public int lead_atendentes_filaUm { get; set; }
        public int lead_atendentes_filaDois { get; set; }
        public int lead_atendentes_conta_id { get; set; }
    }
}
