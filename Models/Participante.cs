using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace gestaoContadorcomvc.Models
{
    public class Participante
    {
        public int participante_id { get; set; }
        public string participante_nome { get; set; }
        public DateTime participante_dataCriacao { get; set; }
        public string participante_fantasia { get; set; }
        public string participante_codigo { get; set; }
        public string participante_tipoPessoa { get; set; }
        public DateTime participante_clienteDesde { get; set; }
        public int participante_contribuinte { get; set; }
        public string participante_inscricaoEstadual { get; set; }
        public string participante_cnpj_cpf { get; set; }
        public string participante_rg { get; set; }
        public string participante_orgaoEmissor { get; set; }
        public string participante_cep { get; set; }
        public int participante_uf { get; set; }
        public string participante_cidade { get; set; }
        public string participante_bairro { get; set; }
        public string participante_logradouro { get; set; }
        public string participante_numero { get; set; }
        public string participante_complemento { get; set; }
        public int participante_pais { get; set; }
        public int participante_categoria { get; set; }
        public string participante_obs { get; set; }
        public int participante_conta_id { get; set; }
        public string participante_status { get; set; }
        public string participante_insc_municipal { get; set; }
        public int participante_regime_tributario { get; set; }
        public string participante_suframa { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Participante()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Listar participante
        public List<Vm_participante> listaParticipantes(int usuario_id, int conta_id)
        {
            List<Vm_participante> participantes = new List<Vm_participante>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {                
                comando.CommandText = "SELECT * from participante where participante_conta_id = @conta_id and participante_status = 'Ativo';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_participante participante = new Vm_participante();

                        if (DBNull.Value != leitor["participante_id"])
                        {
                            participante.participante_id = Convert.ToInt32(leitor["participante_id"]);
                        }
                        else
                        {
                            participante.participante_id = 0;
                        }

                        if (DBNull.Value != leitor["participante_contribuinte"])
                        {
                            participante.participante_contribuinte = Convert.ToInt32(leitor["participante_contribuinte"]);
                        }
                        else
                        {
                            participante.participante_contribuinte = 0;
                        }

                        if (DBNull.Value != leitor["participante_uf"])
                        {
                            participante.participante_uf = Convert.ToInt32(leitor["participante_uf"]);
                        }
                        else
                        {
                            participante.participante_uf = 0;
                        }

                        if (DBNull.Value != leitor["participante_categoria"])
                        {
                            participante.participante_categoria = Convert.ToInt32(leitor["participante_categoria"]);
                        }
                        else
                        {
                            participante.participante_categoria = 0;
                        }

                        if (DBNull.Value != leitor["participante_conta_id"])
                        {
                            participante.participante_conta_id = Convert.ToInt32(leitor["participante_conta_id"]);
                        }
                        else
                        {
                            participante.participante_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["participante_pais"])
                        {
                            participante.participante_pais = Convert.ToInt32(leitor["participante_pais"]);
                        }
                        else
                        {
                            participante.participante_pais = 0;
                        }

                        if (DBNull.Value != leitor["participante_regime_tributario"])
                        {
                            participante.participante_regime_tributario = Convert.ToInt32(leitor["participante_regime_tributario"]);
                        }
                        else
                        {
                            participante.participante_regime_tributario = 0;
                        }

                        if (DBNull.Value != leitor["participante_dataCriacao"])
                        {
                            participante.participante_dataCriacao = Convert.ToDateTime(leitor["participante_dataCriacao"]);
                        }
                        else
                        {
                            participante.participante_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["participante_clienteDesde"])
                        {
                            participante.participante_clienteDesde = Convert.ToDateTime(leitor["participante_clienteDesde"]);
                        }
                        else
                        {
                            participante.participante_clienteDesde = new DateTime();
                        }

                        participante.participante_nome = leitor["participante_nome"].ToString();
                        participante.participante_fantasia = leitor["participante_fantasia"].ToString();
                        participante.participante_codigo = leitor["participante_codigo"].ToString();
                        participante.participante_tipoPessoa = leitor["participante_tipoPessoa"].ToString();
                        participante.participante_inscricaoEstadual = leitor["participante_inscricaoEstadual"].ToString();
                        participante.participante_cnpj_cpf = leitor["participante_cnpj_cpf"].ToString();
                        participante.participante_rg = leitor["participante_rg"].ToString();
                        participante.participante_orgaoEmissor = leitor["participante_orgaoEmissor"].ToString();
                        participante.participante_cep = leitor["participante_cep"].ToString();
                        participante.participante_cidade = leitor["participante_cidade"].ToString();
                        participante.participante_bairro = leitor["participante_bairro"].ToString();
                        participante.participante_logradouro = leitor["participante_logradouro"].ToString();
                        participante.participante_numero = leitor["participante_numero"].ToString();
                        participante.participante_complemento = leitor["participante_complemento"].ToString();
                        participante.participante_obs = leitor["participante_obs"].ToString();
                        participante.participante_status = leitor["participante_status"].ToString();
                        participante.participante_insc_municipal = leitor["participante_insc_municipal"].ToString();
                        participante.participante_suframa = leitor["participante_suframa"].ToString();
                        

                        participantes.Add(participante);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Participante", "listaParticipantes", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return participantes;
        }

        //Cadastrar participante
        public string cadastrarParticipante(int usuario_id, int conta_id,
            DateTime participante_clienteDesde, int participante_contribuinte, int participante_uf, 
            int participante_categoria, int participante_conta_id, int participante_pais, string participante_cep, string participante_nome, 
            string participante_logradouro, string participante_rg, string participante_orgaoEmissor, string participante_numero, string participante_codigo, 
            string participante_tipoPessoa, string participante_inscricaoEstadual, string participante_cnpj_cpf, string participante_complemento, 
            string participante_obs, string participante_bairro, string participante_cidade, string participante_fantasia, string participante_insc_municipal, int participante_regime_tributario, string participante_suframa)
        {
            string retorno = "Participante cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into participante " +
                    "(participante_nome, participante_fantasia, participante_codigo, participante_tipoPessoa, participante_clienteDesde, participante_contribuinte, participante_inscricaoEstadual, participante_cnpj_cpf, participante_rg, participante_orgaoEmissor, participante_cep, participante_uf, participante_cidade, participante_bairro, participante_logradouro, participante_numero, participante_complemento, participante_categoria, participante_obs, participante_conta_id, participante_pais, participante_insc_municipal, participante_regime_tributario, participante_suframa) " +
                    "values (" +
                    "@participante_nome, @participante_fantasia, @participante_codigo, @participante_tipoPessoa, @participante_clienteDesde, @participante_contribuinte, @participante_inscricaoEstadual, @participante_cnpj_cpf, @participante_rg, @participante_orgaoEmissor, @participante_cep, @participante_uf, @participante_cidade, @participante_bairro, @participante_logradouro, @participante_numero, @participante_complemento, @participante_categoria, @participante_obs, @participante_conta_id, @participante_pais, @participante_insc_municipal, @participante_regime_tributario, @participante_suframa);";
                comando.Parameters.AddWithValue("@participante_nome", participante_nome);
                comando.Parameters.AddWithValue("@participante_fantasia", participante_fantasia);
                comando.Parameters.AddWithValue("@participante_codigo", participante_codigo);
                comando.Parameters.AddWithValue("@participante_tipoPessoa", participante_tipoPessoa);
                comando.Parameters.AddWithValue("@participante_clienteDesde", participante_clienteDesde);
                comando.Parameters.AddWithValue("@participante_contribuinte", participante_contribuinte);
                comando.Parameters.AddWithValue("@participante_inscricaoEstadual", participante_inscricaoEstadual);
                comando.Parameters.AddWithValue("@participante_cnpj_cpf", participante_cnpj_cpf);
                comando.Parameters.AddWithValue("@participante_rg", participante_rg);
                comando.Parameters.AddWithValue("@participante_orgaoEmissor", participante_orgaoEmissor);
                comando.Parameters.AddWithValue("@participante_cep", participante_cep);
                comando.Parameters.AddWithValue("@participante_uf", participante_uf);
                comando.Parameters.AddWithValue("@participante_cidade", participante_cidade);
                comando.Parameters.AddWithValue("@participante_bairro", participante_bairro);
                comando.Parameters.AddWithValue("@participante_logradouro", participante_logradouro);
                comando.Parameters.AddWithValue("@participante_numero", participante_numero);
                comando.Parameters.AddWithValue("@participante_complemento", participante_complemento);
                comando.Parameters.AddWithValue("@participante_categoria", participante_categoria);
                comando.Parameters.AddWithValue("@participante_obs", participante_obs);
                comando.Parameters.AddWithValue("@participante_conta_id", participante_conta_id);                
                comando.Parameters.AddWithValue("@participante_pais", participante_pais);
                comando.Parameters.AddWithValue("@participante_insc_municipal", participante_insc_municipal);
                comando.Parameters.AddWithValue("@participante_regime_tributario", participante_regime_tributario);
                comando.Parameters.AddWithValue("@participante_suframa", participante_suframa);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro de novo participante nome: " + participante_nome + " Cadastrado com sucesso";
                log.log("Participante", "cadastrarParticipante", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar o participante. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Participante", "cadastrarParticipantes", "Erro", msg, conta_id, usuario_id);
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

        //Buscar participante por id
        public Vm_participante buscarParticipantes(int usuario_id, int conta_id, int participante_id)
        {
            Vm_participante participante = new Vm_participante();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from participante where participante_conta_id = @conta_id and participante_id = @participante_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@participante_id", participante_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["participante_id"])
                        {
                            participante.participante_id = Convert.ToInt32(leitor["participante_id"]);
                        }
                        else
                        {
                            participante.participante_id = 0;
                        }

                        if (DBNull.Value != leitor["participante_contribuinte"])
                        {
                            participante.participante_contribuinte = Convert.ToInt32(leitor["participante_contribuinte"]);
                        }
                        else
                        {
                            participante.participante_contribuinte = 0;
                        }

                        if (DBNull.Value != leitor["participante_uf"])
                        {
                            participante.participante_uf = Convert.ToInt32(leitor["participante_uf"]);
                        }
                        else
                        {
                            participante.participante_uf = 0;
                        }

                        if (DBNull.Value != leitor["participante_categoria"])
                        {
                            participante.participante_categoria = Convert.ToInt32(leitor["participante_categoria"]);
                        }
                        else
                        {
                            participante.participante_categoria = 0;
                        }

                        if (DBNull.Value != leitor["participante_conta_id"])
                        {
                            participante.participante_conta_id = Convert.ToInt32(leitor["participante_conta_id"]);
                        }
                        else
                        {
                            participante.participante_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["participante_pais"])
                        {
                            participante.participante_pais = Convert.ToInt32(leitor["participante_pais"]);
                        }
                        else
                        {
                            participante.participante_pais = 0;
                        }

                        if (DBNull.Value != leitor["participante_regime_tributario"])
                        {
                            participante.participante_regime_tributario = Convert.ToInt32(leitor["participante_regime_tributario"]);
                        }
                        else
                        {
                            participante.participante_regime_tributario = 0;
                        }

                        if (DBNull.Value != leitor["participante_dataCriacao"])
                        {
                            participante.participante_dataCriacao = Convert.ToDateTime(leitor["participante_dataCriacao"]);
                        }
                        else
                        {
                            participante.participante_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["participante_clienteDesde"])
                        {
                            participante.participante_clienteDesde = Convert.ToDateTime(leitor["participante_clienteDesde"]);
                        }
                        else
                        {
                            participante.participante_clienteDesde = new DateTime();
                        }

                        participante.participante_nome = leitor["participante_nome"].ToString();
                        participante.participante_fantasia = leitor["participante_fantasia"].ToString();
                        participante.participante_codigo = leitor["participante_codigo"].ToString();
                        participante.participante_tipoPessoa = leitor["participante_tipoPessoa"].ToString();
                        participante.participante_inscricaoEstadual = leitor["participante_inscricaoEstadual"].ToString();
                        participante.participante_cnpj_cpf = leitor["participante_cnpj_cpf"].ToString();
                        participante.participante_rg = leitor["participante_rg"].ToString();
                        participante.participante_orgaoEmissor = leitor["participante_orgaoEmissor"].ToString();
                        participante.participante_cep = leitor["participante_cep"].ToString();
                        participante.participante_cidade = leitor["participante_cidade"].ToString();
                        participante.participante_bairro = leitor["participante_bairro"].ToString();
                        participante.participante_logradouro = leitor["participante_logradouro"].ToString();
                        participante.participante_numero = leitor["participante_numero"].ToString();
                        participante.participante_complemento = leitor["participante_complemento"].ToString();
                        participante.participante_obs = leitor["participante_obs"].ToString();
                        participante.participante_status = leitor["participante_status"].ToString();
                        participante.participante_insc_municipal = leitor["participante_insc_municipal"].ToString();
                        participante.participante_suframa = leitor["participante_suframa"].ToString();

                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Participante", "buscarParticipantes", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return participante;
        }

        //Alterar participante
        public string alterarParticipante(int usuario_id, int conta_id, int participante_id,
            DateTime participante_clienteDesde, int participante_contribuinte, int participante_uf,
            int participante_categoria, int participante_pais, string participante_cep, string participante_nome,
            string participante_logradouro, string participante_rg, string participante_orgaoEmissor, string participante_numero, string participante_codigo,
            string participante_tipoPessoa, string participante_inscricaoEstadual, string participante_cnpj_cpf, string participante_complemento,
            string participante_obs, string participante_bairro, string participante_cidade, string participante_fantasia, string participante_status, string participante_insc_municipal, int participante_regime_tributario, string participante_suframa)
        {
            string retorno = "Participante alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update participante set participante_nome = @participante_nome, participante_fantasia = @participante_fantasia, participante_codigo = @participante_codigo, participante_tipoPessoa = @participante_tipoPessoa, participante_clienteDesde = @participante_clienteDesde, participante_contribuinte = @participante_contribuinte, participante_inscricaoEstadual = @participante_inscricaoEstadual, participante_cnpj_cpf = @participante_cnpj_cpf, participante_rg = @participante_rg, participante_orgaoEmissor = @participante_orgaoEmissor, participante_cep = @participante_cep, participante_uf = @participante_uf, participante_cidade = @participante_cidade, participante_bairro = @participante_bairro, participante_logradouro = @participante_logradouro, participante_numero = @participante_numero, participante_complemento = @participante_complemento, participante_categoria = @participante_categoria, participante_obs = @participante_obs, participante_conta_id = @conta_id, participante_status = @participante_status, participante_pais = @participante_pais, participante_insc_municipal = @participante_insc_municipal, participante_regime_tributario = @participante_regime_tributario, participante_suframa = @participante_suframa where participante_conta_id = @conta_id and participante_id = @participante_id;";
                comando.Parameters.AddWithValue("@participante_nome", participante_nome);
                comando.Parameters.AddWithValue("@participante_fantasia", participante_fantasia);
                comando.Parameters.AddWithValue("@participante_codigo", participante_codigo);
                comando.Parameters.AddWithValue("@participante_tipoPessoa", participante_tipoPessoa);
                comando.Parameters.AddWithValue("@participante_clienteDesde", participante_clienteDesde);
                comando.Parameters.AddWithValue("@participante_contribuinte", participante_contribuinte);
                comando.Parameters.AddWithValue("@participante_inscricaoEstadual", participante_inscricaoEstadual);
                comando.Parameters.AddWithValue("@participante_cnpj_cpf", participante_cnpj_cpf);
                comando.Parameters.AddWithValue("@participante_rg", participante_rg);
                comando.Parameters.AddWithValue("@participante_orgaoEmissor", participante_orgaoEmissor);
                comando.Parameters.AddWithValue("@participante_cep", participante_cep);
                comando.Parameters.AddWithValue("@participante_uf", participante_uf);
                comando.Parameters.AddWithValue("@participante_cidade", participante_cidade);
                comando.Parameters.AddWithValue("@participante_bairro", participante_bairro);
                comando.Parameters.AddWithValue("@participante_logradouro", participante_logradouro);
                comando.Parameters.AddWithValue("@participante_numero", participante_numero);
                comando.Parameters.AddWithValue("@participante_complemento", participante_complemento);
                comando.Parameters.AddWithValue("@participante_categoria", participante_categoria);
                comando.Parameters.AddWithValue("@participante_obs", participante_obs);
                comando.Parameters.AddWithValue("@participante_conta_id", conta_id);
                comando.Parameters.AddWithValue("@participante_pais", participante_pais);
                comando.Parameters.AddWithValue("@participante_status", participante_status);
                comando.Parameters.AddWithValue("@participante_id", participante_id);
                comando.Parameters.AddWithValue("@participante_insc_municipal", participante_insc_municipal);
                comando.Parameters.AddWithValue("@participante_regime_tributario", participante_regime_tributario);
                comando.Parameters.AddWithValue("@participante_suframa", participante_suframa);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração do participante nome: " + participante_nome + " Alterado com sucesso";
                log.log("Participante", "alterarParticipante", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar o participante. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Participante", "alterarParticipantes", "Erro", msg, conta_id, usuario_id);
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

        //Exluir participante
        public string deletarParticipante(int usuario_id, int conta_id, int partecipante_id)
        {
            string retorno = "Participante excluído com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update participante set participante_status = 'Deletado' where participante_conta_id = @conta_id and participante_id = @participante_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@participante_id", partecipante_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão do participante ID: " + partecipante_id + " Excluído com sucesso";
                log.log("Participante", "deletarParticipante", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir o participante. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Participante", "deletarParticipante", "Erro", msg, conta_id, usuario_id);
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

        //Listar por termo (autocomple de participante)
        public List<Vm_participante> listaParticipantesPorTermo(int usuario_id, int conta_id, string termo)
        {
            List<Vm_participante> participantes = new List<Vm_participante>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from participante where participante_conta_id = @conta_id and participante_status = 'Ativo' and (participante.participante_nome LIKE concat(@termo,'%') || participante.participante_cnpj_cpf LIKE concat(@termo,'%'));";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@termo", termo);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_participante participante = new Vm_participante();

                        if (DBNull.Value != leitor["participante_id"])
                        {
                            participante.participante_id = Convert.ToInt32(leitor["participante_id"]);
                        }
                        else
                        {
                            participante.participante_id = 0;
                        }

                        if (DBNull.Value != leitor["participante_contribuinte"])
                        {
                            participante.participante_contribuinte = Convert.ToInt32(leitor["participante_contribuinte"]);
                        }
                        else
                        {
                            participante.participante_contribuinte = 0;
                        }

                        if (DBNull.Value != leitor["participante_uf"])
                        {
                            participante.participante_uf = Convert.ToInt32(leitor["participante_uf"]);
                        }
                        else
                        {
                            participante.participante_uf = 0;
                        }

                        if (DBNull.Value != leitor["participante_categoria"])
                        {
                            participante.participante_categoria = Convert.ToInt32(leitor["participante_categoria"]);
                        }
                        else
                        {
                            participante.participante_categoria = 0;
                        }

                        if (DBNull.Value != leitor["participante_conta_id"])
                        {
                            participante.participante_conta_id = Convert.ToInt32(leitor["participante_conta_id"]);
                        }
                        else
                        {
                            participante.participante_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["participante_pais"])
                        {
                            participante.participante_pais = Convert.ToInt32(leitor["participante_pais"]);
                        }
                        else
                        {
                            participante.participante_pais = 0;
                        }

                        if (DBNull.Value != leitor["participante_regime_tributario"])
                        {
                            participante.participante_regime_tributario = Convert.ToInt32(leitor["participante_regime_tributario"]);
                        }
                        else
                        {
                            participante.participante_regime_tributario = 0;
                        }

                        if (DBNull.Value != leitor["participante_dataCriacao"])
                        {
                            participante.participante_dataCriacao = Convert.ToDateTime(leitor["participante_dataCriacao"]);
                        }
                        else
                        {
                            participante.participante_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["participante_clienteDesde"])
                        {
                            participante.participante_clienteDesde = Convert.ToDateTime(leitor["participante_clienteDesde"]);
                        }
                        else
                        {
                            participante.participante_clienteDesde = new DateTime();
                        }

                        participante.participante_nome = leitor["participante_nome"].ToString();
                        participante.participante_fantasia = leitor["participante_fantasia"].ToString();
                        participante.participante_codigo = leitor["participante_codigo"].ToString();
                        participante.participante_tipoPessoa = leitor["participante_tipoPessoa"].ToString();
                        participante.participante_inscricaoEstadual = leitor["participante_inscricaoEstadual"].ToString();
                        participante.participante_cnpj_cpf = leitor["participante_cnpj_cpf"].ToString();
                        participante.participante_rg = leitor["participante_rg"].ToString();
                        participante.participante_orgaoEmissor = leitor["participante_orgaoEmissor"].ToString();
                        participante.participante_cep = leitor["participante_cep"].ToString();
                        participante.participante_cidade = leitor["participante_cidade"].ToString();
                        participante.participante_bairro = leitor["participante_bairro"].ToString();
                        participante.participante_logradouro = leitor["participante_logradouro"].ToString();
                        participante.participante_numero = leitor["participante_numero"].ToString();
                        participante.participante_complemento = leitor["participante_complemento"].ToString();
                        participante.participante_obs = leitor["participante_obs"].ToString();
                        participante.participante_status = leitor["participante_status"].ToString();
                        participante.participante_insc_municipal = leitor["participante_insc_municipal"].ToString();
                        participante.participante_suframa = leitor["participante_suframa"].ToString();


                        participantes.Add(participante);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Participante", "listaParticipantes", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return participantes;
        }

        public bool participanteExiste(string valor, int conta_id, int participante_id)
        {
            bool localizado = false;
            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT p.participante_cnpj_cpf from participante as p WHERE p.participante_conta_id = @conta_id and p.participante_cnpj_cpf = @valor and p.participante_id <> @participante_id;", conn);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@participante_id", participante_id);
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
