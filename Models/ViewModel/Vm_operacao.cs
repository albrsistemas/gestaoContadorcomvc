using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_operacao
    {
        public Operacao operacao { get; set; }
        public Op_participante participante { get; set; }
        public List<Op_itens> itens { get; set; }
        public Op_retencoes retencoes { get; set; }
        public Op_totais totais { get; set; }
        public List<Op_parcelas> parcelas { get; set; }
        public Op_transportador transportador { get; set; }
        public Op_nf nf { get; set; }
        public Op_servico servico { get; set; }
        public IEnumerable<Vm_compra> compras { get; set; }
        public IEnumerable<Vm_operacao> operacoes { get; set; }
        public Vm_usuario user { get; set; }

        //Campos necessários nas views
        public int forma_pgto { get; set; }
        public string condicoes_pgto { get; set; }
        public Op_itens item { get; set; }
        public List<Vm_participante> participantes { get; set; } //autocomplete

        
        

    }
}
