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
                comando.CommandText = "SELECT ld.lancamento_id as 'ld_id', ld.lancamento_origem as 'ld_origem', ld.lancamento_tipo as 'ld_tipo', ld.lancamento_data as 'ld_data', ld.lancamento_valor as 'ld_valor', ld.lancamento_contraPartida_id as 'ld_contraPartida', ld.lancamento_cliente_conta_id as 'ld_cliente_id', ld.lancamento_contador_conta_id as 'ld_contador_id', ld.lancamento_dataCriacao as 'ld_dataCriacao', ld.lancamento_ccontabil_id as 'ld_ccontabil', ccd.ccontabil_classificacao as 'ld_conta_classificacao', ccd.ccontabil_nome as 'ld_conta_nome' ,ld.lancamento_historico as 'ld_historico', lc.lancamento_id as 'lc_id', lc.lancamento_origem as 'lc_origem', lc.lancamento_tipo as 'lc_tipo', lc.lancamento_data as 'lc_data', lc.lancamento_valor as 'lc_valor', lc.lancamento_contraPartida_id as 'lc_contraPartida', lc.lancamento_cliente_conta_id as 'lc_cliente_id', lc.lancamento_contador_conta_id as 'lc_contador_id', lc.lancamento_dataCriacao as 'lc_dataCriacao', lc.lancamento_ccontabil_id as 'lc_ccontabil', ccc.ccontabil_classificacao as 'lc_conta_classificacao', ccc.ccontabil_nome as 'lc_conta_nome', lc.lancamento_historico as 'lc_historico' from lancamento as ld LEFT JOIN lancamento as lc on ld.lancamento_contraPartida_id = lc.lancamento_id left JOIN contacontabil as ccd on ccd.ccontabil_id = ld.lancamento_ccontabil_id LEFT join contacontabil as ccc on ccc.ccontabil_id = lc.lancamento_ccontabil_id WHERE ld.lancamento_tipo = 'D' and ld.lancamento_cliente_conta_id = @cliente_id and ld.lancamento_contador_conta_id = @contador_id order by ld.lancamento_id DESC limit @limit;";
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
                            lancamento.lancamento_debito_conta_id = Convert.ToInt32(leitor["ld_id"]);
                        }
                        else
                        {
                            lancamento.lancamento_debito_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["lc_id"])
                        {
                            lancamento.lancamento_credito_conta_id = Convert.ToInt32(leitor["lc_id"]);
                        }
                        else
                        {
                            lancamento.lancamento_credito_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["ld_data"])
                        {
                            lancamento.lancamento_data = Convert.ToDateTime(leitor["ld_data"]);
                        }
                        else
                        {
                            lancamento.lancamento_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["ld_valor"])
                        {
                            lancamento.lancamento_valor = Convert.ToDecimal(leitor["ld_valor"]);
                        }
                        else
                        {
                            lancamento.lancamento_valor = 0;
                        }

                        lancamento.lancamento_historico = leitor["ld_historico"].ToString();

                        if (DBNull.Value != leitor["ld_cliente_id"])
                        {
                            lancamento.lancamento_cliente_id = Convert.ToInt32(leitor["ld_cliente_id"]);
                        }
                        else
                        {
                            lancamento.lancamento_cliente_id = 0;
                        }

                        if (DBNull.Value != leitor["ld_contador_id"])
                        {
                            lancamento.lancamento_contador_id = Convert.ToInt32(leitor["ld_contador_id"]);
                        }
                        else
                        {
                            lancamento.lancamento_contador_id = 0;
                        }

                        lancamento.lancamento_historico = leitor["ld_historico"].ToString();
                        lancamento.lancamento_debito_classificacao = leitor["ld_conta_classificacao"].ToString();
                        lancamento.lancamento_credito_classificacao = leitor["lc_conta_classificacao"].ToString();
                        lancamento.lancamento_debito_nome = leitor["ld_conta_nome"].ToString();
                        lancamento.lancamento_credito_nome = leitor["lc_conta_nome"].ToString();


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

        //Cadastrar lançamento
        public string cadastrarLancamento(int usuario_id, int cliente_id, int contador_id, string origem, DateTime lancamento_data, string lancamento_valor, string lancamento_debito, string lancamento_credito, string lancamento_historico)
        {
            string retorno = "Lançamento contábil registrado com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "CALL pr_novolancamento('Lançamento Contábil',@data,@valor,@cliente_id,@contador_id,@contaDebito,@contaCredito,@histotico);";
                comando.Parameters.AddWithValue("@data", lancamento_data);
                comando.Parameters.AddWithValue("@valor", lancamento_valor);
                comando.Parameters.AddWithValue("@cliente_id", cliente_id);
                comando.Parameters.AddWithValue("@contador_id", contador_id);
                comando.Parameters.AddWithValue("@contaDebito", lancamento_debito);
                comando.Parameters.AddWithValue("@contaCredito", lancamento_credito);
                comando.Parameters.AddWithValue("@histotico", lancamento_historico);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Lançamento de código id débito: " + lancamento_debito + " e crédito id: " + lancamento_credito + " cadastrado com sucesso";
                log.log("Lançamento", "cadastrarLancamento", "Sucesso", msg, cliente_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao registrar o lançamento contábil!";

                string msg = "Tentativa de registrar lançamento de id débito: " + lancamento_debito + " e crédito id: " + lancamento_credito + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("Lançamento", "Lançamento", "Erro", msg, cliente_id, usuario_id);
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
