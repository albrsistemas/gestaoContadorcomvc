using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_transportador
    {
        public int op_transportador_id { get; set; }

        [Display(Name = "Nome")]
        public string op_transportador_nome { get; set; }

        [Display(Name = "Documento")]
        public string op_transportador_cnpj_cpf { get; set; }
        public string op_transportador_modalidade_frete { get; set; }

        [Display(Name = "Quantidade")]
        public Decimal op_transportador_volume_qtd { get; set; }

        [Display(Name = "Peso Bruto")]
        public Decimal op_transportador_volume_peso_bruto { get; set; }
        public int op_transportador_op_id { get; set; }

        public int op_transportador_participante_id { get; set; } 
    }
}
