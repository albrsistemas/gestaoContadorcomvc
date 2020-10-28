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
    public class FormaPagamento
    {
        public int fp_id { get; set; }
        public string fp_nome { get; set; }
        public string fp_meio_pgto_nfe { get; set; }
        public bool fp_baixa_automatica { get; set; }
        public string fp_vinc_conta_corrente { get; set; }
        public string fp_identificacao { get; set; }
        public string fp_tipo_integracao_nfe { get; set; }
        public string fp_bandeira_cartao { get; set; }
        public string fp_cnpj_credenciadora_cartao { get; set; }
        public int fp_conta_id { get; set; }
        public DateTime fp_dataCriacao { get; set; }
        public string fp_status { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public FormaPagamento()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Listar forma de pagamento
        public List<Vm_forma_pagamento> listFormasPagamento(int conta_id, int usuario_id)
        {
            List<Vm_forma_pagamento> fps = new List<Vm_forma_pagamento>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from forma_pagamento where fp_conta_id = @conta_id and fp_status = 'Ativo';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_forma_pagamento fp = new Vm_forma_pagamento();

                        if (DBNull.Value != leitor["fp_id"])
                        {
                            fp.fp_id = Convert.ToInt32(leitor["fp_id"]);
                        }
                        else
                        {
                            fp.fp_id = 0;
                        }

                        if (DBNull.Value != leitor["fp_conta_id"])
                        {
                            fp.fp_conta_id = Convert.ToInt32(leitor["fp_conta_id"]);
                        }
                        else
                        {
                            fp.fp_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["fp_baixa_automatica"])
                        {
                            fp.fp_baixa_automatica = Convert.ToBoolean(leitor["fp_baixa_automatica"]);
                        }
                        else
                        {
                            fp.fp_baixa_automatica = false;
                        }

                        if (DBNull.Value != leitor["fp_dataCriacao"])
                        {
                            fp.fp_dataCriacao = Convert.ToDateTime(leitor["fp_dataCriacao"]);
                        }
                        else
                        {
                            fp.fp_dataCriacao = new DateTime();
                        }

                        fp.fp_nome = leitor["fp_nome"].ToString();
                        fp.fp_meio_pgto_nfe = leitor["fp_meio_pgto_nfe"].ToString();
                        fp.fp_vinc_conta_corrente = leitor["fp_vinc_conta_corrente"].ToString();
                        fp.fp_identificacao = leitor["fp_identificacao"].ToString();
                        fp.fp_tipo_integracao_nfe = leitor["fp_tipo_integracao_nfe"].ToString();
                        fp.fp_bandeira_cartao = leitor["fp_bandeira_cartao"].ToString();
                        fp.fp_cnpj_credenciadora_cartao = leitor["fp_cnpj_credenciadora_cartao"].ToString();
                        fp.fp_status = leitor["fp_status"].ToString();

                        fps.Add(fp);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("FormaPagamento", "listFormasPagamento", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return fps;
        }

        //Busca forma de pagamento por id
        public Vm_forma_pagamento buscaFormasPagamento(int conta_id, int usuario_id, int fp_id)
        {
            Vm_forma_pagamento fp = new Vm_forma_pagamento();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from forma_pagamento where fp_conta_id = @conta_id and fp_id = @fp_id';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@fp_id", fp_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["fp_id"])
                        {
                            fp.fp_id = Convert.ToInt32(leitor["fp_id"]);
                        }
                        else
                        {
                            fp.fp_id = 0;
                        }

                        if (DBNull.Value != leitor["fp_conta_id"])
                        {
                            fp.fp_conta_id = Convert.ToInt32(leitor["fp_conta_id"]);
                        }
                        else
                        {
                            fp.fp_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["fp_baixa_automatica"])
                        {
                            fp.fp_baixa_automatica = Convert.ToBoolean(leitor["fp_baixa_automatica"]);
                        }
                        else
                        {
                            fp.fp_baixa_automatica = false;
                        }

                        if (DBNull.Value != leitor["fp_dataCriacao"])
                        {
                            fp.fp_dataCriacao = Convert.ToDateTime(leitor["fp_dataCriacao"]);
                        }
                        else
                        {
                            fp.fp_dataCriacao = new DateTime();
                        }

                        fp.fp_nome = leitor["fp_nome"].ToString();
                        fp.fp_meio_pgto_nfe = leitor["fp_meio_pgto_nfe"].ToString();
                        fp.fp_vinc_conta_corrente = leitor["fp_vinc_conta_corrente"].ToString();
                        fp.fp_identificacao = leitor["fp_identificacao"].ToString();
                        fp.fp_tipo_integracao_nfe = leitor["fp_tipo_integracao_nfe"].ToString();
                        fp.fp_bandeira_cartao = leitor["fp_bandeira_cartao"].ToString();
                        fp.fp_cnpj_credenciadora_cartao = leitor["fp_cnpj_credenciadora_cartao"].ToString();
                        fp.fp_status = leitor["fp_status"].ToString();                        
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("FormaPagamento", "buscaFormasPagamento", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return fp;
        }

        //Cadastrar forma de pagamento

        //Alterar forma de pagamento

        //Deletar forma de pagamento


    }
}
