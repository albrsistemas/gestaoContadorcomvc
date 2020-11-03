﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_parcelas
    {
        public int op_parcela_id { get; set; }
        public int op_parcela_dias { get; set; }
        public DateTime op_parcela_vencimento { get; set; }
        public int op_parcela_fp_id { get; set; }
        public int op_parcela_op_id { get; set; }
        public Decimal op_parcela_valor { get; set; }
        public Decimal op_parcela_saldo { get; set; }
        public string op_parcela_obs { get; set; }
    }
}
