using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel
{
    [Area("Contabilidade")]
    public class vm_ContaContabil
    {
        public int ccontabil_id { get; set; }
        public int ccontabil_plano_id { get; set; }

        [Display(Name = "Classificação")]
        [Required(ErrorMessage = "A classificação é obrigatória.")]
        [StringLength(13, MinimumLength = 2, ErrorMessage = "Mínimo de 2 caracteres e máximo de 13")]
        public string ccontabil_classificacao { get; set; }

        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(80, MinimumLength = 5, ErrorMessage = "Mínimo de 5 caracteres e máximo de 80")]
        public string ccontabil_nome { get; set; }

        [Display(Name = "Apelido")]
        [MaxLength(20, ErrorMessage = "Máximo de 20 caracteres")]        
        public string ccontabil_apelido { get; set; }

        [Display(Name = "Nível")]
        [Required(ErrorMessage = "O nível é obrigatório.")]
        public int ccontabil_nivel { get; set; }
        public string ccontabil_grupo { get; set; }
        public string ccontabil_tipo { get; set; }
        public DateTime ccontabil_dataCriacao { get; set; }
        public DateTime ccontabil_dataInativacao { get; set; }
        public string ccontabil_status { get; set; }
        public IEnumerable<gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel.vm_ContaContabil> contasContabeis { get; set; }
        public Vm_usuario user { get; set; }
        public PlanoContas plano { get; set; }


        public string grupoConta(string classificacao)
        {
            string grupo = "";

            string raiz = classificacao.Substring(0, 2);

            switch (raiz)
            {
                case "01":
                    grupo = "Ativo";
                    break;
                case "02":
                    grupo = "Passivo";
                    break;
                case "03":
                    grupo = "Receita";
                    break;
                case "04":
                    grupo = "Despesa";
                    break;
                case "05":
                    grupo = "RLE";
                    break;
            }
            return grupo;
        }

    }
}
