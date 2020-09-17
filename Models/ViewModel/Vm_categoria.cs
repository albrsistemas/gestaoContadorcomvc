using gestaoContadorcomvc.Models.Autenticacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_categoria
    {
        //Os atrubutos abaixo são da model ContaPadrao

        public int contaPadrao_id { get; set; }
        public int contaPadrao_conta_id { get; set; }
        public string contaPadrao_classificacao { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome")]
        public string contaPadrao_descricao { get; set; }
                
        [Display(Name = "Apelido")]
        public string contaPadrao_apelido { get; set; }
        public string contaPadrao_grupo { get; set; }
        public string contaPadrao_tipo { get; set; }        
        public string contaPadrao_especie { get; set; }
        public string contaPadrao_natureza { get; set; }
        public string vm_categoria_caixaBanco { get; set; }
        public int contaPadrao_filhos { get; set; }
        public string contaPadrao_status { get; set; }
        public string contaPadrao_tags { get; set; }
        public string caixaBanco_conta_id { get; set; }

        public List<Vm_categoria> caixaBcos { get; set; }
        public List<Vm_categoria> receitas { get; set; }
        public List<Vm_categoria> despesas { get; set; }

        public Usuario userLogado { get; set; }

    }
}
