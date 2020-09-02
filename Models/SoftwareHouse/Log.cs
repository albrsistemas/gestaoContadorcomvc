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
            catch (Exception e)
            {
                myTrans.Rollback();
            }
            finally
            {
                conn.Close();
            }

        } 
    }
}
