using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace gestaoContadorcomvc.Models.Autenticacao
{
    public class Usuario
    {        
        //Atributos
        public int usuario_id { get; set; }
        public int usuario_conta_id { get; set; }
        public string usuario_nome { get; set; }     
        public string usuario_dcto { get; set; }   
        public string usuario_user { get; set; }        
        public string usuario_senha { get; set; }        
        public string usuario_email { get; set; }
        public string Role { get; set; }
        public string permissoes { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Usuario()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Localizar usuario pelo user e senha para verificação de login
        public Usuario buscaUsuarioLogin(string usuario, string senha)
        {
            Usuario user = new Usuario();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("select * from usuario where usuario_senha = md5(@senha) and usuario_user = md5(@usuario) and usuario_status != 'Deletado';", conn);
                comando.Parameters.AddWithValue("@senha", senha);
                comando.Parameters.AddWithValue("@usuario", usuario);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        user.usuario_id = Convert.ToInt32(leitor["usuario_id"]);
                        user.usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]);
                        user.usuario_nome = leitor["usuario_nome"].ToString();
                        user.usuario_dcto = leitor["usuario_dcto"].ToString();
                        user.usuario_email = leitor["usuario_email"].ToString();                        
                        user.Role = leitor["Role"].ToString();
                        user.permissoes = leitor["usuario_permissoes"].ToString();
                    }
                }
                else
                {
                    user = null;
                }

                conn.Close();
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

            return user;
        }

        //Lista usuario pela conta (lista não traz o usuário 'adm' e o usuario que fez a requisição

        public List<Vm_usuario> listaUsuarios(int conta_id, int usuario_id)
        {
            List<Vm_usuario> lista = new List<Vm_usuario>();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_conta_id = @conta and Role = 'user' and usuario_id != @usuario_id and usuario_status != 'Deletado';", conn);
                comando.Parameters.AddWithValue("@conta", conta_id);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        lista.Add(new Vm_usuario
                        {
                            usuario_nome = leitor["usuario_nome"].ToString(),
                            usuario_email = leitor["usuario_email"].ToString(),
                            usuario_dcto = leitor["usuario_dcto"].ToString(),                            
                            usuario_id = Convert.ToInt32(leitor["usuario_id"]),                            
                        });
                    }
                }
                else
                {
                    lista = null;
                }

                conn.Close();
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

            return lista;
        }

        //Lista de usuários com exlusão do usuário 'adm' e o usuário requerente
        public List<Usuario> ListaUsuario(int conta_id, int usuario_id)
        {
            List<Usuario> usuarios = new List<Usuario>(usuario_id);


            //List<Usuario>.Enumerator<Usuario> usuarios = new List<Usuario>();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_conta_id = @conta and Role = 'user' and usuario_id != @usuario_id and usuario_status != 'Deletado';", conn);
                comando.Parameters.AddWithValue("@conta", conta_id);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        usuarios.Add(new gestaoContadorcomvc.Models.Autenticacao.Usuario
                        {
                            usuario_nome = leitor["usuario_nome"].ToString(),
                            usuario_email = leitor["usuario_email"].ToString(),
                            usuario_dcto = leitor["usuario_dcto"].ToString(),
                            usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]),
                            usuario_id = Convert.ToInt32(leitor["usuario_id"]),
                            Role = leitor["Role"].ToString(),
                            permissoes = leitor["usuario_permissoes"].ToString()
                        });
                    }
                }
                else
                {
                    usuarios = null;
                }

                conn.Close();
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

            return usuarios;
        }

        //Cadastrar novo usuário
        public string novoUsuario(string nome, string dcto, string usuario, string senha, int conta_id, string email, string permissoes, int usuario_id)
        {
            string retorno = "Usuário cadastrado com sucesso !";
            
            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_novoUsuario (@usuario_nome, @usuario_dcto, md5(@usuario_user), md5(@usuario_senha), @usuario_conta_id, @usuario_permissoes, @usuario_email);";
                comando.Parameters.AddWithValue("@usuario_nome", nome);
                comando.Parameters.AddWithValue("@usuario_dcto", dcto);
                comando.Parameters.AddWithValue("@usuario_user", usuario);
                comando.Parameters.AddWithValue("@usuario_senha", senha);
                comando.Parameters.AddWithValue("@usuario_conta_id", conta_id);
                comando.Parameters.AddWithValue("@usuario_email", email);
                comando.Parameters.AddWithValue("@usuario_permissoes", permissoes);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de criar novo usuário de nome: " + nome + " Cadastrado com sucesso";
                log.log("Usuario", "novoUsuario", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Tentativa de criar novo usuário de nome: " + nome + " fracassou" + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao cadastrar usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "novoUsuario", "Erro", msg, conta_id, usuario_id);
                Transacao.Rollback();                                
            }
            finally
            {
                conn.Close();
            }

            return retorno;
        }

        //Busca um usuário por id
        public Vm_usuario BuscaUsuario(int usuario_id)
        {
            Vm_usuario usuario = new Vm_usuario();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_id = @usuario_id;", conn);                
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        usuario.usuario_nome = leitor["usuario_nome"].ToString();
                        usuario.usuario_email = leitor["usuario_email"].ToString();
                        usuario.usuario_dcto = leitor["usuario_dcto"].ToString();
                        usuario.usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]);
                        usuario.usuario_id = Convert.ToInt32(leitor["usuario_id"]);
                        usuario.Role = leitor["Role"].ToString();
                        usuario.permissoes = leitor["usuario_permissoes"].ToString();
                    }
                }               

                conn.Close();
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

            return usuario;
        }

        //Altera usuário
        public string alteraUsuario(string nome, string dcto, int conta_id, string email, string permissoes, int usuario_id, int usuarioLogado)
        {
            string retorno = "Usuário alterado com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE usuario set usuario_nome = @usuario_nome, usuario_dcto = @usuario_dcto, usuario_email = @usuario_email, usuario_permissoes = @usuario_permissoes where usuario_id = @usuario_id;";
                comando.Parameters.AddWithValue("@usuario_nome", nome);
                comando.Parameters.AddWithValue("@usuario_dcto", dcto);                
                comando.Parameters.AddWithValue("@usuario_email", email);
                comando.Parameters.AddWithValue("@usuario_permissoes", permissoes);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de alterar o usuário ID: " + usuario_id + " Alterado com sucesso";
                log.log("Usuario", "alteraUsuario", "Sucesso", retorno, conta_id, usuarioLogado);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de alterar o usuário ID: " + usuario_id + " fracassou" + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao alterar o usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "alteraUsuario", "Erro", msg, conta_id, usuarioLogado);
                Transacao.Rollback();
            }
            finally
            {
                conn.Close();
            }

            return retorno;
        }

        //Altera usuário
        public string deletaUsuario(int conta_id, int usuario_id, int usuarioLogado)
        {
            string retorno = "Usuário apagado com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE usuario set usuario_status = 'Deletado' where usuario_id = @usuario_id;";                
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de apapar o usuário ID: " + usuario_id + " Apagado com Sucesso";
                log.log("Usuario", "deletaUsuario", "Sucesso", msg, conta_id, usuarioLogado);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de apapar o usuário ID: " + usuario_id + " fracassou" + e.Message.ToString().Substring(0, 300);
                retorno = "Erro ao apagar o usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "deletaUsuario", "Erro", msg , conta_id, usuarioLogado);
                Transacao.Rollback();
            }
            finally
            {
                conn.Close();
            }

            return retorno;
        }

        //Altera usuário
        public string EditPassword(int conta_id, int usuario_id, int usuarioLogado, string usuario_user, string usuario_senha)
        {
            string retorno = "Senha do usuário alterada com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE usuario set usuario_user =  md5(@usuario_user), usuario_senha = md5(@usuario_senha) where usuario_id = @usuario_id;";
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.Parameters.AddWithValue("@usuario_user", usuario_user);
                comando.Parameters.AddWithValue("@usuario_senha", usuario_senha);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de alterar os dados de acesso do usuário ID: " + usuario_id + " Alterado com Sucesso";
                log.log("Usuario", "EditPassword", "Sucesso", msg, conta_id, usuarioLogado);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de alterar a senha do usuário ID: " + usuario_id + " fracassou" + e.Message.ToString().Substring(0, 300);
                retorno = "Erro ao alterar os dados de acesso do usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "EditPassword", "Erro", msg, conta_id, usuarioLogado);
                Transacao.Rollback();
            }
            finally
            {
                conn.Close();
            }

            return retorno;
        }


    }

    
}
