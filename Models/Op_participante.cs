using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_participante
    {
        public int op_part_id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome do participante é obrigatório")]
        public string op_part_nome { get; set; }

        [Display(Name = "Tipo Pessoa")]
        public string op_part_tipo { get; set; }

        [Display(Name = "CPF")]
        public string op_part_cnpj_cpf { get; set; }

        [Display(Name = "CEP")]
        public string op_part_cep { get; set; }

        [Display(Name = "Cidade")]
        public string op_part_cidade { get; set; }

        [Display(Name = "Bairro")]
        public string op_part_bairro { get; set; }

        [Display(Name = "Logradouro")]
        public string op_part_logradouro { get; set; }

        [Display(Name = "Número")]
        public string op_part_numero { get; set; }

        [Display(Name = "Coomplemento")]
        public string op_part_complemento { get; set; }

        [Display(Name = "País")]
        public int op_paisesIBGE_codigo { get; set; }

        [Display(Name = "UF")]
        public int op_uf_ibge_codigo { get; set; }
        public int op_id { get; set; }
        public int op_part_participante_id { get; set; }

        //Atributos de controle         
        public bool existe { get; set; } //Atributo de controle de edição
        public string controleEdit { get; set; } //Atributo de controle de edição


    }
}
