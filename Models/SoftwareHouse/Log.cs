using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.SoftwareHouse
{
    public class Log
    {
        public int log_id { get; set; }
        public string log_classe { get; set; }
        public string log_metodo { get; set; }
        public string log_tipo { get; set; }
        public string log_mensagem { get; set; }
        public int log_conta_id { get; set; }
        public int log_usuario_id { get; set; }
        public string log_data { get; set; }

        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Log()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        public void log(string classe, string metodo, string tipo, string mensagem, int conta_id, int usuario_id)
        {
            conn.Open();
            MySqlCommand myCommand = conn.CreateCommand();
            MySqlTransaction myTrans;
            myTrans = conn.BeginTransaction();
            myCommand.Connection = conn;
            myCommand.Transaction = myTrans;

            try
            {                
                myCommand.CommandText = "call registrarLog(@id,@classe,@metodo,@tipo,@mensagem,@conta_id,@usuario_id)";
                myCommand.Parameters.AddWithValue("@id", null);
                myCommand.Parameters.AddWithValue("@classe", classe);
                myCommand.Parameters.AddWithValue("@metodo", metodo);
                myCommand.Parameters.AddWithValue("@tipo", tipo);
                myCommand.Parameters.AddWithValue("@mensagem", mensagem);
                myCommand.Parameters.AddWithValue("@conta_id", conta_id);
                myCommand.Parameters.AddWithValue("@usuario_id", usuario_id);
                myCommand.ExecuteNonQuery();
                myTrans.Commit();
                /*
                    myCommand.CommandText = "insert into Test (id, desc) VALUES (100, 'Description')";
                    myCommand.ExecuteNonQuery();
                    myCommand.CommandText = "insert into Test (id, desc) VALUES (101, 'Description')";
                    myCommand.ExecuteNonQuery();
                    myTrans.Commit();
                */
            }
            catch (Exception)
            {
                myTrans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        //Lista logs
        public List<Log> logs(int conta_id)
        {
            List<Log> logs = new List<Log>();


            //List<Usuario>.Enumerator<Usuario> usuarios = new List<Usuario>();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from log where log_conta_id = @conta_id ORDER by log.log_id DESC;", conn);
                comando.Parameters.AddWithValue("@conta_id", conta_id);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        logs.Add(new Log
                        {
                            log_id = Convert.ToInt32(leitor["log_id"]),
                            log_conta_id = Convert.ToInt32(leitor["log_conta_id"]),
                            log_usuario_id = Convert.ToInt32(leitor["log_usuario_id"]),
                            log_classe = leitor["log_classe"].ToString(),
                            log_metodo = leitor["log_metodo"].ToString(),
                            log_tipo = leitor["log_tipo"].ToString(),
                            log_mensagem = leitor["log_mensagem"].ToString(),
                            log_data = leitor["log_data"].ToString()
                        });
                    }
                }                

                conn.Close();
            }
            catch (Exception e)
            {
                _ = e.StackTrace;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return logs;
        }

        public void log_txt(string texto)
        {
            using (StreamWriter writer = new StreamWriter("log.txt"))
            {
                //writer.Write("Macoratti .net ");
                writer.WriteLine("==> " + texto);
            }
        }
    }
}
