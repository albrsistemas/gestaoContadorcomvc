using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel
{
    public class vm_categoria_contaonline
    {
        public int cco_id { get; set; }
        public int cco_cliente_conta_id { get; set; }
        public int cco_contador_conta_id { get; set; }
        public int cco_plano_id { get; set; }

        [Display(Name = "Conta Contábil")]
        [Required(ErrorMessage = "A conta contábil é obrigatória.")]
        public int cco_ccontabil_id { get; set; }
        public int cco_categoria_id { get; set; }
        public string categoria_nome { get; set; }
    }
}
