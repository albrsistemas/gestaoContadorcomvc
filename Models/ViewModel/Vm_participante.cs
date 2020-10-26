using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_participante
    {
        public int participante_id { get; set; }

        [Display(Name = "Nome")]
        public string participante_nome { get; set; }
        public DateTime participante_dataCriacao { get; set; }

        [Display(Name = "Nome Fantasia")]
        public string participante_fantasia { get; set; }

        [Display(Name = "Código")]
        public string participante_codigo { get; set; }

        [Display(Name = "TIpo da Pessoa")]
        public string participante_tipoPessoa { get; set; }

        [Display(Name = "Cliente/Forn. Desde")]
        public DateTime participante_clienteDesde { get; set; }

        [Display(Name = "Contribuinte ICMS")]
        public int participante_contribuinte { get; set; }

        [Display(Name = "Inscrição Estadual")]
        public string participante_inscricaoEstadual { get; set; }

        [Display(Name = "CPF")]
        public string participante_cnpj_cpf { get; set; }

        [Display(Name = "RG")]
        public string participante_rg { get; set; }

        [Display(Name = "Orgão Emissor")]
        public string participante_orgaoEmissor { get; set; }

        [Display(Name = "CEP")]
        public string participante_cep { get; set; }

        [Display(Name = "UF")]
        public int participante_uf { get; set; }

        [Display(Name = "Cidade")]
        public string participante_cidade { get; set; }

        [Display(Name = "Bairro")]
        public string participante_bairro { get; set; }

        [Display(Name = "Endereço")]
        public string participante_logradouro { get; set; }

        [Display(Name = "Número")]
        public string participante_numero { get; set; }

        [Display(Name = "Complemento")]
        public string participante_complemento { get; set; }

        [Display(Name = "País")]
        public int participante_pais { get; set; }

        [Display(Name = "Categoria")]
        public int participante_categoria { get; set; }

        [Display(Name = "Observações")]
        public string participante_obs { get; set; }
        public int participante_conta_id { get; set; }

        [Display(Name = "Status")]
        public string participante_status { get; set; }

        [Display(Name = "Inscrição Municipal")]
        public string participante_insc_municipal { get; set; }

        [Display(Name = "Regime Tributário")]
        public int participante_regime_tributario { get; set; }

        [Display(Name = "Suframa")]
        public string participante_suframa { get; set; }

        //Campos de controle
        public Vm_usuario user { get; set; }
        public IEnumerable<Vm_participante> participantes { get; set; }
    }
}
