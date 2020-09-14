using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_categoria
    {
        public int categoria_id { get; set; }
        public string categoria_nome { get; set; }
        public int categoria_titulo { get; set; }
        public string categoria_classificacao { get; set; }
        public string categoria_apelido { get; set; }
        public string categoria_tipo { get; set; }
    }
}
