using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace gestaoContadorcomvc.Models
{
    public class Participante_tipo
    {
        public int pt_id { get; set; }
        public string pt_nome { get; set; }
        public int pt_conta_id { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Participante_tipo()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public List<Participante_tipo> index(int conta_id, int usuario_id)
        {
            List<Participante_tipo> lista = new List<Participante_tipo>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from participante_tipo WHERE participante_tipo.pt_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();
                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Participante_tipo p = new Participante_tipo();
                        
                        if (DBNull.Value != leitor["pt_id"])
                        {
                            p.pt_id = Convert.ToInt32(leitor["pt_id"]);
                        }
                        else
                        {
                            p.pt_id = 0;
                        }

                        if (DBNull.Value != leitor["pt_conta_id"])
                        {
                            p.pt_conta_id = Convert.ToInt32(leitor["pt_conta_id"]);
                        }
                        else
                        {
                            p.pt_conta_id = 0;
                        }

                        p.pt_nome = leitor["pt_nome"].ToString();

                        lista.Add(p);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Participante_tipo", "index", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return lista;
        }

        public string create(int conta_id, int usuario_id, string pt_nome)
        {
            string retorno = "Tipo de participante cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into participante_tipo (participante_tipo.pt_nome, participante_tipo.pt_conta_id) VALUES (@pt_nome, @conta_id);";
                comando.Parameters.AddWithValue("@pt_nome", pt_nome);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro de tipo participante nome: " + pt_nome + " Cadastrado com sucesso";
                log.log("Participante_tipo", "create", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 250);
                log.log("Participante_tipo", "create", "Erro", msg, conta_id, usuario_id);
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

        public Participante_tipo buscaParticipante_tipo(int usuario_id, int conta_id, int pt_id)
        {
            Participante_tipo p = new Participante_tipo();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from participante_tipo as p WHERE p.pt_conta_id = @conta_id and p.pt_id = @pt_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@pt_id", pt_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();
                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["pt_id"])
                        {
                            p.pt_id = Convert.ToInt32(leitor["pt_id"]);
                        }
                        else
                        {
                            p.pt_id = 0;
                        }

                        if (DBNull.Value != leitor["pt_conta_id"])
                        {
                            p.pt_conta_id = Convert.ToInt32(leitor["pt_conta_id"]);
                        }
                        else
                        {
                            p.pt_conta_id = 0;
                        }

                        p.pt_nome = leitor["pt_nome"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Participante_tipo", "index", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return p;
        }

        public string edit(int conta_id, int usuario_id, int pt_id, string pt_nome)
        {
            string retorno = "Tipo de participante alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE participante_tipo set participante_tipo.pt_nome = @pt_nome WHERE participante_tipo.pt_conta_id = @conta_id and participante_tipo.pt_id = @pt_id;";
                comando.Parameters.AddWithValue("@pt_nome", pt_nome);
                comando.Parameters.AddWithValue("@pt_id", pt_id);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração de tipo participante ID: " + pt_id + " alterado com sucesso";
                log.log("Participante_tipo", "edit", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 250);
                log.log("Participante_tipo", "edit", "Erro", msg, conta_id, usuario_id);
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

        public string delete(int conta_id, int usuario_id, int pt_id)
        {
            string retorno = "Tipo de participante excluído com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "DELETE from participante_tipo WHERE participante_tipo.pt_conta_id = @conta_id and participante_tipo.pt_id = @pt_id;";                
                comando.Parameters.AddWithValue("@pt_id", pt_id);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão do tipo participante ID: " + pt_id + " excluído com sucesso";
                log.log("Participante_tipo", "delete", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 250);
                log.log("Participante_tipo", "delete", "Erro", msg, conta_id, usuario_id);
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
