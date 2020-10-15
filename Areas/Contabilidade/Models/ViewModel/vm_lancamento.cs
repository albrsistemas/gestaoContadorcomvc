using gestaoContadorcomvc.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel
{
    public class vm_lancamento
    {
        //Lançamento do débito
        public int ld_id { get; set; }
        public string ld_origem { get; set; }
        public string ld_tipo { get; set; }
        public DateTime ld_data { get; set; }
        public Decimal ld_valor { get; set; }
        public int ld_contraPartida { get; set; }
        public int ld_cliente_id { get; set; }
        public int ld_contador_id { get; set; }
        public DateTime ld_dataCriacao { get; set; }
        public int ld_ccontabil { get; set; }
        public string ld_historico { get; set; }
        //Lançamento do crédito
        public int lc_id { get; set; }
        public string lc_origem { get; set; }
        public string lc_tipo { get; set; }
        public DateTime lc_data { get; set; }
        public Decimal lc_valor { get; set; }
        public int lc_contraPartida { get; set; }
        public int lc_cliente_id { get; set; }
        public int lc_contador_id { get; set; }
        public DateTime lc_dataCriacao { get; set; }
        public int lc_ccontabil { get; set; }
        public string lc_historico { get; set; }
        //Campos de controle
        public IEnumerable<gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel.vm_lancamento> MyProperty { get; set; }
        public Vm_usuario user { get; set; }

        //Campos do formulário
        [Display(Name = "Data")]
        [Required(ErrorMessage = "A data é obrigatória")]
        public DateTime lancamento_data { get; set; }

        [Display(Name = "Débito")]
        [Required(ErrorMessage = "É obrigatório um conta débito")]
        public string lancamento_debito { get; set; }

    }
}
