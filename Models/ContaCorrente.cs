using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.IO;

namespace gestaoContadorcomvc.Models
{
    public class ContaCorrente
    {
        public int ccorrente_id { get; set; }
        public string ccorrente_nome { get; set; }
        public string ccorrente_tipo { get; set; }
        public Decimal ccorrente_saldo_abertura { get; set; }
        public string ccorrente_masc_contabil { get; set; }
        public int ccorrente_conta_id { get; set; }
        public DateTime ccorrente_dataCriacao { get; set; }
        public string ccorrente_status { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ContaCorrente()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Lista de conta corrente
        public List<Vm_conta_corrente> listContaCorrete(int conta_id, int usuario_id)
        {
            List<Vm_conta_corrente> contas_corrente = new List<Vm_conta_corrente>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from conta_corrente where ccorrente_conta_id = @conta_id and ccorrente_status = 'Ativo';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_conta_corrente conta_corrente = new Vm_conta_corrente();

                        if (DBNull.Value != leitor["ccorrente_id"])
                        {
                            conta_corrente.ccorrente_id = Convert.ToInt32(leitor["ccorrente_id"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_id = 0;
                        }

                        if (DBNull.Value != leitor["ccorrente_conta_id"])
                        {
                            conta_corrente.ccorrente_conta_id = Convert.ToInt32(leitor["ccorrente_conta_id"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["ccorrente_saldo_abertura"])
                        {
                            conta_corrente.ccorrente_saldo_abertura = Convert.ToDecimal(leitor["ccorrente_saldo_abertura"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_saldo_abertura = 0;
                        }

                        if (DBNull.Value != leitor["ccorrente_dataCriacao"])
                        {
                            conta_corrente.ccorrente_dataCriacao = Convert.ToDateTime(leitor["ccorrente_dataCriacao"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_dataCriacao = new DateTime();
                        }

                        conta_corrente.ccorrente_nome = leitor["ccorrente_nome"].ToString();
                        conta_corrente.ccorrente_tipo = leitor["ccorrente_tipo"].ToString();
                        conta_corrente.ccorrente_masc_contabil = leitor["ccorrente_masc_contabil"].ToString();
                        conta_corrente.ccorrente_status = leitor["ccorrente_status"].ToString();

                        contas_corrente.Add(conta_corrente);
                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("ContaCorrente", "listContaCorrente", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return contas_corrente;
        }

        //Cadastrar conta corrente
        public string cadastraContaCorrente(
            int conta_id,
            int usuario_id,
            string ccorrente_nome,
            string ccorrente_tipo,
            string ccorrente_masc_contabil,
            Decimal ccorrente_saldo_abertura
            )
        {
            string retorno = "Conta corrente cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into conta_corrente (ccorrente_nome, ccorrente_tipo, ccorrente_saldo_abertura, ccorrente_masc_contabil, ccorrente_conta_id) values (@ccorrente_nome, @ccorrente_tipo, @ccorrente_saldo_abertura, @ccorrente_masc_contabil, @ccorrente_conta_id);";
                comando.Parameters.AddWithValue("@ccorrente_nome", ccorrente_nome);
                comando.Parameters.AddWithValue("@ccorrente_tipo", ccorrente_tipo);
                comando.Parameters.AddWithValue("@ccorrente_saldo_abertura", ccorrente_saldo_abertura);
                comando.Parameters.AddWithValue("@ccorrente_masc_contabil", ccorrente_masc_contabil);
                comando.Parameters.AddWithValue("@ccorrente_conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro de nova conta corrente nome: " + ccorrente_nome + " Cadastrado com sucesso";
                log.log("ContaCorrente", "cadastraContaCorrente", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a conta corrente. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("ContaCorrente", "cadastraContaCorrente", "Erro", msg, conta_id, usuario_id);
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

        //Buscar conta corrente
        public Vm_conta_corrente buscaContaCorrete(int conta_id, int usuario_id, int ccorrente_id)
        {
            Vm_conta_corrente conta_corrente = new Vm_conta_corrente();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from conta_corrente where ccorrente_conta_id = @conta_id and ccorrente_id = @ccorrente_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@ccorrente_id", ccorrente_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["ccorrente_id"])
                        {
                            conta_corrente.ccorrente_id = Convert.ToInt32(leitor["ccorrente_id"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_id = 0;
                        }

                        if (DBNull.Value != leitor["ccorrente_conta_id"])
                        {
                            conta_corrente.ccorrente_conta_id = Convert.ToInt32(leitor["ccorrente_conta_id"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["ccorrente_saldo_abertura"])
                        {
                            conta_corrente.ccorrente_saldo_abertura = Convert.ToDecimal(leitor["ccorrente_saldo_abertura"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_saldo_abertura = 0;
                        }

                        if (DBNull.Value != leitor["ccorrente_dataCriacao"])
                        {
                            conta_corrente.ccorrente_dataCriacao = Convert.ToDateTime(leitor["ccorrente_dataCriacao"]);
                        }
                        else
                        {
                            conta_corrente.ccorrente_dataCriacao = new DateTime();
                        }

                        conta_corrente.ccorrente_nome = leitor["ccorrente_nome"].ToString();
                        conta_corrente.ccorrente_tipo = leitor["ccorrente_tipo"].ToString();
                        conta_corrente.ccorrente_masc_contabil = leitor["ccorrente_masc_contabil"].ToString();
                        conta_corrente.ccorrente_status = leitor["ccorrente_status"].ToString();
                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("ContaCorrente", "buscaContaCorrente", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return conta_corrente;
        }

        //Alterar conta corrente
        public string alteraContaCorrente(
            int conta_id,
            int usuario_id,
            string ccorrente_nome,
            string ccorrente_tipo,
            string ccorrente_masc_contabil,
            Decimal ccorrente_saldo_abertura,
            string ccorrente_status,
            int ccorrente_id
            )
        {
            string retorno = "Conta corrente alterada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE conta_corrente set ccorrente_nome = @ccorrente_nome, ccorrente_tipo = @ccorrente_tipo, ccorrente_saldo_abertura = @ccorrente_saldo_abertura, ccorrente_masc_contabil = @ccorrente_masc_contabil, ccorrente_status = @ccorrente_status WHERE ccorrente_id = @ccorrente_id and ccorrente_conta_id = @ccorrente_conta_id;";
                comando.Parameters.AddWithValue("@ccorrente_nome", ccorrente_nome);
                comando.Parameters.AddWithValue("@ccorrente_tipo", ccorrente_tipo);
                comando.Parameters.AddWithValue("@ccorrente_saldo_abertura", ccorrente_saldo_abertura);
                comando.Parameters.AddWithValue("@ccorrente_masc_contabil", ccorrente_masc_contabil);
                comando.Parameters.AddWithValue("@ccorrente_conta_id", conta_id);
                comando.Parameters.AddWithValue("@ccorrente_status", ccorrente_status);
                comando.Parameters.AddWithValue("@ccorrente_id", ccorrente_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração da conta corrente nome: " + ccorrente_nome + " Alterada com sucesso";
                log.log("ContaCorrente", "alteraContaCorrente", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar a conta corrente. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("ContaCorrente", "alteraContaCorrente", "Erro", msg, conta_id, usuario_id);
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


        //Deletar conta corrente
        public string deleteContaCorrente(
           int conta_id,
           int usuario_id,           
           int ccorrente_id
           )
        {
            string retorno = "Conta corrente apagada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE conta_corrente set ccorrente_status = 'Deletado' WHERE ccorrente_id = @ccorrente_id and ccorrente_conta_id = @ccorrente_conta_id;";
                comando.Parameters.AddWithValue("@ccorrente_conta_id", conta_id);                
                comando.Parameters.AddWithValue("@ccorrente_id", ccorrente_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão da conta corrente ID: " + ccorrente_id + " Excluído com sucesso";
                log.log("ContaCorrente", "deleteContaCorrente", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir a conta corrente. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("ContaCorrente", "deleteContaCorrente", "Erro", msg, conta_id, usuario_id);
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
