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
        public string cadastrarCCM(int usuario_id, int conta_id, DateTime data, DateTime ccm_data_competencia, Decimal valor, string memorando, int categoria_id, int participante_id, int ccorrente_id, bool ccm_nf, DateTime ccm_nf_data_emissao, Decimal ccm_nf_valor, string ccm_nf_serie, string ccm_nf_numero, string ccm_nf_chave)
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
                comando.CommandText = "call pr_cadastrarCCM(@ccm_conta_id, @ccm_ccorrente_id, @ccm_data, @ccm_valor, @ccm_memorando, @ccm_participante_id, @categoria_id, @ccm_nf, @ccm_nf_data_emissao, @ccm_nf_valor, @ccm_nf_serie, @ccm_nf_numero, @ccm_nf_chave, @ccm_data_competencia)";
                comando.Parameters.AddWithValue("@ccm_conta_id", conta_id);
                comando.Parameters.AddWithValue("@ccm_ccorrente_id", ccorrente_id);
                comando.Parameters.AddWithValue("@ccm_data", data);
                comando.Parameters.AddWithValue("@ccm_data_competencia", ccm_data_competencia);
                comando.Parameters.AddWithValue("@ccm_valor", valor);
                comando.Parameters.AddWithValue("@ccm_memorando", memorando);
                comando.Parameters.AddWithValue("@ccm_participante_id", participante_id);
                comando.Parameters.AddWithValue("@categoria_id", categoria_id);
                comando.Parameters.AddWithValue("@ccm_nf", ccm_nf);
                comando.Parameters.AddWithValue("@ccm_nf_data_emissao", ccm_nf_data_emissao);
                comando.Parameters.AddWithValue("@ccm_nf_valor", ccm_nf_valor);
                comando.Parameters.AddWithValue("@ccm_nf_serie", ccm_nf_serie);
                comando.Parameters.AddWithValue("@ccm_nf_numero", ccm_nf_numero);
                comando.Parameters.AddWithValue("@ccm_nf_chave", ccm_nf_chave);
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

        //Busca CCM
        public Vm_ccm buscaCCM(int usuario_id, int conta_id, int ccm_id)
        {
            Vm_ccm vm = new Vm_ccm();
            Ccm_nf nf = new Ccm_nf();
            nf.ccm_nf = false;
            vm.nf = nf;

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT ccm.*, participante.participante_nome , COALESCE(nf.ccm_nf_id, 0) as nota ,nf.* from conta_corrente_mov as ccm left JOIN ccm_nf as nf on nf.ccm_nf_ccm_id = ccm.ccm_id LEFT JOIN participante on participante.participante_id = ccm.ccm_participante_id WHERE ccm.ccm_id = @ccm_id and ccm.ccm_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@ccm_id", ccm_id);                
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["ccm_id"])
                        {
                            vm.ccm_id = Convert.ToInt32(leitor["ccm_id"]);
                        }
                        else
                        {
                            vm.ccm_id = 0;
                        }

                        if (DBNull.Value != leitor["ccm_conta_id"])
                        {
                            vm.ccm_conta_id = Convert.ToInt32(leitor["ccm_conta_id"]);
                        }
                        else
                        {
                            vm.ccm_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["ccm_ccorrente_id"])
                        {
                            vm.ccm_ccorrente_id = Convert.ToInt32(leitor["ccm_ccorrente_id"]);
                        }
                        else
                        {
                            vm.ccm_ccorrente_id = 0;
                        }

                        vm.ccm_movimento = leitor["ccm_movimento"].ToString();
                        vm.ccm_contra_partida_tipo = leitor["ccm_contra_partida_tipo"].ToString();
                        vm.ccm_memorando = leitor["ccm_memorando"].ToString();
                        vm.ccm_origem = leitor["ccm_origem"].ToString();
                        vm.participante_nome = leitor["participante_nome"].ToString();

                        if (DBNull.Value != leitor["ccm_participante_id"])
                        {
                            vm.ccm_participante_id = Convert.ToInt32(leitor["ccm_participante_id"]);
                        }
                        else
                        {
                            vm.ccm_participante_id = 0;
                        }

                        if (DBNull.Value != leitor["ccm_contra_partida_id"])
                        {
                            vm.ccm_contra_partida_id = Convert.ToInt32(leitor["ccm_contra_partida_id"]);
                        }
                        else
                        {
                            vm.ccm_contra_partida_id = 0;
                        }

                        if (DBNull.Value != leitor["ccm_data"])
                        {
                            vm.ccm_data = Convert.ToDateTime(leitor["ccm_data"]);
                        }
                        else
                        {
                            vm.ccm_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["ccm_data_competencia"])
                        {
                            vm.ccm_data_competencia = Convert.ToDateTime(leitor["ccm_data_competencia"]);
                        }
                        else
                        {
                            vm.ccm_data_competencia = new DateTime();
                        }

                        if (DBNull.Value != leitor["ccm_valor"])
                        {
                            vm.ccm_valor = Convert.ToDecimal(leitor["ccm_valor"]);
                        }
                        else
                        {
                            vm.ccm_valor = 0;
                        }

                        if (DBNull.Value != leitor["ccm_op_id"])
                        {
                            vm.ccm_op_id = Convert.ToInt32(leitor["ccm_op_id"]);
                        }
                        else
                        {
                            vm.ccm_op_id = 0;
                        }

                        if (DBNull.Value != leitor["ccm_oppb_id"])
                        {
                            vm.ccm_oppb_id = Convert.ToInt32(leitor["ccm_oppb_id"]);
                        }
                        else
                        {
                            vm.ccm_oppb_id = 0;
                        }

                        int nota = 0;
                        if (DBNull.Value != leitor["nota"])
                        {
                            nota = Convert.ToInt32(leitor["nota"]);
                        }
                        else
                        {
                            nota = 0;
                        }

                        if(nota != 0)
                        {
                            if (DBNull.Value != leitor["ccm_nf_id"])
                            {
                                vm.nf.ccm_nf_id = Convert.ToInt32(leitor["ccm_nf_id"]);
                            }
                            else
                            {
                                vm.nf.ccm_nf_id = 0;
                            }

                            if (DBNull.Value != leitor["ccm_nf_ccm_id"])
                            {
                                vm.nf.ccm_nf_ccm_id = Convert.ToInt32(leitor["ccm_nf_ccm_id"]);
                            }
                            else
                            {
                                vm.nf.ccm_nf_ccm_id = 0;
                            }

                            if (DBNull.Value != leitor["ccm_nf_conta_id"])
                            {
                                vm.nf.ccm_nf_conta_id = Convert.ToInt32(leitor["ccm_nf_conta_id"]);
                            }
                            else
                            {
                                vm.nf.ccm_nf_conta_id = 0;
                            }

                            if (DBNull.Value != leitor["ccm_nf_data_emissao"])
                            {
                                vm.nf.ccm_nf_data_emissao = Convert.ToDateTime(leitor["ccm_nf_data_emissao"]);
                            }
                            else
                            {
                                vm.nf.ccm_nf_data_emissao = new DateTime();
                            }

                            if (DBNull.Value != leitor["ccm_nf_valor"])
                            {
                                vm.nf.ccm_nf_valor = Convert.ToDecimal(leitor["ccm_nf_valor"]);
                            }
                            else
                            {
                                vm.nf.ccm_nf_valor = 0;
                            }

                            vm.nf.ccm_nf_serie = leitor["ccm_nf_serie"].ToString();
                            vm.nf.ccm_nf_numero = leitor["ccm_nf_numero"].ToString();
                            vm.nf.ccm_nf_chave = leitor["ccm_nf_chave"].ToString();

                            vm.nf.ccm_nf = true;
                        }    
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Conta_corrente_mov", "buscaCCM", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vm;
        }

        //Alterar lançamento caixa
        public string alterarCCM(int usuario_id, int conta_id, DateTime data, DateTime ccm_data_competencia, Decimal valor, string memorando, int categoria_id, int participante_id, int ccorrente_id, bool ccm_nf, DateTime ccm_nf_data_emissao, Decimal ccm_nf_valor, string ccm_nf_serie, string ccm_nf_numero, string ccm_nf_chave, int ccm_id)
        {
            string retorno = "Lançamento alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_alterarCCM(@ccm_conta_id, @ccm_ccorrente_id, @ccm_data, @ccm_valor, @ccm_memorando, @ccm_participante_id, @categoria_id, @ccm_nf, @ccm_nf_data_emissao, @ccm_nf_valor, @ccm_nf_serie, @ccm_nf_numero, @ccm_nf_chave, @ccm_data_competencia, @ccm_id)";
                comando.Parameters.AddWithValue("@ccm_conta_id", conta_id);
                comando.Parameters.AddWithValue("@ccm_ccorrente_id", ccorrente_id);
                comando.Parameters.AddWithValue("@ccm_data", data);
                comando.Parameters.AddWithValue("@ccm_data_competencia", ccm_data_competencia);
                comando.Parameters.AddWithValue("@ccm_valor", valor);
                comando.Parameters.AddWithValue("@ccm_memorando", memorando);
                comando.Parameters.AddWithValue("@ccm_participante_id", participante_id);
                comando.Parameters.AddWithValue("@categoria_id", categoria_id);
                comando.Parameters.AddWithValue("@ccm_nf", ccm_nf);
                comando.Parameters.AddWithValue("@ccm_nf_data_emissao", ccm_nf_data_emissao);
                comando.Parameters.AddWithValue("@ccm_nf_valor", ccm_nf_valor);
                comando.Parameters.AddWithValue("@ccm_nf_serie", ccm_nf_serie);
                comando.Parameters.AddWithValue("@ccm_nf_numero", ccm_nf_numero);
                comando.Parameters.AddWithValue("@ccm_nf_chave", ccm_nf_chave);
                comando.Parameters.AddWithValue("@ccm_id", ccm_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Lançamento conta corrente movimento alterado com sucesso";
                log.log("Conta_corrente_mov", "alterarCCM", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar o participante. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Conta_corrente_mov", "alterarCCM", "Erro", msg, conta_id, usuario_id);
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
