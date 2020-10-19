using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    public class Balancete
    {
        public string plano_id { get; set; }
        public string classificacao { get; set; }
        public string descricao { get; set; }
        public Decimal saldo_inicial { get; set; }
        public Decimal debito { get; set; }
        public Decimal credito { get; set; }
        public Decimal saldo_final { get; set; }

        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Balancete()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Gerar balancete contábil
        public List<vm_balancete> gerarBalancete(DateTime data_inicial, DateTime data_final, string plano_id, int cliente_id, int contador_id, int usuario_id)
        {
            List<vm_balancete> balancete = new List<vm_balancete>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "CALL pr_balancete(@data_inicial, @data_final, @plano_id, @cliente_id, @contador_id);";
                comando.Parameters.AddWithValue("@cliente_id", cliente_id);
                comando.Parameters.AddWithValue("@contador_id", contador_id);
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.Parameters.AddWithValue("@data_inicial", data_inicial);
                comando.Parameters.AddWithValue("@data_final", data_final);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        vm_balancete b = new vm_balancete();

                        if (DBNull.Value != leitor["saldo_inicial"])
                        {
                            b.saldo_inicial = Convert.ToDecimal(leitor["saldo_inicial"]);
                        }
                        else
                        {
                            b.saldo_inicial =  0;
                        }

                        if (DBNull.Value != leitor["debito"])
                        {
                            b.debito = Convert.ToDecimal(leitor["debito"]);
                        }
                        else
                        {
                            b.debito = 0;
                        }
                        if (DBNull.Value != leitor["credito"])
                        {
                            b.credito = Convert.ToDecimal(leitor["credito"]);
                        }
                        else
                        {
                            b.credito = 0;
                        }
                        if (DBNull.Value != leitor["saldo_final"])
                        {
                            b.saldo_final = Convert.ToDecimal(leitor["saldo_final"]);
                        }
                        else
                        {
                            b.saldo_final = 0;
                        }

                        b.plano_id = leitor["plano_id"].ToString();
                        b.classificacao = leitor["classificacao"].ToString();
                        b.descricao = leitor["descricao"].ToString();

                        balancete.Add(b);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Balancete", "gerarBalancete", "Erro", msg, cliente_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return balancete;
        }

    }
}
