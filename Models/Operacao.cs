using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Operacao
    {
        public int op_id { get; set; }
        public string op_tipo { get; set; }
        public DateTime op_dataCriacao { get; set; }        
        public DateTime op_data { get; set; }
        public DateTime op_previsao_entrega { get; set; } //Previsão da entrega da venda, da compra, do serviço prestado...
        public DateTime op_data_saida { get; set; } //Data de saída no caso de operação de venda
        public int op_conta_id { get; set; }
        public string op_obs { get; set; }
        public int op_numero_ordem { get; set; }
        public int op_categoria_id { get; set; }
    }
}
