using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_ContaPadrao
    {
        public int contaPadrao_id { get; set; }
        [Display(Name = "Classificação")]
        public string contaPadrao_classificacao { get; set; }

        [Display(Name = "Descrição")]
        public string contaPadrao_descricao { get; set; }

        [Display(Name = "Apelido")]
        public string contaPadrao_apelido { get; set; }

        [Display(Name = "Grupo")]
        public string contaPadrao_grupo { get; set; }
        public string contaPadrao_tipo { get; set; }
        public int contaPadrao_conta_id { get; set; }
        public string contaPadrao_especie { get; set; }
        public string contaPadrao_natureza { get; set; }
        public int contaPadrao_filhos { get; set; }
        public string contaPadrao_status { get; set; }
        public string contapadrao_tags { get; set; }
        public string contaPadrao_codigoBanco { get; set; }

    }
}
