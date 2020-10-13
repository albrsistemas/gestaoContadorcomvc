using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel
{
    public class vm_ConfigContadorCliente
    {
        public int ccc_id { get; set; }
        public int ccc_contador_id { get; set; }
        public int ccc_cliente_id { get; set; }

        [Display(Name = "Plano de Contas")]
        //[BindRequired]
        [Required(ErrorMessage = "Informe um plano de contas")]        
        public string ccc_planoContasVigente { get; set; }
        public bool ccc_pref_contabilizacao { get; set; }
        public bool ccc_pref_novaCategoria { get; set; }
        public bool ccc_pref_editCategoria { get; set; }
        public bool ccc_pref_deleteCategoria { get; set; }
        public string ccc_liberaPlano { get; set; } //Atributa recebe sim, se não houver lançamentos sem encerramento para o plano vigente. A pesquisa é feita no controller.
        public IEnumerable<gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel.vm_ConfigContadorCliente> configuracoes { get; set; }
        public Vm_usuario user { get; set; }
    }
}
