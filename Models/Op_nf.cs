using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_nf
    {
        public int op_nf_id { get; set; }
        public string op_nf_chave { get; set; }
        public DateTime op_nf_data_emissao { get; set; }
        public DateTime op_nf_data_entrada_saida { get; set; }
        public string op_nf_serie { get; set; }
        public string op_nf_numero { get; set; }
    }
}
