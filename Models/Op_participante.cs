using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Op_participante
    {
        public int op_part_id { get; set; }
        public string op_part_nome { get; set; }
        public string op_part_tipo { get; set; }
        public string op_part_cnpj_cpf { get; set; }
        public string op_part_cep { get; set; }
        public string op_part_cidade { get; set; }
        public string op_part_bairro { get; set; }
        public string op_part_logradouro { get; set; }
        public string op_part_numero { get; set; }
        public string op_part_complemento { get; set; }
        public int op_paisesIBGE_codigo { get; set; }
        public int op_uf_ibge_codigo { get; set; }
        public int op_id { get; set; }
    }
}
