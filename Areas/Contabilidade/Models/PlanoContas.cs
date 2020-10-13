using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
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
    public class PlanoContas
    {        
        public int plano_id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Mínimo de cinco caracteres e máximo de 100")]
        public string plano_nome { get; set; }

        public string plano_status { get; set; }

        public int plano_conta_id { get; set; }

        //Atributos de controle
        public IEnumerable<gestaoContadorcomvc.Areas.Contabilidade.Models.PlanoContas> planosConta { get; set; }
        public Vm_usuario user { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public PlanoContas()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Lista plano de contas
        public List<PlanoContas> listaPlanoContas(int conta_id, int usuario_id)
        {
            List<PlanoContas> planos = new List<PlanoContas>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "select * from planocontas where planocontas.plano_conta_id = @conta_id and planocontas.plano_status = 'Ativo'";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        PlanoContas plano = new PlanoContas();

                        if (DBNull.Value != leitor["plano_id"])
                        {
                            plano.plano_id = Convert.ToInt32(leitor["plano_id"]);
                        }
                        else
                        {
                            plano.plano_id = 0;
                        }

                        if (DBNull.Value != leitor["plano_conta_id"])
                        {
                            plano.plano_conta_id = Convert.ToInt32(leitor["plano_conta_id"]);
                        }
                        else
                        {
                            plano.plano_conta_id = 0;
                        }                        
                        plano.plano_nome = leitor["plano_nome"].ToString();
                        plano.plano_status = leitor["plano_status"].ToString();

                        planos.Add(plano);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("PlanoContas", "listaPlanoContas", "Erro", msg, conta_id, usuario_id);
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
        public string cadastrarPlanoContas(int conta_id, string plano_nome, int usuario_id)
        {
            string retorno = "Plano de contas cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into planocontas (plano_nome, plano_conta_id) values (@plano_nome, @conta_id)";
                comando.Parameters.AddWithValue("@plano_nome", plano_nome);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Plano de contas de nome: " + plano_nome + " cadastrado com sucesso";
                log.log("PlanoContas", "cadastrarPlanoContas", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de cadastrar plano de contas de nome: " + plano_nome + " fracassou [" +  e.Message.Substring(0, 300) + "]";
                log.log("PlanoContas", "cadastrarPlanoContas", "Erro", msg, conta_id, usuario_id);
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
        public PlanoContas buscaPlanoContas(int plano_id, int conta_id, int usuario_id)
        {
            PlanoContas plano = new PlanoContas();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "select * from planocontas where planocontas.plano_id = @plano_id";
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    { 
                        if (DBNull.Value != leitor["plano_id"])
                        {
                            plano.plano_id = Convert.ToInt32(leitor["plano_id"]);
                        }
                        else
                        {
                            plano.plano_id = 0;
                        }

                        if (DBNull.Value != leitor["plano_conta_id"])
                        {
                            plano.plano_conta_id = Convert.ToInt32(leitor["plano_conta_id"]);
                        }
                        else
                        {
                            plano.plano_conta_id = 0;
                        }
                        plano.plano_nome = leitor["plano_nome"].ToString();
                        plano.plano_status = leitor["plano_status"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                string msg = "Tentativa de buscar plano id " + plano_id + "falhou" + e.Message.Substring(0, 300);
                log.log("PlanoContas", "buscaPlanoContas", "Erro", msg, conta_id, usuario_id);
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
        public string alterarPlanoContas(int plano_id, string plano_nome, int conta_id, int usuario_id)
        {
            string retorno = "Plano de contas alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update planocontas set planocontas.plano_nome = @plano_nome where planocontas.plano_id = @plano_id;";
                comando.Parameters.AddWithValue("@plano_nome", plano_nome);
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Plano de contas de nome: " + plano_nome + " alterado com sucesso";
                log.log("PlanoContas", "alterarPlanoContas", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de alterar plano de contas de nome: " + plano_nome + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("PlanoContas", "alterarPlanoContas", "Erro", msg, conta_id, usuario_id);
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
        public string deletarPlanoContas(int plano_id, string plano_nome, int conta_id, int usuario_id)
        {
            string retorno = "Plano de contas apagado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update planocontas set planocontas.plano_status = 'Deletado' where planocontas.plano_id = @plano_id;";                
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Plano de contas de nome: " + plano_nome + " deletado com sucesso";
                log.log("PlanoContas", "deletarPlanoContas", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de deletar plano de contas de nome: " + plano_nome + " fracassou [" + e.Message.Substring(0, 300) + "]";
                log.log("PlanoContas", "deletarPlanoContas", "Erro", msg, conta_id, usuario_id);
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
