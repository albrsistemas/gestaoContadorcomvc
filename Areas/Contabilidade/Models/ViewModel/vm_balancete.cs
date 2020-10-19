using gestaoContadorcomvc.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel
{
    public class vm_balancete
    {
        //Filtros do balancete
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "É obrigatório uma data inicial")]
        [DataType(DataType.Date, ErrorMessage = "preencha uma data válida")]
        public DateTime data_inicial { get; set; }

        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "É obrigatório uma data final")]
        [DataType(DataType.Date, ErrorMessage = "preencha uma data válida")]
        public DateTime data_final { get; set; }        
        //Colunas do balancete
        public string plano_id { get; set; }
        public string classificacao { get; set; }
        public string descricao { get; set; }
        public Decimal saldo_inicial { get; set; }
        public Decimal debito { get; set; }
        public Decimal credito { get; set; }
        public Decimal saldo_final { get; set; }

        public IEnumerable<gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel.vm_balancete> balancete { get; set; }
        public Vm_usuario user { get; set; }
        public vm_ConfigContadorCliente vm_config { get; set; }
    }
}
