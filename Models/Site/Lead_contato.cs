using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site
{
    public class Lead_contato
    {
        public int lead_contato_id { get; set; }
        public DateTime lead_contato_data_hora { get; set; }
        public int lead_contato_lead_id { get; set; }
        public string lead_contato_tipo { get; set; }
        public string lead_contato_msg { get; set; }        
    }
}
