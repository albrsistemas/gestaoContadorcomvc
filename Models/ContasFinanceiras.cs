using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.IO;

namespace gestaoContadorcomvc.Models
{
    public class ContasFinanceiras
    {
        public int cf_id { get; set; }
        public string cf_nome { get; set; }
        public int cf_categoria_id { get; set; }
        public Decimal cf_valor_operacao { get; set; }
        public Decimal cf_valor_parcela_bruta { get; set; }
        public Decimal cf_valor_parcela_liquida { get; set; }
        public string cf_recorrencia { get; set; }
        public DateTime cf_data_inicial { get; set; }
        public DateTime? cf_data_final { get; set; }
        public string cf_escopo { get; set; }
        public string cf_tipo { get; set; }
        public string cf_status { get; set; }
        public int cf_conta_id { get; set; }
        public int cf_numero_parcelas { get; set; }
        public DateTime cf_dataCriacao { get; set; }
        public int cf_op_id { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ContasFinanceiras()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public string cadastrarContaFinanceira(int usuario_id, int conta_id, Vm_contasFinanceiras vmcf)
        {
            string retorno = "Conta Financeira cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "CALL pr_contasFinanceirasCreate(@op_conta_id, @op_data, @op_obs, @op_categoria_id, @op_comParticipante, @op_comNF, @cf_nome, @cf_valor_operacao, @cf_valor_parcela_bruta, @cf_valor_parcela_liquida, @cf_recorrencia, @cf_data_inicial, @cf_data_final, @cf_tipo, @cf_numero_parcelas, @op_parcela_fp_id, @participante_id, @op_nf_chave, @op_nf_data_emissao, @op_nf_serie, @op_nf_numero, @op_nf_tipo);";
                comando.Parameters.AddWithValue("@op_conta_id", conta_id);
                if(vmcf.cf.cf_tipo == "Parcelada")
                {
                    comando.Parameters.AddWithValue("@op_data", vmcf.nf.op_nf_data_emissao);
                }
                if (vmcf.cf.cf_tipo == "Recorrente")
                {
                    comando.Parameters.AddWithValue("@op_data", vmcf.cf.cf_data_inicial);
                }                                
                comando.Parameters.AddWithValue("@op_obs", vmcf.op.op_obs);
                comando.Parameters.AddWithValue("@op_categoria_id", vmcf.op.op_categoria_id);
                comando.Parameters.AddWithValue("@op_comParticipante", vmcf.op.op_comParticipante);
                comando.Parameters.AddWithValue("@op_comNF", vmcf.op.op_comNF);
                comando.Parameters.AddWithValue("@cf_nome", vmcf.cf.cf_nome);
                comando.Parameters.AddWithValue("@cf_valor_operacao", vmcf.cf.cf_valor_operacao);
                comando.Parameters.AddWithValue("@cf_valor_parcela_bruta", vmcf.cf.cf_valor_parcela_bruta);
                comando.Parameters.AddWithValue("@cf_valor_parcela_liquida", vmcf.cf.cf_valor_parcela_liquida);
                comando.Parameters.AddWithValue("@cf_recorrencia", vmcf.cf.cf_recorrencia);
                comando.Parameters.AddWithValue("@cf_data_inicial", vmcf.cf.cf_data_inicial);
                if(vmcf.cf.cf_data_final == null)
                {
                    comando.Parameters.AddWithValue("@cf_data_final", null);
                }
                else
                {
                    comando.Parameters.AddWithValue("@cf_data_final", vmcf.cf.cf_data_final);
                }                                
                comando.Parameters.AddWithValue("@cf_tipo", vmcf.cf.cf_tipo);
                if(vmcf.cf.cf_tipo == "Parcelada")
                {
                    comando.Parameters.AddWithValue("@cf_numero_parcelas", vmcf.cf.cf_numero_parcelas);
                    comando.Parameters.AddWithValue("@op_parcela_fp_id", vmcf.parcelas.op_parcela_fp_id);
                    comando.Parameters.AddWithValue("@op_nf_chave", vmcf.nf.op_nf_chave);
                    comando.Parameters.AddWithValue("@op_nf_data_emissao", vmcf.nf.op_nf_data_emissao);
                    comando.Parameters.AddWithValue("@op_nf_serie", vmcf.nf.op_nf_serie);
                    comando.Parameters.AddWithValue("@op_nf_numero", vmcf.nf.op_nf_numero);
                    comando.Parameters.AddWithValue("@op_nf_tipo", vmcf.nf.op_nf_tipo);
                }
                else
                {
                    comando.Parameters.AddWithValue("@cf_numero_parcelas", 0);
                    comando.Parameters.AddWithValue("@op_parcela_fp_id", 0);
                    comando.Parameters.AddWithValue("@op_nf_chave", "");
                    comando.Parameters.AddWithValue("@op_nf_data_emissao", null);
                    comando.Parameters.AddWithValue("@op_nf_serie", "");
                    comando.Parameters.AddWithValue("@op_nf_numero", "");
                    comando.Parameters.AddWithValue("@op_nf_tipo", 0);
                }
                if(vmcf.op.op_comParticipante)
                {
                    comando.Parameters.AddWithValue("@participante_id", vmcf.participante.op_part_participante_id);
                }
                else
                {
                    comando.Parameters.AddWithValue("@participante_id", 0);
                }

                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro da conta financeira nome: " + vmcf.cf.cf_nome + " cadastrada com sucesso";
                log.log("ContasFinanceiras", "cadastrarContaFinanceira", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a conta financeira. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("ContasFinanceiras", "cadastrarContaFinanceira", "Erro", msg, conta_id, usuario_id);
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
