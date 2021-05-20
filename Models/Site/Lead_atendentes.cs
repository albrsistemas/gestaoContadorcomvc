using gestaoContadorcomvc.Models.Site.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site
{
    public class Lead_atendentes
    {
        public int lead_atendentes_id { get; set; }
        public string lead_atendentes_nome { get; set; }
        public string lead_atendentes_celular { get; set; }
        public string lead_atendentes_email { get; set; }
        public int lead_atendentes_filaUm { get; set; }
        public int lead_atendentes_filaDois { get; set; }
        public int lead_atendentes_conta_id { get; set; }
        public bool lead_atendentes_atende_fila_um { get; set; }
        public bool lead_atendentes_atende_fila_dois { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Lead_atendentes()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }


        public string create(string lead_atendentes_nome, string lead_atendentes_celular, string lead_atendentes_email, int lead_atendentes_conta_id, bool lead_atendentes_atende_fila_um, bool lead_atendentes_atende_fila_dois)
        {
            string retorno = "Atendente cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_leadAtendente_create(@lead_atendentes_nome, @lead_atendentes_celular, @lead_atendentes_email, @lead_atendentes_conta_id, @lead_atendentes_atende_fila_um, @lead_atendentes_atende_fila_dois);";
                comando.Parameters.AddWithValue("@lead_atendentes_nome", lead_atendentes_nome);
                comando.Parameters.AddWithValue("@lead_atendentes_celular", lead_atendentes_celular);
                comando.Parameters.AddWithValue("@lead_atendentes_email", lead_atendentes_email);
                comando.Parameters.AddWithValue("@lead_atendentes_conta_id", lead_atendentes_conta_id);
                comando.Parameters.AddWithValue("@lead_atendentes_atende_fila_um", lead_atendentes_atende_fila_um);
                comando.Parameters.AddWithValue("@lead_atendentes_atende_fila_dois", lead_atendentes_atende_fila_dois);                
                comando.ExecuteNonQuery();
                Transacao.Commit();
            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar o atendente!";
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

        public Vm_lead_atendentes list_lead_atendentes(int conta_id)
        {
            Vm_lead_atendentes atendentes_conta = new Vm_lead_atendentes();
            List<Lead_atendentes> atendentes = new List<Lead_atendentes>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from lead_atendentes as la WHERE la.lead_atendentes_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Lead_atendentes atendente = new Lead_atendentes();

                        if (DBNull.Value != leitor["lead_atendentes_id"])
                        {
                            atendente.lead_atendentes_id = Convert.ToInt32(leitor["lead_atendentes_id"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_id = 0;
                        }

                        if (DBNull.Value != leitor["lead_atendentes_conta_id"])
                        {
                            atendente.lead_atendentes_conta_id = Convert.ToInt32(leitor["lead_atendentes_conta_id"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["lead_atendentes_filaUm"])
                        {
                            atendente.lead_atendentes_filaUm = Convert.ToInt32(leitor["lead_atendentes_filaUm"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_filaUm = 0;
                        }

                        if (DBNull.Value != leitor["lead_atendentes_filaDois"])
                        {
                            atendente.lead_atendentes_filaDois = Convert.ToInt32(leitor["lead_atendentes_filaDois"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_filaDois = 0;
                        }

                        atendente.lead_atendentes_atende_fila_um = Convert.ToBoolean(leitor["lead_atendentes_atende_fila_um"]);
                        atendente.lead_atendentes_atende_fila_dois = Convert.ToBoolean(leitor["lead_atendentes_atende_fila_dois"]);
                        atendente.lead_atendentes_nome = leitor["lead_atendentes_nome"].ToString();
                        atendente.lead_atendentes_celular = leitor["lead_atendentes_celular"].ToString();
                        atendente.lead_atendentes_email = leitor["lead_atendentes_email"].ToString();

                        atendentes.Add(atendente);
                    }
                }
                atendentes_conta.status = "sucesso";
                atendentes_conta.atendentes = atendentes;
            }
            catch (Exception)
            {
                atendentes_conta.status = "Erro";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return atendentes_conta;
        }

        public Lead_atendentes busca(int conta_id, int lead_atendentes_id)
        {
            Lead_atendentes atendente = new Lead_atendentes();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from lead_atendentes as l WHERE l.lead_atendentes_id = @lead_atendentes_id and l.lead_atendentes_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@lead_atendentes_id", lead_atendentes_id);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["lead_atendentes_id"])
                        {
                            atendente.lead_atendentes_id = Convert.ToInt32(leitor["lead_atendentes_id"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_id = 0;
                        }

                        if (DBNull.Value != leitor["lead_atendentes_conta_id"])
                        {
                            atendente.lead_atendentes_conta_id = Convert.ToInt32(leitor["lead_atendentes_conta_id"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["lead_atendentes_filaUm"])
                        {
                            atendente.lead_atendentes_filaUm = Convert.ToInt32(leitor["lead_atendentes_filaUm"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_filaUm = 0;
                        }

                        if (DBNull.Value != leitor["lead_atendentes_filaDois"])
                        {
                            atendente.lead_atendentes_filaDois = Convert.ToInt32(leitor["lead_atendentes_filaDois"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_filaDois = 0;
                        }

                        atendente.lead_atendentes_atende_fila_um = Convert.ToBoolean(leitor["lead_atendentes_atende_fila_um"]);
                        atendente.lead_atendentes_atende_fila_dois = Convert.ToBoolean(leitor["lead_atendentes_atende_fila_dois"]);
                        atendente.lead_atendentes_nome = leitor["lead_atendentes_nome"].ToString();
                        atendente.lead_atendentes_celular = leitor["lead_atendentes_celular"].ToString();
                        atendente.lead_atendentes_email = leitor["lead_atendentes_email"].ToString();
                    }
                }                
            }
            catch (Exception)
            {
                
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return atendente;
        }

        public string edit(int lead_atendentes_id, string lead_atendentes_nome, string lead_atendentes_celular, string lead_atendentes_email, int lead_atendentes_conta_id, bool lead_atendentes_atende_fila_um, bool lead_atendentes_atende_fila_dois)
        {
            string retorno = "Atendente alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE lead_atendentes set lead_atendentes.lead_atendentes_nome = @lead_atendentes_nome, lead_atendentes.lead_atendentes_celular = @lead_atendentes_celular, lead_atendentes.lead_atendentes_email = @lead_atendentes_email, lead_atendentes.lead_atendentes_atende_fila_um = @lead_atendentes_atende_fila_um, lead_atendentes.lead_atendentes_atende_fila_dois = @lead_atendentes_atende_fila_dois WHERE lead_atendentes.lead_atendentes_conta_id = @conta_id and lead_atendentes.lead_atendentes_id = @lead_atendentes_id;";
                comando.Parameters.AddWithValue("@lead_atendentes_nome", lead_atendentes_nome);
                comando.Parameters.AddWithValue("@lead_atendentes_celular", lead_atendentes_celular);
                comando.Parameters.AddWithValue("@lead_atendentes_email", lead_atendentes_email);
                comando.Parameters.AddWithValue("@conta_id", lead_atendentes_conta_id);
                comando.Parameters.AddWithValue("@lead_atendentes_id", lead_atendentes_id);
                comando.Parameters.AddWithValue("@lead_atendentes_atende_fila_um", lead_atendentes_atende_fila_um);
                comando.Parameters.AddWithValue("@lead_atendentes_atende_fila_dois", lead_atendentes_atende_fila_dois);
                comando.ExecuteNonQuery();
                Transacao.Commit();
            }
            catch (Exception e)
            {
                retorno = "Erro ao altrerar o atendente!";
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

        public string delete(int conta_id, int lead_atendentes_id)
        {
            string retorno = "Atendente excluído com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                MySqlDataReader dr_1;
                MySqlCommand dr_1_c = conn.CreateCommand();
                dr_1_c.Connection = conn;
                dr_1_c.Transaction = Transacao;
                dr_1_c.CommandText = "SELECT COUNT(l.lead_id) as 'leads' from lead as l WHERE l.lead_lead_atendentes_id = @lead_lead_atendentes_id;";
                dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_1_c.Parameters.AddWithValue("lead_lead_atendentes_id", lead_atendentes_id);                
                dr_1 = dr_1_c.ExecuteReader();

                int leads = 0;

                if (dr_1.HasRows)
                {
                    while (dr_1.Read())
                    {
                        leads = Convert.ToInt32(dr_1["leads"]);
                    }
                }
                dr_1.Close();

                if (leads > 0)
                {
                    retorno = "Atendente não pode ser excluído, pois existem " + leads + " LEAD´s atribuídos a ele!";                    

                    return retorno;
                }
                else
                {
                    comando.CommandText = "DELETE from lead_atendentes WHERE lead_atendentes.lead_atendentes_id = @lead_atendentes_id and lead_atendentes.lead_atendentes_conta_id = @conta_id;";
                    comando.Parameters.AddWithValue("@conta_id", conta_id);
                    comando.Parameters.AddWithValue("@lead_atendentes_id", lead_atendentes_id);
                    comando.ExecuteNonQuery();
                    Transacao.Commit();
                }
            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir o atendente!";
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

        public Lead_atendentes busca_proximo_atendente(int conta_id, int fila)
        {
            Lead_atendentes atendente = new Lead_atendentes();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                if(fila == 1)
                {
                    comando.CommandText = "SELECT MIN(l.lead_atendentes_filaUm), l.lead_atendentes_id, l.lead_atendentes_nome, l.lead_atendentes_celular, l.lead_atendentes_email from lead_atendentes as l WHERE l.lead_atendentes_conta_id = @conta_id and l.lead_atendentes_atende_fila_um = true and l.lead_atendentes_filaUm = (SELECT MIN(l.lead_atendentes_filaUm) from lead_atendentes as l WHERE l.lead_atendentes_conta_id = @conta_id and l.lead_atendentes_atende_fila_um = true);";
                }
                if (fila == 2)
                {
                    comando.CommandText = "SELECT MIN(l.lead_atendentes_filaDois), l.lead_atendentes_id, l.lead_atendentes_nome, l.lead_atendentes_celular, l.lead_atendentes_email from lead_atendentes as l WHERE l.lead_atendentes_conta_id = @conta_id and l.lead_atendentes_atende_fila_dois = true and l.lead_atendentes_filaDois = (SELECT MIN(l.lead_atendentes_filaDois) from lead_atendentes as l WHERE l.lead_atendentes_conta_id = @conta_id and l.lead_atendentes_atende_fila_dois = true);";
                }                
                comando.Parameters.AddWithValue("@conta_id", conta_id);                
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["lead_atendentes_id"])
                        {
                            atendente.lead_atendentes_id = Convert.ToInt32(leitor["lead_atendentes_id"]);
                        }
                        else
                        {
                            atendente.lead_atendentes_id = 0;
                        }                        
                        atendente.lead_atendentes_nome = leitor["lead_atendentes_nome"].ToString();
                        atendente.lead_atendentes_celular = leitor["lead_atendentes_celular"].ToString();
                        atendente.lead_atendentes_email = leitor["lead_atendentes_email"].ToString();
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return atendente;
        }


    }
}
