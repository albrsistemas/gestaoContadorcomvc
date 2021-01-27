using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_conta_corrente
    {
        public int ccorrente_id { get; set; }

        [Display(Name = "Nome")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Mínimo de 2 caracteres e máximo de 30")]
        public string ccorrente_nome { get; set; }

        [Display(Name = "Tipo")]
        public string ccorrente_tipo { get; set; }

        [Display(Name = "Saldo de Abertura")]
        public Decimal ccorrente_saldo_abertura { get; set; }

        [Display(Name = "Conta Contábil")]
        [StringLength(30, ErrorMessage = "Máximo de 30 caracteres")]
        public string ccorrente_masc_contabil { get; set; }
        public int ccorrente_conta_id { get; set; }
        public DateTime ccorrente_dataCriacao { get; set; }

        [Display(Name = "Status")]
        public string ccorrente_status { get; set; }
        //Atributos de controle
        public IEnumerable<Vm_conta_corrente> contas_corrente { get; set; }
        public Vm_usuario user { get; set; }
    }
}
