using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Autenticacao
{    
    public class Usuario
    {        
        //Atributos
        public int usuario_id { get; set; }

        public int usuario_conta_id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]

        public string usuario_nome { get; set; }

        [Required]
        public string usuario_dcto { get; set; }

        [Required]
        public string usuario_user { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "usuario_senha")]
        public string usuario_senha { get; set; }

        public string usuario_email { get; set; }

        public string Role { get; set; }

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




    }

    
}
