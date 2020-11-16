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
    public class ContasPagar
    {
        public int operacao { get; set; }
        public DateTime vencimento { get; set; }
        public string fornecedor { get; set; }
        public string referencia { get; set; }
        public Decimal valorOriginal { get; set; }
        public Decimal baixas { get; set; }
        public Decimal saldo { get; set; }
        public int prazo { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ContasPagar()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //lista contas a pagar para index
        public Vm_contas_pagar listaContasPagar(int usuario_id, int conta_id)
        {   
            List<ContasPagar> cps = new List<ContasPagar>();
            Vm_contas_pagar cp = new Vm_contas_pagar();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;                        

            try
            {
                DateTime today = DateTime.Today;

                comando.CommandText = "SELECT cp.*, case WHEN vencimento<@hoje THEN '2' WHEN vencimento = @hoje THEN '1' ELSE '3' END as prazo from view_contaspagar as cp WHERE conta_id = @conta_id and tipo = 'Compra' and(fp_meio_pgto_nfe != '02' and fp_meio_pgto_nfe != '03') and saldo > 0 ORDER by prazo, vencimento ASC;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@hoje", today);
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        ContasPagar conta = new ContasPagar();

                        if (DBNull.Value != leitor["operacao_"])
                        {
                            conta.operacao = Convert.ToInt32(leitor["operacao_"]);
                        }
                        else
                        {
                            conta.operacao = 0;
                        }

                        if (DBNull.Value != leitor["vencimento"])
                        {
                            conta.vencimento = Convert.ToDateTime(leitor["vencimento"]);
                        }
                        else
                        {
                            conta.vencimento = new DateTime();
                        }

                        conta.fornecedor = leitor["fornecedor"].ToString();
                        conta.referencia = leitor["referencia"].ToString();

                        if (DBNull.Value != leitor["valorOriginal"])
                        {
                            conta.valorOriginal = Convert.ToDecimal(leitor["valorOriginal"]);
                        }
                        else
                        {
                            conta.valorOriginal = 0;
                        }

                        if (DBNull.Value != leitor["valorOriginal"])
                        {
                            conta.valorOriginal = Convert.ToDecimal(leitor["valorOriginal"]);
                        }
                        else
                        {
                            conta.valorOriginal = 0;
                        }

                        if (DBNull.Value != leitor["baixas"])
                        {
                            conta.baixas = Convert.ToDecimal(leitor["baixas"]);
                        }
                        else
                        {
                            conta.baixas = 0;
                        }

                        if (DBNull.Value != leitor["saldo"])
                        {
                            conta.saldo = Convert.ToDecimal(leitor["saldo"]);
                        }
                        else
                        {
                            conta.saldo = 0;
                        }

                        if (DBNull.Value != leitor["prazo"])
                        {
                            conta.prazo = Convert.ToInt32(leitor["prazo"]);
                        }
                        else
                        {
                            conta.prazo = 0;
                        }

                        cps.Add(conta);
                    }
                } 
                
                cp.contasPagar = cps;
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("ContasPagar", "listaContasPagar", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return cp;
        }

    }
}
