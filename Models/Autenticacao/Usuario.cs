using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
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

        [Required(ErrorMessage = "O nome é obrigatório.")]

        [Display(Name = "Nome")]
        public string usuario_nome { get; set; }

        [Required]
        [Display(Name = "CPF")]
        public string usuario_dcto { get; set; }

        [Required]
        public string usuario_user { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "usuario_senha")]
        public string usuario_senha { get; set; }

        [Display(Name = "E-mail")]
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
                MySqlCommand comando = new MySqlCommand("select * from usuario where usuario_senha = md5(@senha) and usuario_user = md5(@usuario)", conn);
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
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_conta_id = @conta and Role = 'user' and usuario_id != @usuario_id;", conn);
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




        public List<Usuario> ListaUsuario(int conta_id, int usuario_id)
        {
            List<Usuario> usuarios = new List<Usuario>(usuario_id);


            //List<Usuario>.Enumerator<Usuario> usuarios = new List<Usuario>();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_conta_id = @conta and Role = 'user' and usuario_id != @usuario_id;", conn);
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


    }

    
}
