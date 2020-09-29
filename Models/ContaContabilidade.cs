using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class ContaContabilidade
    {
        public int cc_id { get; set; }
        public string cc_dctoContador { get; set; }
        public string cc_nomeContador { get; set; }
        public string cc_termo { get; set; }
        public DateTime cc_dataVinculacao { get; set; }
        public DateTime cc_dataDesvinculacao { get; set; }
        public int cc_conta_id { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ContaContabilidade()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public string vincularContabilidade(int usuario_id, int cc_conta_id, int cc_conta_id_contador, string cc_dctoContador, string cc_nomeContador, string cc_termo)
        {
            string retorno = "Contador vinculado com sucesso!";

            int recuperacao_id = 0;

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into contacontabilidade (cc_dctoContador, cc_nomeContador, cc_termo, cc_conta_id, cc_conta_id_contador) values (@cc_dctoContador, @cc_nomeContador, @cc_termo, @cc_conta_id, @cc_conta_id_contador);";
                comando.Parameters.AddWithValue("@cc_dctoContador", cc_dctoContador);
                comando.Parameters.AddWithValue("@cc_nomeContador", cc_nomeContador);
                comando.Parameters.AddWithValue("@cc_termo", cc_termo);
                comando.Parameters.AddWithValue("@cc_conta_id", cc_conta_id);
                comando.Parameters.AddWithValue("@cc_conta_id_contador", cc_conta_id_contador);
                comando.ExecuteNonQuery();

                //comando.Parameters.Add(new MySqlParameter("ultimoId", comando.LastInsertedId));
                //recuperacao_id = Convert.ToInt32(comando.Parameters["@ultimoId"].Value);
                //comando.ExecuteNonQuery();

                comando.CommandText = "UPDATE conta set conta_contador = @conta_contador where conta.conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_contador", comando.LastInsertedId);
                comando.Parameters.AddWithValue("@conta_id", cc_conta_id);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Vinculação do contador: " + cc_nomeContador + " Vinculado com sucesso";
                log.log("ContaContabilidade", "vincularContabilidade", "Sucesso", msg, cc_conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Vinculação do contador: " + cc_nomeContador + " fracassou > " + e.Message.ToString().Substring(0, 300);
                log.log("ContaContabilidade", "vincularContabilidade", "Erro", msg, cc_conta_id, usuario_id);

                retorno = "Erro ao vincular o contador. Tente novamente, se persistir entre em contato com o suporte.";

                Transacao.Rollback();
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

        //Buscar contabildiade cliente
        public Vm_contabilidade buscarContabilidade(int cc_id)
        {
            Vm_contabilidade contabildiade = new Vm_contabilidade();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "Select * from contacontabilidade where contacontabilidade.cc_id = @cc_id";
                comando.Parameters.AddWithValue("@cc_id", cc_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["cc_id"])
                        {
                            contabildiade.cc_id = Convert.ToInt32(leitor["cc_id"]);
                        }
                        else
                        {
                            contabildiade.cc_id = 0;
                        }
                        if (DBNull.Value != leitor["cc_conta_id"])
                        {
                            contabildiade.cc_conta_id = Convert.ToInt32(leitor["cc_conta_id"]);
                        }
                        else
                        {
                            contabildiade.cc_conta_id = 0;
                        }
                        if (DBNull.Value != leitor["cc_conta_id_contador"])
                        {
                            contabildiade.cc_conta_id_contador = Convert.ToInt32(leitor["cc_conta_id_contador"]);
                        }
                        else
                        {
                            contabildiade.cc_conta_id_contador = 0;
                        }
                        contabildiade.cc_dctoContador = leitor["cc_dctoContador"].ToString();
                        contabildiade.cc_nomeContador = leitor["cc_nomeContador"].ToString();
                        contabildiade.cc_termo = leitor["cc_termo"].ToString();
                        if (DBNull.Value != leitor["cc_dataVinculacao"])
                        {
                            contabildiade.cc_dataVinculacao = Convert.ToDateTime(leitor["cc_dataVinculacao"]);
                        }
                        else
                        {
                            contabildiade.cc_dataVinculacao = new DateTime();
                        }
                        if (DBNull.Value != leitor["cc_dataDesvinculacao"])
                        {
                            contabildiade.cc_dataDesvinculacao = Convert.ToDateTime(leitor["cc_dataDesvinculacao"]);
                        }
                        else
                        {
                            contabildiade.cc_dataDesvinculacao = new DateTime();
                        }
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

            return contabildiade;
        }

        public string desvincularContabilidade(int conta_id, int usuario_id, int cc_id, string cc_nomeContador)
        {
            string retorno = "Contador desvinculado com sucesso!";
            DateTime data = DateTime.UtcNow;
            string str_data = data.Year + "-" + data.Month + "-" + data.Day + "-" + data.Hour + ":" + data.Minute + ":" + data.Second;

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE contacontabilidade set contacontabilidade.cc_dataDesvinculacao = @data WHERE contacontabilidade.cc_id = @cc_id;";
                comando.Parameters.AddWithValue("@data", str_data);                
                comando.Parameters.AddWithValue("@cc_id", cc_id);                
                comando.ExecuteNonQuery();

                comando.CommandText = "UPDATE conta set conta_contador = 0 where conta.conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Desvinculação do contador: " + cc_nomeContador + " Desvinculado com sucesso";
                log.log("ContaContabilidade", "desvincularContabilidade", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Desvinculação do contador: " + cc_nomeContador + " fracassou > " + e.Message.ToString().Substring(0, 300);
                log.log("ContaContabilidade", "desvincularContabilidade", "Erro", msg, conta_id, usuario_id);

                retorno = "Erro ao desvincular o contador. Tente novamente, se persistir entre em contato com o suporte.";

                Transacao.Rollback();
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
