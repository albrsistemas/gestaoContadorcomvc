using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    public class Clientes
    {
        public int cliente_id { get; set; }
        public string cliente_nome { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Clientes()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public List<Clientes> listaClientesPorTermo(int conta_id_contador, string termo)
        {
            List<Clientes> clientes = new List<Clientes>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT conta.conta_id, conta.conta_nome, contacontabilidade.cc_conta_id_contador from conta left join contacontabilidade on conta.conta_contador = contacontabilidade.cc_id where contacontabilidade.cc_conta_id_contador = @conta_id_contador and conta.conta_nome LIKE concat(@termo,'%');";
                comando.Parameters.AddWithValue("@conta_id_contador", conta_id_contador);
                comando.Parameters.AddWithValue("@termo", termo);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Clientes cliente = new Clientes();

                        if (DBNull.Value != leitor["conta_id"])
                        {
                           cliente.cliente_id = Convert.ToInt32(leitor["conta_id"]);
                        }
                        else
                        {
                            cliente.cliente_id = 0;
                        }

                        cliente.cliente_nome = leitor["conta_nome"].ToString();

                        clientes.Add(cliente);
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

            return clientes;
        }



    }
}
