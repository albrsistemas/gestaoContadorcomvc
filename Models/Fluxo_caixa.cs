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
            MySqlDataReader fluxo;

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
                comando.CommandText = "SELECT sum(if(yccm.ccm_movimento = 'Recebimento', yccm.ccm_valor, -yccm.ccm_valor)) as movimentos from conta_corrente_mov as yccm WHERE yccm.ccm_conta_id = @conta_id_2 and yccm.ccm_ccorrente_id = @contaCorrente_2 and yccm.ccm_data < @dataInicial_2;";
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

                //Fluxo de lançamentos do período
                comando.CommandText = "SELECT xccm.ccm_id as id, xccm.ccm_data as data, xccm.ccm_memorando as memorando, if(xccm.ccm_movimento = 'Recebimento', xccm.ccm_valor, -xccm.ccm_valor) as valor from conta_corrente_mov as xccm WHERE xccm.ccm_conta_id = @conta_id_3 and xccm.ccm_ccorrente_id = @contaCorrente_3 and xccm.ccm_data BETWEEN @dataInicial_3 AND @dataFinal ORDER by xccm.ccm_data ASC;";
                comando.Parameters.AddWithValue("@contaCorrente_3", contaCorrente);
                comando.Parameters.AddWithValue("@conta_id_3", conta_id);
                comando.Parameters.AddWithValue("@dataInicial_3", dataInicial);
                comando.Parameters.AddWithValue("@dataFinal", dataFinal);
                comando.ExecuteNonQuery();

                fluxo = comando.ExecuteReader();
                if (fluxo.HasRows)
                {
                    while (fluxo.Read())
                    {
                        Fluxo_caixa fluxo_cx = new Fluxo_caixa();

                        if (DBNull.Value != fluxo["id"])
                        {
                            fluxo_cx.id = Convert.ToInt32(fluxo["id"]);
                        }
                        else
                        {
                            fluxo_cx.id = 0;
                        }

                        if (DBNull.Value != fluxo["data"])
                        {
                            fluxo_cx.data = Convert.ToDateTime(fluxo["data"]);
                        }
                        else
                        {
                            fluxo_cx.data = new DateTime();
                        }

                        fluxo_cx.memorando = fluxo["memorando"].ToString();

                        if (DBNull.Value != fluxo["valor"])
                        {
                            fluxo_cx.valor = Convert.ToDecimal(fluxo["valor"]);
                        }
                        else
                        {
                            fluxo_cx.valor = 0;
                        }

                        fc.Add(fluxo_cx);
                    }

                    vm_fc.fluxo = fc;
                }
                fluxo.Close();

                Transacao.Commit();
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

            return vm_fc;
        }
    }
}
