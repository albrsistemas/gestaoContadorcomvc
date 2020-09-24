using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Selects
    {
        //Atributos
        public string value { get; set; }
        public string text { get; set; }

        //Método para listar bancos
        public List<Selects> getBancos()
        {
            List<Selects> selectBancos = new List<Selects>();
            ContaPadrao contaPadrao = new ContaPadrao();
            List<ContaPadrao> bancos = new List<ContaPadrao>();

            bancos = contaPadrao.listaBancos();

            foreach (var item in bancos)
            {                
                selectBancos.Add(new Selects
                {
                    value = item.contaPadrao_id.ToString(),
                    text = (item.contaPadrao_descricao + " (" + item.contaPadrao_codigoBanco + ")")
                });
            }

            return selectBancos;
        }

        //Grupos das contas padrão
        public List<Selects> getGrupoContas()
        {
            List<Selects> contas = new List<Selects>();
            contas.Add(new Selects
            {
                value = "Ativo",
                text = "Ativo"
            });
            contas.Add(new Selects
            {
                value = "Passico",
                text = "Passivo"
            });
            contas.Add(new Selects
            {
                value = "Receita",
                text = "Receita"
            });
            contas.Add(new Selects
            {
                value = "Despesa",
                text = "Despesa"
            });

            return contas;
        }



    }
}
