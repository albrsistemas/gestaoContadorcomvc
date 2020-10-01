using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel
{
    public class vm_ConfigContadorCliente
    {
        public int ccc_id { get; set; }
        public int ccc_contador_id { get; set; }
        public int ccc_cliente_id { get; set; }
        public int ccc_planoContasVigente { get; set; }
        public string ccc_pref_novaCategoria { get; set; }
        public string ccc_pref_editCategoria { get; set; }
        public string ccc_pref_deleteCategoria { get; set; }
        public string ccc_liberaPlano { get; set; } //Atributa recebe sim, se não houver lançamentos sem encerramento para o plano vigente. A pesquisa é feita no controller.
    }
}
