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
    public class ContasPagar
    {
        public int operacao { get; set; }
        public DateTime vencimento { get; set; }
        public string fornecedor { get; set; }
        public string referencia { get; set; }
        public Decimal valorOriginal { get; set; }
        public Decimal baixas { get; set; }
        public Decimal saldo { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ContasPagar()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //lista contas a pagar para index
        public Vm_contas_pagar listaContasPagar(int usuario_id, int conta_id)
        {
            List<ContasPagar> hoje = new List<ContasPagar>();
            List<ContasPagar> atrasadas = new List<ContasPagar>();
            List<ContasPagar> proximas = new List<ContasPagar>();
            Vm_contas_pagar cp = new Vm_contas_pagar();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;                        

            try
            {
                DateTime today = DateTime.Today;

                comando.CommandText = "SELECT p.op_parcela_op_id as operacao_, p.op_parcela_vencimento as vencimento, pa.op_part_nome as fornecedor, concat(operacao.op_tipo,' número: ', operacao.op_numero_ordem) as referencia, p.op_parcela_valor as valorOriginal, (SELECT COALESCE(sum(b.oppb_valor), 0.00) from op_parcelas_baixa as b WHERE b.oppb_op_parcela_id = p.op_parcela_id) as baixas, (SELECT COALESCE(sum(valorOriginal - baixas), 0.00)) as saldo, (SELECT IF(vencimento = @data_inicial, 'YES', 'NO')) as hoje, (SELECT IF(vencimento < @data_inicial, 'YES', 'NO')) as atrasadas, (SELECT IF(vencimento > @data_inicial, 'YES', 'NO')) as proximas from op_parcelas as p LEFT join op_participante as pa on pa.op_id = p.op_parcela_op_id left JOIN operacao on operacao.op_id = p.op_parcela_op_id WHERE operacao.op_conta_id = @conta_id and operacao.op_tipo = 'Compra';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@data_inicial", today);
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        ContasPagar conta = new ContasPagar();

                        if (DBNull.Value != leitor["operacao_"])
                        {
                            conta.operacao = Convert.ToInt32(leitor["operacao_"]);
                        }
                        else
                        {
                            conta.operacao = 0;
                        }

                        if (DBNull.Value != leitor["vencimento"])
                        {
                            conta.vencimento = Convert.ToDateTime(leitor["vencimento"]);
                        }
                        else
                        {
                            conta.vencimento = new DateTime();
                        }

                        conta.fornecedor = leitor["fornecedor"].ToString();
                        conta.referencia = leitor["referencia"].ToString();

                        if (DBNull.Value != leitor["valorOriginal"])
                        {
                            conta.valorOriginal = Convert.ToDecimal(leitor["valorOriginal"]);
                        }
                        else
                        {
                            conta.valorOriginal = 0;
                        }

                        if (DBNull.Value != leitor["valorOriginal"])
                        {
                            conta.valorOriginal = Convert.ToDecimal(leitor["valorOriginal"]);
                        }
                        else
                        {
                            conta.valorOriginal = 0;
                        }

                        if (DBNull.Value != leitor["baixas"])
                        {
                            conta.baixas = Convert.ToDecimal(leitor["baixas"]);
                        }
                        else
                        {
                            conta.baixas = 0;
                        }

                        if (DBNull.Value != leitor["saldo"])
                        {
                            conta.saldo = Convert.ToDecimal(leitor["saldo"]);
                        }
                        else
                        {
                            conta.saldo = 0;
                        }

                        string hoje_ = leitor["hoje"].ToString();
                        string atr = leitor["atrasadas"].ToString();
                        string pro = leitor["proximas"].ToString();

                        if(hoje_ == "YES")
                        {
                            hoje.Add(conta);
                        }

                        if (atr == "YES")
                        {
                            atrasadas.Add(conta);
                        }

                        if (pro == "YES")
                        {
                            proximas.Add(conta);
                        }
                    }
                } 

                cp.cp_hoje = hoje;
                cp.cp_atrasadas = atrasadas;
                cp.cp_proximas = proximas;                
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("ContasPagar", "listaContasPagar", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return cp;
        }

    }
}
