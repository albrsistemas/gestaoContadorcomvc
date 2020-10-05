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
    public class Categoria
    {
        //Atributos
        public int categoria_id { get; set; }
        public string categoria_classificacao { get; set; }
        public string categoria_nome { get; set; }
        public DateTime categoria_dataCriacao { get; set; }
        public int categoria_conta_id { get; set; }
        public string categoria_tipo { get; set; } //sintética ou analítica
        public string categoria_escopo { get; set; } //entrada ou saída de recursos
        public string categoria_status { get; set; } //Ativo ou Deletado
        public string categoria_conta_contabil { get; set; }
        public string categoria_requer_provisao { get; set; } //Campo criado, mas não está usado. Veriricar necesside e contexto.

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Categoria()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Listar categorias cliente
        public List<Vm_categoria> listaCategorias(int conta_id, int usuario_id, string contador_id, string plano_id)
        {
            List<Vm_categoria> categorias = new List<Vm_categoria>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                //comando.CommandText = "SELECT * from categoria where categoria_conta_id = @conta_id and categoria.categoria_status = 'Ativo' order by categoria_classificacao";
                comando.CommandText = "SELECT * from categoria LEFT JOIN categoria_contaonline on categoria.categoria_id = categoria_contaonline.cco_categoria_id and categoria_contaonline.cco_contador_conta_id = @contador_id and categoria_contaonline.cco_plano_id = @plano_id LEFT JOIN contacontabil on categoria_contaonline.cco_ccontabil_id = contacontabil.ccontabil_id where categoria_conta_id = @conta_id and categoria.categoria_status = 'Ativo' order by categoria_classificacao;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@contador_id", contador_id);
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_categoria categoria = new Vm_categoria();

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
                        if (DBNull.Value != leitor["categoria_dataCriacao"])
                        {
                            categoria.categoria_dataCriacao = Convert.ToDateTime(leitor["categoria_dataCriacao"]);
                        }
                        else
                        {
                            categoria.categoria_dataCriacao = new DateTime();
                        }
                        categoria.categoria_classificacao = leitor["categoria_classificacao"].ToString();
                        categoria.categoria_nome = leitor["categoria_nome"].ToString();
                        categoria.categoria_tipo = leitor["categoria_tipo"].ToString();
                        categoria.categoria_escopo = leitor["categoria_escopo"].ToString();
                        categoria.categoria_status = leitor["categoria_status"].ToString();
                        categoria.categoria_requer_provisao = leitor["categoria_requer_provisao"].ToString();                       
                        categoria.categoria_conta_contabil = leitor["categoria_conta_contabil"].ToString();                       
                        categoria.categoria_contaonline = leitor["ccontabil_classificacao"].ToString();                       

                        categorias.Add(categoria);
                    }
                }
            }
            catch(Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Categoria", "listaCategorias", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return categorias;
        }

        //Cadastrar categoria
        public string startCategoria(int conta_id, int usuario_id)
        {
            string retorno = "Categoria cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into categoria (categoria_classificacao, categoria_nome, categoria_tipo, categoria_conta_id, categoria_escopo, categoria_status) VALUES('1', 'ENTRADA DE RECURSOS', 'Sintetica' , @conta_id_comand1, 'Entrada', 'Ativo');";
                comando.Parameters.AddWithValue("@conta_id_comand1", conta_id);
                comando.ExecuteNonQuery();
                comando.CommandText = "INSERT into categoria (categoria_classificacao, categoria_nome, categoria_tipo, categoria_conta_id, categoria_escopo, categoria_status) VALUES('2', 'SAIDA DE RECURSOS', 'Sintetica' , @conta_id, 'Saida', 'Ativo');";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Start de categorias do cliente ID: " + conta_id + " Cadastrado com sucesso";
                log.log("Categoria", "startCategoria", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Start de categorias do cliente ID: " + conta_id + " fracassou > " + e.Message.ToString().Substring(0, 300);

                log.log("Categoria", "startCategoria", "Erro", msg, conta_id, usuario_id);

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

        //Verificar se classificação da categoria existe
        public bool classificacaoExiste(string valor, int conta_id)
        {
            bool localizado = false;
            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT categoria.categoria_classificacao from categoria WHERE categoria.categoria_conta_id = @conta_id and categoria.categoria_status = 'Ativo' and categoria.categoria_classificacao = @valor;", conn);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@valor", valor);
                var leitor = comando.ExecuteReader();
                localizado = leitor.HasRows;
                conn.Clone();
            }
            catch (Exception e)
            {
                string erro = e.ToString();
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

        //Cadastrar grupo categoria
        public string cadastrarCategoriaGrupo(string classificacao, string nome, string escopo, int conta_id, int usuario_id)
        {
            string retorno = "Grupo cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into categoria (categoria_classificacao, categoria_nome, categoria_tipo, categoria_conta_id, categoria_escopo, categoria_status) VALUES(@classificacao, @nome, 'Sintetica' , @conta_id, @escopo, 'Ativo');";
                comando.Parameters.AddWithValue("@classificacao", classificacao);
                comando.Parameters.AddWithValue("@nome", nome);
                comando.Parameters.AddWithValue("@escopo", escopo);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();               
                

                Transacao.Commit();

                string msg = "Cadastro de novo grupo de categoria com nome: " + nome + " Cadastrado com sucesso";
                log.log("Categoria", "cadastrarCategoriaGrupo", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Cadastro de novo grupo de categoria com nome: " + nome + " fracassou > " + e.Message.ToString().Substring(0, 300);

                log.log("Categoria", "cadastrarCategoriaGrupo", "Erro", msg, conta_id, usuario_id);

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

        //Cadastrar categoria
        public string cadastrarCategoria(string classificacao, string nome, string escopo, int conta_id, int usuario_id, string categoria_conta_contabil)
        {
            string retorno = "Categoria cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into categoria (categoria_classificacao, categoria_nome, categoria_tipo, categoria_conta_id, categoria_escopo, categoria_status, categoria_conta_contabil) VALUES(@classificacao, @nome, 'Analítica' , @conta_id, @escopo, 'Ativo', @categoria_conta_contabil);";
                comando.Parameters.AddWithValue("@classificacao", classificacao);
                comando.Parameters.AddWithValue("@nome", nome);
                comando.Parameters.AddWithValue("@escopo", escopo);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@categoria_conta_contabil", categoria_conta_contabil);
                comando.ExecuteNonQuery();


                Transacao.Commit();

                string msg = "Cadastro de nova categoria com nome: " + nome + " Cadastrada com sucesso";
                log.log("Categoria", "cadastrarCategoria", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Cadastro de nova categoria com nome: " + nome + " fracassou > " + e.Message.ToString().Substring(0, 300);

                log.log("Categoria", "cadastrarCategoria", "Erro", msg, conta_id, usuario_id);

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

        //Buscar categoria por ID
        public Vm_categoria buscaCategoria(int id)
        {
            Vm_categoria categoria = new Vm_categoria();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from categoria where categoria_id = @id";
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
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
                        if (DBNull.Value != leitor["categoria_dataCriacao"])
                        {
                            categoria.categoria_dataCriacao = Convert.ToDateTime(leitor["categoria_dataCriacao"]);
                        }
                        else
                        {
                            categoria.categoria_dataCriacao = new DateTime();
                        }
                        categoria.categoria_classificacao = leitor["categoria_classificacao"].ToString();
                        categoria.categoria_nome = leitor["categoria_nome"].ToString();
                        categoria.categoria_tipo = leitor["categoria_tipo"].ToString();
                        categoria.categoria_escopo = leitor["categoria_escopo"].ToString();
                        categoria.categoria_status = leitor["categoria_status"].ToString();
                        categoria.categoria_conta_contabil = leitor["categoria_conta_contabil"].ToString();
                        categoria.categoria_requer_provisao = leitor["categoria_requer_provisao"].ToString();
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

            return categoria;
        }

        //Alterar nome categoria
        public string alterarNomeCategoria(string nome, string categoria_conta_contabil, int id, int conta_id, int usuario_id)
        {
            string retorno = "Categoria alterada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE categoria set categoria.categoria_nome = @nome, categoria_conta_contabil = @categoria_conta_contabil where categoria.categoria_id = @id;";                
                comando.Parameters.AddWithValue("@nome", nome);                
                comando.Parameters.AddWithValue("@categoria_conta_contabil", categoria_conta_contabil);                
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Alteração da categoria nome: " + nome + " Alterada com sucesso";
                log.log("Categoria", "alterarNomeCategoria", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Alteração da categoria nome: " + nome + " fracassou > " + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao alterar a categoria. Tente novamente, caso persita entre em contato com o suporte!";

                log.log("Categoria", "alterarNomeCategoria", "Erro", msg, conta_id, usuario_id);

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

        //Buscar categoria por ID
        public int quatidadeRegistroGrupo(string classificacao, int conta_id)
        {
            int retorno = 0;

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                string classific = classificacao + "%";
                comando.CommandText = "SELECT COUNT(*) as 'quantidade_registros' from categoria WHERE categoria.categoria_conta_id = @conta_id and categoria.categoria_status = 'Ativo' AND categoria.categoria_classificacao like @classific;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@classific", classific);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["quantidade_registros"])
                        {
                            retorno = Convert.ToInt32(leitor["quantidade_registros"]);
                        }
                        else
                        {
                            retorno = 0;
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

            return retorno;
        }

        //Delete categoria (atualizar status para 'Deletado')
        public string deletarCategoria(int id, string nome, int conta_id, int usuario_id)
        {
            string retorno = "Categoria apagada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE categoria set categoria.categoria_status = 'Deletado' where categoria.categoria_id = @id;";                
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Exclusão da categoria nome: " + nome + " Excluida com sucesso";
                log.log("Categoria", "alterarNomeCategoria", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Exclusão da categoria nome: " + nome + " fracassou > " + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao excluir a categoria. Tente novamente, caso persita entre em contato com o suporte!";

                log.log("Categoria", "deletarCategoria", "Erro", msg, conta_id, usuario_id);

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
