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
        public string formaPgto { get; set; }
        public Decimal valorOriginal { get; set; }
        public Decimal baixas { get; set; }
        public Decimal saldo { get; set; }
        public int prazo { get; set; }
        public int parcela_id { get; set; }
        public int meio_pgto { get; set; }
        public int fp_id { get; set; }

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
        public Vm_contas_pagar listaContasPagar(int usuario_id, int conta_id, int participante, int formaPgto, int vencimento, int situacao, int tipo, string dataInicial, string dataFinal)
        {   
            List<ContasPagar> cps = new List<ContasPagar>();
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

                comando.CommandText = "call pr_contasPagar(@conta_id,@hoje,@participante,@formaPgto,@vencimento,@situacao,@tipo,@dataInicial,@dataFinal);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@hoje", today);
                comando.Parameters.AddWithValue("@participante", participante);
                comando.Parameters.AddWithValue("@formaPgto", formaPgto);
                comando.Parameters.AddWithValue("@vencimento", vencimento);
                comando.Parameters.AddWithValue("@situacao", situacao);
                comando.Parameters.AddWithValue("@tipo", tipo);
                comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                comando.Parameters.AddWithValue("@dataFinal", dataFinal);
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        ContasPagar conta = new ContasPagar();

                        if (DBNull.Value != leitor["parcela_id"])
                        {
                            conta.parcela_id = Convert.ToInt32(leitor["parcela_id"]);
                        }
                        else
                        {
                            conta.parcela_id = 0;
                        }

                        if (DBNull.Value != leitor["fp_meio_pgto_nfe"])
                        {
                            conta.meio_pgto = Convert.ToInt32(leitor["fp_meio_pgto_nfe"]);
                        }
                        else
                        {
                            conta.meio_pgto = 0;
                        }

                        if (DBNull.Value != leitor["fp_id"])
                        {
                            conta.fp_id = Convert.ToInt32(leitor["fp_id"]);
                        }
                        else
                        {
                            conta.fp_id = 0;
                        }

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

                        conta.fornecedor = leitor["participante"].ToString();
                        conta.referencia = leitor["referencia"].ToString();                       
                        conta.formaPgto = leitor["formaPgto"].ToString();                       

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

                        if (DBNull.Value != leitor["prazo"])
                        {
                            conta.prazo = Convert.ToInt32(leitor["prazo"]);
                        }
                        else
                        {
                            conta.prazo = 0;
                        }

                        cps.Add(conta);
                    }
                } 
                
                cp.contasPagar = cps;
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

    public class cp_filter
    {
        public string dataInicial { get; set; }
        public string dataFinal { get; set; }
        public int vencimento { get; set; }
        public string fornecedor_nome { get; set; }
        public int fornecedor_id { get; set; }
        public int formaPgto { get; set; }
        public int operacao { get; set; }
        public int situacao { get; set; }
        public bool contexto { get; set; } //oriente a view se as informações estão filtradas ou não;
    }
}
