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
    public class Fluxo_caixa
    {
        public int id { get; set; }
        public DateTime data { get; set; }
        public string memorando { get; set; }
        public Decimal valor { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Fluxo_caixa()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Listar fluxo de caixa
        public Vm_fluxo_caixa fluxoCaixa(int usuario_id, int conta_id, int contaCorrente, DateTime dataInicial, DateTime dataFinal)
        {
            Vm_fluxo_caixa vm_fc = new Vm_fluxo_caixa();
            List<Fluxo_caixa> fc = new List<Fluxo_caixa>();            

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;
            MySqlDataReader saldo;
            MySqlDataReader movimentos;

            try
            {
                //Saldo de abertura
                comando.CommandText = "SELECT COALESCE(cc.ccorrente_saldo_abertura, 0.00) as saldoAbertura from conta_corrente as cc WHERE cc.ccorrente_conta_id = @conta_id and cc.ccorrente_id = @contaCorrente;";
                comando.Parameters.AddWithValue("@contaCorrente", contaCorrente);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                saldo = comando.ExecuteReader();
                if (saldo.HasRows)
                {
                    while (saldo.Read())
                    {
                        vm_fc.saldo_abertura = Convert.ToDecimal(saldo["saldoAbertura"]);
                    }
                }
                saldo.Close();

                //Movimentos anteriores a data inicial
                comando.CommandText = "SELECT sum(if(yccm.ccm_movimento = 'Recebimento', yccm.ccm_valor, -yccm.ccm_valor)) as movimentos from conta_corrente_mov as yccm WHERE yccm.ccm_conta_id = @conta_id_2 and yccm.ccm_ccorrente_id = @contaCorrente_2 and yccm.ccm_data < @dataInicial_2;;";
                comando.Parameters.AddWithValue("@contaCorrente_2", contaCorrente);
                comando.Parameters.AddWithValue("@conta_id_2", conta_id);
                comando.Parameters.AddWithValue("@dataInicial_2", dataInicial);
                comando.ExecuteNonQuery();

                movimentos = comando.ExecuteReader();
                if (movimentos.HasRows)
                {
                    while (movimentos.Read())
                    {
                        vm_fc.saldo_movimentos = Convert.ToDecimal(saldo["movimentos"]);
                    }
                }
                movimentos.Close();








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



    }
}
