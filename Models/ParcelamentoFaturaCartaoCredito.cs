using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class ParcelamentoFaturaCartaoCredito
    {
        public int pfcc_id { get; set; }
        public int pfcc_fcc_id { get; set; }
        public Decimal pfcc_total_fatura { get; set; }
        public Decimal pfcc_valor_parcelado { get; set; }
        public int pfcc_numero_parcelas { get; set; }
        public Decimal pfcc_valor_parcela { get; set; }
        public Decimal pfcc_juros { get; set; }
        public int pfcc_categoria_id { get; set; }
        public DateTime pfcc_data_parcelamento { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ParcelamentoFaturaCartaoCredito()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        




    }
}
