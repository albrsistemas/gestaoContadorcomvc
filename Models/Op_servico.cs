using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_servico
    {
        public int op_servico_id { get; set; }
        public int op_servico_op_id { get; set; }
        public string op_servico_equipamento { get; set; }

        [StringLength(30,ErrorMessage = "Máximo de 30 caracteres")]
        public string op_servico_nSerie { get; set; }
        public string op_servico_problema { get; set; }
        public string op_servico_obsReceb { get; set; }
        public string op_servico_servico_executado { get; set; }
        public Decimal op_servico_valor { get; set; }
        public string op_servico_status { get; set; }
        public string op_servico_lc116 { get; set; }
    }
}
