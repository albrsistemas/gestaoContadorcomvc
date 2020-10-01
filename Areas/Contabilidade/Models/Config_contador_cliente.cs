using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    public class Config_contador_cliente
    {
        public int ccc_id { get; set; }
        public int ccc_contador_id { get; set; }
        public int ccc_cliente_id { get; set; }
        public int ccc_planoContasVigente { get; set; }
        public string ccc_pref_novaCategoria { get; set; }
        public string ccc_pref_editCategoria { get; set; }
        public string ccc_pref_deleteCategoria { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Config_contador_cliente()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();


        //Busca conta contábil por id
        public vm_ConfigContadorCliente buscaCCC(int usuario_id, int cliente_id, int contador_id)
        {
            vm_ConfigContadorCliente ccc = new vm_ConfigContadorCliente();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from config_contador_cliente where config_contador_cliente.ccc_contador_id = @contador_id and config_contador_cliente.ccc_cliente_id = @cliente_id;";
                comando.Parameters.AddWithValue("@contador_id", contador_id);
                comando.Parameters.AddWithValue("@cliente_id", cliente_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["ccc_id"])
                        {
                            ccc.ccc_id = Convert.ToInt32(leitor["ccc_id"]);
                        }
                        else
                        {
                            ccc.ccc_id = 0;
                        }

                        if (DBNull.Value != leitor["ccc_contador_id"])
                        {
                            ccc.ccc_contador_id = Convert.ToInt32(leitor["ccc_contador_id"]);
                        }
                        else
                        {
                            ccc.ccc_contador_id = 0;
                        }

                        if (DBNull.Value != leitor["ccc_cliente_id"])
                        {
                            ccc.ccc_cliente_id = Convert.ToInt32(leitor["ccc_cliente_id"]);
                        }
                        else
                        {
                            ccc.ccc_cliente_id = 0;
                        }

                        if (DBNull.Value != leitor["ccc_planoContasVigente"])
                        {
                            ccc.ccc_planoContasVigente = Convert.ToInt32(leitor["ccc_planoContasVigente"]);
                        }
                        else
                        {
                            ccc.ccc_planoContasVigente = 0;
                        }
                        ccc.ccc_pref_novaCategoria = leitor["ccc_pref_novaCategoria"].ToString();
                        ccc.ccc_pref_editCategoria = leitor["ccc_pref_editCategoria"].ToString();
                        ccc.ccc_pref_deleteCategoria = leitor["ccc_pref_deleteCategoria"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Config_contador_cliente", "buscaCCC", "Erro", msg, contador_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return ccc;
        }
    }
}
