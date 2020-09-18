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
                }); ;
            }

            return selectBancos;
        }



    }
}
