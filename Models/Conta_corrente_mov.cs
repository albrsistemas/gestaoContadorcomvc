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
    public class Conta_corrente_mov
    {
        public int ccm_id { get; set; }
        public int ccm_conta_id { get; set; }
        public int ccm_ccorrente_id { get; set; }
        public string ccm_movimento { get; set; }
        public string ccm_contra_partida_tipo { get; set; }
        public int ccm_contra_partida_id { get; set; }
        public DateTime ccm_data { get; set; }
        public DateTime ccm_dataCriacao { get; set; }
        public Decimal ccm_valor { get; set; }
        public int ccm_op_id { get; set; }
        public int ccm_oppb_id { get; set; }
        public string ccm_memorando { get; set; }
        public string ccm_origem { get; set; }
        public int ccm_participante_id { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Conta_corrente_mov()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Lista movimento conta corrente
        public List<Vm_conta_corrente_mov> listaCCM(int usuario_id, int conta_id, int contacorrente_id, DateTime dataInicio, DateTime dataFim)
        {   
            List<Vm_conta_corrente_mov> ccms = new List<Vm_conta_corrente_mov>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_fluxoCaixa(@conta_id,@contacorrente_id,@dataInicio,@dataFim);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@contacorrente_id", contacorrente_id);
                comando.Parameters.AddWithValue("@dataInicio", dataInicio);
                comando.Parameters.AddWithValue("@dataFim", dataFim);
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_conta_corrente_mov mov = new Vm_conta_corrente_mov();

                        if (DBNull.Value != leitor["abertura"])
                        {
                            mov.abertura = Convert.ToDecimal(leitor["abertura"]);
                        }
                        else
                        {
                            mov.abertura = 0;
                        }

                        if (DBNull.Value != leitor["data"])
                        {
                            mov.data = Convert.ToDateTime(leitor["data"]);
                        }
                        else
                        {
                            mov.data = new DateTime();
                        }

                        mov.categoria = leitor["categoria"].ToString();
                        mov.memorando = leitor["memorando"].ToString();
                        mov.participante = leitor["cliente_fornecedor"].ToString();

                        if (DBNull.Value != leitor["valor"])
                        {
                            mov.valor = Convert.ToDecimal(leitor["valor"]);
                        }
                        else
                        {
                            mov.valor = 0;
                        }

                        if (DBNull.Value != leitor["saldo"])
                        {
                            mov.saldo = Convert.ToDecimal(leitor["saldo"]);
                        }
                        else
                        {
                            mov.saldo = 0;
                        }

                        ccms.Add(mov);   
                    }                    
                }               
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Conta_corrente_mov", "Vm_conta_corrente_mov", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return ccms;
        }

        //Criar lançamento caixa
        public string cadastrarCCM(int usuario_id, int conta_id, DateTime data, Decimal valor, string memorando, int categoria_id, int participante_id, int ccorrente_id)
        {
            string retorno = "Lançamento cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_cadastrarCCM(@ccm_conta_id, @ccm_ccorrente_id, @ccm_data, @ccm_valor, @ccm_memorando, @ccm_participante_id, @categoria_id)";
                comando.Parameters.AddWithValue("@ccm_conta_id", conta_id);
                comando.Parameters.AddWithValue("@ccm_ccorrente_id", ccorrente_id);
                comando.Parameters.AddWithValue("@ccm_data", data);
                comando.Parameters.AddWithValue("@ccm_valor", valor);
                comando.Parameters.AddWithValue("@ccm_memorando", memorando);
                comando.Parameters.AddWithValue("@ccm_participante_id", participante_id);
                comando.Parameters.AddWithValue("@categoria_id", categoria_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Lançamento conta corrente movimento cadastrado com sucesso";
                log.log("Conta_corrente_mov", "cadastrarCCM", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar o participante. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Conta_corrente_mov", "cadastrarCCM", "Erro", msg, conta_id, usuario_id);
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
