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
    public class Op_parcelas_baixa
    {
        public int oppb_id { get; set; }
        public int oppb_op_parcela_id { get; set; }
        public int oppb_op_id { get; set; }
        public int oppb_conta_corrente { get; set; }
        public DateTime oppb_data { get; set; }
        public DateTime oppb_dataCriacao { get; set; }
        public Decimal oppb_valor { get; set; }
        public string oppb_obs { get; set; }
        public Decimal oppb_juros { get; set; }
        public Decimal oppb_multa { get; set; }
        public Decimal oppb_desconto { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Op_parcelas_baixa()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Busca parcela a ser baixada
        public Vm_op_parcelas_baixa buscaDados_para_Baixa(int conta_id, int usuario_id, int parcela_id)
        {
            Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT p.*, concat('Parcela com vencimento em ', DATE_FORMAT(p.op_parcela_vencimento, '%d/%m/%Y'),' referente ',op.op_tipo,' número ',op.op_numero_ordem) as referencia, (SELECT COALESCE(sum(op_parcelas_baixa.oppb_valor), 0.00) from op_parcelas_baixa WHERE op_parcelas_baixa.oppb_op_parcela_id = p.op_parcela_id) as saldo from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id where op.op_conta_id = @conta_id and p.op_parcela_id = @parcela_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@parcela_id", parcela_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        vm_baixa.referencia = leitor["referencia"].ToString();

                        if (DBNull.Value != leitor["op_parcela_vencimento"])
                        {
                            vm_baixa.vencimento = Convert.ToDateTime(leitor["op_parcela_vencimento"]);
                        }
                        else
                        {
                            vm_baixa.vencimento = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_parcela_valor"])
                        {
                            vm_baixa.valor_parcela_original = Convert.ToDecimal(leitor["op_parcela_valor"]);
                        }
                        else
                        {
                            vm_baixa.valor_parcela_original = 0;
                        }

                        if (DBNull.Value != leitor["saldo"])
                        {
                            vm_baixa.saldo_parcela = Convert.ToDecimal(leitor["saldo"]);
                        }
                        else
                        {
                            vm_baixa.saldo_parcela = 0;
                        }

                        if (DBNull.Value != leitor["op_parcela_id"])
                        {
                            vm_baixa.parcela_id = Convert.ToInt32(leitor["op_parcela_id"]);
                        }
                        else
                        {
                            vm_baixa.parcela_id = 0;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas_baixa", "buscaDados_para_Baixa", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vm_baixa;
        }

        //gravar baixa
        public string cadastrarBaixa(
            int usuario_id,
            int conta_id,
            int parcela_id,
            int contaCorrente,
            DateTime data,
            Decimal valor,
            string memorando,
            Decimal juros,
            Decimal multa,
            Decimal desconto
            )
        {
            string retorno = "Baixa realizada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_baixaParcela (@conta_id, @parcela_id, @contaCorrente, @data, @valor, @memorando, @nOppb_juros, @nOppb_multa, @nOppb_desconto);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@parcela_id", parcela_id);
                comando.Parameters.AddWithValue("@contaCorrente", contaCorrente);
                comando.Parameters.AddWithValue("@data", data);
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@memorando", memorando);
                comando.Parameters.AddWithValue("@nOppb_juros", juros);
                comando.Parameters.AddWithValue("@nOppb_multa", multa);
                comando.Parameters.AddWithValue("@nOppb_desconto", desconto);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Baixa parcela ID: " + parcela_id + " baixada com sucesso";
                log.log("Op_parcelas_baixa", "cadastrarBaixa", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao realizar a baixa. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas_baixa", "cadastrarBaixa", "Erro", msg, conta_id, usuario_id);
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

        //Busca parcela a ser baixada
        public Vm_op_parcelas_baixa buscaBaixa(int conta_id, int usuario_id, int baixa_id)
        {
            Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT b.*, concat('Baixa da parcela vencida em ', DATE_FORMAT(p.op_parcela_vencimento, '%d/%m/%Y'),' referente ',op.op_tipo,' número ', op.op_numero_ordem) as referencia, (SELECT COALESCE(sum(op_parcelas_baixa.oppb_valor), 0.00) from op_parcelas_baixa WHERE op_parcelas_baixa.oppb_op_parcela_id = p.op_parcela_id) as saldo, p.op_parcela_vencimento, p.op_parcela_valor as valor_parcela_original from op_parcelas_baixa as b LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN operacao as op on op.op_id = b.oppb_op_id WHERE b.oppb_id = @baixa_id and op.op_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@baixa_id", baixa_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        vm_baixa.referencia = leitor["referencia"].ToString();

                        if (DBNull.Value != leitor["op_parcela_vencimento"])
                        {
                            vm_baixa.vencimento = Convert.ToDateTime(leitor["op_parcela_vencimento"]);
                        }
                        else
                        {
                            vm_baixa.vencimento = new DateTime();
                        }

                        if (DBNull.Value != leitor["valor_parcela_original"])
                        {
                            vm_baixa.valor_parcela_original = Convert.ToDecimal(leitor["valor_parcela_original"]);
                        }
                        else
                        {
                            vm_baixa.valor_parcela_original = 0;
                        }

                        if (DBNull.Value != leitor["oppb_valor"])
                        {
                            vm_baixa.valor = Convert.ToDecimal(leitor["oppb_valor"]);
                        }
                        else
                        {
                            vm_baixa.valor = 0;
                        }

                        if (DBNull.Value != leitor["oppb_juros"])
                        {
                            vm_baixa.juros = Convert.ToDecimal(leitor["oppb_juros"]);
                        }
                        else
                        {
                            vm_baixa.juros = 0;
                        }

                        if (DBNull.Value != leitor["oppb_multa"])
                        {
                            vm_baixa.multa = Convert.ToDecimal(leitor["oppb_multa"]);
                        }
                        else
                        {
                            vm_baixa.multa = 0;
                        }

                        if (DBNull.Value != leitor["oppb_desconto"])
                        {
                            vm_baixa.desconto = Convert.ToDecimal(leitor["oppb_desconto"]);
                        }
                        else
                        {
                            vm_baixa.desconto = 0;
                        }

                        if (DBNull.Value != leitor["oppb_data"])
                        {
                            vm_baixa.data = Convert.ToDateTime(leitor["oppb_data"]);
                        }
                        else
                        {
                            vm_baixa.data = new DateTime();
                        }

                        if (DBNull.Value != leitor["saldo"])
                        {
                            vm_baixa.saldo_parcela = Convert.ToDecimal(leitor["saldo"]);
                        }
                        else
                        {
                            vm_baixa.saldo_parcela = 0;
                        }

                        if (DBNull.Value != leitor["oppb_conta_corrente"])
                        {
                            vm_baixa.contaCorrente = Convert.ToInt32(leitor["oppb_conta_corrente"]);
                        }
                        else
                        {
                            vm_baixa.contaCorrente = 0;
                        }

                        if (DBNull.Value != leitor["oppb_op_parcela_id"])
                        {
                            vm_baixa.parcela_id = Convert.ToInt32(leitor["oppb_op_parcela_id"]);
                        }
                        else
                        {
                            vm_baixa.parcela_id = 0;
                        }

                        if (DBNull.Value != leitor["oppb_id"])
                        {
                            vm_baixa.baixa_id = Convert.ToInt32(leitor["oppb_id"]);
                        }
                        else
                        {
                            vm_baixa.baixa_id = 0;
                        }

                        vm_baixa.obs = leitor["oppb_obs"].ToString();

                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas_baixa", "buscaBaixa", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vm_baixa;
        }

        //alterar baixa
        public string alterarBaixa(            
            int usuario_id,
            int conta_id,
            int baixa_id,            
            int contaCorrente,
            DateTime data,
            Decimal valor,
            string memorando,
            Decimal juros,
            Decimal multa,
            Decimal desconto
            )
        {
            string retorno = "Baixa alterada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_alteraBaixa (@baixa_id, @contaCorrente, @data, @valor, @memorando, @juros, @multa, @desconto)";                              
                comando.Parameters.AddWithValue("@contaCorrente", contaCorrente);
                comando.Parameters.AddWithValue("@data", data);
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@memorando", memorando);
                comando.Parameters.AddWithValue("@juros", juros);
                comando.Parameters.AddWithValue("@multa", multa);
                comando.Parameters.AddWithValue("@desconto", desconto);
                comando.Parameters.AddWithValue("@baixa_id", baixa_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração da baixa ID: " + baixa_id + " alterada com sucesso";
                log.log("Op_parcelas_baixa", "alterarBaixa", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao realizar a alteração da baixa. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas_baixa", "alterarBaixa", "Erro", msg, conta_id, usuario_id);
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

        //alterar baixa
        public string excluirBaixa(
            int usuario_id,
            int conta_id,
            int baixa_id            
            )
        {
            string retorno = "Baixa excluída com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_excluirBaixa (@baixa_id)";            
                comando.Parameters.AddWithValue("@baixa_id", baixa_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão da baixa ID: " + baixa_id + " excluida com sucesso";
                log.log("Op_parcelas_baixa", "excluirBaixa", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao realizar a exclusão da baixa. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas_baixa", "excluirBaixa", "Erro", msg, conta_id, usuario_id);
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
