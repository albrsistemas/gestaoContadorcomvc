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
        public int op_id { get; set; }
        public int baixa_id { get; set; }
        public string contra_partida_tipo { get; set; }
        public string ccm_origem { get; set; }

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
        public Vm_fluxo_caixa fluxoCaixa(int usuario_id, int conta_id, int contaCorrente, DateTime dataInicial, DateTime dataFinal, int tipoOperacao, int nOperacao, int participante_id)
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
                comando.CommandText = "SELECT COALESCE(sum(if(yccm.ccm_movimento = 'E', yccm.ccm_valor, -yccm.ccm_valor)),0.00) as movimentos from conta_corrente_mov as yccm WHERE yccm.ccm_conta_id = @conta_id_2 and yccm.ccm_ccorrente_id = @contaCorrente_2 and yccm.ccm_data < @dataInicial_2;";
                comando.Parameters.AddWithValue("@contaCorrente_2", contaCorrente);
                comando.Parameters.AddWithValue("@conta_id_2", conta_id);
                comando.Parameters.AddWithValue("@dataInicial_2", dataInicial);
                comando.ExecuteNonQuery();

                movimentos = comando.ExecuteReader();
                if (movimentos.HasRows)
                {
                    while (movimentos.Read())
                    {
                        vm_fc.saldo_movimentos = Convert.ToDecimal(movimentos["movimentos"]);
                    }
                }
                movimentos.Close();

                string filter = "xccm.ccm_conta_id = @conta_id_3 and xccm.ccm_ccorrente_id = @contaCorrente_3 and xccm.ccm_data BETWEEN @dataInicial_3 AND @dataFinal";
                if(tipoOperacao != 0)
                {
                    filter += " and op.op_tipo = @tipoOperacao";                    
                }
                if(participante_id != 0)
                {
                    filter += " and (xccm.ccm_contra_partida_tipo = 'Participante' and xccm.ccm_contra_partida_id = @participante_id)";                    
                }
                if(nOperacao != 0)
                {
                    filter += " and op.op_numero_ordem = @nOperacao";                    
                }

                string cmd = "SELECT xccm.ccm_id as id, xccm.ccm_data as data, concat(xccm.ccm_memorando, '. ', COALESCE(op.op_obs,'')) as memorando, if(xccm.ccm_movimento = 'E', xccm.ccm_valor, -xccm.ccm_valor) as valor, xccm.ccm_op_id as op_id, op.op_tipo, op.op_numero_ordem, xccm.ccm_oppb_id as baixa_id, xccm.ccm_contra_partida_tipo as contra_partida_tipo, xccm.ccm_contra_partida_id, xccm.ccm_origem from conta_corrente_mov as xccm LEFT join operacao as op on op.op_id = xccm.ccm_op_id WHERE " + filter + " ORDER by xccm.ccm_data ASC;";
                //Fluxo de lançamentos do período
                //comando.CommandText = "SELECT xccm.ccm_id as id, xccm.ccm_data as data, xccm.ccm_memorando as memorando, if(xccm.ccm_movimento = 'Recebimento', xccm.ccm_valor, -xccm.ccm_valor) as valor, xccm.ccm_op_id as op_id, xccm.ccm_oppb_id as baixa_id, xccm.ccm_contra_partida_tipo as contra_partida_tipo, xccm.ccm_contra_partida_id from conta_corrente_mov as xccm WHERE xccm.ccm_conta_id = @conta_id_3 and xccm.ccm_ccorrente_id = @contaCorrente_3 and xccm.ccm_data BETWEEN @dataInicial_3 AND @dataFinal ORDER by xccm.ccm_data ASC;";
                comando.CommandText = cmd;
                comando.Parameters.AddWithValue("@contaCorrente_3", contaCorrente);
                comando.Parameters.AddWithValue("@conta_id_3", conta_id);
                comando.Parameters.AddWithValue("@dataInicial_3", dataInicial);
                comando.Parameters.AddWithValue("@dataFinal", dataFinal);
                if (tipoOperacao != 0)
                {                    
                    comando.Parameters.AddWithValue("@tipoOperacao", tipoOperacao);
                }
                if (participante_id != 0)
                {                 
                    comando.Parameters.AddWithValue("@participante_id", participante_id);
                }
                if (nOperacao != 0)
                {                    
                    comando.Parameters.AddWithValue("@nOperacao", nOperacao);
                }
                comando.ExecuteNonQuery();

                if(tipoOperacao > 0 || participante_id > 0 || nOperacao > 0)
                {
                    vm_fc.filtro = "Informações com filtros!";
                }
                else
                {
                    vm_fc.filtro = "Sem filtros!";
                }

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

                        if (DBNull.Value != fluxo["op_id"])
                        {
                            fluxo_cx.op_id = Convert.ToInt32(fluxo["op_id"]);
                        }
                        else
                        {
                            fluxo_cx.op_id = 0;
                        }

                        if (DBNull.Value != fluxo["baixa_id"])
                        {
                            fluxo_cx.baixa_id = Convert.ToInt32(fluxo["baixa_id"]);
                        }
                        else
                        {
                            fluxo_cx.baixa_id = 0;
                        }

                        fluxo_cx.contra_partida_tipo = fluxo["contra_partida_tipo"].ToString();
                        fluxo_cx.ccm_origem = fluxo["ccm_origem"].ToString();

                        fc.Add(fluxo_cx);
                    }
                }
                fluxo.Close();

                vm_fc.fluxo = fc;

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

        //Transferência entre contas correntes
        public string transferencia(int usuario_id, int conta_id, DateTime data, Decimal valor, int ccorrente_de, int ccorrente_para, string memorando)
        {
            string retorno = "Transferência realizada com sucesso";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;            

            try
            {
                comando.CommandText = "call pr_transferencia(@de, @para, @valor, @data, @memorando, @conta_id)";                
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@de", ccorrente_de);
                comando.Parameters.AddWithValue("@para", ccorrente_para);
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@data", data);
                comando.Parameters.AddWithValue("@memorando", memorando);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Transferência no valor de " + valor.ToString("N") + " realizada com sucesso";
                log.log("Fluxo_caixa", "Transferencia", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao efeturar a transferência. Tente novamente, se persistir, entre em contato com o suporte!";
                string msg = e.Message.Substring(0, 300);
                log.log("Fluxo_caixa", "Transferencia", "Erro", msg, conta_id, usuario_id);
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

        //Busca dados transferência entre contas correntes
        public Vm_transferencia buscaTransferencia(int usuario_id, int conta_id, int ccm_id)
        {
            Vm_transferencia transf = new Vm_transferencia();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT ccm.ccm_id, if(ccm.ccm_movimento = 'S', ccm.ccm_ccorrente_id, (SELECT conta_corrente_mov.ccm_ccorrente_id from conta_corrente_mov WHERE conta_corrente_mov.ccm_id = ccm.ccm_origem_id)) as de, if(ccm.ccm_movimento = 'S', (SELECT conta_corrente_mov.ccm_ccorrente_id from conta_corrente_mov WHERE conta_corrente_mov.ccm_id = ccm.ccm_origem_id) ,ccm.ccm_ccorrente_id) as para, ccm.ccm_data as data, ccm.ccm_valor as valor, ccm.ccm_memorando as memorando from conta_corrente_mov as ccm WHERE ccm.ccm_id = @ccm_id and ccm.ccm_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@ccm_id", ccm_id);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["ccm_id"])
                        {
                            transf.ccm_id = Convert.ToInt32(leitor["ccm_id"]);
                        }
                        else
                        {
                            transf.ccm_id = 0;
                        }

                        if (DBNull.Value != leitor["de"])
                        {
                            transf.ccorrente_de = Convert.ToInt32(leitor["de"]);
                        }
                        else
                        {
                            transf.ccorrente_de = 0;
                        }

                        if (DBNull.Value != leitor["para"])
                        {
                            transf.ccorrente_para = Convert.ToInt32(leitor["para"]);
                        }
                        else
                        {
                            transf.ccorrente_para = 0;
                        }

                        if (DBNull.Value != leitor["data"])
                        {
                            transf.data = Convert.ToDateTime(leitor["data"]);
                        }
                        else
                        {
                            transf.data = new DateTime();
                        }

                        if (DBNull.Value != leitor["valor"])
                        {
                            transf.valor = Convert.ToDecimal(leitor["valor"]);
                        }
                        else
                        {
                            transf.valor = 0;
                        }

                        transf.memorando = leitor["memorando"].ToString();

                    }
                }

            }
            catch (Exception e)
            {                
                string msg = e.Message.Substring(0, 300);
                log.log("Fluxo_caixa", "buscaTransferencia", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return transf;
        }

        //Alterar dados transferência entre contas correntes
        public string alteraTransferencia(int usuario_id, int conta_id, int ccm_id, DateTime data, Decimal valor, int ccorrente_de, int ccorrente_para, string memorando)
        {
            string retorno = "Transferência alterada com sucesso";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_altera_transferencia(@ccm_id, @de, @para, @valor, @data, @memorando, @conta_id)";
                comando.Parameters.AddWithValue("@ccm_id", ccm_id);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@de", ccorrente_de);
                comando.Parameters.AddWithValue("@para", ccorrente_para);
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@data", data);
                comando.Parameters.AddWithValue("@memorando", memorando);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração da transferência no valor de " + valor.ToString("N") + "da data "+ data.ToShortDateString() + " realizada com sucesso";
                log.log("Fluxo_caixa", "alteraTransferencia", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar a transferência. Tente novamente, se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Fluxo_caixa", "alteraTransferencia", "Erro", msg, conta_id, usuario_id);
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

        //Excluir transferência entre contas correntes
        public string excluirTransferencia(int usuario_id, int conta_id, int ccm_id)
        {
            string retorno = "Transferência excluida com sucesso";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_excluir_transferencia(@ccm_id, @conta_id)";
                comando.Parameters.AddWithValue("@ccm_id", ccm_id);
                comando.Parameters.AddWithValue("@conta_id", conta_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão da transferência ID " + ccm_id + " realizada com sucesso";
                log.log("Fluxo_caixa", "excluirTransferencia", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir a transferência. Tente novamente, se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Fluxo_caixa", "excluirTransferencia", "Erro", msg, conta_id, usuario_id);
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
