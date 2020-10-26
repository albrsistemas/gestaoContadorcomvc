using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class UF_ibge
    {
        public int uf_ibge_codigo { get; set; }
        public string uf_ibge_sigla { get; set; }
        public DateTime uf_ibge_data_inicio { get; set; }
        public DateTime uf_ibge_data_fim { get; set; }
    }
}
