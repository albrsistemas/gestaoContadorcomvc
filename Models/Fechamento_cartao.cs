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
        public string fc_matriz_parcelas { get; set; }

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
        public string cadastraFechamentoCartao(int conta_id, int usuario_id, Fechamento_cartao fc)
        {
            string retorno = "Fatura cartão cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                DateTime today = DateTime.Today;

                string matriz = "";

                for (int x = 0; x < fc.fc_matriz_parcelas.Length; x++)
                {
                    if (x + 1 == fc.fc_matriz_parcelas.Length)
                    {
                        matriz += fc.fc_matriz_parcelas[x];
                    }
                    else
                    {
                        matriz += fc.fc_matriz_parcelas[x] + ", ";
                    }

                }

                comando.CommandText = "call pr_fechamentoCartao (@conta_id, @hoje, @fc_forma_pagamento, @fc_qtd_parcelas, @fc_valor_total, @fc_tarifas_bancarias, @fc_seguro_cartao, @fc_abatimentos_cartao, @fc_acrescimos_cartao, @fc_referencia, @fc_vencimento, @fc_nome_cartao, @fc_matriz_parcelas);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
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
                comando.Parameters.AddWithValue("@fc_matriz_parcelas", matriz);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro fatura cartão " + fc.fc_nome_cartao + " da referência " + fc.fc_referencia + " Cadastrado com sucesso";
                log.log("Fechamento_cartao", "cadastraFechamentoCartao", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a fatura do cartão. Tente novamente. Se persistir, entre em contato com o suporte!";

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




    }
}
