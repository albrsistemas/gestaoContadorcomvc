using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    public class PlanoCategorias
    {
        public int pc_id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Mínimo de cinco caracteres e máximo de 100")]
        public string pc_nome { get; set; }
        public int pc_conta_id { get; set; }
        public string pc_status { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public PlanoCategorias()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Lista plano de contas
        public List<PlanoCategorias> listaPlanoCategorias(int conta_id, int usuario_id)
        {
            List<PlanoCategorias> planos = new List<PlanoCategorias>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "select * from planocategorias where planocategorias.pc_conta_id = @conta_id and planocategorias.pc_status = 'Ativo'";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        PlanoCategorias plano = new PlanoCategorias();

                        if (DBNull.Value != leitor["pc_id"])
                        {
                            plano.pc_id = Convert.ToInt32(leitor["pc_id"]);
                        }
                        else
                        {
                            plano.pc_id = 0;
                        }

                        if (DBNull.Value != leitor["pc_conta_id"])
                        {
                            plano.pc_conta_id = Convert.ToInt32(leitor["pc_conta_id"]);
                        }
                        else
                        {
                            plano.pc_conta_id = 0;
                        }
                        plano.pc_nome = leitor["pc_nome"].ToString();
                        plano.pc_status = leitor["pc_status"].ToString();

                        planos.Add(plano);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("PlanoCategorias", "listaPlanoCategorias", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return planos;
        }

        //Cadastrar plano de contas
        public string cadastrarPlanoCategorias(int conta_id, string plano_nome, int usuario_id)
        {
            string retorno = "Plano de categorias cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into planocategorias (pc_nome, pc_conta_id) values (@plano_nome, @conta_id)";
                comando.Parameters.AddWithValue("@plano_nome", plano_nome);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Plano de categorias de nome: " + plano_nome + " cadastrado com sucesso";
                log.log("PlanoCategorias", "cadastrarPlanoCategorias", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de cadastrar plano de categorias de nome: " + plano_nome + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("PlanoCategorias", "cadastrarPlanoCategorias", "Erro", msg, conta_id, usuario_id);
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

        //Busca plano de contas por id
        public PlanoCategorias buscaPlanoCategorias(int plano_id, int conta_id, int usuario_id)
        {
            PlanoCategorias plano = new PlanoCategorias();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "select * from planocategorias where planocategorias.pc_id = @plano_id";
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["pc_id"])
                        {
                            plano.pc_id = Convert.ToInt32(leitor["pc_id"]);
                        }
                        else
                        {
                            plano.pc_id = 0;
                        }

                        if (DBNull.Value != leitor["pc_conta_id"])
                        {
                            plano.pc_conta_id = Convert.ToInt32(leitor["pc_conta_id"]);
                        }
                        else
                        {
                            plano.pc_conta_id = 0;
                        }
                        plano.pc_nome = leitor["pc_nome"].ToString();
                        plano.pc_status = leitor["pc_status"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = "Tentativa de buscar plano de castegorias id " + plano_id + "falhou" + e.Message.Substring(0, 300);
                log.log("PlanoCategorias", "buscaPlanoCategorias", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return plano;
        }

        //Alterar plano de contas
        public string alterarPlanoCategorias(int plano_id, string plano_nome, int conta_id, int usuario_id)
        {
            string retorno = "Plano de categorias alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update planocategorias set planocategorias.pc_nome = @plano_nome where planocategorias.pc_id = @plano_id;";
                comando.Parameters.AddWithValue("@plano_nome", plano_nome);
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Plano de categorias de nome: " + plano_nome + " alterado com sucesso";
                log.log("PlanoCategorias", "alterarPlanoCategorias", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de alterar plano de categprias de nome: " + plano_nome + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("PlanoCategorias", "alterarPlanoCategorias", "Erro", msg, conta_id, usuario_id);
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

        //Alterar plano de contas função Delete
        public string deletarPlanoCategorias(int plano_id, string plano_nome, int conta_id, int usuario_id)
        {
            string retorno = "Plano de categorias apagado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update planocategorias set planocategorias.pc_status = 'Deletado' where planocategorias.pc_id = @plano_id;";
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Plano de categorias de nome: " + plano_nome + " deletado com sucesso";
                log.log("PlanoCategorias", "deletarPlanoCategorias", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de deletar plano de categorias de nome: " + plano_nome + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("PlanoCategorias", "deletarPlanoCategorias", "Erro", msg, conta_id, usuario_id);
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
