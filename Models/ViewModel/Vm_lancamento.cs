using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_lancamento
    {
        public int lancamento_id { get; set; }
        public DateTime lancamento_data { get; set; }
        public int lacamento_debito_id { get; set; }
        public string lancamento_debito_descricao { get; set; }
        public int lancamento_credito_id { get; set; }
        public string lancamento_credito_descricao { get; set; }
        public Decimal lancamento_valor { get; set; }        
        public string lancamento_historico { get; set; }
        public string lancamento_tipo { get; set; }
    }
}
