using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_usuario
    {
        public int usuario_id { get; set; }
        public string usuario_nome { get; set; }
        public string usuario_dcto { get; set; }
        public string usuario_email { get; set; }
        public Usuario usuario_logado { get; set; }


        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Vm_usuario()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //Lista usuario pela conta (lista não traz o usuário 'adm' e o usuario que fez a requisição

        public List<Vm_usuario> listaVmUsuario(int conta_id, int usuario_id)
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

            return (List<Vm_usuario>)lista.AsEnumerable();
        }
    }
}
