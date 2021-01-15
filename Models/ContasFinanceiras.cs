using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace gestaoContadorcomvc.Models
{
    public class ContasFinanceiras
    {
        public int cf_id { get; set; }
        public string cf_nome { get; set; }
        public int cf_categoria_id { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public Decimal cf_valor_operacao { get; set; }        
        public Decimal cf_valor_parcela_bruta { get; set; }
        public Decimal cf_valor_parcela_liquida { get; set; }
        public string cf_recorrencia { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime cf_data_inicial { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime cf_data_final { get; set; }
        public string cf_escopo { get; set; }
        public string cf_tipo { get; set; }
        public string cf_status { get; set; }
        public int cf_conta_id { get; set; }
        public int cf_numero_parcelas { get; set; }
        public DateTime cf_dataCriacao { get; set; }
        public int cf_op_id { get; set; }
        public int cf_forma_pgto { get; set; }
        public Decimal baixas { get; set; }

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
                comando.CommandText = "CALL pr_contasFinanceirasCreate(@op_conta_id, @op_data, @op_obs, @op_categoria_id, @op_comParticipante, @op_comNF, @cf_nome, @cf_valor_operacao, @cf_valor_parcela_bruta, @cf_valor_parcela_liquida, @cf_recorrencia, @cf_data_inicial, @cf_data_final, @cf_tipo, @cf_numero_parcelas, @op_parcela_fp_id, @participante_id, @op_nf_chave, @op_nf_data_emissao, @op_nf_serie, @op_nf_numero, @op_nf_tipo, @context, @parcela_id);";
                comando.Parameters.AddWithValue("@context", "Create");
                comando.Parameters.AddWithValue("@parcela_id", 0);
                comando.Parameters.AddWithValue("@op_conta_id", conta_id);
                if(vmcf.cf.cf_tipo == "Realizada")
                {
                    comando.Parameters.AddWithValue("@op_data", vmcf.nf.op_nf_data_emissao);
                }
                if (vmcf.cf.cf_tipo == "Realizar")
                {
                    comando.Parameters.AddWithValue("@op_data", vmcf.op.op_data);
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
                if(vmcf.cf.cf_data_final == null || vmcf.cf.cf_data_final.ToString().Equals("01/01/0001 00:00:00"))
                {
                    comando.Parameters.AddWithValue("@cf_data_final", null);
                }
                else
                {
                    comando.Parameters.AddWithValue("@cf_data_final", vmcf.cf.cf_data_final);
                }                                
                comando.Parameters.AddWithValue("@cf_tipo", vmcf.cf.cf_tipo);
                if(vmcf.cf.cf_tipo == "Realizada")
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

        //Buscar conta financeira para gerar CFR_realização
        public Vm_contasFinanceiras gerarCFR(int parcela_id, int conta_id)
        {
            Vm_contasFinanceiras vmcf = new Vm_contasFinanceiras();
            Operacao op = new Operacao();
            ContasFinanceiras cf = new ContasFinanceiras();
            Op_parcelas parcelas = new Op_parcelas();
            Op_participante participante = new Op_participante();
            Op_nf nf = new Op_nf();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT opp.*, op.*,cf.*,part.* from op_parcelas as opp left JOIN operacao as op on op.op_id = opp.op_parcela_op_id left JOIN contas_financeiras as cf on cf.cf_op_id = op.op_id LEFT JOIN op_participante as part on part.op_id = op.op_id WHERE opp.op_parcela_id = @parcela_id and op.op_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@parcela_id", parcela_id);
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        //contas financeiras
                        cf.cf_nome = leitor["cf_nome"].ToString();

                        if (DBNull.Value != leitor["cf_categoria_id"])
                        {
                            cf.cf_categoria_id = Convert.ToInt32(leitor["cf_categoria_id"]);
                        }
                        else
                        {
                            cf.cf_categoria_id = 0;
                        }

                        if (DBNull.Value != leitor["cf_forma_pgto"])
                        {
                            cf.cf_forma_pgto = Convert.ToInt32(leitor["cf_forma_pgto"]);
                        }
                        else
                        {
                            cf.cf_forma_pgto = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_operacao"])
                        {
                            cf.cf_valor_operacao = Convert.ToDecimal(leitor["cf_valor_operacao"]);
                        }
                        else
                        {
                            cf.cf_valor_operacao = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_parcela_bruta"])
                        {
                            cf.cf_valor_parcela_bruta = Convert.ToDecimal(leitor["cf_valor_parcela_bruta"]);
                        }
                        else
                        {
                            cf.cf_valor_parcela_bruta = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_parcela_liquida"])
                        {
                            cf.cf_valor_parcela_liquida = Convert.ToDecimal(leitor["cf_valor_parcela_liquida"]);
                        }
                        else
                        {
                            cf.cf_valor_parcela_liquida = 0;
                        }

                        DateTime hj = DateTime.Today;
                        //A data inicial e final da parcela é a data de hoje, pois está sendo realizada. Caso seja posterior usuário deve alterar
                        cf.cf_data_inicial = hj;
                        cf.cf_data_final = hj;
                        //A data final é igual a data inicial, pois na realização a recorrência é 'Única'

                        //Operação
                        DateTime thisDay = DateTime.Today;

                        op.op_obs = leitor["op_obs"].ToString();
                        op.op_data = thisDay;                        

                        //participante
                        if (DBNull.Value != leitor["op_part_participante_id"])
                        {
                            participante.op_part_participante_id = Convert.ToInt32(leitor["op_part_participante_id"]);
                        }
                        else
                        {
                            participante.op_part_participante_id = 0;
                        }

                        participante.op_part_nome = leitor["op_part_nome"].ToString();
                        participante.op_part_tipo = leitor["op_part_tipo"].ToString();
                        participante.op_part_cnpj_cpf = leitor["op_part_cnpj_cpf"].ToString();
                        participante.op_part_cep = leitor["op_part_cep"].ToString();
                        participante.op_part_logradouro = leitor["op_part_logradouro"].ToString();
                        participante.op_part_numero = leitor["op_part_numero"].ToString();
                        participante.op_part_complemento = leitor["op_part_complemento"].ToString();
                        participante.op_part_bairro = leitor["op_part_bairro"].ToString();
                        participante.op_part_cidade = leitor["op_part_cidade"].ToString();

                        if (DBNull.Value != leitor["op_uf_ibge_codigo"])
                        {
                            participante.op_uf_ibge_codigo = Convert.ToInt32(leitor["op_uf_ibge_codigo"]);
                        }
                        else
                        {
                            participante.op_uf_ibge_codigo = 0;
                        }

                        if (DBNull.Value != leitor["op_paisesIBGE_codigo"])
                        {
                            participante.op_paisesIBGE_codigo = Convert.ToInt32(leitor["op_paisesIBGE_codigo"]);
                        }
                        else
                        {
                            participante.op_paisesIBGE_codigo = 0;
                        }
                    }
                }
                leitor.Close();

                vmcf.cf = cf;
                vmcf.op = op;
                vmcf.participante = participante;
                vmcf.parcelas = parcelas;
                vmcf.nf = nf;
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);                
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vmcf;
        }


        public string cadastrarContaFinanceiraCFR(int usuario_id, int conta_id, Vm_contasFinanceiras vmcf, int parcela_id)
        {
            string retorno = "Conta Financeira realizada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "CALL pr_contasFinanceirasCreate(@op_conta_id, @op_data, @op_obs, @op_categoria_id, @op_comParticipante, @op_comNF, @cf_nome, @cf_valor_operacao, @cf_valor_parcela_bruta, @cf_valor_parcela_liquida, @cf_recorrencia, @cf_data_inicial, @cf_data_final, @cf_tipo, @cf_numero_parcelas, @op_parcela_fp_id, @participante_id, @op_nf_chave, @op_nf_data_emissao, @op_nf_serie, @op_nf_numero, @op_nf_tipo, @context, @parcela_id);";
                comando.Parameters.AddWithValue("@context", "realizacao");
                comando.Parameters.AddWithValue("@parcela_id", parcela_id);
                comando.Parameters.AddWithValue("@op_conta_id", conta_id);
                if (vmcf.cf.cf_tipo == "Realizada")
                {
                    comando.Parameters.AddWithValue("@op_data", vmcf.nf.op_nf_data_emissao);
                }
                if (vmcf.cf.cf_tipo == "Realizar")
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
                if (vmcf.cf.cf_data_final == null)
                {
                    comando.Parameters.AddWithValue("@cf_data_final", null);
                }
                else
                {
                    comando.Parameters.AddWithValue("@cf_data_final", vmcf.cf.cf_data_final);
                }
                comando.Parameters.AddWithValue("@cf_tipo", vmcf.cf.cf_tipo);
                if (vmcf.cf.cf_tipo == "Realizada")
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
                if (vmcf.op.op_comParticipante)
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

        //Lista contas financeiras
        public Vm_contasFinanceiras listaCF(int usuario_id, int conta_id)
        {
            Vm_contasFinanceiras vmcf = new Vm_contasFinanceiras();

            List<ContasFinanceiras> lista = new List<ContasFinanceiras>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT (SELECT COALESCE(sum(op_parcelas_baixa.oppb_valor),0) from op_parcelas_baixa WHERE op_parcelas_baixa.oppb_op_id = cf.cf_op_id) as baixas, cf.* from contas_financeiras as cf WHERE cf.cf_conta_id = @conta_id ORDER by cf.cf_dataCriacao DESC";
                comando.Parameters.AddWithValue("@conta_id", conta_id);                
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        ContasFinanceiras cf = new ContasFinanceiras();

                        //Baixas soma
                        if (DBNull.Value != leitor["baixas"])
                        {
                            cf.baixas = Convert.ToDecimal(leitor["baixas"]);
                        }
                        else
                        {
                            cf.baixas = 0;
                        }

                        //contas financeiras
                        if (DBNull.Value != leitor["cf_id"])
                        {
                            cf.cf_id = Convert.ToInt32(leitor["cf_id"]);
                        }
                        else
                        {
                            cf.cf_id = 0;
                        }

                        if (DBNull.Value != leitor["cf_op_id"])
                        {
                            cf.cf_op_id = Convert.ToInt32(leitor["cf_op_id"]);
                        }
                        else
                        {
                            cf.cf_op_id = 0;
                        }

                        if (DBNull.Value != leitor["cf_categoria_id"])
                        {
                            cf.cf_categoria_id = Convert.ToInt32(leitor["cf_categoria_id"]);
                        }
                        else
                        {
                            cf.cf_categoria_id = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_operacao"])
                        {
                            cf.cf_valor_operacao = Convert.ToDecimal(leitor["cf_valor_operacao"]);
                        }
                        else
                        {
                            cf.cf_valor_operacao = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_parcela_bruta"])
                        {
                            cf.cf_valor_parcela_bruta = Convert.ToDecimal(leitor["cf_valor_parcela_bruta"]);
                        }
                        else
                        {
                            cf.cf_valor_parcela_bruta = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_parcela_liquida"])
                        {
                            cf.cf_valor_parcela_liquida = Convert.ToDecimal(leitor["cf_valor_parcela_liquida"]);
                        }
                        else
                        {
                            cf.cf_valor_parcela_liquida = 0;
                        }

                        if (DBNull.Value != leitor["cf_data_inicial"])
                        {
                            cf.cf_data_inicial = Convert.ToDateTime(leitor["cf_data_inicial"]);
                        }
                        else
                        {
                            cf.cf_data_inicial = new DateTime();
                        }

                        if (DBNull.Value != leitor["cf_data_final"])
                        {
                            cf.cf_data_final = Convert.ToDateTime(leitor["cf_data_final"]);
                        }
                        else
                        {
                            cf.cf_data_final = new DateTime();
                        }

                        if (DBNull.Value != leitor["cf_dataCriacao"])
                        {
                            cf.cf_dataCriacao = Convert.ToDateTime(leitor["cf_dataCriacao"]);
                        }
                        else
                        {
                            cf.cf_dataCriacao = new DateTime();
                        }

                        cf.cf_nome = leitor["cf_nome"].ToString();
                        cf.cf_recorrencia = leitor["cf_recorrencia"].ToString();
                        cf.cf_status = leitor["cf_status"].ToString();
                        cf.cf_tipo = leitor["cf_tipo"].ToString();

                        lista.Add(cf);
                    }
                }
                leitor.Close();
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            vmcf.contas = lista;

            return vmcf;
        }

        //Buscar conta financeira
        public Vm_contasFinanceiras buscaCF(int id, int conta_id)
        {
            Vm_contasFinanceiras vmcf = new Vm_contasFinanceiras();
            Operacao op = new Operacao();
            ContasFinanceiras cf = new ContasFinanceiras();
            Op_parcelas parcelas = new Op_parcelas();
            Op_participante participante = new Op_participante();
            Op_nf nf = new Op_nf();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT cf.*, part.*, nf.*, op.op_obs, op.op_data, op.op_escopo_caixa FROM contas_financeiras as cf left JOIN operacao as op on op.op_id = cf.cf_op_id left JOIN op_participante as part on part.op_id = op.op_id left JOIN op_nf as nf on nf.op_nf_op_id = op.op_id WHERE cf.cf_id = @id and op.op_conta_id = @conta_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@id", id);
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        //contas financeiras
                        cf.cf_nome = leitor["cf_nome"].ToString();

                        if (DBNull.Value != leitor["cf_id"])
                        {
                            cf.cf_id = Convert.ToInt32(leitor["cf_id"]);
                        }
                        else
                        {
                            cf.cf_id = 0;
                        }

                        if (DBNull.Value != leitor["cf_categoria_id"])
                        {
                            cf.cf_categoria_id = Convert.ToInt32(leitor["cf_categoria_id"]);
                        }
                        else
                        {
                            cf.cf_categoria_id = 0;
                        }

                        if (DBNull.Value != leitor["cf_forma_pgto"])
                        {
                            cf.cf_forma_pgto = Convert.ToInt32(leitor["cf_forma_pgto"]);
                        }
                        else
                        {
                            cf.cf_forma_pgto = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_operacao"])
                        {
                            cf.cf_valor_operacao = Convert.ToDecimal(leitor["cf_valor_operacao"]);
                        }
                        else
                        {
                            cf.cf_valor_operacao = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_parcela_bruta"])
                        {
                            cf.cf_valor_parcela_bruta = Convert.ToDecimal(leitor["cf_valor_parcela_bruta"]);
                        }
                        else
                        {
                            cf.cf_valor_parcela_bruta = 0;
                        }

                        if (DBNull.Value != leitor["cf_valor_parcela_liquida"])
                        {
                            cf.cf_valor_parcela_liquida = Convert.ToDecimal(leitor["cf_valor_parcela_liquida"]);
                        }
                        else
                        {
                            cf.cf_valor_parcela_liquida = 0;
                        }

                        if (DBNull.Value != leitor["cf_data_inicial"])
                        {
                            cf.cf_data_inicial = Convert.ToDateTime(leitor["cf_data_inicial"]);
                        }
                        else
                        {
                            cf.cf_data_inicial = new DateTime();
                        }

                        if (DBNull.Value != leitor["cf_data_final"])
                        {
                            cf.cf_data_final = Convert.ToDateTime(leitor["cf_data_final"]);
                        }
                        else
                        {
                            cf.cf_data_final = new DateTime();
                        }

                        if (DBNull.Value != leitor["cf_dataCriacao"])
                        {
                            cf.cf_dataCriacao = Convert.ToDateTime(leitor["cf_dataCriacao"]);
                        }
                        else
                        {
                            cf.cf_dataCriacao = new DateTime();
                        }

                        cf.cf_status = leitor["cf_status"].ToString();
                        cf.cf_tipo = leitor["cf_tipo"].ToString();
                        cf.cf_recorrencia = leitor["cf_recorrencia"].ToString();

                        //Operação
                        op.op_obs = leitor["op_obs"].ToString();
                        op.op_escopo_caixa = leitor["op_escopo_caixa"].ToString();
                        if (DBNull.Value != leitor["op_data"])
                        {
                            op.op_data = Convert.ToDateTime(leitor["op_data"]);
                        }
                        else
                        {
                            op.op_data = new DateTime();
                        }

                        //participante
                        if (DBNull.Value != leitor["op_part_participante_id"])
                        {
                            participante.op_part_participante_id = Convert.ToInt32(leitor["op_part_participante_id"]);
                        }
                        else
                        {
                            participante.op_part_participante_id = 0;
                        }

                        participante.op_part_nome = leitor["op_part_nome"].ToString();
                        participante.op_part_tipo = leitor["op_part_tipo"].ToString();
                        participante.op_part_cnpj_cpf = leitor["op_part_cnpj_cpf"].ToString();
                        participante.op_part_cep = leitor["op_part_cep"].ToString();
                        participante.op_part_logradouro = leitor["op_part_logradouro"].ToString();
                        participante.op_part_numero = leitor["op_part_numero"].ToString();
                        participante.op_part_complemento = leitor["op_part_complemento"].ToString();
                        participante.op_part_bairro = leitor["op_part_bairro"].ToString();
                        participante.op_part_cidade = leitor["op_part_cidade"].ToString();

                        if (DBNull.Value != leitor["op_uf_ibge_codigo"])
                        {
                            participante.op_uf_ibge_codigo = Convert.ToInt32(leitor["op_uf_ibge_codigo"]);
                        }
                        else
                        {
                            participante.op_uf_ibge_codigo = 0;
                        }

                        if (DBNull.Value != leitor["op_paisesIBGE_codigo"])
                        {
                            participante.op_paisesIBGE_codigo = Convert.ToInt32(leitor["op_paisesIBGE_codigo"]);
                        }
                        else
                        {
                            participante.op_paisesIBGE_codigo = 0;
                        }

                        //NF
                        if (DBNull.Value != leitor["op_nf_id"])
                        {
                            nf.op_nf_id = Convert.ToInt32(leitor["op_nf_id"]);
                        }
                        else
                        {
                            nf.op_nf_id = 0;
                        }

                        if (DBNull.Value != leitor["op_nf_op_id"])
                        {
                            nf.op_nf_op_id = Convert.ToInt32(leitor["op_nf_op_id"]);
                        }
                        else
                        {
                            nf.op_nf_op_id = 0;
                        }

                        if (DBNull.Value != leitor["op_nf_tipo"])
                        {
                            nf.op_nf_tipo = Convert.ToInt32(leitor["op_nf_tipo"]);
                        }
                        else
                        {
                            nf.op_nf_tipo = 0;
                        }

                        if (DBNull.Value != leitor["op_nf_data_emissao"])
                        {
                            nf.op_nf_data_emissao = Convert.ToDateTime(leitor["op_nf_data_emissao"]);
                        }
                        else
                        {
                            nf.op_nf_data_emissao = new DateTime();
                        }

                        nf.op_nf_serie = leitor["op_nf_serie"].ToString();
                        nf.op_nf_numero = leitor["op_nf_numero"].ToString();
                        
                    }
                }
                leitor.Close();

                vmcf.cf = cf;
                vmcf.op = op;
                vmcf.participante = participante;
                vmcf.parcelas = parcelas;
                vmcf.nf = nf;
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vmcf;
        }

        public string alterarContaFinanceira(int usuario_id, int conta_id, Vm_contasFinanceiras vmcf)
        {
            string retorno = "Conta Financeira alterada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_contasFinanceirasEdit(@conta_id, @cf_id, @categoria_id, @cf_nome, @cf_status, @op_nf_tipo, @op_nf_chave, @op_nf_data_emissao, @op_nf_serie, @op_nf_numero, @op_obs);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@cf_id", vmcf.cf.cf_id);
                comando.Parameters.AddWithValue("@categoria_id", vmcf.op.op_categoria_id);
                comando.Parameters.AddWithValue("@cf_nome", vmcf.cf.cf_nome);
                if(vmcf.cf.cf_tipo == "Realizar")
                {
                    comando.Parameters.AddWithValue("@cf_status", vmcf.cf.cf_status);
                }
                else
                {
                    comando.Parameters.AddWithValue("@cf_status", "Ativo");
                }                
                comando.Parameters.AddWithValue("@op_nf_tipo", vmcf.nf.op_nf_tipo);
                comando.Parameters.AddWithValue("@op_nf_chave", vmcf.nf.op_nf_chave);
                comando.Parameters.AddWithValue("@op_nf_data_emissao", vmcf.nf.op_nf_data_emissao);
                comando.Parameters.AddWithValue("@op_nf_serie", vmcf.nf.op_nf_serie);
                comando.Parameters.AddWithValue("@op_nf_numero", vmcf.nf.op_nf_numero);
                comando.Parameters.AddWithValue("@op_obs", vmcf.op.op_obs);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração da conta financeira nome atual: " + vmcf.cf.cf_nome + " alterada com sucesso";
                log.log("ContasFinanceiras", "alterarContaFinanceira", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar a conta financeira. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("ContasFinanceiras", "alterarContaFinanceira", "Erro", msg, conta_id, usuario_id);
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

        

        public Decimal baixasPorCF(int cf_id)
        {
            Decimal baixas = 0;

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT COALESCE(sum(op_parcelas_baixa.oppb_valor),0) as baixas from op_parcelas_baixa WHERE op_parcelas_baixa.oppb_op_id = (SELECT contas_financeiras.cf_op_id from contas_financeiras WHERE contas_financeiras.cf_id = @cf_id);";                
                comando.Parameters.AddWithValue("@cf_id", cf_id);                
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["baixas"])
                        {
                            baixas = Convert.ToDecimal(leitor["baixas"]);
                        }
                        else
                        {
                            baixas = 0;
                        }
                    }
                }
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

            return baixas;
        }
        

        public string deleteContaFinanceira(int usuario_id, int conta_id, int cf_id)
        {
            string retorno = "Conta Financeira apagada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_contasFinanceirasDelete(@conta_id, @cf_id);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@cf_id", cf_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão da conta financeira ID: " + cf_id + " excluida com sucesso";
                log.log("ContasFinanceiras", "deleteContaFinanceira", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir a conta financeira. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("ContasFinanceiras", "deleteContaFinanceira", "Erro", msg, conta_id, usuario_id);
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
