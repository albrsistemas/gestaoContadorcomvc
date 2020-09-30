using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    public class ContaContabil
    {
        public int ccontabil_id { get; set; }
        public int ccontabil_plano_id { get; set; }
        public string ccontabil_classificacao { get; set; }
        public string ccontabil_nome { get; set; }
        public string ccontabil_nivel { get; set; }
        public string ccontabil_grupo { get; set; }
        public string ccontabil_tipo { get; set; }
        public DateTime ccontabil_dataCriacao { get; set; }
        public DateTime ccontabil_dataInativacao { get; set; }
    }
}
