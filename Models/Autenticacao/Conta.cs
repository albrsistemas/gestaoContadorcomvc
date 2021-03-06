﻿using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Autenticacao
{
    public class Conta
    {
        public int conta_id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        public string conta_dcto { get; set; }

        public string conta_tipo { get; set; }

        [Required]
        public string conta_email { get; set; }

        public string conta_nome { get; set; }
        public int conta_contador { get; set; }
        public int contador_id { get; set; }


        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Conta()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public Conta buscarConta(int conta_id)
        {
            Conta conta = new Conta();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT conta.*, COALESCE(cc.cc_conta_id_contador,0) as 'cc_conta_id_contador' from conta LEFT join contacontabilidade as cc on cc.cc_id = conta.conta_contador where conta.conta_id = @conta_id";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["conta_id"])
                        {
                            conta.conta_id = Convert.ToInt32(leitor["conta_id"]);
                        }
                        else
                        {
                            conta.conta_id = 0;
                        }
                        if (DBNull.Value != leitor["conta_contador"])
                        {
                            conta.conta_contador = Convert.ToInt32(leitor["conta_contador"]);
                        }
                        else
                        {
                            conta.conta_contador = 0;
                        }
                        conta.conta_dcto = leitor["conta_dcto"].ToString();
                        conta.conta_tipo = leitor["conta_tipo"].ToString();
                        conta.conta_email = leitor["conta_email"].ToString();                        
                        conta.conta_nome = leitor["conta_nome"].ToString();
                        if (DBNull.Value != leitor["cc_conta_id_contador"])
                        {
                            conta.contador_id = Convert.ToInt32(leitor["cc_conta_id_contador"]);
                        }
                        else
                        {
                            conta.contador_id = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string erro = e.Message;
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

        public Conta buscarContaPorDcto(string dcto)
        {
            Conta conta = new Conta();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "Select conta.*, COALESCE(cc.cc_conta_id_contador,0) as 'cc_conta_id_contador' from conta LEFT join contacontabilidade as cc on cc.cc_id = conta.conta_contador where conta.conta_dcto = @dcto;";
                comando.Parameters.AddWithValue("@dcto", dcto);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["conta_id"])
                        {
                            conta.conta_id = Convert.ToInt32(leitor["conta_id"]);
                        }
                        else
                        {
                            conta.conta_id = 0;
                        }
                        
                        if (DBNull.Value != leitor["conta_contador"])
                        {
                            conta.conta_contador = Convert.ToInt32(leitor["conta_contador"]);
                        }
                        else
                        {
                            conta.conta_contador = 0;
                        }
                        if (DBNull.Value != leitor["cc_conta_id_contador"])
                        {
                            conta.contador_id = Convert.ToInt32(leitor["cc_conta_id_contador"]);
                        }
                        else
                        {
                            conta.contador_id = 0;
                        }
                        conta.conta_dcto = leitor["conta_dcto"].ToString();
                        conta.conta_tipo = leitor["conta_tipo"].ToString();
                        conta.conta_email = leitor["conta_email"].ToString();
                        conta.conta_nome = leitor["conta_nome"].ToString();                        
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

            return conta;
        }

        //Método para retornar conxteto do cliente selecionado na contabilidade
        public Conta contextoCliente(int id)
        {
            Conta conta = new Conta();

            if (id > 0)
            {
                conta = conta.buscarConta(id);                
            }
            else
            {
                conta.conta_id = 0;
                conta.conta_nome = "Nenhum cliente selecionado!";
            }

            return conta;
        }


    }
}
