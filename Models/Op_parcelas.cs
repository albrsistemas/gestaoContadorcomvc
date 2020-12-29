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
    public class Op_parcelas
    {
        public int op_parcela_id { get; set; }
        public int op_parcela_dias { get; set; }
        public DateTime op_parcela_vencimento { get; set; }
        public int op_parcela_fp_id { get; set; }
        public int op_parcela_op_id { get; set; }
        public Decimal op_parcela_valor_bruto { get; set; }
        public Decimal op_parcela_valor { get; set; }
        public Decimal op_parcela_saldo { get; set; }
        public string op_parcela_obs { get; set; }
        public string controleEdit { get; set; } //Campo criado para controle edição. Não consta no banco de dados.
        public Decimal op_parcela_ret_inss { get; set; }
        public Decimal op_parcela_ret_issqn { get; set; }
        public Decimal op_parcela_ret_irrf { get; set; }
        public Decimal op_parcela_ret_pis { get; set; }
        public Decimal op_parcela_ret_cofins { get; set; }
        public Decimal op_parcela_ret_csll { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Op_parcelas()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //detalhamento de parcdelas
        public Vm_detalhamento_parcela detalhamentoParcelas(int usuario_id, int conta_id, int parcela_id)
        {
            Vm_detalhamento_parcela vm_dp = new Vm_detalhamento_parcela();
            ContasPagar conta = new ContasPagar();
            List<parcelas_relacionadas> parcelas_relacionadas = new List<parcelas_relacionadas>();
            List<baixas> baixas = new List<baixas>();            

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;
            MySqlDataReader parc;
            MySqlDataReader relacionadas;
            MySqlDataReader b;

            try
            {
                DateTime today = DateTime.Today;

                //Parcela
                comando.CommandText = "call pr_detalhamentoParcelasCP(@conta,@parcela,@hoje);";                
                comando.Parameters.AddWithValue("@conta", conta_id);
                comando.Parameters.AddWithValue("@parcela", parcela_id);
                comando.Parameters.AddWithValue("@hoje", today);

                parc = comando.ExecuteReader();
                if (parc.HasRows)
                {
                    while (parc.Read())
                    {
                        

                        if (DBNull.Value != parc["parcela_id"])
                        {
                            conta.parcela_id = Convert.ToInt32(parc["parcela_id"]);
                        }
                        else
                        {
                            conta.parcela_id = 0;
                        }

                        if (DBNull.Value != parc["fp_meio_pgto_nfe"])
                        {
                            conta.meio_pgto = Convert.ToInt32(parc["fp_meio_pgto_nfe"]);
                        }
                        else
                        {
                            conta.meio_pgto = 0;
                        }

                        if (DBNull.Value != parc["fp_id"])
                        {
                            conta.fp_id = Convert.ToInt32(parc["fp_id"]);
                        }
                        else
                        {
                            conta.fp_id = 0;
                        }

                        if (DBNull.Value != parc["operacao_"])
                        {
                            conta.operacao = Convert.ToInt32(parc["operacao_"]);
                        }
                        else
                        {
                            conta.operacao = 0;
                        }

                        if (DBNull.Value != parc["venc"])
                        {
                            conta.vencimento = Convert.ToDateTime(parc["venc"]);
                        }
                        else
                        {
                            conta.vencimento = new DateTime();
                        }

                        conta.fornecedor = parc["participante"].ToString();
                        conta.referencia = parc["referencia"].ToString();
                        conta.formaPgto = parc["formaPgto"].ToString();
                        conta.tipo = parc["tipo"].ToString();

                        if (DBNull.Value != parc["valorOriginal"])
                        {
                            conta.valorOriginal = Convert.ToDecimal(parc["valorOriginal"]);
                        }
                        else
                        {
                            conta.valorOriginal = 0;
                        }

                        if (DBNull.Value != parc["baixas"])
                        {
                            conta.baixas = Convert.ToDecimal(parc["baixas"]);
                        }
                        else
                        {
                            conta.baixas = 0;
                        }

                        if (DBNull.Value != parc["saldo"])
                        {
                            conta.saldo = Convert.ToDecimal(parc["saldo"]);
                        }
                        else
                        {
                            conta.saldo = 0;
                        }

                        if (DBNull.Value != parc["prazo"])
                        {
                            conta.prazo = Convert.ToInt32(parc["prazo"]);
                        }
                        else
                        {
                            conta.prazo = 0;
                        }
                    }
                }
                parc.Close();

                //Parcelas relaconadas
                comando.CommandText = "call pr_parcelas_referenciadas(@conta_id,@parcela_id);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@parcela_id", parcela_id);

                relacionadas = comando.ExecuteReader();
                if (relacionadas.HasRows)
                {
                    while (relacionadas.Read())
                    {
                        parcelas_relacionadas pr = new parcelas_relacionadas();


                        if (DBNull.Value != relacionadas["op_parcela_vencimento"])
                        {
                            pr.vencimento = Convert.ToDateTime(relacionadas["op_parcela_vencimento"]);
                        }
                        else
                        {
                            pr.vencimento = new DateTime();
                        }

                        if (DBNull.Value != relacionadas["op_parcela_valor"])
                        {
                            pr.valor = Convert.ToDecimal(relacionadas["op_parcela_valor"]);
                        }
                        else
                        {
                            pr.valor = 0;
                        }

                        pr.referencia = relacionadas["referencia"].ToString();

                        parcelas_relacionadas.Add(pr);
                    }
                }
                relacionadas.Close();

                //Baixas
                comando.CommandText = "SELECT conta_corrente.ccorrente_nome, op_parcelas_baixa.* from op_parcelas_baixa LEFT join conta_corrente on conta_corrente.ccorrente_id = op_parcelas_baixa.oppb_conta_corrente WHERE op_parcelas_baixa.oppb_op_parcela_id = @p_id;";
                comando.Parameters.AddWithValue("@p_id", parcela_id);

                b = comando.ExecuteReader();
                if (b.HasRows)
                {
                    while (b.Read())
                    {
                        baixas ba = new baixas();

                        if (DBNull.Value != b["oppb_data"])
                        {
                            ba.data = Convert.ToDateTime(b["oppb_data"]);
                        }
                        else
                        {
                            ba.data = new DateTime();
                        }

                        if (DBNull.Value != b["oppb_valor"])
                        {
                            ba.valor = Convert.ToDecimal(b["oppb_valor"]);
                        }
                        else
                        {
                            ba.valor = 0;
                        }

                        ba.acrescimos = Convert.ToDecimal(b["oppb_juros"]) + Convert.ToDecimal(b["oppb_multa"]);
                        ba.descontos = Convert.ToDecimal(b["oppb_desconto"]);
                        ba.conta_corrente = b["ccorrente_nome"].ToString();

                        baixas.Add(ba);
                    }
                }

                vm_dp.parcela = conta;
                vm_dp.parcelas_referenciadas = parcelas_relacionadas;
                vm_dp.baixas = baixas;

            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas", "detalhamentoParcelas", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vm_dp;
        }

        public string deleteParcelaCartaoCredito(int usuario_id, int conta_id, int parcela_id)
        {
            string retorno = "Fatura cartao de crédito excluída com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;
            
            try
            {
                //Parcela
                comando.CommandText = "call pr_excluirFaturaCartao(@nConta_id,@nParcela_id);";
                comando.Parameters.AddWithValue("@nConta_id", conta_id);
                comando.Parameters.AddWithValue("@nParcela_id", parcela_id);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Exclusão da fatura de cartão de crédito de parcela_id: " + parcela_id + " excluída com sucesso";
                log.log("Op_parcelas", "deleteParcelaCartaoCredito", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir a fatura do cartão de crédito. Tente novamente. Se persistir entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas", "deleteParcelaCartaoCredito", "Erro", msg, conta_id, usuario_id);
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

        //Altera data de vencimento parcela
        public string alteraVencimentoParcela(int usuario_id, int conta_id, int parcela_id, DateTime venc)
        {
            string retorno = "Data de vencimento alterada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                //Parcela
                comando.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcela_vencimento_alterado = @data WHERE op_parcelas.op_parcela_id = @parcela_id;";
                comando.Parameters.AddWithValue("@data", venc);
                comando.Parameters.AddWithValue("@parcela_id", parcela_id);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Alteração data de vencimento da parcela_id: " + parcela_id + " alterada com sucesso";
                log.log("Op_parcelas", "alteraVencimentoParcela", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar o vencimento da parcela. Tente novamente. Se persistir entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Op_parcelas", "alteraVencimentoParcela", "Erro", msg, conta_id, usuario_id);
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
