using gestaoContadorcomvc.Models.Site.ViewModel;
using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Site
{
    public class Lead
    {
        public int lead_id { get; set; }
        public DateTime lead_dataCadastro { get; set; }
        public string lead_nome { get; set; }
        public string lead_celular { get; set; }
        public string lead_email { get; set; }
        public int lead_conta_id { get; set; }
        public string lead_tipo { get; set; }
        public string lead_situacao { get; set; }
        public int lead_lead_atendentes_id { get; set; }
        public string  lead_site_origem { get; set; }
        public IEnumerable<Lead_contato> contatos { get; set; }
        
        //Atributos left join
        public string lead_atendentes_nome { get; set; }
        public int lead_contato_nao_lida { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Lead()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        public string create(int conta_id, int lead_lead_atendentes_id, string lead_nome, string lead_celular, string lead_email, string lead_tipo, string lead_situacao, string lead_contato_tipo, string lead_contato_msg, string lead_site_origem)
        {
            string retorno = "";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_lead (@lead_conta_id, @lead_lead_atendentes_id, @lead_nome, @lead_celular, @lead_email, @lead_tipo, @lead_situacao, @lead_contato_tipo, @lead_contato_msg, @lead_site_origem)";
                comando.Parameters.AddWithValue("@lead_conta_id", conta_id);
                comando.Parameters.AddWithValue("@lead_lead_atendentes_id", lead_lead_atendentes_id);
                comando.Parameters.AddWithValue("@lead_nome", lead_nome);
                comando.Parameters.AddWithValue("@lead_celular", lead_celular);
                comando.Parameters.AddWithValue("@lead_email", lead_email);
                comando.Parameters.AddWithValue("@lead_tipo", lead_tipo);
                comando.Parameters.AddWithValue("@lead_situacao", lead_situacao);
                comando.Parameters.AddWithValue("@lead_contato_tipo", lead_contato_tipo);
                comando.Parameters.AddWithValue("@lead_contato_msg", lead_contato_msg);
                comando.Parameters.AddWithValue("@lead_site_origem", lead_site_origem);
                comando.ExecuteNonQuery();
                Transacao.Commit();
                retorno = "Contato cadastrado com sucesso!!";
            }
            catch (Exception e)
            {   
                retorno = "Desculpe, infelizmente tivemos um problema com o envio do contato. Favor tentar novamente.";
                Log l = new Log();
                l.log_txt(e.Message);
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

        public Vm_lead list_leads(int conta_id)
        {
            Vm_lead vm_Lead = new Vm_lead();
            List<Lead> leads = new List<Lead>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT l.*, a.lead_atendentes_nome, (SELECT COUNT(lead_contato.lead_contato_id) from lead_contato WHERE lead_contato.lead_contato_lida = false and lead_contato.lead_contato_lead_id = l.lead_id) as 'lead_contato_nao_lida' from lead as l LEFT JOIN lead_atendentes as a on a.lead_atendentes_id = l.lead_lead_atendentes_id WHERE l.lead_conta_id = @conta_id and l.lead_situacao <> 'Convertido';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Lead lead = new Lead();

                        if (DBNull.Value != leitor["lead_id"])
                        {
                            lead.lead_id = Convert.ToInt32(leitor["lead_id"]);
                        }
                        else
                        {
                            lead.lead_id = 0;
                        }

                        if (DBNull.Value != leitor["lead_conta_id"])
                        {
                            lead.lead_conta_id = Convert.ToInt32(leitor["lead_conta_id"]);
                        }
                        else
                        {
                            lead.lead_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["lead_lead_atendentes_id"])
                        {
                            lead.lead_lead_atendentes_id = Convert.ToInt32(leitor["lead_lead_atendentes_id"]);
                        }
                        else
                        {
                            lead.lead_lead_atendentes_id = 0;
                        }

                        if (DBNull.Value != leitor["lead_dataCadastro"])
                        {
                            lead.lead_dataCadastro = Convert.ToDateTime(leitor["lead_dataCadastro"]);
                        }
                        else
                        {
                            lead.lead_dataCadastro = new DateTime();
                        }

                        lead.lead_nome = leitor["lead_nome"].ToString();
                        lead.lead_celular = leitor["lead_celular"].ToString();
                        lead.lead_email = leitor["lead_email"].ToString();
                        lead.lead_tipo = leitor["lead_tipo"].ToString();
                        lead.lead_situacao = leitor["lead_situacao"].ToString();

                        lead.lead_atendentes_nome = leitor["lead_atendentes_nome"].ToString();
                        lead.lead_contato_nao_lida = Convert.ToInt32(leitor["lead_contato_nao_lida"]);
                        lead.lead_site_origem = leitor["lead_site_origem"].ToString();

                        leads.Add(lead);
                    }
                }

                vm_Lead.leads = leads;
                vm_Lead.status = "Sucesso";

            }
            catch (Exception e)
            {
                vm_Lead.status = "Erro";
                vm_Lead.leads = new List<Lead>();

                Log log = new Log();
                log.log_txt(e.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vm_Lead;
        }




    }
}
