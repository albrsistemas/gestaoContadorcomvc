using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_participante
    {
        public int participante_id { get; set; }
        public string participante_nome { get; set; }
        public DateTime participante_dataCriacao { get; set; }
        public string participante_fantasia { get; set; }
        public string participante_codigo { get; set; }
        public string participante_tipoPessoa { get; set; }
        public DateTime participante_clienteDesde { get; set; }
        public int participante_contribuinte { get; set; }
        public string participante_inscricaoEstadual { get; set; }
        public string participante_cnpj_cpf { get; set; }
        public string participante_rg { get; set; }
        public string participante_orgaoEmissor { get; set; }
        public string participante_cep { get; set; }
        public int participante_uf { get; set; }
        public string participante_cidade { get; set; }
        public string participante_bairro { get; set; }
        public string participante_logradouro { get; set; }
        public string participante_numero { get; set; }
        public string participante_complemento { get; set; }
        public int participante_pais { get; set; }
        public int participante_categoria { get; set; }
        public string participante_obs { get; set; }
        public int participante_conta_id { get; set; }
        public string participante_status { get; set; }
        public string participante_insc_municipal { get; set; } //criar no banco
        public int participante_regime_tributario { get; set; } //criar no banco   
        public string participante_suframa { get; set; } //criar no banco

        //Campos de controle
        public Vm_usuario user { get; set; }
        public IEnumerable<Vm_participante> participantes { get; set; }
    }
}
