﻿using gestaoContadorcomvc.Models.SoftwareHouse;
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
        public int fp_dia_fechamento_cartao { get; set; }
        public int fp_dia_vencimento_cartao { get; set; }
        public int fp_categoria_id_cartao { get; set; }
        public int fp_categoria_id_cartao_pagamento { get; set; }

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

        //Listar forma de pagamento por meio pgto
        public List<FormaPagamento> listFormasPagamentoPorMeioPgto(int conta_id, int usuario_id, int meio_pgto)
        {
            List<FormaPagamento> fps = new List<FormaPagamento>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from forma_pagamento as fp where fp.fp_conta_id = @conta_id and fp.fp_identificacao = 'Pagamento' and fp.fp_meio_pgto_nfe = @meio_pgto ORDER by fp.fp_status asc, fp.fp_nome ASC;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@meio_pgto", meio_pgto);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        FormaPagamento fp = new FormaPagamento();

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
                comando.CommandText = "SELECT forma_pagamento.*, COALESCE(cc.ccorrente_tipo,0) as ccorrente_tipo from forma_pagamento LEFT JOIN conta_corrente as cc on cc.ccorrente_id = forma_pagamento.fp_vinc_conta_corrente where fp_conta_id = @conta_id and fp_id = @fp_id;";
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
                        fp.ccorrente_tipo = leitor["ccorrente_tipo"].ToString();

                        if (DBNull.Value != leitor["fp_dia_fechamento_cartao"])
                        {
                            fp.fp_dia_fechamento_cartao = Convert.ToInt32(leitor["fp_dia_fechamento_cartao"]);
                        }
                        else
                        {
                            fp.fp_dia_fechamento_cartao = 0;
                        }

                        if (DBNull.Value != leitor["fp_dia_vencimento_cartao"])
                        {
                            fp.fp_dia_vencimento_cartao = Convert.ToInt32(leitor["fp_dia_vencimento_cartao"]);
                        }
                        else
                        {
                            fp.fp_dia_vencimento_cartao = 0;
                        }

                        if (DBNull.Value != leitor["fp_categoria_id_cartao"])
                        {
                            fp.fp_categoria_id_cartao = Convert.ToInt32(leitor["fp_categoria_id_cartao"]);
                        }
                        else
                        {
                            fp.fp_categoria_id_cartao = 0;
                        }

                        if (DBNull.Value != leitor["fp_categoria_id_cartao_pagamento"])
                        {
                            fp.fp_categoria_id_cartao_pagamento = Convert.ToInt32(leitor["fp_categoria_id_cartao_pagamento"]);
                        }
                        else
                        {
                            fp.fp_categoria_id_cartao_pagamento = 0;
                        }
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
        public string cadastraFormaPagamento(
            int conta_id,
            int usuario_id,
            string fp_nome,
            string fp_meio_pgto_nfe,
            bool fp_baixa_automatica,
            string fp_vinc_conta_corrente,
            string fp_identificacao,
            string fp_tipo_integracao_nfe,
            string fp_bandeira_cartao,
            string fp_cnpj_credenciadora_cartao,
            int fp_dia_fechamento_cartao,
            int fp_dia_vencimento_cartao,
            int fp_categoria_id_cartao,
            int fp_categoria_id_cartao_pagamento
            )
        {
            string retorno = "Forma de pagamento cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into forma_pagamento (fp_nome, fp_meio_pgto_nfe, fp_baixa_automatica, fp_vinc_conta_corrente, fp_identificacao, fp_tipo_integracao_nfe, fp_bandeira_cartao, fp_cnpj_credenciadora_cartao, fp_conta_id, fp_dia_fechamento_cartao, fp_dia_vencimento_cartao, fp_categoria_id_cartao, fp_categoria_id_cartao_pagamento) VALUES (@fp_nome, @fp_meio_pgto_nfe, @fp_baixa_automatica, @fp_vinc_conta_corrente, @fp_identificacao, @fp_tipo_integracao_nfe, @fp_bandeira_cartao, @fp_cnpj_credenciadora_cartao, @fp_conta_id, @fp_dia_fechamento_cartao, @fp_dia_vencimento_cartao, @fp_categoria_id_cartao, @fp_categoria_id_cartao_pagamento);";
                comando.Parameters.AddWithValue("@fp_nome", fp_nome);
                comando.Parameters.AddWithValue("@fp_meio_pgto_nfe", fp_meio_pgto_nfe);
                comando.Parameters.AddWithValue("@fp_baixa_automatica", fp_baixa_automatica);
                comando.Parameters.AddWithValue("@fp_vinc_conta_corrente", fp_vinc_conta_corrente);
                comando.Parameters.AddWithValue("@fp_identificacao", fp_identificacao);
                comando.Parameters.AddWithValue("@fp_tipo_integracao_nfe", fp_tipo_integracao_nfe);
                comando.Parameters.AddWithValue("@fp_bandeira_cartao", fp_bandeira_cartao);
                comando.Parameters.AddWithValue("@fp_cnpj_credenciadora_cartao", fp_cnpj_credenciadora_cartao);
                comando.Parameters.AddWithValue("@fp_conta_id", conta_id);
                comando.Parameters.AddWithValue("@fp_dia_fechamento_cartao", fp_dia_fechamento_cartao);
                comando.Parameters.AddWithValue("@fp_dia_vencimento_cartao", fp_dia_vencimento_cartao);
                comando.Parameters.AddWithValue("@fp_categoria_id_cartao", fp_categoria_id_cartao);
                comando.Parameters.AddWithValue("@fp_categoria_id_cartao_pagamento", fp_categoria_id_cartao_pagamento);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro de nova forma de pagamento nome: " + fp_nome + " Cadastrado com sucesso";
                log.log("FormaPagamento", "cadastraFormaPagamento", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a forma de pagamento. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("FormaPagamento", "cadastraFormaPagamento", "Erro", msg, conta_id, usuario_id);
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


        //Alterar forma de pagamento
        public string alteraFormaPagamento(
           int conta_id,
           int usuario_id,
           string fp_nome,
           string fp_meio_pgto_nfe,
           bool fp_baixa_automatica,
           string fp_vinc_conta_corrente,
           string fp_identificacao,
           string fp_tipo_integracao_nfe,
           string fp_bandeira_cartao,
           string fp_cnpj_credenciadora_cartao,
           int fp_dia_fechamento_cartao,
           int fp_dia_vencimento_cartao,
           string fp_status,
           int fp_id,
           int fp_categoria_id_cartao,
           int fp_categoria_id_cartao_pagamento
           )
        {
            string retorno = "Forma de pagamento alterada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update forma_pagamento set fp_nome = @fp_nome, fp_meio_pgto_nfe = @fp_meio_pgto_nfe, fp_baixa_automatica = @fp_baixa_automatica, fp_vinc_conta_corrente = @fp_vinc_conta_corrente, fp_identificacao = @fp_identificacao, fp_tipo_integracao_nfe = @fp_tipo_integracao_nfe, fp_bandeira_cartao = @fp_bandeira_cartao, fp_cnpj_credenciadora_cartao = @fp_cnpj_credenciadora_cartao, fp_dia_fechamento_cartao = @fp_dia_fechamento_cartao, fp_dia_vencimento_cartao = @fp_dia_vencimento_cartao, fp_status = @fp_status, fp_categoria_id_cartao = @fp_categoria_id_cartao, fp_categoria_id_cartao_pagamento = @fp_categoria_id_cartao_pagamento WHERE fp_conta_id = @conta_id and fp_id = @fp_id;";
                comando.Parameters.AddWithValue("@fp_nome", fp_nome);
                comando.Parameters.AddWithValue("@fp_meio_pgto_nfe", fp_meio_pgto_nfe);
                comando.Parameters.AddWithValue("@fp_baixa_automatica", fp_baixa_automatica);
                comando.Parameters.AddWithValue("@fp_vinc_conta_corrente", fp_vinc_conta_corrente);
                comando.Parameters.AddWithValue("@fp_identificacao", fp_identificacao);
                comando.Parameters.AddWithValue("@fp_tipo_integracao_nfe", fp_tipo_integracao_nfe);
                comando.Parameters.AddWithValue("@fp_bandeira_cartao", fp_bandeira_cartao);
                comando.Parameters.AddWithValue("@fp_cnpj_credenciadora_cartao", fp_cnpj_credenciadora_cartao);                
                comando.Parameters.AddWithValue("@fp_dia_fechamento_cartao", fp_dia_fechamento_cartao);
                comando.Parameters.AddWithValue("@fp_dia_vencimento_cartao", fp_dia_vencimento_cartao);
                comando.Parameters.AddWithValue("@fp_status", fp_status);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@fp_id", fp_id);
                comando.Parameters.AddWithValue("@fp_categoria_id_cartao", fp_categoria_id_cartao);
                comando.Parameters.AddWithValue("@fp_categoria_id_cartao_pagamento", fp_categoria_id_cartao_pagamento);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração de forma de pagamento nome: " + fp_nome + " Alterada com sucesso";
                log.log("FormaPagamento", "alteraFormaPagamento", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar a forma de pagamento. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("FormaPagamento", "alteraFormaPagamento", "Erro", msg, conta_id, usuario_id);
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

        //Deletar forma de pagamento
        public string deletaFormaPagamento(
           int conta_id,
           int usuario_id,                      
           int fp_id
           )
        {
            string retorno = "Forma de pagamento excluída com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update forma_pagamento set fp_status = 'Deletado' WHERE fp_conta_id = @conta_id and fp_id = @fp_id;";                
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@fp_id", fp_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão de forma de pagamento ID: " + fp_id + " Excluída com sucesso";
                log.log("FormaPagamento", "deletaFormaPagamento", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir a forma de pagamento. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("FormaPagamento", "deletaFormaPagamento", "Erro", msg, conta_id, usuario_id);
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

        //Listar forma de pagamento para view index
        public List<Vm_forma_pagamento> listFormasPagamentoViewIndex(int conta_id, int usuario_id)
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
                comando.CommandText = "SELECT fp.fp_id, fp.fp_nome as nome, mp.meio_pgto_descricao, COALESCE(cc.ccorrente_nome,0) as destino, fp.fp_identificacao as aplicavel, (case WHEN fp.fp_baixa_automatica = false THEN 'Não' WHEN fp.fp_baixa_automatica = true THEN cc.ccorrente_nome END) as baixa_automatica from forma_pagamento as fp LEFT JOIN meio_pgto as mp on fp.fp_meio_pgto_nfe = mp.meio_pgto_codigo LEFT JOIN conta_corrente as cc on fp.fp_vinc_conta_corrente = cc.ccorrente_id WHERE fp.fp_conta_id = @conta_id and fp.fp_status = 'Ativo' ORDER by fp.fp_identificacao DESC;";
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

                        fp.fp_nome = leitor["nome"].ToString();
                        fp.meioPgto = leitor["meio_pgto_descricao"].ToString();
                        fp.aplicavel = leitor["aplicavel"].ToString();
                        fp.baixa_automatica = leitor["baixa_automatica"].ToString();
                        string dest = leitor["destino"].ToString();

                        if(dest.Equals("0"))
                        {
                            fp.destino = "Não";                            
                        }
                        else
                        {
                            fp.destino = dest;
                        }

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



    }
}
