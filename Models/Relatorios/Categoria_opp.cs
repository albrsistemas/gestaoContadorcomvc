using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace gestaoContadorcomvc.Models.Relatorios
{
    public class Categoria_opp
    {
        public string classificacao { get; set; }
        public string descricao { get; set; }
        public Decimal jan { get; set; }
        public Decimal fev { get; set; }
        public Decimal marc { get; set; }
        public Decimal abr { get; set; }
        public Decimal mai { get; set; }
        public Decimal jun { get; set; }
        public Decimal jul { get; set; }
        public Decimal ago { get; set; }
        public Decimal sete { get; set; }
        public Decimal outu { get; set; }
        public Decimal nov { get; set; }
        public Decimal dez { get; set; }
        public Decimal soma { get; set; }
        //filtro
        public string visao { get; set; }
        public string ano { get; set; }

        public Vm_usuario user { get; set; }
        public IEnumerable<Categoria_opp> lista { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Categoria_opp()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        public Categoria_opp gerarRelatorio(int conta_id, string ano, string visao)
        {
            List<Categoria_opp> lista = new List<Categoria_opp>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_r_categ_op_parcelas(@conta_id, @ano, @visao)";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@ano", ano);
                comando.Parameters.AddWithValue("@visao", visao);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Categoria_opp copp = new Categoria_opp();
                        copp.classificacao = leitor["categoria_classificacao"].ToString();
                        copp.descricao = leitor["categoria_nome"].ToString();
                        copp.jan = Convert.ToDecimal(leitor["jan"]);
                        copp.fev = Convert.ToDecimal(leitor["fev"]);
                        copp.marc = Convert.ToDecimal(leitor["marc"]);
                        copp.abr = Convert.ToDecimal(leitor["abr"]);
                        copp.mai = Convert.ToDecimal(leitor["mai"]);
                        copp.jun = Convert.ToDecimal(leitor["jun"]);
                        copp.jul = Convert.ToDecimal(leitor["jul"]);
                        copp.ago = Convert.ToDecimal(leitor["ago"]);
                        copp.sete = Convert.ToDecimal(leitor["sete"]);
                        copp.outu = Convert.ToDecimal(leitor["outu"]);
                        copp.nov = Convert.ToDecimal(leitor["nov"]);
                        copp.dez = Convert.ToDecimal(leitor["dez"]);
                        lista.Add(copp);
                    }
                }


            }
            catch (Exception)
            {
                
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            Categoria_opp copp_r = new Categoria_opp();
            copp_r.lista = lista;

            return copp_r;

        }


    }
}
