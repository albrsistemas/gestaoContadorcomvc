using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    [Area("Contabilidade")]    
    public class ContaContabil
    {
        public int ccontabil_id { get; set; }
        public int ccontabil_plano_id { get; set; }
        public string ccontabil_classificacao { get; set; }
        public string ccontabil_nome { get; set; }
        public string ccontabil_apelido { get; set; }
        public int ccontabil_nivel { get; set; }
        public string ccontabil_grupo { get; set; }
        public string ccontabil_tipo { get; set; }
        public DateTime ccontabil_dataCriacao { get; set; }
        public DateTime ccontabil_dataInativacao { get; set; }
        public string ccontabil_status { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ContaContabil()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Lista plano de contas
        public List<vm_ContaContabil> listaContasContabeisPorPlano(int conta_id, int plano_id, int usuario_id)
        {
            List<vm_ContaContabil> contas = new List<vm_ContaContabil>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "select * from contacontabil where contacontabil.ccontabil_plano_id = @plano_id and contacontabil.ccontabil_status = 'Ativo' order by contacontabil.ccontabil_classificacao";
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        vm_ContaContabil conta = new vm_ContaContabil();

                        if (DBNull.Value != leitor["ccontabil_id"])
                        {
                            conta.ccontabil_id = Convert.ToInt32(leitor["ccontabil_id"]);
                        }
                        else
                        {
                            conta.ccontabil_id = 0;
                        }

                        if (DBNull.Value != leitor["ccontabil_plano_id"])
                        {
                            conta.ccontabil_plano_id = Convert.ToInt32(leitor["ccontabil_plano_id"]);
                        }
                        else
                        {
                            conta.ccontabil_plano_id = 0;
                        }

                        if (DBNull.Value != leitor["ccontabil_nivel"])
                        {
                            conta.ccontabil_nivel = Convert.ToInt32(leitor["ccontabil_nivel"]);
                        }
                        else
                        {
                            conta.ccontabil_nivel = 0;
                        }

                        if (DBNull.Value != leitor["ccontabil_dataCriacao"])
                        {
                            conta.ccontabil_dataCriacao = Convert.ToDateTime(leitor["ccontabil_dataCriacao"]);
                        }
                        else
                        {
                            conta.ccontabil_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["ccontabil_dataInativacao"])
                        {
                            conta.ccontabil_dataInativacao = Convert.ToDateTime(leitor["ccontabil_dataInativacao"]);
                        }
                        else
                        {
                            conta.ccontabil_dataInativacao = new DateTime();
                        }

                        conta.ccontabil_classificacao = leitor["ccontabil_classificacao"].ToString();
                        conta.ccontabil_nome = leitor["ccontabil_nome"].ToString();
                        conta.ccontabil_apelido = leitor["ccontabil_apelido"].ToString();
                        conta.ccontabil_grupo = leitor["ccontabil_grupo"].ToString();
                        conta.ccontabil_status = leitor["ccontabil_status"].ToString();
                        

                        contas.Add(conta);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("ContaContabil", "listaContasContabeisPorPlano", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return contas;
        }

        //Cadastrar conta contábil
        public string cadastrarContaContabil(int conta_id, int usuario_id, string plano_id, string classificacao, string nivel, string nome, string apelido)
        {
            vm_ContaContabil conta = new vm_ContaContabil();
            string grupo = conta.grupoConta(classificacao);
            string tipo = "Sintetica";
            if(nivel == "5")
            {
                tipo = "Analitica";
            }

            string retorno = "Conta contábil cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into contacontabil (" +
                    "ccontabil_plano_id, ccontabil_classificacao, ccontabil_nome, ccontabil_nivel, ccontabil_grupo, ccontabil_tipo, ccontabil_apelido) values (" +
                    "@ccontabil_plano_id, @ccontabil_classificacao, @ccontabil_nome, @ccontabil_nivel, @ccontabil_grupo, @ccontabil_tipo, @ccontabil_apelido)";
                comando.Parameters.AddWithValue("@ccontabil_plano_id", plano_id);
                comando.Parameters.AddWithValue("@ccontabil_classificacao", classificacao);
                comando.Parameters.AddWithValue("@ccontabil_nome", nome);
                comando.Parameters.AddWithValue("@ccontabil_nivel", nivel);
                comando.Parameters.AddWithValue("@ccontabil_grupo", grupo);
                comando.Parameters.AddWithValue("@ccontabil_tipo", tipo);
                comando.Parameters.AddWithValue("@ccontabil_apelido", apelido);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Conta contábil de nome: " + nome + " do plano de conta " + plano_id + " cadastrado com sucesso";
                log.log("ContaContabil", "cadastrarContaContabil", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a conta contábil. Tente novamente. Se persistir o problema entre em contato com o suporte!";

                string msg = "Conta contábil de nome: " + nome + " do plano de conta " + plano_id + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("ContaContabil", "cadastrarContaContabil", "Erro", msg, conta_id, usuario_id);
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

        //Busca conta contábil por id
        public vm_ContaContabil buscaContaContabeisPorID(int conta_id, int usuario_id, int ccontabil_id)
        {
            vm_ContaContabil conta = new vm_ContaContabil();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "select * from contacontabil where contacontabil.ccontabil_id = @ccontabil_id";
                comando.Parameters.AddWithValue("@ccontabil_id", ccontabil_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["ccontabil_id"])
                        {
                            conta.ccontabil_id = Convert.ToInt32(leitor["ccontabil_id"]);
                        }
                        else
                        {
                            conta.ccontabil_id = 0;
                        }

                        if (DBNull.Value != leitor["ccontabil_plano_id"])
                        {
                            conta.ccontabil_plano_id = Convert.ToInt32(leitor["ccontabil_plano_id"]);
                        }
                        else
                        {
                            conta.ccontabil_plano_id = 0;
                        }

                        if (DBNull.Value != leitor["ccontabil_nivel"])
                        {
                            conta.ccontabil_nivel = Convert.ToInt32(leitor["ccontabil_nivel"]);
                        }
                        else
                        {
                            conta.ccontabil_nivel = 0;
                        }

                        if (DBNull.Value != leitor["ccontabil_dataCriacao"])
                        {
                            conta.ccontabil_dataCriacao = Convert.ToDateTime(leitor["ccontabil_dataCriacao"]);
                        }
                        else
                        {
                            conta.ccontabil_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["ccontabil_dataInativacao"])
                        {
                            conta.ccontabil_dataInativacao = Convert.ToDateTime(leitor["ccontabil_dataInativacao"]);
                        }
                        else
                        {
                            conta.ccontabil_dataInativacao = new DateTime();
                        }

                        conta.ccontabil_classificacao = leitor["ccontabil_classificacao"].ToString();
                        conta.ccontabil_nome = leitor["ccontabil_nome"].ToString();
                        conta.ccontabil_apelido = leitor["ccontabil_apelido"].ToString();
                        conta.ccontabil_grupo = leitor["ccontabil_grupo"].ToString();
                        conta.ccontabil_status = leitor["ccontabil_status"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("ContaContabil", "buscaContaContabeisPorID", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return conta;
        }

        //Editar conta contábil
        public string editarContaContabil(int conta_id, int usuario_id, string plano_id, string ccontabil_id, string nome, string apelido)
        {
            string retorno = "Conta contábil alterada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE contacontabil set contacontabil.ccontabil_nome = @ccontabil_nome, contacontabil.ccontabil_apelido = @ccontabil_apelido where contacontabil.ccontabil_id = @ccontabil_id";                                
                comando.Parameters.AddWithValue("@ccontabil_nome", nome);
                comando.Parameters.AddWithValue("@ccontabil_apelido", apelido);
                comando.Parameters.AddWithValue("@ccontabil_id", ccontabil_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Conta contábil de nome: " + nome + " do plano de conta " + plano_id + " alterada com sucesso";
                log.log("ContaContabil", "editarContaContabil", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a conta contábil. Tente novamente. Se persistir o problema entre em contato com o suporte!";

                string msg = "Conta contábil de nome: " + nome + " do plano de conta " + plano_id + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("ContaContabil", "editarContaContabil", "Erro", msg, conta_id, usuario_id);
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

        //Deletar conta contábil alteração do status e data
        public string deleteContaContabil(int conta_id, int usuario_id, string plano_id, string ccontabil_id, string nome)
        {
            string retorno = "Conta contábil excluída com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE contacontabil set contacontabil.ccontabil_status = 'Deletado', contacontabil.ccontabil_dataInativacao = CURRENT_TIMESTAMP where contacontabil.ccontabil_id = @ccontabil_id";                                
                comando.Parameters.AddWithValue("@ccontabil_id", ccontabil_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Conta contábil de nome: " + nome + " do plano de conta " + plano_id + " excluída com sucesso";
                log.log("ContaContabil", "deleteContaContabil", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a conta contábil. Tente novamente. Se persistir o problema entre em contato com o suporte!";

                string msg = "Conta contábil de nome: " + nome + " do plano de conta " + plano_id + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("ContaContabil", "deleteContaContabil", "Erro", msg, conta_id, usuario_id);
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

        //Lista contas por nome ou apelido para atender a tela de lançamento contábil
        //public List<vm_ContaContabil> listaContas(int plano_id)
        //{
        //    List<vm_ContaContabil> contas = new List<vm_ContaContabil>();

        //    conn.Open();
        //    MySqlCommand comando = conn.CreateCommand();
        //    MySqlTransaction Transacao;
        //    Transacao = conn.BeginTransaction();
        //    comando.Connection = conn;
        //    comando.Transaction = Transacao;

        //    try
        //    {
        //        comando.CommandText = "select * from contacontabil where contacontabil.ccontabil_plano_id = @plano_id and contacontabil.ccontabil_status = 'Ativo' order by contacontabil.ccontabil_classificacao";
        //        comando.Parameters.AddWithValue("@plano_id", plano_id);
        //        comando.ExecuteNonQuery();
        //        Transacao.Commit();

        //        var leitor = comando.ExecuteReader();

        //        if (leitor.HasRows)
        //        {
        //            while (leitor.Read())
        //            {
        //                vm_ContaContabil conta = new vm_ContaContabil();

        //                if (DBNull.Value != leitor["ccontabil_id"])
        //                {
        //                    conta.ccontabil_id = Convert.ToInt32(leitor["ccontabil_id"]);
        //                }
        //                else
        //                {
        //                    conta.ccontabil_id = 0;
        //                }

        //                if (DBNull.Value != leitor["ccontabil_plano_id"])
        //                {
        //                    conta.ccontabil_plano_id = Convert.ToInt32(leitor["ccontabil_plano_id"]);
        //                }
        //                else
        //                {
        //                    conta.ccontabil_plano_id = 0;
        //                }

        //                if (DBNull.Value != leitor["ccontabil_nivel"])
        //                {
        //                    conta.ccontabil_nivel = Convert.ToInt32(leitor["ccontabil_nivel"]);
        //                }
        //                else
        //                {
        //                    conta.ccontabil_nivel = 0;
        //                }

        //                if (DBNull.Value != leitor["ccontabil_dataCriacao"])
        //                {
        //                    conta.ccontabil_dataCriacao = Convert.ToDateTime(leitor["ccontabil_dataCriacao"]);
        //                }
        //                else
        //                {
        //                    conta.ccontabil_dataCriacao = new DateTime();
        //                }

        //                if (DBNull.Value != leitor["ccontabil_dataInativacao"])
        //                {
        //                    conta.ccontabil_dataInativacao = Convert.ToDateTime(leitor["ccontabil_dataInativacao"]);
        //                }
        //                else
        //                {
        //                    conta.ccontabil_dataInativacao = new DateTime();
        //                }

        //                conta.ccontabil_classificacao = leitor["ccontabil_classificacao"].ToString();
        //                conta.ccontabil_nome = leitor["ccontabil_nome"].ToString();
        //                conta.ccontabil_apelido = leitor["ccontabil_apelido"].ToString();
        //                conta.ccontabil_grupo = leitor["ccontabil_grupo"].ToString();
        //                conta.ccontabil_status = leitor["ccontabil_status"].ToString();


        //                contas.Add(conta);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string msg = e.Message.Substring(0, 300);
        //        log.log("ContaContabil", "listaContasContabeisPorPlano", "Erro", msg, conta_id, usuario_id);
        //    }
        //    finally
        //    {
        //        if (conn.State == System.Data.ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }

        //    return contas;
        //}

    }
}
