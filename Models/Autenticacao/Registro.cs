using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.AspNetCore.Mvc;
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
    public class Registro
    {
        //Atributos
        public string conta_tipo { get; set; }

        [Required(ErrorMessage = "O cnpj ou cpf é obrigatório")]
        [RegularExpression(@"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})",
         ErrorMessage = "Quantidade de dígitos do cnpj ou cpf inválido.")]
        [Remote("dctoExiste", "Conta", ErrorMessage = "Já existe uma conta com este cnpj ou cpf")]
        public string conta_dcto { get; set; }

        [Required(ErrorMessage = "O nome da conta é obrigatório")]
        public string conta_nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email inválido", ErrorMessageResourceName = "Email inválido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Remote("emailExiste", "Conta", ErrorMessage = "E-mail já cadastrado")]
        public string conta_email { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Mínimo de 4 caracteres e máximo de 100")]
        public string usuario_nome { get; set; }

        [Required(ErrorMessage = "O cpf é obrigatório.")]
        [RegularExpression(@"[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}",
         ErrorMessage = "Quantidade de dígitos do cnpj ou cpf inválido.")]
        public string usuario_dcto { get; set; }

        [Required(ErrorMessage = "É obrigatório definir um usuário.")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Mínimo de 4 caracteres e máximo de 40")]
        [Remote("userExiste", "Conta", ErrorMessage = "Usuário já existe")]
        public string usuario_user { get; set; }

        [Required(ErrorMessage = "É obrigatório definir uma senha.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Mínimo de 4 caracteres e máximo de 10")]
        [DataType(DataType.Password)]
        [Display(Name = "usuario_senha")]
        public string usuario_senha { get; set; }

        [Compare("usuario_senha", ErrorMessage = "A senha não confere")]
        [DataType(DataType.Password)]
        public string confirmaSenha { get; set; }

        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Registro()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS

        //objeto de log para uso nos métodos
        Log log = new Log();

        public void registro(string conta_dcto, string conta_tipo, string usuario_nome, string usuario_dcto, string usuario_user, string usuario_senha, string conta_email, string conta_nome)
        {
            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction transacao;
            transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = transacao;

            try
            {
                comando.CommandText = "CALL registrarConta(@conta_dcto,@conta_tipo,@usuario_nome, @usuario_dcto,@usuario_user,@usuario_senha, @conta_email, @conta_email, @conta_nome);";                                
                comando.Parameters.AddWithValue("@conta_dcto", conta_dcto);
                comando.Parameters.AddWithValue("@conta_tipo", conta_tipo);
                comando.Parameters.AddWithValue("@conta_email", conta_email);
                comando.Parameters.AddWithValue("@conta_nome", conta_nome);
                comando.Parameters.AddWithValue("@usuario_nome", usuario_nome);
                comando.Parameters.AddWithValue("@usuario_dcto", usuario_dcto);
                comando.Parameters.AddWithValue("@usuario_user", usuario_user);
                comando.Parameters.AddWithValue("@usuario_senha", usuario_senha);
                comando.ExecuteNonQuery();
                transacao.Commit();
            }
            catch (Exception e)
            {
                transacao.Rollback();
                log.log("Registro", "registro", "Erro", e.ToString().Substring(0, 300), 0, 0);
            }
            finally
            {
                conn.Close();
            }
        }

        //Verificar existencia de usuario para validação formulário de registro
        public bool userExiste(string usuario_user, int usuario_id)
        {
            bool localizado = false;
            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("select usuario_user from usuario where usuario_user = md5(@user) and usuario_id <> @usuario_id", conn);
                comando.Parameters.AddWithValue("@user", usuario_user);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
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

        //Verificar existencia de e-mail de usuario para validação formulário de registro
        public bool emailExiste(string conta_email, int usuario_id)
        {
            bool localizado = false;
            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("select usuario_email from usuario where usuario_email = @email and usuario_id <> @usuario_id", conn);
                comando.Parameters.AddWithValue("@email", conta_email);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
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

        //Verificar existencia de conta com mesmo cnpj para validação formulário de registro
        public bool dctoExiste(string conta_dcto)
        {
            bool localizado = false;
            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("select conta_dcto from conta where conta_dcto = @dcto", conn);
                comando.Parameters.AddWithValue("@dcto", conta_dcto);
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
    }
}
