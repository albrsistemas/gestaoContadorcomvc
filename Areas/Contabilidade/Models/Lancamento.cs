using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    public class Lancamento
    {
        public int lancamento_id { get; set; }
        public string lancamento_origem { get; set; }
        public string lancamento_tipo { get; set; }
        public DateTime lancamento_data { get; set; }
        public Decimal lancamento_valor { get; set; }
        public int lancamento_contraPartida_id { get; set; }
        public int lancamento_cliente_conta_id { get; set; }
        public int lancamento_contador_conta_id { get; set; }
        public DateTime lancamento_dataCriacao { get; set; }
        public int lancamento_ccontabil_id { get; set; }
        public string lancamento_historico { get; set; }


        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Lancamento()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Listar lançamentos com limite
        public List<vm_lancamento> listarLancamentosLimit(int user_id, int cliente_id, int contador_id, int limit)
        {
            List<vm_lancamento> lancamentos = new List<vm_lancamento>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT ld.lancamento_id as 'ld_id', ld.lancamento_origem as 'ld_origem', ld.lancamento_tipo as 'ld_tipo', ld.lancamento_data as 'ld_data', ld.lancamento_valor as 'ld_valor', ld.lancamento_contraPartida_id as 'ld_contraPartida', ld.lancamento_cliente_conta_id as 'ld_cliente_id', ld.lancamento_contador_conta_id as 'ld_contador_id', ld.lancamento_dataCriacao as 'ld_dataCriacao', ld.lancamento_ccontabil_id as 'ld_ccontabil', ld.lancamento_historico as 'ld_historico', lc.lancamento_id as 'lc_id', lc.lancamento_origem as 'lc_origem', lc.lancamento_tipo as 'lc_tipo', lc.lancamento_data as 'lc_data', lc.lancamento_valor as 'lc_valor', lc.lancamento_contraPartida_id as 'lc_contraPartida', lc.lancamento_cliente_conta_id as 'lc_cliente_id', lc.lancamento_contador_conta_id as 'lc_contador_id', lc.lancamento_dataCriacao as 'lc_dataCriacao', lc.lancamento_ccontabil_id as 'lc_ccontabil', lc.lancamento_historico as 'lc_historico' from lancamento as ld LEFT JOIN lancamento as lc on ld.lancamento_contraPartida_id = lc.lancamento_id WHERE ld.lancamento_tipo = 'D' and ld.lancamento_cliente_conta_id = @cliente_id and ld.lancamento_contador_conta_id = @contador_id order by ld.lancamento_id DESC limit @limit;";
                comando.Parameters.AddWithValue("@cliente_id", cliente_id);
                comando.Parameters.AddWithValue("@contador_id", contador_id);
                comando.Parameters.AddWithValue("@limit", limit);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        vm_lancamento lancamento = new vm_lancamento();

                        if (DBNull.Value != leitor["ld_id"])
                        {
                            lancamento.ld_id = Convert.ToInt32(leitor["ld_id"]);
                        }
                        else
                        {
                            lancamento.ld_id = 0;
                        }

                        if (DBNull.Value != leitor["ld_contraPartida"])
                        {
                            lancamento.ld_contraPartida = Convert.ToInt32(leitor["ld_contraPartida"]);
                        }
                        else
                        {
                            lancamento.ld_contraPartida = 0;
                        }

                        if (DBNull.Value != leitor["ld_cliente_id"])
                        {
                            lancamento.ld_cliente_id = Convert.ToInt32(leitor["ld_cliente_id"]);
                        }
                        else
                        {
                            lancamento.ld_cliente_id = 0;
                        }

                        if (DBNull.Value != leitor["ld_contador_id"])
                        {
                            lancamento.ld_contador_id = Convert.ToInt32(leitor["ld_contador_id"]);
                        }
                        else
                        {
                            lancamento.ld_contador_id = 0;
                        }

                        if (DBNull.Value != leitor["ld_ccontabil"])
                        {
                            lancamento.ld_ccontabil = Convert.ToInt32(leitor["ld_ccontabil"]);
                        }
                        else
                        {
                            lancamento.ld_ccontabil = 0;
                        }

                        if (DBNull.Value != leitor["lc_id"])
                        {
                            lancamento.lc_id = Convert.ToInt32(leitor["lc_id"]);
                        }
                        else
                        {
                            lancamento.lc_id = 0;
                        }

                        if (DBNull.Value != leitor["lc_contraPartida"])
                        {
                            lancamento.lc_contraPartida = Convert.ToInt32(leitor["lc_contraPartida"]);
                        }
                        else
                        {
                            lancamento.lc_contraPartida = 0;
                        }

                        if (DBNull.Value != leitor["lc_cliente_id"])
                        {
                            lancamento.lc_cliente_id = Convert.ToInt32(leitor["lc_cliente_id"]);
                        }
                        else
                        {
                            lancamento.lc_cliente_id = 0;
                        }

                        if (DBNull.Value != leitor["lc_contador_id"])
                        {
                            lancamento.lc_contador_id = Convert.ToInt32(leitor["lc_contador_id"]);
                        }
                        else
                        {
                            lancamento.lc_contador_id = 0;
                        }

                        if (DBNull.Value != leitor["lc_ccontabil"])
                        {
                            lancamento.lc_ccontabil = Convert.ToInt32(leitor["lc_ccontabil"]);
                        }
                        else
                        {
                            lancamento.lc_ccontabil = 0;
                        }

                        if (DBNull.Value != leitor["ld_data"])
                        {
                            lancamento.ld_data = Convert.ToDateTime(leitor["ld_data"]);
                        }
                        else
                        {
                            lancamento.ld_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["ld_dataCriacao"])
                        {
                            lancamento.ld_dataCriacao = Convert.ToDateTime(leitor["ld_dataCriacao"]);
                        }
                        else
                        {
                            lancamento.ld_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["lc_data"])
                        {
                            lancamento.lc_data = Convert.ToDateTime(leitor["lc_data"]);
                        }
                        else
                        {
                            lancamento.lc_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["lc_dataCriacao"])
                        {
                            lancamento.lc_dataCriacao = Convert.ToDateTime(leitor["lc_dataCriacao"]);
                        }
                        else
                        {
                            lancamento.lc_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["ld_valor"])
                        {
                            lancamento.ld_valor = Convert.ToDecimal(leitor["ld_valor"]);
                        }
                        else
                        {
                            lancamento.ld_valor = 0;
                        }

                        if (DBNull.Value != leitor["lc_valor"])
                        {
                            lancamento.lc_valor = Convert.ToDecimal(leitor["lc_valor"]);
                        }
                        else
                        {
                            lancamento.lc_valor = 0;
                        }

                        lancamento.ld_origem = leitor["ld_origem"].ToString();
                        lancamento.ld_tipo = leitor["ld_tipo"].ToString();
                        lancamento.ld_historico = leitor["ld_historico"].ToString();

                        lancamento.lc_origem = leitor["lc_origem"].ToString();
                        lancamento.lc_tipo = leitor["lc_tipo"].ToString();
                        lancamento.lc_historico = leitor["lc_historico"].ToString();


                        lancamentos.Add(lancamento);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Lancamento", "listarLancamentosLimit", "Erro", msg, cliente_id, user_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return lancamentos;
        }

    }
}
