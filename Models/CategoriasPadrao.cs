using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class CategoriasPadrao
    {
        public Categoria multas_pagas { get; set; }
        public Categoria juros_pagos { get; set; }
        public Categoria descontos_obtidos { get; set; }
        public Categoria multas_recebidas { get; set; }
        public Categoria juros_recebidos { get; set; }
        public Categoria descotos_concedidos { get; set; }
        public Categoria multas_impostos { get; set; }
        public Categoria juros_impostos { get; set; }
        public Categoria descontos_impostos { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public CategoriasPadrao()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        public CategoriasPadrao categoria_padrao(int conta_id)
        {
            CategoriasPadrao categoria_padrao = new CategoriasPadrao();
            categoria_padrao.multas_pagas = new Categoria();
            categoria_padrao.juros_pagos = new Categoria();
            categoria_padrao.descontos_obtidos = new Categoria();
            categoria_padrao.multas_recebidas = new Categoria();
            categoria_padrao.juros_recebidos = new Categoria();
            categoria_padrao.descotos_concedidos = new Categoria();
            categoria_padrao.multas_impostos = new Categoria();
            categoria_padrao.juros_impostos = new Categoria();
            categoria_padrao.descontos_impostos = new Categoria();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {   
                comando.CommandText = "SELECT * from categoria WHERE categoria.categoria_padrao is not null and categoria.categoria_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Categoria categoria = new Categoria();

                        if (DBNull.Value != leitor["categoria_id"])
                        {
                            categoria.categoria_id = Convert.ToInt32(leitor["categoria_id"]);
                        }
                        else
                        {
                            categoria.categoria_id = 0;
                        }

                        if (DBNull.Value != leitor["categoria_conta_id"])
                        {
                            categoria.categoria_conta_id = Convert.ToInt32(leitor["categoria_conta_id"]);
                        }
                        else
                        {
                            categoria.categoria_conta_id = 0;
                        }
                        //if (DBNull.Value != leitor["categoria_dataCriacao"])
                        //{
                        //    categoria.categoria_dataCriacao = Convert.ToDateTime(leitor["categoria_dataCriacao"]);
                        //}
                        //else
                        //{
                        //    categoria.categoria_dataCriacao = new DateTime();
                        //}
                        categoria.categoria_categoria_fiscal = Convert.ToBoolean(leitor["categoria_categoria_fiscal"]);
                        categoria.categoria_classificacao = leitor["categoria_classificacao"].ToString();
                        categoria.categoria_nome = leitor["categoria_nome"].ToString();
                        categoria.categoria_tipo = leitor["categoria_tipo"].ToString();
                        categoria.categoria_escopo = leitor["categoria_escopo"].ToString();
                        categoria.categoria_status = leitor["categoria_status"].ToString();
                        //categoria.categoria_requer_provisao = leitor["categoria_requer_provisao"].ToString();
                        categoria.categoria_conta_contabil = leitor["categoria_conta_contabil"].ToString();
                        //categoria.categoria_contaonline = leitor["ccontabil_classificacao"].ToString();
                        //categoria.categoria_contaonline_id = leitor["cco_id"].ToString();
                        categoria.categoria_padrao = leitor["categoria_padrao"].ToString();

                        if(categoria.categoria_padrao == "Multas Pagas")
                        {
                            categoria_padrao.multas_pagas = categoria;                            
                        }

                        if (categoria.categoria_padrao == "Juros Pagos")
                        {
                            categoria_padrao.juros_pagos = categoria;
                        }

                        if (categoria.categoria_padrao == "Descotos Obtidos")
                        {
                            categoria_padrao.descontos_obtidos = categoria;
                        }

                        if (categoria.categoria_padrao == "Multas Recebidas")
                        {
                            categoria_padrao.multas_recebidas = categoria;
                        }

                        if (categoria.categoria_padrao == "Juros Recebidos")
                        {
                            categoria_padrao.juros_recebidos = categoria;
                        }

                        if (categoria.categoria_padrao == "Descontos Concedidos")
                        {
                            categoria_padrao.descotos_concedidos = categoria;
                        }

                        if (categoria.categoria_padrao == "Multas Impostos")
                        {
                            categoria_padrao.multas_impostos = categoria;
                        }

                        if (categoria.categoria_padrao == "Juros Impostos")
                        {
                            categoria_padrao.juros_impostos = categoria;
                        }

                        if (categoria.categoria_padrao == "Descontos Impostos")
                        {
                            categoria_padrao.descontos_impostos = categoria;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);                
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return categoria_padrao;
        }


    }
}
