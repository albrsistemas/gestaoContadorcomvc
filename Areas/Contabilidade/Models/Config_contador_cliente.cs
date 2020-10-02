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
        public string ccc_planoContasVigente { get; set; }
        public bool ccc_pref_contabilizacao { get; set; }
        public bool ccc_pref_novaCategoria { get; set; }
        public bool ccc_pref_editCategoria { get; set; }
        public bool ccc_pref_deleteCategoria { get; set; }

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

                        ccc.ccc_planoContasVigente = leitor["ccc_planoContasVigente"].ToString();

                        if (DBNull.Value != leitor["ccc_pref_novaCategoria"])
                        {
                            ccc.ccc_pref_novaCategoria = Convert.ToBoolean(leitor["ccc_pref_novaCategoria"]);
                        }
                        else
                        {
                            ccc.ccc_pref_novaCategoria = true;
                        }

                        if (DBNull.Value != leitor["ccc_pref_editCategoria"])
                        {
                            ccc.ccc_pref_editCategoria = Convert.ToBoolean(leitor["ccc_pref_editCategoria"]);
                        }
                        else
                        {
                            ccc.ccc_pref_editCategoria = true;
                        }

                        if (DBNull.Value != leitor["ccc_pref_deleteCategoria"])
                        {
                            ccc.ccc_pref_deleteCategoria = Convert.ToBoolean(leitor["ccc_pref_deleteCategoria"]);
                        }
                        else
                        {
                            ccc.ccc_pref_deleteCategoria = true;
                        }
                        if (DBNull.Value != leitor["ccc_pref_contabilizacao"])
                        {
                            ccc.ccc_pref_contabilizacao = Convert.ToBoolean(leitor["ccc_pref_contabilizacao"]);
                        }
                        else
                        {
                            ccc.ccc_pref_contabilizacao = false;
                        }
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

        //Cadastrar configurações
        public string cadastrarConfiguracoes(int conta_id, int usuario_id, int ccc_contador_id, int ccc_cliente_id, bool ccc_pref_contabilizacao, string ccc_planoContasVigente, bool ccc_pref_novaCategoria, bool ccc_pref_editCategoria, bool ccc_pref_deleteCategoria)
        {            
            string retorno = "Configurações gravada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into config_contador_cliente (ccc_contador_id, ccc_cliente_id, ccc_planoContasVigente, ccc_pref_contabilizacao, ccc_pref_novaCategoria, ccc_pref_editCategoria, ccc_pref_deleteCategoria) values (@ccc_contador_id, @ccc_cliente_id, @ccc_planoContasVigente, @ccc_pref_contabilizacao, @ccc_pref_novaCategoria, @ccc_pref_editCategoria, @ccc_pref_deleteCategoria);";
                comando.Parameters.AddWithValue("@ccc_contador_id", ccc_contador_id);
                comando.Parameters.AddWithValue("@ccc_cliente_id", ccc_cliente_id);
                comando.Parameters.AddWithValue("@ccc_planoContasVigente", ccc_planoContasVigente);
                comando.Parameters.AddWithValue("@ccc_pref_contabilizacao", ccc_pref_contabilizacao);
                comando.Parameters.AddWithValue("@ccc_pref_novaCategoria", ccc_pref_novaCategoria);
                comando.Parameters.AddWithValue("@ccc_pref_editCategoria", ccc_pref_editCategoria);
                comando.Parameters.AddWithValue("@ccc_pref_deleteCategoria", ccc_pref_deleteCategoria);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Configurações do cliente id: " + ccc_cliente_id + " do contador id: " + ccc_contador_id + " cadastrado com sucesso";
                log.log("Config_contador_cliente", "cadastrarConfiguracoes", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao gravar as configurações do cliente. Tente novamente. Se persistir o problema entre em contato com o suporte!";

                string msg = "Cadastro das Configurações do cliente id: " + ccc_cliente_id + " do contador id: " + ccc_contador_id + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("Config_contador_cliente", "cadastrarConfiguracoes", "Erro", msg, conta_id, usuario_id);
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

        //Cadastrar configurações
        public string alterarConfiguracoes(int conta_id, int usuario_id, int ccc_contador_id, int ccc_cliente_id, bool ccc_pref_contabilizacao, string ccc_planoContasVigente, bool ccc_pref_novaCategoria, bool ccc_pref_editCategoria, bool ccc_pref_deleteCategoria)
        {
            string retorno = "Configurações gravada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE config_contador_cliente set ccc_planoContasVigente = @ccc_planoContasVigente, ccc_pref_contabilizacao = @ccc_pref_contabilizacao, ccc_pref_novaCategoria = @ccc_pref_novaCategoria, ccc_pref_editCategoria = @ccc_pref_editCategoria, ccc_pref_deleteCategoria = @ccc_pref_deleteCategoria where ccc_contador_id = @ccc_contador_id and ccc_cliente_id = @ccc_cliente_id;";
                comando.Parameters.AddWithValue("@ccc_contador_id", ccc_contador_id);
                comando.Parameters.AddWithValue("@ccc_cliente_id", ccc_cliente_id);
                comando.Parameters.AddWithValue("@ccc_planoContasVigente", ccc_planoContasVigente);
                comando.Parameters.AddWithValue("@ccc_pref_contabilizacao", ccc_pref_contabilizacao);
                comando.Parameters.AddWithValue("@ccc_pref_novaCategoria", ccc_pref_novaCategoria);
                comando.Parameters.AddWithValue("@ccc_pref_editCategoria", ccc_pref_editCategoria);
                comando.Parameters.AddWithValue("@ccc_pref_deleteCategoria", ccc_pref_deleteCategoria);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Configurações do cliente id: " + ccc_cliente_id + " do contador id: " + ccc_contador_id + " alterada com sucesso";
                log.log("Config_contador_cliente", "alterarConfiguracoes", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao gravar as configurações do cliente. Tente novamente. Se persistir o problema entre em contato com o suporte!";

                string msg = "Alteração das Configurações do cliente id: " + ccc_cliente_id + " do contador id: " + ccc_contador_id + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("Config_contador_cliente", "alterarConfiguracoes", "Erro", msg, conta_id, usuario_id);
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

    }
}
