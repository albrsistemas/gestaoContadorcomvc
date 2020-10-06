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
    public class Categoria_contaonline
    {
        public int cco_id { get; set; }
        public int cco_cliente_conta_id { get; set; }
        public int cco_contador_conta_id { get; set; }
        public int cco_plano_id { get; set; }
        public int cco_ccontabil_id { get; set; }
        public int cco_categoria_id { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Categoria_contaonline()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Vinculação de conta contábil
        public string vinculacaoCCO(int usuario_id, int cliente_conta_id, int contador_conta_id, string plano_id, string ccontabil_id, string categoria_id)
        {
            string retorno = "Conta on line vinculada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into categoria_contaonline (categoria_contaonline.cco_cliente_conta_id, categoria_contaonline.cco_contador_conta_id, categoria_contaonline.cco_plano_id, categoria_contaonline.cco_ccontabil_id, categoria_contaonline.cco_categoria_id) " +
                    "values (@cliente_conta_id, @contador_conta_id, @plano_id, @ccontabil_id, @categoria_id);";
                comando.Parameters.AddWithValue("@cliente_conta_id", cliente_conta_id);
                comando.Parameters.AddWithValue("@contador_conta_id", contador_conta_id);
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.Parameters.AddWithValue("@ccontabil_id", ccontabil_id);
                comando.Parameters.AddWithValue("@categoria_id", categoria_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Vinculação da conta on line id: " + ccontabil_id + " do plano id: " + plano_id + " na categoria id: " + categoria_id + " vinculada com sucesso";
                log.log("Categoria_contaonline", "vinculacaoCCO", "Sucesso", msg, contador_conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao vincular a conta on line. Tente novamente. Se persistir o problema entre em contato com o suporte!";

                string msg = "Vinculação da conta on line id: " + ccontabil_id + " do plano id: " + plano_id + " na categoria id: " + categoria_id + " fracassou [" + e.Message.Substring(0, 300) + "]";
                
                log.log("Categoria_contaonline", "vinculacaoCCO", "Erro", msg, cliente_conta_id, usuario_id);
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

        //Buscar vínculo
        public vm_categoria_contaonline buscarVinculo(int conta_id, int usuario_id, int cco_id)
        {
            vm_categoria_contaonline cco = new vm_categoria_contaonline();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT conta.conta_nome, categoria.categoria_nome, plano.plano_nome, ccontabil.ccontabil_nome, ccontabil.ccontabil_classificacao, cco.* from categoria_contaonline as cco LEFT join categoria on categoria.categoria_id = cco.cco_categoria_id LEFT JOIN planocontas as plano on plano.plano_id = cco.cco_plano_id left JOIN contacontabil as ccontabil on ccontabil.ccontabil_id = cco.cco_ccontabil_id LEFT join conta on conta.conta_id = cco.cco_cliente_conta_id where cco.cco_id = @cco_id;";
                comando.Parameters.AddWithValue("@cco_id", cco_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["cco_id"])
                        {
                            cco.cco_id = Convert.ToInt32(leitor["cco_id"]);
                        }
                        else
                        {
                            cco.cco_id = 0;
                        }

                        if (DBNull.Value != leitor["cco_cliente_conta_id"])
                        {
                            cco.cco_cliente_conta_id = Convert.ToInt32(leitor["cco_cliente_conta_id"]);
                        }
                        else
                        {
                            cco.cco_cliente_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["cco_contador_conta_id"])
                        {
                            cco.cco_contador_conta_id = Convert.ToInt32(leitor["cco_contador_conta_id"]);
                        }
                        else
                        {
                            cco.cco_contador_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["cco_plano_id"])
                        {
                            cco.cco_plano_id = Convert.ToInt32(leitor["cco_plano_id"]);
                        }
                        else
                        {
                            cco.cco_plano_id = 0;
                        }

                        if (DBNull.Value != leitor["cco_ccontabil_id"])
                        {
                            cco.cco_ccontabil_id = Convert.ToInt32(leitor["cco_ccontabil_id"]);
                        }
                        else
                        {
                            cco.cco_ccontabil_id = 0;
                        }

                        if (DBNull.Value != leitor["cco_categoria_id"])
                        {
                            cco.cco_categoria_id = Convert.ToInt32(leitor["cco_categoria_id"]);
                        }
                        else
                        {
                            cco.cco_categoria_id = 0;
                        }
                        cco.categoria_nome = leitor["categoria_nome"].ToString();
                        cco.cco_cliente_nome = leitor["conta_nome"].ToString();                        
                        cco.cco_plano_nome = leitor["plano_nome"].ToString();
                        cco.cco_ccontabil_nome = leitor["ccontabil_nome"].ToString();
                        cco.cco_ccontabil_classificacao = leitor["ccontabil_classificacao"].ToString();                        
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Categoria_contaonline", "buscarVinculo", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return cco;
        }

        //Desvinculação de conta contábil
        public string desvinculacaoCCO(int usuario_id, int cliente_conta_id, int contador_conta_id, int cco_id, string plano_id, string ccontabil_id, string categoria_id)
        {
            string retorno = "Conta on line desvinculada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "DELETE from categoria_contaonline WHERE categoria_contaonline.cco_id = @cco_id;";
                comando.Parameters.AddWithValue("@cco_id", cco_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Desvinculação da conta on line id: " + ccontabil_id + " do plano id: " + plano_id + " na categoria id: " + categoria_id + " desvinculada com sucesso";
                log.log("Categoria_contaonline", "desvinculacaoCCO", "Sucesso", msg, contador_conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao desvincular a conta on line. Tente novamente. Se persistir o problema entre em contato com o suporte!";

                string msg = "dsvinculação da conta on line id: " + ccontabil_id + " do plano id: " + plano_id + " na categoria id: " + categoria_id + " fracassou [" + e.Message.Substring(0, 300) + "]";

                log.log("Categoria_contaonline", "desvinculacaoCCO", "Erro", msg, cliente_conta_id, usuario_id);
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
