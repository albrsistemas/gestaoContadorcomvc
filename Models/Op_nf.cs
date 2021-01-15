using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_nf
    {
        public int op_nf_id { get; set; }
        public int op_nf_op_id { get; set; }
        public int op_nf_tipo { get; set; }
        public string op_nf_chave { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime op_nf_data_emissao { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime op_nf_data_entrada_saida { get; set; }
        public string op_nf_serie { get; set; }
        public string op_nf_numero { get; set; }
        public bool existe { get; set; } //Atributo de controle de edição
    }
}
