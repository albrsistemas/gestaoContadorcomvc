using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site
{
    public class Lead
    {
        public int lead_id { get; set; }
        public string lead_nome { get; set; }
        public string lead_celular { get; set; }
        public string lead_email { get; set; }
        public int lead_conta_id { get; set; }
        public string lead_tipo { get; set; }
        public string lead_situacao { get; set; }
        public int lead_lead_atendentes_id { get; set; }
        public IEnumerable<Lead_contato> contatos { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Lead()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        public string create(int conta_id, int lead_lead_atendentes_id, string lead_nome, string lead_celular, string lead_email, string lead_tipo, int lead_situacao, string lead_contato_tipo, string lead_contato_msg)
        {
            string retorno = "";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_lead (lead_conta_id, lead_lead_atendentes_id, lead_nome, lead_celular, lead_email, lead_tipo, lead_situacao, lead_contato_tipo, lead_contato_msg)";
                comando.Parameters.AddWithValue("@lead_conta_id", conta_id);
                comando.Parameters.AddWithValue("@lead_lead_atendentes_id", lead_lead_atendentes_id);
                comando.Parameters.AddWithValue("@lead_nome", lead_nome);
                comando.Parameters.AddWithValue("@lead_celular", lead_celular);
                comando.Parameters.AddWithValue("@lead_email", lead_email);
                comando.Parameters.AddWithValue("@lead_tipo", lead_tipo);
                comando.Parameters.AddWithValue("@lead_situacao", lead_situacao);
                comando.Parameters.AddWithValue("@lead_contato_tipo", lead_contato_tipo);
                comando.Parameters.AddWithValue("@lead_contato_msg", lead_contato_msg);
                comando.ExecuteNonQuery();
                Transacao.Commit();
            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar o contato!";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return retorno;
        }




    }
}
