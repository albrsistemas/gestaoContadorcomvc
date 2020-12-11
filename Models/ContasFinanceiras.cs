using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class ContasFinanceiras
    {
        public int cf_id { get; set; }
        public string cf_nome { get; set; }
        public int cf_categoria_id { get; set; }
        public Decimal cf_valor_operacao { get; set; }
        public Decimal cf_valor_parcela_bruta { get; set; }
        public Decimal cf_valor_parcela_liquida { get; set; }
        public string cf_recorrencia { get; set; }
        public DateTime cf_data_inicial { get; set; }
        public DateTime cf_data_final { get; set; }
        public string cf_escopo { get; set; }
        public string cf_tipo { get; set; }
        public string cf_status { get; set; }
        public int cf_conta_id { get; set; }
        public int cf_numero_parcelas { get; set; }
        public DateTime cf_dataCriacao { get; set; }
        public int cf_op_id { get; set; }


    }
}
