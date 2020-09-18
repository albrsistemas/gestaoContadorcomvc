using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace gestaoContadorcomvc.Models
{
    public class ContaPadrao
    {
        public int contaPadrao_id { get; set; }
        public string contaPadrao_classificacao { get; set; }
        public string contaPadrao_descricao { get; set; }
        public string contaPadrao_apelido { get; set; }
        public string contaPadrao_grupo { get; set; }
        public string contaPadrao_tipo { get; set; }
        public int contaPadrao_conta_id { get; set; }
        public string contaPadrao_especie { get; set; }
        public string contaPadrao_natureza { get; set; }
        public int contaPadrao_filhos { get; set; }
        public string contaPadrao_status { get; set; }
        public string contapadrao_tags { get; set; }
        public string contaPadrao_codigoBanco { get; set; }


        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ContaPadrao()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();


        //Lista de categorias
        public Vm_categoria listaCategorias(int conta_id, int usuario_id, int contador_id, Usuario user)
        {
            Vm_categoria categoria = new Vm_categoria();
            List<Vm_categoria> caixaBanco = new List<Vm_categoria>();
            List<Vm_categoria> receitas = new List<Vm_categoria>();
            List<Vm_categoria> despesas = new List<Vm_categoria>();


            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from contapadrao LEFT join caixabanco on caixabanco.caixaBanco_contaPadrao_id = contapadrao.contaPadrao_id where contapadrao.contaPadrao_status = 'Ativo' and (contapadrao_classificacao LIKE '01.1.1.01.%' or contapadrao_classificacao LIKE '01.1.1.02.%' or contapadrao_classificacao LIKE '03.2.1.01.%' or contapadrao_classificacao LIKE '04.2.1.01.%' or contapadrao_classificacao LIKE '04.2.1.02.%' or contapadrao_classificacao LIKE '04.2.1.03.%') order by contapadrao.contaPadrao_classificacao asc;", conn);
                comando.Parameters.AddWithValue("@contador_id", contador_id);
                comando.Parameters.AddWithValue("@conta_id", conta_id);

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_categoria _categoria = new Vm_categoria();
                        if (DBNull.Value != leitor["contaPadrao_id"])
                        {
                            _categoria.contaPadrao_id = Convert.ToInt32(leitor["contaPadrao_id"]);
                        }
                        else
                        {
                            _categoria.contaPadrao_id = 0;
                        }

                        if (DBNull.Value != leitor["contaPadrao_conta_id"])
                        {
                            _categoria.contaPadrao_conta_id = Convert.ToInt32(leitor["contaPadrao_conta_id"]);
                        }
                        else
                        {
                            _categoria.contaPadrao_conta_id = 0;
                        }                        
                        _categoria.contaPadrao_classificacao = leitor["contaPadrao_classificacao"].ToString();
                        _categoria.contaPadrao_descricao = leitor["contaPadrao_descricao"].ToString();
                        _categoria.contaPadrao_apelido = leitor["contaPadrao_apelido"].ToString();
                        _categoria.contaPadrao_grupo = leitor["contaPadrao_grupo"].ToString();
                        _categoria.contaPadrao_tipo = leitor["contaPadrao_tipo"].ToString();
                        _categoria.contaPadrao_especie = leitor["contaPadrao_especie"].ToString();
                        _categoria.contaPadrao_natureza = leitor["contaPadrao_natureza"].ToString();
                        _categoria.vm_categoria_caixaBanco = leitor["caixaBanco_conta_id"].ToString();
                        if(DBNull.Value != leitor["contaPadrao_filhos"])
                        {
                            _categoria.contaPadrao_filhos = Convert.ToInt32(leitor["contaPadrao_filhos"]);
                        }
                        else
                        {
                            _categoria.contaPadrao_filhos = 0;
                        }
                        _categoria.contaPadrao_status = leitor["contaPadrao_status"].ToString();
                        _categoria.contaPadrao_tags = leitor["contapadrao_tags"].ToString();
                        _categoria.contaPadrao_codigoBanco = leitor["contaPadrao_codigoBanco"].ToString();
                        _categoria.caixaBanco_conta_id = leitor["caixaBanco_conta_id"].ToString();

                        /*----*/

                        if(_categoria.contaPadrao_grupo.ToUpper() == "RECEITA")
                        {
                            receitas.Add(_categoria);
                        }
                        if (_categoria.contaPadrao_grupo.ToUpper() == "DESPESA")
                        {
                            despesas.Add(_categoria);
                        }

                        if(_categoria.contaPadrao_classificacao.Contains("01.1.1.01.") || _categoria.caixaBanco_conta_id == conta_id.ToString())
                        {
                            caixaBanco.Add(_categoria);
                        }
                    }
                }

                categoria.caixaBcos = caixaBanco;
                categoria.receitas = receitas;
                categoria.despesas = despesas;
                categoria.userLogado = user;
            }
            catch (Exception e)
            {
                string msg = "Tentativa de listar categorias fracassou: " + e.Message.ToString().Substring(0, 300);

                //log.log("ContaPadrao", "listaCategorias", "Erro", msg, conta_id, usuario_id);                
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

        //Busca categoria por id
        public Vm_categoria buscaCategoria(string id, int conta_id, int usuario_id)
        {
            Vm_categoria _categoria = new Vm_categoria();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from contapadrao where contapadrao.contaPadrao_id = @id", conn);                
                comando.Parameters.AddWithValue("@id", id);

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {   
                        if (DBNull.Value != leitor["contaPadrao_id"])
                        {
                            _categoria.contaPadrao_id = Convert.ToInt32(leitor["contaPadrao_id"]);
                        }
                        else
                        {
                            _categoria.contaPadrao_id = 0;
                        }

                        if (DBNull.Value != leitor["contaPadrao_conta_id"])
                        {
                            _categoria.contaPadrao_conta_id = Convert.ToInt32(leitor["contaPadrao_conta_id"]);
                        }
                        else
                        {
                            _categoria.contaPadrao_conta_id = 0;
                        }
                        _categoria.contaPadrao_classificacao = leitor["contaPadrao_classificacao"].ToString();
                        _categoria.contaPadrao_descricao = leitor["contaPadrao_descricao"].ToString();
                        _categoria.contaPadrao_apelido = leitor["contaPadrao_apelido"].ToString();
                        _categoria.contaPadrao_grupo = leitor["contaPadrao_grupo"].ToString();
                        _categoria.contaPadrao_tipo = leitor["contaPadrao_tipo"].ToString();
                        _categoria.contaPadrao_especie = leitor["contaPadrao_especie"].ToString();
                        _categoria.contaPadrao_natureza = leitor["contaPadrao_natureza"].ToString();                        
                        if (DBNull.Value != leitor["contaPadrao_filhos"])
                        {
                            _categoria.contaPadrao_filhos = Convert.ToInt32(leitor["contaPadrao_filhos"]);
                        }
                        else
                        {
                            _categoria.contaPadrao_filhos = 0;
                        }
                        _categoria.contaPadrao_status = leitor["contaPadrao_status"].ToString();
                        _categoria.contaPadrao_tags = leitor["contapadrao_tags"].ToString();                        
                    }
                }
            }
            catch (Exception e)
            {
                string msg = "Tentativa de listar categorias fracassou: " + e.Message.ToString().Substring(0, 300);

                //log.log("ContaPadrao", "listaCategorias", "Erro", msg, conta_id, usuario_id);                
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }


            return _categoria;
        }

        //Criar categoria tipo 'Clinte'
        public string criarCategoriaCliente(Vm_categoria categoriaPai, string nome, string apelido, int conta_id, int usuario_id)
        {
            string retorno = "Categoria cadastrada com sucesso !";

            string filho_classificacao = categoriaPai.contaPadrao_classificacao + "." + (categoriaPai.contaPadrao_filhos + 1).ToString().PadLeft(8, '0');

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into contaPadrao (contaPadrao_classificacao, contaPadrao_descricao, contaPadrao_apelido, contaPadrao_grupo, contaPadrao_tipo, contaPadrao_especie, contaPadrao_natureza, contaPadrao_filhos, contaPadrao_status, contapadrao_tags, contaPadrao_conta_id) values " +
                    "(@classificacao, @descricao, @apelido, @grupo, @tipo, @especie, @natureza, @filhos, @status, @tags, @conta_id);";
                comando.Parameters.AddWithValue("@classificacao", filho_classificacao);
                comando.Parameters.AddWithValue("@descricao", nome);
                comando.Parameters.AddWithValue("@apelido", apelido);
                comando.Parameters.AddWithValue("@grupo", categoriaPai.contaPadrao_grupo);
                comando.Parameters.AddWithValue("@tipo", "Cliente");
                comando.Parameters.AddWithValue("@especie", "Analitica");
                comando.Parameters.AddWithValue("@natureza", categoriaPai.contaPadrao_natureza);
                comando.Parameters.AddWithValue("@filhos", 0);
                comando.Parameters.AddWithValue("@status", "Ativo");
                comando.Parameters.AddWithValue("@tags", "ID" + conta_id);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();

                comando.CommandText = "UPDATE contapadrao set contapadrao.contaPadrao_filhos = @qtd where contapadrao.contaPadrao_id = @id";
                comando.Parameters.AddWithValue("@id", categoriaPai.contaPadrao_id);
                comando.Parameters.AddWithValue("@qtd", categoriaPai.contaPadrao_filhos + 1);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Tentativa de criar nova categoria de nome: " + nome + " Cadastrado com sucesso";
                log.log("ContaPadrao", "criarCategoriaCliente", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Tentativa de criar nova categoria de nome: " + nome + " fracassou" + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao cadastrar categoria, tente novamente. Se persistir entre em contato com o suporte !";

                log.log("ContaPadrao", "criarCategoriaCliente", "Erro", msg, conta_id, usuario_id);

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

        //Altera categoria para 'Deletado'
        public string deletaCategoria(int conta_id, int usuario_id, int categoria_id)
        {
            string retorno = "Categoria apagada com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE contapadrao set contapadrao.contaPadrao_status = 'Deletado' where contapadrao.contaPadrao_id = @id;";
                comando.Parameters.AddWithValue("@id", categoria_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de apapar a categoria ID: " + categoria_id + " Apagado com Sucesso";
                log.log("ContaPadrao", "deletaCategoria", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de apapar a categoria ID: " + categoria_id + " fracassou" + e.Message.ToString().Substring(0, 300);
                retorno = "Erro ao apagar a categoria, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("ContaPadrao", "deletaCategoria", "Erro", msg, conta_id, usuario_id);
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

        //Vincular conta bancária ao Cliente (conta)'
        public string vincularBanco(string codigoBanco, int conta_id, int usuario_id)
        {
            string retorno = "Conta bancária cadastrada com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "INSERT into caixabanco (caixaBanco_contaPadrao_id, caixaBanco_conta_id) values (@caixaBanco_contaPadrao_id, @caixaBanco_conta_id)";
                comando.Parameters.AddWithValue("@caixaBanco_contaPadrao_id", codigoBanco);
                comando.Parameters.AddWithValue("@caixaBanco_conta_id", conta_id);                
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Tentativa de vincular novo banco de codigo: " + codigoBanco + " Cadastrado com sucesso";
                log.log("ContaPadrao", "vincularBanco", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Tentativa de vincular novo banco de codigo: " + codigoBanco + " fracassou" + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao cadastrar conta bancária, tente novamente. Se persistir entre em contato com o suporte !";

                log.log("ContaPadrao", "vincularBanco", "Erro", msg, conta_id, usuario_id);

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

        //Listar bancos por códigos'
        public List<ContaPadrao> listaBancos()
        {
            List<ContaPadrao> bancos = new List<ContaPadrao>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from contapadrao where contapadrao.contaPadrao_classificacao like '01.1.1.02.%';";                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        bancos.Add(new ContaPadrao
                        {
                            contaPadrao_id = Convert.ToInt32(leitor["contaPadrao_id"]),
                            contaPadrao_descricao = leitor["contaPadrao_descricao"].ToString(),
                            contaPadrao_codigoBanco = leitor["contaPadrao_codigoBanco"].ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Transacao.Rollback();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return bancos;
        }

        //Delete vínculo cliente a conta bancária'
        public string deletaBanco(string id_banco, int conta_id, int usuario_id)
        {
            string retorno = "Conta bancária excluída com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "DELETE from caixabanco WHERE caixabanco_conta_id = @conta_id and caixabanco_contaPadrao_id = @id_banco;";
                comando.Parameters.AddWithValue("@id_banco", id_banco);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de excluir conta bancária: " + id_banco + " com sucesso";
                log.log("ContaPadrao", "deletaBanco", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de escluir conta bancária: " + id_banco + " fracassou" + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao excluir a conta bancária, tente novamente. Se persistir entre em contato com o suporte !";

                log.log("ContaPadrao", "deletaBanco", "Erro", msg, conta_id, usuario_id);

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

        /*METODOS DO CONTROLADOR ContaPadrao*/

        //Lista contas
        public List<ContaPadrao> listContasPadrao()
        {
            List<ContaPadrao> lista = new List<ContaPadrao>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from contapadrao order by contapadrao.contaPadrao_classificacao;";
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        ContaPadrao conta = new ContaPadrao();                      
                        
                        if (DBNull.Value != leitor["contaPadrao_id"])
                        {
                            conta.contaPadrao_id = Convert.ToInt32(leitor["contaPadrao_id"]);
                        }
                        else
                        {
                            conta.contaPadrao_id = 0;
                        }

                        if (DBNull.Value != leitor["contaPadrao_conta_id"])
                        {
                            conta.contaPadrao_conta_id = Convert.ToInt32(leitor["contaPadrao_conta_id"]);
                        }
                        else
                        {
                            conta.contaPadrao_conta_id = 0;
                        }                        
                        conta.contaPadrao_classificacao = leitor["contaPadrao_classificacao"].ToString();
                        conta.contaPadrao_descricao = leitor["contaPadrao_descricao"].ToString();
                        conta.contaPadrao_apelido = leitor["contaPadrao_apelido"].ToString();
                        conta.contaPadrao_grupo = leitor["contaPadrao_grupo"].ToString();
                        conta.contaPadrao_tipo = leitor["contaPadrao_tipo"].ToString();
                        conta.contaPadrao_especie = leitor["contaPadrao_especie"].ToString();
                        conta.contaPadrao_natureza = leitor["contaPadrao_natureza"].ToString();                        
                        if (DBNull.Value != leitor["contaPadrao_filhos"])
                        {
                            conta.contaPadrao_filhos = Convert.ToInt32(leitor["contaPadrao_filhos"]);
                        }
                        else
                        {
                            conta.contaPadrao_filhos = 0;
                        }
                        conta.contaPadrao_status = leitor["contaPadrao_status"].ToString();
                        conta.contapadrao_tags = leitor["contapadrao_tags"].ToString();
                        conta.contaPadrao_codigoBanco = leitor["contaPadrao_codigoBanco"].ToString();

                        lista.Add(conta);
                    }
                }
            }
            catch (Exception)
            {
                Transacao.Rollback();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return lista;
        }


    }
}
