using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_ccm
    {
        public int ccm_id { get; set; }
        public int ccm_conta_id { get; set; }
        public int ccm_ccorrente_id { get; set; }
        public string ccm_movimento { get; set; }
        public string ccm_contra_partida_tipo { get; set; }
        public int ccm_contra_partida_id { get; set; }
        public DateTime ccm_data { get; set; }
        public DateTime ccm_data_competencia { get; set; }
        public DateTime ccm_dataCriacao { get; set; }        
        public Decimal ccm_valor { get; set; }
        public int ccm_op_id { get; set; }
        public int ccm_oppb_id { get; set; }
        public string ccm_memorando { get; set; }
        public string ccm_origem { get; set; }
        public int ccm_participante_id { get; set; }
        public int categoria_id { get; set; }
        public string participante_nome { get; set; }
        public Ccm_nf nf { get; set; }
        public filtro filtro { get; set; }

    }

    public class filtro
    {
        public int contacorrente_id { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }
    }
}
