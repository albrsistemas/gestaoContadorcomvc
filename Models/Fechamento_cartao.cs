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
    public class Fechamento_cartao
    {
        public int fc_id { get; set; }
        public int fc_conta_id { get; set; }
        public int fc_forma_pagamento { get; set; }
        public int fc_qtd_parcelas { get; set; }
        public int fc_op_id { get; set; }
        public Decimal fc_valor_total { get; set; }
        public Decimal fc_seguro_cartao { get; set; }
        public Decimal fc_abatimentos_cartao { get; set; }
        public Decimal fc_acrescimos_cartao { get; set; }
        public Decimal fc_tarifas_bancarias { get; set; }
        public DateTime fc_vencimento { get; set; }
        public string fc_referencia { get; set; }
        public string fc_nome_cartao { get; set; }
        public string fc_matriz_parcelas_text { get; set; }
        public int fc_forma_pgto_boleto_fatura { get; set; }
        public string fc_op_obs { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Fechamento_cartao()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Cadastrar fechamento cartão
        public string cadastraFechamentoCartao(int conta_id, int usuario_id, Fechamento_cartao fc, bool fechamento_existe, int fechamento_existe_op, int fechamento_existe_fc_id)
        {
            string retorno = "Fatura cartão cadastrada com sucesso!";

            if (fechamento_existe)
            {
                retorno = "Fatura do cartão atualizada com sucesso!";
            }
            

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                DateTime today = DateTime.Today;

                comando.CommandText = "call pr_fechamentoCartao (@conta_id, @hoje, @fc_forma_pagamento, @fc_qtd_parcelas, @fc_valor_total, @fc_tarifas_bancarias, @fc_seguro_cartao, @fc_abatimentos_cartao, @fc_acrescimos_cartao, @fc_referencia, @fc_vencimento, @fc_nome_cartao, @fc_matriz_parcelas, @fc_forma_pgto_boleto_fatura, @fechamento_existe, @fechamento_existe_op, @fechamento_existe_fc_id, @fc_op_obs);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@fechamento_existe", fechamento_existe);
                comando.Parameters.AddWithValue("@fechamento_existe_op", fechamento_existe_op);
                comando.Parameters.AddWithValue("@fechamento_existe_fc_id", fechamento_existe_fc_id);
                comando.Parameters.AddWithValue("@hoje", today);
                comando.Parameters.AddWithValue("@fc_forma_pagamento", fc.fc_forma_pagamento);
                comando.Parameters.AddWithValue("@fc_qtd_parcelas", fc.fc_qtd_parcelas);
                comando.Parameters.AddWithValue("@fc_valor_total", fc.fc_valor_total);
                comando.Parameters.AddWithValue("@fc_tarifas_bancarias", fc.fc_tarifas_bancarias);
                comando.Parameters.AddWithValue("@fc_seguro_cartao", fc.fc_seguro_cartao);
                comando.Parameters.AddWithValue("@fc_abatimentos_cartao", fc.fc_abatimentos_cartao);
                comando.Parameters.AddWithValue("@fc_acrescimos_cartao", fc.fc_acrescimos_cartao);
                comando.Parameters.AddWithValue("@fc_referencia", fc.fc_referencia);
                comando.Parameters.AddWithValue("@fc_vencimento", fc.fc_vencimento);
                comando.Parameters.AddWithValue("@fc_nome_cartao", fc.fc_nome_cartao);
                comando.Parameters.AddWithValue("@fc_matriz_parcelas", fc.fc_matriz_parcelas_text);                
                comando.Parameters.AddWithValue("@fc_forma_pgto_boleto_fatura", fc.fc_forma_pgto_boleto_fatura);                
                comando.Parameters.AddWithValue("@fc_op_obs", fc.fc_op_obs);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro fatura cartão " + fc.fc_nome_cartao + " da referência " + fc.fc_referencia + " Cadastrado com sucesso";
                log.log("Fechamento_cartao", "cadastraFechamentoCartao", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a fatura do cartão. Tente novamente. Se persistir, entre em contato com o suporte!";

                if (fechamento_existe)
                {
                    retorno = "Erro ao atualizar a fatura do cartão. Tente novamente. Se persistir, entre em contato com o suporte!";
                }

                string msg = e.Message.Substring(0, 300);
                log.log("Fechamento_cartao", "cadastraFechamentoCartao", "Erro", msg, conta_id, usuario_id);
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

        public Fechamento_cartao buscaPorRef(int fc_forma_pagamento, string fc_referencia)
        {
            Fechamento_cartao fc = new Fechamento_cartao();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from fechamento_cartao as f WHERE f.fc_forma_pagamento = @fc_forma_pagamento and f.fc_referencia = @fc_referencia;";
                comando.Parameters.AddWithValue("@fc_forma_pagamento", fc_forma_pagamento);
                comando.Parameters.AddWithValue("@fc_referencia", fc_referencia);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["fc_id"])
                        {
                            fc.fc_id = Convert.ToInt32(leitor["fc_id"]);
                        }
                        else
                        {
                            fc.fc_id = 0;
                        }

                        if (DBNull.Value != leitor["fc_conta_id"])
                        {
                            fc.fc_conta_id = Convert.ToInt32(leitor["fc_conta_id"]);
                        }
                        else
                        {
                            fc.fc_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["fc_forma_pagamento"])
                        {
                            fc.fc_forma_pagamento = Convert.ToInt32(leitor["fc_forma_pagamento"]);
                        }
                        else
                        {
                            fc.fc_forma_pagamento = 0;
                        }

                        if (DBNull.Value != leitor["fc_qtd_parcelas"])
                        {
                            fc.fc_qtd_parcelas = Convert.ToInt32(leitor["fc_qtd_parcelas"]);
                        }
                        else
                        {
                            fc.fc_qtd_parcelas = 0;
                        }

                        if (DBNull.Value != leitor["fc_op_id"])
                        {
                            fc.fc_op_id = Convert.ToInt32(leitor["fc_op_id"]);
                        }
                        else
                        {
                            fc.fc_op_id = 0;
                        }

                        if (DBNull.Value != leitor["fc_forma_pgto_boleto_fatura"])
                        {
                            fc.fc_forma_pgto_boleto_fatura = Convert.ToInt32(leitor["fc_forma_pgto_boleto_fatura"]);
                        }
                        else
                        {
                            fc.fc_forma_pgto_boleto_fatura = 0;
                        }

                        if (DBNull.Value != leitor["fc_valor_total"])
                        {
                            fc.fc_valor_total = Convert.ToDecimal(leitor["fc_valor_total"]);
                        }
                        else
                        {
                            fc.fc_valor_total = 0;
                        }

                        if (DBNull.Value != leitor["fc_seguro_cartao"])
                        {
                            fc.fc_seguro_cartao = Convert.ToDecimal(leitor["fc_seguro_cartao"]);
                        }
                        else
                        {
                            fc.fc_seguro_cartao = 0;
                        }

                        if (DBNull.Value != leitor["fc_abatimentos_cartao"])
                        {
                            fc.fc_abatimentos_cartao = Convert.ToDecimal(leitor["fc_abatimentos_cartao"]);
                        }
                        else
                        {
                            fc.fc_abatimentos_cartao = 0;
                        }

                        if (DBNull.Value != leitor["fc_acrescimos_cartao"])
                        {
                            fc.fc_acrescimos_cartao = Convert.ToDecimal(leitor["fc_acrescimos_cartao"]);
                        }
                        else
                        {
                            fc.fc_acrescimos_cartao = 0;
                        }

                        if (DBNull.Value != leitor["fc_tarifas_bancarias"])
                        {
                            fc.fc_tarifas_bancarias = Convert.ToDecimal(leitor["fc_tarifas_bancarias"]);
                        }
                        else
                        {
                            fc.fc_tarifas_bancarias = 0;
                        }

                        if (DBNull.Value != leitor["fc_vencimento"])
                        {
                            fc.fc_vencimento = Convert.ToDateTime(leitor["fc_vencimento"]);
                        }
                        else
                        {
                            fc.fc_vencimento = new DateTime();
                        }

                        fc.fc_referencia = leitor["fc_referencia"].ToString();
                        fc.fc_nome_cartao = leitor["fc_nome_cartão"].ToString();
                        fc.fc_matriz_parcelas_text = leitor["fc_matriz_parcelas"].ToString();
                    }
                }
                else
                {
                    fc = null;
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

            return fc;
        }




    }
}
