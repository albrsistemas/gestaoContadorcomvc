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
        //Campos de controle
        public IEnumerable<gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel.vm_lancamento> lancamentos { get; set; }
        public Vm_usuario user { get; set; }
        public vm_ConfigContadorCliente vm_config { get; set; }

        //Campos do formulário
        [Display(Name = "Débito")]
        [Required(ErrorMessage = "É obrigatório uma conta débito")]
        public int lancamento_debito_conta_id { get; set; }

        [Display(Name = "Crédito")]
        [Required(ErrorMessage = "É obrigatório uma conta crédito")]
        public int lancamento_credito_conta_id { get; set; }

        [Display(Name = "Data")]
        [Required(ErrorMessage = "A data é obrigatória")]        
        public DateTime lancamento_data { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "É obrigatório informar o valor do lançamento")]        
        public Decimal lancamento_valor { get; set; }

        [Display(Name = "Histórico")]
        [Required(ErrorMessage = "É obrigatório informar o histótico")]
        [MinLength(3, ErrorMessage = "Mínimo de 3 caracteres no histórico")]
        [MaxLength(80, ErrorMessage = "Máximo de 80 caracteres no histórico")]
        public string lancamento_historico { get; set; }

        [Display(Name = "Participante Débito")]
        public int lancamento_participante_debito { get; set; }

        [Display(Name = "Participante Crédito")]
        public int lancamento_participante_credito { get; set; }

        public int lancamento_cliente_id { get; set; }
        public int lancamento_contador_id { get; set; }
        public string lancamento_debito_classificacao { get; set; }
        public string lancamento_credito_classificacao { get; set; }
        public string lancamento_debito_nome { get; set; }
        public string lancamento_credito_nome { get; set; }
    }
}
