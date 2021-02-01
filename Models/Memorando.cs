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
    public class Memorando
    {
        public int memorando_id { get; set; }
        public int memorando_conta_id { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "O código é obrigatório.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Mínimo de 1 caracter e máximo de 10")]
        public string memorando_codigo { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Mínimo de 2 caracteres e máximo de 150")]
        public string memorando_descricao { get; set; }
        public Vm_usuario user { get; set; } //Dados do usuário logado
        public IEnumerable<Memorando> memorandos { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Memorando()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Lista de conta corrente
        public List<Memorando> listMemorando(int conta_id, int usuario_id)
        {
            List<Memorando> memorandos = new List<Memorando>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from memorando WHERE memorando.memorando_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Memorando memorando = new Memorando();

                        if (DBNull.Value != leitor["memorando_id"])
                        {
                            memorando.memorando_id = Convert.ToInt32(leitor["memorando_id"]);
                        }
                        else
                        {
                            memorando.memorando_id = 0;
                        }

                        if (DBNull.Value != leitor["memorando_conta_id"])
                        {
                            memorando.memorando_conta_id = Convert.ToInt32(leitor["memorando_conta_id"]);
                        }
                        else
                        {
                            memorando.memorando_conta_id = 0;
                        }                       

                        memorando.memorando_codigo = leitor["memorando_codigo"].ToString();
                        memorando.memorando_descricao = leitor["memorando_descricao"].ToString();                        

                        memorandos.Add(memorando);
                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Memorando", "listMemorando", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return memorandos;
        }

        //Cadastrar conta corrente
        public string cadastraMemorando(
            int conta_id,
            int usuario_id,
            string memorando_codigo,
            string memorando_descricao            
            )
        {
            string retorno = "Memorando cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT INTO memorando (memorando.memorando_codigo, memorando.memorando_descricao, memorando.memorando_conta_id) values (@memorando_codigo, @memorando_descricao, @memorando_conta_id);";
                comando.Parameters.AddWithValue("@memorando_codigo", memorando_codigo);
                comando.Parameters.AddWithValue("@memorando_descricao", memorando_descricao);                
                comando.Parameters.AddWithValue("@memorando_conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro de novo memorando nome: " + memorando_descricao + " Cadastrado com sucesso";
                log.log("Memorando", "cadastraMemorando", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar o memorando. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Memorando", "cadastraMemorando", "Erro", msg, conta_id, usuario_id);
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
        public Memorando buscaMemorando(int conta_id, int usuario_id, int memorando_id)
        {
            Memorando memorando = new Memorando();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from memorando WHERE memorando.memorando_id = @memorando_id and memorando.memorando_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@memorando_id", memorando_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["memorando_id"])
                        {
                            memorando.memorando_id = Convert.ToInt32(leitor["memorando_id"]);
                        }
                        else
                        {
                            memorando.memorando_id = 0;
                        }

                        if (DBNull.Value != leitor["memorando_conta_id"])
                        {
                            memorando.memorando_conta_id = Convert.ToInt32(leitor["memorando_conta_id"]);
                        }
                        else
                        {
                            memorando.memorando_conta_id = 0;
                        }

                        memorando.memorando_codigo = leitor["memorando_codigo"].ToString();
                        memorando.memorando_descricao = leitor["memorando_descricao"].ToString();
                    }

                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Memorando", "buscaMemorando", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return memorando;
        }

        //Alterar conta corrente
        public string alteraMemorando(
            int conta_id,
            int usuario_id,
            int memorando_id,
            string memorando_codigo,
            string memorando_descricao
            )
        {
            string retorno = "Memorando alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE conta_corrente set ccorrente_nome = @ccorrente_nome, ccorrente_tipo = @ccorrente_tipo, ccorrente_saldo_abertura = @ccorrente_saldo_abertura, ccorrente_masc_contabil = @ccorrente_masc_contabil, ccorrente_status = @ccorrente_status WHERE ccorrente_id = @ccorrente_id and ccorrente_conta_id = @ccorrente_conta_id;";
                comando.Parameters.AddWithValue("@memorando_codigo", memorando_codigo);
                comando.Parameters.AddWithValue("@memorando_descricao", memorando_descricao);
                comando.Parameters.AddWithValue("@memorando_id", memorando_id);
                comando.Parameters.AddWithValue("@memorando_conta_id", conta_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração do memorando ID: "+ memorando_id +" para nome: " + memorando_descricao + " Alterado com sucesso";
                log.log("Memorando", "alteraMemorando", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar o memorandi. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Memorando", "alteraMemorando", "Erro", msg, conta_id, usuario_id);
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
        public string deleteMemorando(
           int conta_id,
           int usuario_id,
           int memorando_id
           )
        {
            string retorno = "Memorando apagado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "DELETE from memorando WHERE memorando.memorando_id = @memorando_id and memorando.memorando_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@memorando_id", memorando_id);
                comando.Parameters.AddWithValue("@memorando_conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão do memorando ID: " + memorando_id + " Excluído com sucesso";
                log.log("ContaCorrente", "deleteMemorando", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir a conta corrente. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Memorando", "deleteMemorando", "Erro", msg, conta_id, usuario_id);
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
