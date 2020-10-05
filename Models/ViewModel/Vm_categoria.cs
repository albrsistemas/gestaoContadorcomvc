using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_categoria
    {
        public int categoria_id { get; set; }

        [Display(Name = "Classificação")]
        [Required(ErrorMessage = "A classificação é obrigatória.")]
        [Remote("classificacaoExiste", "Categoria", ErrorMessage = "Classificação já existe")]
        public string categoria_classificacao { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Mínimo de dois caracteres")]
        public string categoria_nome { get; set; }
        
        public DateTime categoria_dataCriacao { get; set; }
        
        public int categoria_conta_id { get; set; }
        public string categoria_tipo { get; set; } //sintética ou analítica
        public string categoria_escopo { get; set; } //entrada ou saída de recursos

        [Display(Name = "Situação")]
        public string categoria_status { get; set; } //Ativo ou Deletado
        public string categoria_conta_contabil { get; set; }
        public string categoria_requer_provisao { get; set; }

        [Required(ErrorMessage = "A sequencia é obrigatória.")]
        public string categoria_sequencia { get; set; }        
        public string categoria_contaonline { get; set; }        
    }
}
