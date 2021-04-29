using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace gestaoContadorcomvc.Models
{
    public class Centro_custo
    {
        public int centro_custo_id { get; set; }
        public int centro_custo_conta_id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome do centro de custo é obrigatória.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Mínimo de 2 caracteres e máximo de 50")]
        public string centro_custo_nome { get; set; }
        public Vm_usuario user { get; set; }
        public IEnumerable<Centro_custo> centros_custo { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Centro_custo()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Lista de centro de custo
        public List<Centro_custo> listCentroCusto(int conta_id, int usuario_id)
        {
            List<Centro_custo> centros_custo = new List<Centro_custo>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from centro_custo WHERE centro_custo.centro_custo_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Centro_custo c = new Centro_custo();

                        if (DBNull.Value != leitor["centro_custo_id"])
                        {
                            c.centro_custo_id = Convert.ToInt32(leitor["centro_custo_id"]);
                        }
                        else
                        {
                            c.centro_custo_id = 0;
                        }

                        if (DBNull.Value != leitor["centro_custo_conta_id"])
                        {
                            c.centro_custo_conta_id = Convert.ToInt32(leitor["centro_custo_conta_id"]);
                        }
                        else
                        {
                            c.centro_custo_conta_id = 0;
                        }

                        c.centro_custo_nome = leitor["centro_custo_nome"].ToString();                        

                        centros_custo.Add(c);
                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Centro_custo", "listCentroCusto", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return centros_custo;
        }

        //Cadastrar conta corrente
        public string cadastraCentroCusto(
            int conta_id,
            int usuario_id,
            string centro_custo_nome
            )
        {
            string retorno = "Centro de custo cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into centro_custo (centro_custo.centro_custo_conta_id, centro_custo.centro_custo_nome) values (@conta_id, @centro_custo_nome);";
                comando.Parameters.AddWithValue("@centro_custo_nome", centro_custo_nome);                
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro de novo centro de custo nome: " + centro_custo_nome + " Cadastrado com sucesso";
                log.log("Centro_custo", "cadastraMemorando", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar o memorando. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Memorando", "cadastraCentroCusto", "Erro", msg, conta_id, usuario_id);
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

        //Buscar conta corrente
        public Centro_custo buscaCentroCusto(int conta_id, int usuario_id, int centro_custo_id)
        {
            Centro_custo c = new Centro_custo();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from centro_custo WHERE centro_custo.centro_custo_id = @centro_custo_id and centro_custo.centro_custo_conta_id = @centro_custo_conta_id;";
                comando.Parameters.AddWithValue("@centro_custo_conta_id", conta_id);
                comando.Parameters.AddWithValue("@centro_custo_id", centro_custo_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["centro_custo_id"])
                        {
                            c.centro_custo_id = Convert.ToInt32(leitor["centro_custo_id"]);
                        }
                        else
                        {
                            c.centro_custo_id = 0;
                        }

                        if (DBNull.Value != leitor["centro_custo_conta_id"])
                        {
                            c.centro_custo_conta_id = Convert.ToInt32(leitor["centro_custo_conta_id"]);
                        }
                        else
                        {
                            c.centro_custo_conta_id = 0;
                        }

                        c.centro_custo_nome = leitor["centro_custo_nome"].ToString();
                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Centro_custo", "buscaCentroCusto", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return c;
        }

        //Alterar conta corrente
        public string alteraCentro_custo(
            int conta_id,
            int usuario_id,
            int centro_custo_id,
            string centro_custo_nome
            )
        {
            string retorno = "Centro de custo alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE centro_custo set centro_custo.centro_custo_nome = @centro_custo_nome WHERE centro_custo.centro_custo_id = @centro_custo_id and centro_custo.centro_custo_conta_id = @centro_custo_conta_id;";
                comando.Parameters.AddWithValue("@centro_custo_nome", centro_custo_nome);
                comando.Parameters.AddWithValue("@centro_custo_id", centro_custo_id);                
                comando.Parameters.AddWithValue("@centro_custo_conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração do centro de custo ID: " + centro_custo_id + " para nome: " + centro_custo_nome + " Alterado com sucesso";
                log.log("Centro_custo", "alteraCentro_custo", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar o centro de custo. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Centro_custo", "alteraCentro_custo", "Erro", msg, conta_id, usuario_id);
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


        //Deletar conta corrente
        public string deleteCentroCusto(
           int conta_id,
           int usuario_id,
           int centro_custo_id
           )
        {
            string retorno = "Centro de custo excluído com sucesso!";

            if (centroCustoEmUso(centro_custo_id))
            {
                retorno = "Erro. Exclusão inválida, o centro de custo está sendo utilizado nos itens das operações!";

                return retorno;
            }

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "DELETE from centro_custo WHERE centro_custo.centro_custo_id = @centro_custo_id and centro_custo.centro_custo_conta_id = @centro_custo_conta_id;";
                comando.Parameters.AddWithValue("@centro_custo_id", centro_custo_id);
                comando.Parameters.AddWithValue("@centro_custo_conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão do centro de custo ID: " + centro_custo_id + " Excluído com sucesso";
                log.log("Centro_custo", "deleteCentroCusto", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir o centro de custo. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Centro_custo", "deleteCentroCusto", "Erro", msg, conta_id, usuario_id);
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

        public bool centroCustoEmUso(int centro_custo_id)
        {
            bool localizado = false;
            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT o.op_item_id from op_itens as o WHERE o.op_itens_centro_custo = @centro_custo_id;", conn);
                comando.Parameters.AddWithValue("@centro_custo_id", centro_custo_id);                
                var leitor = comando.ExecuteReader();
                localizado = leitor.HasRows;
                conn.Clone();
            }
            catch (Exception)
            {
                //string erro = e.ToString();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return localizado;
        }




    }
}
