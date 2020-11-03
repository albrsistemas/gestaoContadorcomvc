using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_compra
    {
        public Operacao operacao { get; set; }
        public Op_participante participante { get; set; }
        public List<Op_itens> itens { get; set; }
        public Op_retencoes retencoes { get; set; }
        public Op_totais totais { get; set; }
        public Op_parcelas parcelas { get; set; }
        public Op_transportador transportador { get; set; }
    }
}
