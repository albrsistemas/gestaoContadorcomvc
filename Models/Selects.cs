using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Selects
    {
        //Atributos
        public string value { get; set; }
        public string text { get; set; }

        //Método para listar bancos
        public List<Selects> getBancos()
        {
            List<Selects> selectBancos = new List<Selects>();
            ContaPadrao contaPadrao = new ContaPadrao();
            List<ContaPadrao> bancos = new List<ContaPadrao>();

            bancos = contaPadrao.listaBancos();

            foreach (var item in bancos)
            {                
                selectBancos.Add(new Selects
                {
                    value = item.contaPadrao_id.ToString(),
                    text = (item.contaPadrao_descricao + " (" + item.contaPadrao_codigoBanco + ")")
                });
            }

            return selectBancos;
        }

        //Grupos das contas padrão
        public List<Selects> getGrupoContas()
        {
            List<Selects> contas = new List<Selects>();
            contas.Add(new Selects
            {
                value = "Ativo",
                text = "Ativo"
            });
            contas.Add(new Selects
            {
                value = "Passico",
                text = "Passivo"
            });
            contas.Add(new Selects
            {
                value = "Receita",
                text = "Receita"
            });
            contas.Add(new Selects
            {
                value = "Despesa",
                text = "Despesa"
            });

            return contas;
        }


        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Selects()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        public List<Selects> getEmpresasContador(int conta_id_contador)
        {
            List<Selects> empresas = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT conta.conta_id, conta.conta_nome, contacontabilidade.cc_conta_id_contador from conta left join contacontabilidade on conta.conta_contador = contacontabilidade.cc_id where contacontabilidade.cc_conta_id_contador = @conta_id_contador;";
                comando.Parameters.AddWithValue("@conta_id_contador", conta_id_contador);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        empresas.Add(new Selects
                        {
                            value = leitor["conta_id"].ToString(),
                            text = leitor["conta_nome"].ToString()
                        });
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

            return empresas;
        }



    }
}
