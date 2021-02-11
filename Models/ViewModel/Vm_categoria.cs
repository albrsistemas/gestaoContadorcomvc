using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_categoria
    {
        public int categoria_id { get; set; }

        [Display(Name = "Classificação")]
        [Required(ErrorMessage = "A classificação é obrigatória.")]
        //[Remote("classificacaoExiste", "Categoria", ErrorMessage = "Classificação já existe")]
        public string categoria_classificacao { get; set; }

        [Display(Name = "Classificação")]
        [Required(ErrorMessage = "A classificação é obrigatória.")]
        [Remote("classificacaoExistenoPlano", "Categoria", ErrorMessage = "Classificação já existe")]
        public string categoria_classificacao_dePlano { get; set; }

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

        [Display(Name = "Conta Contábil")]
        public string categoria_conta_contabil { get; set; }
        public string categoria_requer_provisao { get; set; }

        [Required(ErrorMessage = "A sequencia é obrigatória.")]
        public string categoria_sequencia { get; set; }        
        public string categoria_contaonline { get; set; } //Classificação da conta on line        
        public string categoria_contaonline_id { get; set; } //ID da conta on line      

        public IEnumerable<gestaoContadorcomvc.Models.ViewModel.Vm_categoria> categorias { get; set; }

        public Vm_usuario user { get; set; }

        public vm_ConfigContadorCliente cco { get; set; }

        public bool categoria_categoria_fiscal { get; set; }
        public bool categoria_categoria_tributo { get; set; }
        public string categoria_padrao { get; set; }
    }
}
