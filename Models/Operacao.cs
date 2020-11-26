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
    public class Operacao
    {
        public int op_id { get; set; }
        public string op_tipo { get; set; }
        public DateTime op_dataCriacao { get; set; }        
        public DateTime op_data { get; set; }
        public DateTime op_previsao_entrega { get; set; } //Previsão da entrega da venda, da compra, do serviço prestado...
        public DateTime op_data_saida { get; set; } //Data de saída no caso de operação de venda
        public int op_conta_id { get; set; }
        public string op_obs { get; set; }
        public int op_numero_ordem { get; set; }
        public int op_categoria_id { get; set; }
        public bool op_comParticipante { get; set; }
        public bool op_comRetencoes { get; set; }
        public bool op_comTransportador { get; set; }
        public int op_comNF { get; set; }
        public bool possui_parcelas { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Operacao()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public string cadastraOperacao(int usuario_id, int conta_id, Vm_operacao op)
        {
            string retorno = "Operação cadastrada com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;
            MySqlDataReader myReader;

            try
            {
                //Operação
                comando.CommandText = "call pr_operacao(@op_tipo, @op_data, @op_conta_id, @op_obs, @op_previsao_entrega, @op_data_saida, @op_categoria_id, @op_comParticipante, @op_comRetencoes, @op_comTransportador, @op_comNF);";                
                comando.Parameters.AddWithValue("@op_tipo", op.operacao.op_tipo);
                comando.Parameters.AddWithValue("@op_data", op.operacao.op_data);
                comando.Parameters.AddWithValue("@op_conta_id", conta_id);
                comando.Parameters.AddWithValue("@op_obs", op.operacao.op_obs);                
                comando.Parameters.AddWithValue("@op_previsao_entrega", op.operacao.op_previsao_entrega);
                comando.Parameters.AddWithValue("@op_data_saida", op.operacao.op_data_saida);
                comando.Parameters.AddWithValue("@op_categoria_id", op.operacao.op_categoria_id);
                comando.Parameters.AddWithValue("@op_comParticipante", op.operacao.op_comParticipante);
                comando.Parameters.AddWithValue("@op_comRetencoes", op.operacao.op_comRetencoes);
                comando.Parameters.AddWithValue("@op_comTransportador", op.operacao.op_comTransportador);
                comando.Parameters.AddWithValue("@op_comNF", op.operacao.op_comNF);
                comando.ExecuteNonQuery();


                //recuperando id da operação
                int id = 0;
                comando.CommandText = "SELECT LAST_INSERT_ID();";
                myReader = comando.ExecuteReader();
                while (myReader.Read())
                {
                    id = myReader.GetInt32(0);
                }
                myReader.Close();

                //participante
                if(op.participante.op_part_cnpj_cpf != "")
                {
                    comando.CommandText = "insert into op_participante (op_part_nome, op_part_tipo, op_part_cnpj_cpf, op_part_cep, op_part_cidade, op_part_bairro, op_part_logradouro, op_part_numero, op_part_complemento, op_paisesIBGE_codigo, op_uf_ibge_codigo, op_id, op_part_participante_id) values (@op_part_nome, @op_part_tipo, @op_part_cnpj_cpf, @op_part_cep, @op_part_cidade, @op_part_bairro, @op_part_logradouro, @op_part_numero, @op_part_complemento, @op_paisesIBGE_codigo, @op_uf_ibge_codigo, @op_id, @op_part_participante_id);";
                    comando.Parameters.AddWithValue("@op_part_nome", op.participante.op_part_nome);
                    comando.Parameters.AddWithValue("@op_part_tipo", op.participante.op_part_tipo);
                    comando.Parameters.AddWithValue("@op_part_cnpj_cpf", op.participante.op_part_cnpj_cpf);
                    comando.Parameters.AddWithValue("@op_part_cep", op.participante.op_part_cep);
                    comando.Parameters.AddWithValue("@op_part_cidade", op.participante.op_part_cidade);
                    comando.Parameters.AddWithValue("@op_part_bairro", op.participante.op_part_bairro);
                    comando.Parameters.AddWithValue("@op_part_logradouro", op.participante.op_part_logradouro);
                    comando.Parameters.AddWithValue("@op_part_numero", op.participante.op_part_numero);
                    comando.Parameters.AddWithValue("@op_part_complemento", op.participante.op_part_complemento);
                    comando.Parameters.AddWithValue("@op_paisesIBGE_codigo", op.participante.op_paisesIBGE_codigo);
                    comando.Parameters.AddWithValue("@op_uf_ibge_codigo", op.participante.op_uf_ibge_codigo);
                    comando.Parameters.AddWithValue("@op_part_participante_id", op.participante.op_part_participante_id);
                    comando.Parameters.AddWithValue("@op_id", id);
                    comando.ExecuteNonQuery();
                }

                //Itens
                if(op.itens.Count > 0)
                {
                    for(int i = 0; i < op.itens.Count; i++)
                    {
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = Transacao;

                        cmd.CommandText = "insert into op_itens (op_item_codigo, op_item_nome, op_item_unidade, op_item_preco, op_item_gtin_ean, op_item_gtin_ean_trib, op_item_obs, op_item_qtd, op_item_frete, op_item_seguros, op_item_desp_aces, op_item_desconto, op_item_op_id, op_item_vlr_ipi, op_item_vlr_icms_st, op_item_cod_fornecedor, op_item_produto_id, op_item_valor_total) values (@op_item_codigo, @op_item_nome, @op_item_unidade, @op_item_preco, @op_item_gtin_ean, @op_item_gtin_ean_trib, @op_item_obs, @op_item_qtd, @op_item_frete, @op_item_seguros, @op_item_desp_aces, @op_item_desconto, @op_item_op_id, @op_item_vlr_ipi, @op_item_vlr_icms_st, @op_item_cod_fornecedor, @op_item_produto_id, @op_item_valor_total);";
                        cmd.Parameters.AddWithValue("@op_item_op_id", id);
                        cmd.Parameters.AddWithValue("@op_item_codigo", op.itens[i].op_item_codigo);
                        cmd.Parameters.AddWithValue("@op_item_nome", op.itens[i].op_item_nome);
                        cmd.Parameters.AddWithValue("@op_item_unidade", op.itens[i].op_item_unidade);
                        cmd.Parameters.AddWithValue("@op_item_preco", op.itens[i].op_item_preco);
                        cmd.Parameters.AddWithValue("@op_item_gtin_ean", op.itens[i].op_item_gtin_ean);
                        cmd.Parameters.AddWithValue("@op_item_gtin_ean_trib", op.itens[i].op_item_gtin_ean_trib);
                        cmd.Parameters.AddWithValue("@op_item_obs", op.itens[i].op_item_obs);
                        cmd.Parameters.AddWithValue("@op_item_qtd", op.itens[i].op_item_qtd);
                        cmd.Parameters.AddWithValue("@op_item_frete", op.itens[i].op_item_frete);
                        cmd.Parameters.AddWithValue("@op_item_seguros", op.itens[i].op_item_seguros);
                        cmd.Parameters.AddWithValue("@op_item_desp_aces", op.itens[i].op_item_desp_aces);
                        cmd.Parameters.AddWithValue("@op_item_desconto", op.itens[i].op_item_desconto);
                        cmd.Parameters.AddWithValue("@op_item_vlr_ipi", op.itens[i].op_item_vlr_ipi);
                        cmd.Parameters.AddWithValue("@op_item_vlr_icms_st", op.itens[i].op_item_vlr_icms_st);
                        cmd.Parameters.AddWithValue("@op_item_cod_fornecedor", op.itens[i].op_item_cod_fornecedor);
                        cmd.Parameters.AddWithValue("@op_item_produto_id", op.itens[i].op_item_produto_id);
                        cmd.Parameters.AddWithValue("@op_item_valor_total", op.itens[i].op_item_valor_total);
                        cmd.ExecuteNonQuery();
                    }
                }

                //totais
                comando.CommandText = "insert into op_totais (op_totais_preco_itens, op_totais_frete, op_totais_seguro, op_totais_desp_aces, op_totais_desconto, op_totais_itens, op_totais_qtd_itens, op_totais_op_id, op_totais_retencoes, op_totais_total_op, op_totais_ipi, op_totais_icms_st, op_totais_saldoLiquidacao, op_totais_preco_servicos) values (@op_totais_preco_itens, @op_totais_frete, @op_totais_seguro, @op_totais_desp_aces, @op_totais_desconto, @op_totais_itens, @op_totais_qtd_itens, @op_totais_op_id, @op_totais_retencoes, @op_totais_total_op, @op_totais_ipi, @op_totais_icms_st, @op_totais_saldoLiquidacao, @op_totais_preco_servicos);";
                comando.Parameters.AddWithValue("@op_totais_op_id", id);
                comando.Parameters.AddWithValue("@op_totais_preco_itens", op.totais.op_totais_preco_itens);
                comando.Parameters.AddWithValue("@op_totais_frete", op.totais.op_totais_frete);
                comando.Parameters.AddWithValue("@op_totais_seguro", op.totais.op_totais_seguro);
                comando.Parameters.AddWithValue("@op_totais_desp_aces", op.totais.op_totais_desp_aces);
                comando.Parameters.AddWithValue("@op_totais_desconto", op.totais.op_totais_desconto);
                comando.Parameters.AddWithValue("@op_totais_itens", op.totais.op_totais_itens);
                comando.Parameters.AddWithValue("@op_totais_qtd_itens", op.totais.op_totais_qtd_itens);                
                comando.Parameters.AddWithValue("@op_totais_retencoes", op.totais.op_totais_retencoes);
                comando.Parameters.AddWithValue("@op_totais_total_op", op.totais.op_totais_total_op);
                comando.Parameters.AddWithValue("@op_totais_ipi", op.totais.op_totais_ipi);
                comando.Parameters.AddWithValue("@op_totais_icms_st", op.totais.op_totais_icms_st);
                comando.Parameters.AddWithValue("@op_totais_saldoLiquidacao", op.totais.op_totais_saldoLiquidacao);
                comando.Parameters.AddWithValue("@op_totais_preco_servicos", op.totais.op_totais_preco_servicos);
                comando.ExecuteNonQuery();

                //retenções
                if (op.retencoes.op_ret_inss > 0 || op.retencoes.op_ret_issqn > 0 || op.retencoes.op_ret_irrf > 0 || op.retencoes.op_ret_pis > 0 || op.retencoes.op_ret_cofins > 0 || op.retencoes.op_ret_csll > 0)
                {
                    comando.CommandText = "insert into op_retencoes (op_ret_pis, op_ret_cofins, op_ret_csll, op_ret_irrf, op_ret_inss, op_ret_issqn, op_ret_op_id) values (@op_ret_pis, @op_ret_cofins, @op_ret_csll, @op_ret_irrf, @op_ret_inss, @op_ret_issqn, @op_ret_op_id);";
                    comando.Parameters.AddWithValue("@op_ret_op_id", id);
                    comando.Parameters.AddWithValue("@op_ret_pis", op.retencoes.op_ret_pis);
                    comando.Parameters.AddWithValue("@op_ret_cofins", op.retencoes.op_ret_cofins);
                    comando.Parameters.AddWithValue("@op_ret_csll", op.retencoes.op_ret_csll);
                    comando.Parameters.AddWithValue("@op_ret_irrf", op.retencoes.op_ret_irrf);
                    comando.Parameters.AddWithValue("@op_ret_inss", op.retencoes.op_ret_inss);
                    comando.Parameters.AddWithValue("@op_ret_issqn", op.retencoes.op_ret_issqn);
                    comando.ExecuteNonQuery();
                }                

                if(op.parcelas.Count > 0)
                {
                    for (int i = 0; i < op.parcelas.Count; i++)
                    {
                        string contra_partida_tipo = "";
                        int contra_partidade_id = 0;

                        if(op.participante.op_part_participante_id != 0) //se operação possui um participante
                        {
                            contra_partida_tipo = "Participante";
                            contra_partidade_id = op.participante.op_part_participante_id;
                        }
                        else
                        {
                            contra_partida_tipo = "Categoria";
                            contra_partidade_id = op.operacao.op_categoria_id;
                        }

                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = Transacao;


                        cmd.CommandText = "call pr_criaParcela (@op_parcela_dias, @op_parcela_vencimento, @op_parcela_fp_id, @op_parcela_op_id, @op_parcela_valor, @op_parcela_obs, @conta_id, @ccm_contra_partida_tipo, @ccm_contra_partida_id, @op_parcela_valor_bruto, @op_parcela_ret_inss, @op_parcela_ret_issqn, @op_parcela_ret_irrf, @op_parcela_ret_pis, @op_parcela_ret_cofins, @op_parcela_ret_csll);";
                        cmd.Parameters.AddWithValue("@conta_id", conta_id);
                        cmd.Parameters.AddWithValue("@op_parcela_dias", op.parcelas[i].op_parcela_dias);
                        cmd.Parameters.AddWithValue("@op_parcela_vencimento", op.parcelas[i].op_parcela_vencimento);
                        cmd.Parameters.AddWithValue("@op_parcela_fp_id", op.parcelas[i].op_parcela_fp_id);
                        cmd.Parameters.AddWithValue("@op_parcela_op_id", id);
                        cmd.Parameters.AddWithValue("@op_parcela_valor", op.parcelas[i].op_parcela_valor);
                        cmd.Parameters.AddWithValue("@op_parcela_obs", op.parcelas[i].op_parcela_obs);
                        cmd.Parameters.AddWithValue("@ccm_contra_partida_tipo", contra_partida_tipo);
                        cmd.Parameters.AddWithValue("@ccm_contra_partida_id", contra_partidade_id);
                        cmd.Parameters.AddWithValue("@op_parcela_valor_bruto", op.parcelas[i].op_parcela_valor_bruto);
                        cmd.Parameters.AddWithValue("@op_parcela_ret_inss", op.parcelas[i].op_parcela_ret_inss);
                        cmd.Parameters.AddWithValue("@op_parcela_ret_issqn", op.parcelas[i].op_parcela_ret_issqn);
                        cmd.Parameters.AddWithValue("@op_parcela_ret_irrf", op.parcelas[i].op_parcela_ret_irrf);
                        cmd.Parameters.AddWithValue("@op_parcela_ret_pis", op.parcelas[i].op_parcela_ret_pis);
                        cmd.Parameters.AddWithValue("@op_parcela_ret_cofins", op.parcelas[i].op_parcela_ret_cofins);
                        cmd.Parameters.AddWithValue("@op_parcela_ret_csll", op.parcelas[i].op_parcela_ret_csll);
                        cmd.ExecuteNonQuery();
                    }
                }

                if (op.operacao.op_comTransportador)
                {
                    //transportador
                    comando.CommandText = "INSERT into op_transportador (op_transportador_nome, op_transportador_cnpj_cpf, op_transportador_modalidade_frete, op_transportador_volume_qtd, op_transportador_volume_peso_bruto, op_transportador_op_id, op_transportador_participante_id) VALUES (@op_transportador_nome, @op_transportador_cnpj_cpf, @op_transportador_modalidade_frete, @op_transportador_volume_qtd, @op_transportador_volume_peso_bruto, @op_transportador_op_id, @op_transportador_participante_id);";
                    comando.Parameters.AddWithValue("@conta_id", conta_id);
                    comando.Parameters.AddWithValue("@op_transportador_nome", op.transportador.op_transportador_nome);
                    comando.Parameters.AddWithValue("@op_transportador_cnpj_cpf", op.transportador.op_transportador_cnpj_cpf);
                    comando.Parameters.AddWithValue("@op_transportador_modalidade_frete", op.transportador.op_transportador_modalidade_frete);
                    comando.Parameters.AddWithValue("@op_transportador_volume_qtd", op.transportador.op_transportador_volume_qtd);
                    comando.Parameters.AddWithValue("@op_transportador_volume_peso_bruto", op.transportador.op_transportador_volume_peso_bruto);
                    comando.Parameters.AddWithValue("@op_transportador_op_id", id);
                    comando.Parameters.AddWithValue("@op_transportador_participante_id", op.transportador.op_transportador_participante_id);
                    comando.ExecuteNonQuery();
                }
                
                if(op.operacao.op_comNF != 0)
                {
                    //nota fiscal
                    comando.CommandText = "insert into op_nf (op_nf_op_id, op_nf_chave, op_nf_data_emissao, op_nf_data_entrada_saida, op_nf_serie, op_nf_numero) values (@op_nf_op_id, @op_nf_chave, @op_nf_data_emissao, @op_nf_data_entrada_saida, @op_nf_serie, @op_nf_numero);";
                    comando.Parameters.AddWithValue("@op_nf_op_id", id);
                    comando.Parameters.AddWithValue("@op_nf_chave", op.nf.op_nf_chave);
                    comando.Parameters.AddWithValue("@op_nf_data_emissao", op.nf.op_nf_data_emissao);
                    comando.Parameters.AddWithValue("@op_nf_data_entrada_saida", op.nf.op_nf_data_entrada_saida);
                    comando.Parameters.AddWithValue("@op_nf_serie", op.nf.op_nf_serie);
                    comando.Parameters.AddWithValue("@op_nf_numero", op.nf.op_nf_numero);
                    comando.ExecuteNonQuery();
                }

                if(op.operacao.op_tipo == "ServicoPrestado" || op.operacao.op_tipo == "ServicoTomado")
                {
                    comando.CommandText = "INSERT into op_servico (op_servico_op_id, op_servico_equipamento, op_servico_nSerie, op_servico_problema, op_servico_obsReceb, op_servico_servico_executado, op_servico_valor, op_servico_status) values (@op_servico_op_id, @op_servico_equipamento, @op_servico_nSerie, @op_servico_problema, @op_servico_obsReceb, @op_servico_servico_executado, @op_servico_valor, @op_servico_status);";
                    comando.Parameters.AddWithValue("@op_servico_op_id", id);
                    comando.Parameters.AddWithValue("@op_servico_equipamento", op.servico.op_servico_equipamento);
                    comando.Parameters.AddWithValue("@op_servico_nSerie", op.servico.op_servico_nSerie);
                    comando.Parameters.AddWithValue("@op_servico_problema", op.servico.op_servico_problema);
                    comando.Parameters.AddWithValue("@op_servico_obsReceb", op.servico.op_servico_obsReceb);
                    comando.Parameters.AddWithValue("@op_servico_servico_executado", op.servico.op_servico_servico_executado);
                    comando.Parameters.AddWithValue("@op_servico_valor", op.servico.op_servico_valor);
                    comando.Parameters.AddWithValue("@op_servico_status", op.servico.op_servico_status);
                    comando.ExecuteNonQuery();
                }

                Transacao.Commit();

                string msg = "Cadastro de operação ID: " + id + " Cadastrado com sucesso";
                log.log("Operacao", "cadastraOperacao", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar a operação. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Operacao", "cadastraOperacao", "Erro", msg, conta_id, usuario_id);
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

        //Lista operação (para tela index)
        public List<Vm_operacao> listaOperacao(int usuario_id, int conta_id, string tipo)
        {
            List<Vm_operacao> ops = new List<Vm_operacao>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT op.op_id, op.op_numero_ordem, op.op_data, p.op_part_nome, t.op_totais_total_op from operacao as op LEFT join op_participante as p on p.op_id = op.op_id LEFT JOIN op_totais as t on t.op_totais_op_id = op.op_id WHERE op.op_conta_id = @conta_id and op.op_tipo = @tipo;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@tipo", tipo);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_operacao op = new Vm_operacao();
                        Operacao operacao = new Operacao();
                        Op_totais totais = new Op_totais();
                        Op_participante participante = new Op_participante();
                        op.operacao = operacao;
                        op.totais = totais;
                        op.participante = participante;


                        if (DBNull.Value != leitor["op_id"])
                        {
                            op.operacao.op_id = Convert.ToInt32(leitor["op_id"]);
                        }
                        else
                        {
                            op.operacao.op_id = 0;
                        }

                        if (DBNull.Value != leitor["op_numero_ordem"])
                        {
                            op.operacao.op_numero_ordem = Convert.ToInt32(leitor["op_numero_ordem"]);
                        }
                        else
                        {
                            op.operacao.op_numero_ordem = 0;
                        }

                        if (DBNull.Value != leitor["op_data"])
                        {
                            op.operacao.op_data = Convert.ToDateTime(leitor["op_data"]);
                        }
                        else
                        {
                            op.operacao.op_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_totais_total_op"])
                        {
                            op.totais.op_totais_total_op = Convert.ToDecimal(leitor["op_totais_total_op"]);
                        }
                        else
                        {
                            op.totais.op_totais_total_op = 0;
                        }

                        op.participante.op_part_nome = leitor["op_part_nome"].ToString();

                        ops.Add(op);
                    }
                }

            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Operacao", "listaOperacao", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return ops;

        }

        //Busca operação
        public Vm_operacao buscaOperacao(int usuario_id, int conta_id, int op_id)
        {
            Vm_operacao op = new Vm_operacao();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;
            MySqlDataReader leitor;
            MySqlDataReader leitor_2;
            MySqlDataReader leitor_3;
            

            try
            {
                //Objetos da operação
                Operacao operacao = new Operacao();
                Op_totais totais = new Op_totais();
                Op_participante participante = new Op_participante();
                Op_transportador transportador = new Op_transportador();
                Op_retencoes retencoes = new Op_retencoes();
                Op_nf nf = new Op_nf();
                op.operacao = operacao;
                op.totais = totais;
                op.participante = participante;
                op.transportador = transportador;
                op.retencoes = retencoes;
                op.nf = nf;


                MySqlDataReader buscaParcela;

                MySqlCommand cmdpar = conn.CreateCommand();
                cmdpar.Connection = conn;
                cmdpar.Transaction = Transacao;

                cmdpar.CommandText = "SELECT * from op_parcelas_baixa as b where b.oppb_op_id = @op;";
                cmdpar.Parameters.AddWithValue("@op", op_id);

                buscaParcela = cmdpar.ExecuteReader();

                if (buscaParcela.HasRows)
                {
                    operacao.possui_parcelas = true;                    
                }

                buscaParcela.Close();

                comando.CommandText = "SELECT op.*, p.*, t.*, tr.*, ret.*, nf.*, s.* from operacao as op LEFT join op_participante as p on p.op_id = op.op_id LEFT JOIN op_totais as t on t.op_totais_op_id = op.op_id LEFT join op_transportador as tr on tr.op_transportador_op_id = op.op_id LEFT join op_retencoes as ret on ret.op_ret_op_id = op.op_id left join op_nf as nf on nf.op_nf_op_id = op.op_id left join op_servico as s on s.op_servico_op_id = operacao.op_id WHERE op.op_conta_id = @conta_id and op.op_id = @op_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@op_id", op_id);
                comando.ExecuteNonQuery();                

                leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {   
                        //Operação
                        if (DBNull.Value != leitor["op_id"])
                        {
                            op.operacao.op_id = Convert.ToInt32(leitor["op_id"]);
                        }
                        else
                        {
                            op.operacao.op_id = 0;
                        }

                        if (DBNull.Value != leitor["op_numero_ordem"])
                        {
                            op.operacao.op_numero_ordem = Convert.ToInt32(leitor["op_numero_ordem"]);
                        }
                        else
                        {
                            op.operacao.op_numero_ordem = 0;
                        }

                        if (DBNull.Value != leitor["op_data"])
                        {
                            op.operacao.op_data = Convert.ToDateTime(leitor["op_data"]);
                        }
                        else
                        {
                            op.operacao.op_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_dataCriacao"])
                        {
                            op.operacao.op_dataCriacao = Convert.ToDateTime(leitor["op_dataCriacao"]);
                        }
                        else
                        {
                            op.operacao.op_dataCriacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_data_saida"])
                        {
                            op.operacao.op_data_saida = Convert.ToDateTime(leitor["op_data_saida"]);
                        }
                        else
                        {
                            op.operacao.op_data_saida = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_previsao_entrega"])
                        {
                            op.operacao.op_previsao_entrega = Convert.ToDateTime(leitor["op_previsao_entrega"]);
                        }
                        else
                        {
                            op.operacao.op_previsao_entrega = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_conta_id"])
                        {
                            op.operacao.op_conta_id = Convert.ToInt32(leitor["op_conta_id"]);
                        }
                        else
                        {
                            op.operacao.op_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["op_categoria_id"])
                        {
                            op.operacao.op_categoria_id = Convert.ToInt32(leitor["op_categoria_id"]);
                        }
                        else
                        {
                            op.operacao.op_categoria_id = 0;
                        }

                        if (DBNull.Value != leitor["op_categoria_id"])
                        {
                            op.operacao.op_categoria_id = Convert.ToInt32(leitor["op_categoria_id"]);
                        }
                        else
                        {
                            op.operacao.op_categoria_id = 0;
                        }

                        if (DBNull.Value != leitor["op_comNF"])
                        {
                            op.operacao.op_comNF = Convert.ToInt32(leitor["op_comNF"]);
                        }
                        else
                        {
                            op.operacao.op_comNF = 0;
                        }

                        op.operacao.op_comParticipante = Convert.ToBoolean(leitor["op_comParticipante"]);
                        op.operacao.op_comRetencoes = Convert.ToBoolean(leitor["op_comRetencoes"]);
                        op.operacao.op_comTransportador = Convert.ToBoolean(leitor["op_comTransportador"]);

                        op.operacao.op_tipo = leitor["op_tipo"].ToString();
                        op.operacao.op_obs = leitor["op_obs"].ToString();

                        //participante
                        if (DBNull.Value != leitor["op_part_id"])
                        {
                            op.participante.op_part_id = Convert.ToInt32(leitor["op_part_id"]);
                        }
                        else
                        {
                            op.participante.op_part_id = 0;
                        }

                        if (DBNull.Value != leitor["op_paisesIBGE_codigo"])
                        {
                            op.participante.op_paisesIBGE_codigo = Convert.ToInt32(leitor["op_paisesIBGE_codigo"]);
                        }
                        else
                        {
                            op.participante.op_paisesIBGE_codigo = 0;
                        }

                        if (DBNull.Value != leitor["op_uf_ibge_codigo"])
                        {
                            op.participante.op_uf_ibge_codigo = Convert.ToInt32(leitor["op_uf_ibge_codigo"]);
                        }
                        else
                        {
                            op.participante.op_uf_ibge_codigo = 0;
                        }

                        if (DBNull.Value != leitor["op_id"])
                        {
                            op.participante.op_id = Convert.ToInt32(leitor["op_id"]);
                        }
                        else
                        {
                            op.participante.op_id = 0;
                        }

                        if (DBNull.Value != leitor["op_part_participante_id"])
                        {
                            op.participante.op_part_participante_id = Convert.ToInt32(leitor["op_part_participante_id"]);
                        }
                        else
                        {
                            op.participante.op_part_participante_id = 0;
                        }

                        op.participante.op_part_cep = leitor["op_part_cep"].ToString();
                        op.participante.op_part_nome = leitor["op_part_nome"].ToString();
                        op.participante.op_part_logradouro = leitor["op_part_logradouro"].ToString();
                        op.participante.op_part_numero = leitor["op_part_numero"].ToString();
                        op.participante.op_part_tipo = leitor["op_part_tipo"].ToString();
                        op.participante.op_part_cnpj_cpf = leitor["op_part_cnpj_cpf"].ToString();
                        op.participante.op_part_complemento = leitor["op_part_complemento"].ToString();
                        op.participante.op_part_bairro = leitor["op_part_bairro"].ToString();
                        op.participante.op_part_cidade = leitor["op_part_cidade"].ToString();

                        //totais
                        if (DBNull.Value != leitor["op_totais_id"])
                        {
                            op.totais.op_totais_id = Convert.ToInt32(leitor["op_totais_id"]);
                        }
                        else
                        {
                            op.totais.op_totais_id = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_itens"])
                        {
                            op.totais.op_totais_itens = Convert.ToInt32(leitor["op_totais_itens"]);
                        }
                        else
                        {
                            op.totais.op_totais_itens = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_op_id"])
                        {
                            op.totais.op_totais_op_id = Convert.ToInt32(leitor["op_totais_op_id"]);
                        }
                        else
                        {
                            op.totais.op_totais_op_id = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_preco_itens"])
                        {
                            op.totais.op_totais_preco_itens = Convert.ToDecimal(leitor["op_totais_preco_itens"]);
                        }
                        else
                        {
                            op.totais.op_totais_preco_itens = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_frete"])
                        {
                            op.totais.op_totais_frete = Convert.ToDecimal(leitor["op_totais_frete"]);
                        }
                        else
                        {
                            op.totais.op_totais_frete = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_seguro"])
                        {
                            op.totais.op_totais_seguro = Convert.ToDecimal(leitor["op_totais_seguro"]);
                        }
                        else
                        {
                            op.totais.op_totais_seguro = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_desp_aces"])
                        {
                            op.totais.op_totais_desp_aces = Convert.ToDecimal(leitor["op_totais_desp_aces"]);
                        }
                        else
                        {
                            op.totais.op_totais_desp_aces = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_desconto"])
                        {
                            op.totais.op_totais_desconto = Convert.ToDecimal(leitor["op_totais_desconto"]);
                        }
                        else
                        {
                            op.totais.op_totais_desconto = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_qtd_itens"])
                        {
                            op.totais.op_totais_qtd_itens = Convert.ToDecimal(leitor["op_totais_qtd_itens"]);
                        }
                        else
                        {
                            op.totais.op_totais_qtd_itens = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_retencoes"])
                        {
                            op.totais.op_totais_retencoes = Convert.ToDecimal(leitor["op_totais_retencoes"]);
                        }
                        else
                        {
                            op.totais.op_totais_retencoes = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_total_op"])
                        {
                            op.totais.op_totais_total_op = Convert.ToDecimal(leitor["op_totais_total_op"]);
                        }
                        else
                        {
                            op.totais.op_totais_total_op = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_preco_servicos"])
                        {
                            op.totais.op_totais_preco_servicos = Convert.ToDecimal(leitor["op_totais_preco_servicos"]);
                        }
                        else
                        {
                            op.totais.op_totais_preco_servicos = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_ipi"])
                        {
                            op.totais.op_totais_ipi = Convert.ToDecimal(leitor["op_totais_ipi"]);
                        }
                        else
                        {
                            op.totais.op_totais_ipi = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_icms_st"])
                        {
                            op.totais.op_totais_icms_st = Convert.ToDecimal(leitor["op_totais_icms_st"]);
                        }
                        else
                        {
                            op.totais.op_totais_icms_st = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_saldoLiquidacao"])
                        {
                            op.totais.op_totais_saldoLiquidacao = Convert.ToDecimal(leitor["op_totais_saldoLiquidacao"]);
                        }
                        else
                        {
                            op.totais.op_totais_saldoLiquidacao = 0;
                        }

                        //retencoes
                        if (DBNull.Value != leitor["op_ret_pis"])
                        {
                            op.retencoes.op_ret_pis = Convert.ToDecimal(leitor["op_ret_pis"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_pis = 0;
                        }

                        if (DBNull.Value != leitor["op_ret_cofins"])
                        {
                            op.retencoes.op_ret_cofins = Convert.ToDecimal(leitor["op_ret_cofins"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_cofins = 0;
                        }

                        if (DBNull.Value != leitor["op_ret_csll"])
                        {
                            op.retencoes.op_ret_csll = Convert.ToDecimal(leitor["op_ret_csll"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_csll = 0;
                        }

                        if (DBNull.Value != leitor["op_ret_irrf"])
                        {
                            op.retencoes.op_ret_irrf = Convert.ToDecimal(leitor["op_ret_irrf"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_irrf = 0;
                        }

                        if (DBNull.Value != leitor["op_ret_inss"])
                        {
                            op.retencoes.op_ret_inss = Convert.ToDecimal(leitor["op_ret_inss"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_inss = 0;
                        }

                        if (DBNull.Value != leitor["op_ret_issqn"])
                        {
                            op.retencoes.op_ret_issqn = Convert.ToDecimal(leitor["op_ret_issqn"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_issqn = 0;
                        }

                        if (DBNull.Value != leitor["op_ret_id"])
                        {
                            op.retencoes.op_ret_id = Convert.ToInt32(leitor["op_ret_id"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_id = 0;
                        }

                        if (DBNull.Value != leitor["op_ret_op_id"])
                        {
                            op.retencoes.op_ret_op_id = Convert.ToInt32(leitor["op_ret_op_id"]);
                        }
                        else
                        {
                            op.retencoes.op_ret_op_id = 0;
                        }

                        //transportador
                        if (DBNull.Value != leitor["op_transportador_volume_qtd"])
                        {
                            op.transportador.op_transportador_volume_qtd = Convert.ToDecimal(leitor["op_transportador_volume_qtd"]);
                        }
                        else
                        {
                            op.transportador.op_transportador_volume_qtd = 0;
                        }

                        if (DBNull.Value != leitor["op_transportador_volume_peso_bruto"])
                        {
                            op.transportador.op_transportador_volume_peso_bruto = Convert.ToDecimal(leitor["op_transportador_volume_peso_bruto"]);
                        }
                        else
                        {
                            op.transportador.op_transportador_volume_peso_bruto = 0;
                        }

                        if (DBNull.Value != leitor["op_transportador_id"])
                        {
                            op.transportador.op_transportador_id = Convert.ToInt32(leitor["op_transportador_id"]);
                        }
                        else
                        {
                            op.transportador.op_transportador_id = 0;
                        }

                        if (DBNull.Value != leitor["op_transportador_op_id"])
                        {
                            op.transportador.op_transportador_op_id = Convert.ToInt32(leitor["op_transportador_op_id"]);
                        }
                        else
                        {
                            op.transportador.op_transportador_op_id = 0;
                        }

                        if (DBNull.Value != leitor["op_transportador_participante_id"])
                        {
                            op.transportador.op_transportador_participante_id = Convert.ToInt32(leitor["op_transportador_participante_id"]);
                        }
                        else
                        {
                            op.transportador.op_transportador_participante_id = 0;
                        }

                        op.transportador.op_transportador_nome = leitor["op_transportador_nome"].ToString();
                        op.transportador.op_transportador_modalidade_frete = leitor["op_transportador_modalidade_frete"].ToString();
                        op.transportador.op_transportador_cnpj_cpf = leitor["op_transportador_cnpj_cpf"].ToString();

                        //Nota fiscal
                        if (DBNull.Value != leitor["op_nf_id"])
                        {
                            op.nf.op_nf_id = Convert.ToInt32(leitor["op_nf_id"]);
                        }
                        else
                        {
                            op.nf.op_nf_id = 0;
                        }

                        if (DBNull.Value != leitor["op_nf_op_id"])
                        {
                            op.nf.op_nf_op_id = Convert.ToInt32(leitor["op_nf_op_id"]);
                        }
                        else
                        {
                            op.nf.op_nf_op_id = 0;
                        }

                        if (DBNull.Value != leitor["op_nf_data_emissao"])
                        {
                            op.nf.op_nf_data_emissao = Convert.ToDateTime(leitor["op_nf_data_emissao"]);
                        }
                        else
                        {
                            op.nf.op_nf_data_emissao = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_nf_data_entrada_saida"])
                        {
                            op.nf.op_nf_data_entrada_saida = Convert.ToDateTime(leitor["op_nf_data_entrada_saida"]);
                        }
                        else
                        {
                            op.nf.op_nf_data_entrada_saida = new DateTime();
                        }

                        op.nf.op_nf_chave = leitor["op_nf_chave"].ToString();
                        op.nf.op_nf_serie = leitor["op_nf_serie"].ToString();
                        op.nf.op_nf_numero = leitor["op_nf_numero"].ToString();

                        //Serviço
                        if(op.operacao.op_tipo == "ServicoPrestado" || op.operacao.op_tipo == "ServicoTomado")
                        {
                            if (DBNull.Value != leitor["op_servico_id"])
                            {
                                op.servico.op_servico_id = Convert.ToInt32(leitor["op_servico_id"]);
                            }
                            else
                            {
                                op.servico.op_servico_id = 0;
                            }

                            if (DBNull.Value != leitor["op_servico_op_id"])
                            {
                                op.servico.op_servico_op_id = Convert.ToInt32(leitor["op_servico_op_id"]);
                            }
                            else
                            {
                                op.servico.op_servico_op_id = 0;
                            }

                            if (DBNull.Value != leitor["op_servico_valor"])
                            {
                                op.servico.op_servico_valor = Convert.ToDecimal(leitor["op_servico_valor"]);
                            }
                            else
                            {
                                op.servico.op_servico_valor = 0;
                            }

                            op.servico.op_servico_equipamento = leitor["op_servico_equipamento"].ToString();
                            op.servico.op_servico_problema = leitor["op_servico_problema"].ToString();
                            op.servico.op_servico_obsReceb = leitor["op_servico_obsReceb"].ToString();
                            op.servico.op_servico_nSerie = leitor["op_servico_nSerie"].ToString();
                            op.servico.op_servico_servico_executado = leitor["op_servico_servico_executado"].ToString();
                            op.servico.op_servico_status = leitor["op_servico_status"].ToString();
                        }




                    }

                    //Inserção de atributos de controle
                    if(op.participante.op_part_id == 0)
                    {
                        op.participante.existe = false;
                    }
                    else
                    {
                        op.participante.existe = true;
                    }                   

                    if (op.retencoes.op_ret_id == 0)
                    {
                        op.retencoes.existe = false;
                    }
                    else
                    {
                        op.retencoes.existe = true;
                    }

                    if (op.transportador.op_transportador_id == 0)
                    {
                        op.transportador.existe = false;
                    }
                    else
                    {
                        op.transportador.existe = true;
                    }

                    if (op.nf.op_nf_id == 0)
                    {
                        op.nf.existe = false;
                    }
                    else
                    {
                        op.nf.existe = true;
                    }

                    leitor.Close();
                }


                //Itens
                MySqlCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.Transaction = Transacao;

                List<Op_itens> itens = new List<Op_itens>();

                cmd.CommandText = "SELECT * from op_itens WHERE op_itens.op_item_op_id = @op_id_2;";
                cmd.Parameters.AddWithValue("@op_id_2", op.operacao.op_id);

                leitor_2 = cmd.ExecuteReader();
                if (leitor_2.HasRows)
                {
                    while (leitor_2.Read())
                    {
                        Op_itens item = new Op_itens();

                        if (DBNull.Value != leitor_2["op_item_id"])
                        {
                            item.op_item_id = Convert.ToInt32(leitor_2["op_item_id"]);
                        }
                        else
                        {
                            item.op_item_id = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_op_id"])
                        {
                            item.op_item_op_id = Convert.ToInt32(leitor_2["op_item_op_id"]);
                        }
                        else
                        {
                            item.op_item_op_id = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_produto_id"])
                        {
                            item.op_item_produto_id = Convert.ToInt32(leitor_2["op_item_produto_id"]);
                        }
                        else
                        {
                            item.op_item_produto_id = 0;
                        }
                        //decimais
                        if (DBNull.Value != leitor_2["op_item_preco"])
                        {
                            item.op_item_preco = Convert.ToDecimal(leitor_2["op_item_preco"]);
                        }
                        else
                        {
                            item.op_item_preco = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_qtd"])
                        {
                            item.op_item_qtd = Convert.ToDecimal(leitor_2["op_item_qtd"]);
                        }
                        else
                        {
                            item.op_item_qtd = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_frete"])
                        {
                            item.op_item_frete = Convert.ToDecimal(leitor_2["op_item_frete"]);
                        }
                        else
                        {
                            item.op_item_frete = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_seguros"])
                        {
                            item.op_item_seguros = Convert.ToDecimal(leitor_2["op_item_seguros"]);
                        }
                        else
                        {
                            item.op_item_seguros = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_desp_aces"])
                        {
                            item.op_item_desp_aces = Convert.ToDecimal(leitor_2["op_item_desp_aces"]);
                        }
                        else
                        {
                            item.op_item_desp_aces = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_desconto"])
                        {
                            item.op_item_desconto = Convert.ToDecimal(leitor_2["op_item_desconto"]);
                        }
                        else
                        {
                            item.op_item_desconto = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_vlr_ipi"])
                        {
                            item.op_item_vlr_ipi = Convert.ToDecimal(leitor_2["op_item_vlr_ipi"]);
                        }
                        else
                        {
                            item.op_item_vlr_ipi = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_vlr_icms_st"])
                        {
                            item.op_item_vlr_icms_st = Convert.ToDecimal(leitor_2["op_item_vlr_icms_st"]);
                        }
                        else
                        {
                            item.op_item_vlr_icms_st = 0;
                        }

                        if (DBNull.Value != leitor_2["op_item_valor_total"])
                        {
                            item.op_item_valor_total = Convert.ToDecimal(leitor_2["op_item_valor_total"]);
                        }
                        else
                        {
                            item.op_item_valor_total = 0;
                        }

                        item.op_item_nome = leitor_2["op_item_nome"].ToString();
                        item.op_item_obs = leitor_2["op_item_obs"].ToString();
                        item.op_item_codigo = leitor_2["op_item_codigo"].ToString();
                        item.op_item_gtin_ean = leitor_2["op_item_gtin_ean"].ToString();
                        item.op_item_gtin_ean_trib = leitor_2["op_item_gtin_ean_trib"].ToString();
                        item.op_item_cod_fornecedor = leitor_2["op_item_cod_fornecedor"].ToString();
                        item.op_item_unidade = leitor_2["op_item_unidade"].ToString();
                        item.controleEdit = "update";

                        itens.Add(item);
                    }

                    op.itens = itens;
                    leitor_2.Close();
                }

                //Parcelas
                MySqlCommand cmdp = conn.CreateCommand();
                cmdp.Connection = conn;
                cmdp.Transaction = Transacao;

                List<Op_parcelas> parcelas = new List<Op_parcelas>();

                cmdp.CommandText = "SELECT * from op_parcelas WHERE op_parcelas.op_parcela_op_id = @op_id_3;";
                cmdp.Parameters.AddWithValue("@op_id_3", op.operacao.op_id);

                leitor_3 = cmdp.ExecuteReader();
                if (leitor_3.HasRows)
                {
                    while (leitor_3.Read())
                    {
                        Op_parcelas parcela = new Op_parcelas();

                        if (DBNull.Value != leitor_3["op_parcela_vencimento"])
                        {
                            parcela.op_parcela_vencimento = Convert.ToDateTime(leitor_3["op_parcela_vencimento"]);
                        }
                        else
                        {
                            parcela.op_parcela_vencimento = new DateTime();
                        }

                        if (DBNull.Value != leitor_3["op_parcela_valor"])
                        {
                            parcela.op_parcela_valor = Convert.ToDecimal(leitor_3["op_parcela_valor"]);
                        }
                        else
                        {
                            parcela.op_parcela_valor = 0;
                        }

                        if (DBNull.Value != leitor_3["op_parcela_saldo"])
                        {
                            parcela.op_parcela_saldo = Convert.ToDecimal(leitor_3["op_parcela_saldo"]);
                        }
                        else
                        {
                            parcela.op_parcela_saldo = 0;
                        }

                        if (DBNull.Value != leitor_3["op_parcela_id"])
                        {
                            parcela.op_parcela_id = Convert.ToInt32(leitor_3["op_parcela_id"]);
                        }
                        else
                        {
                            parcela.op_parcela_id = 0;
                        }

                        if (DBNull.Value != leitor_3["op_parcela_dias"])
                        {
                            parcela.op_parcela_dias = Convert.ToInt32(leitor_3["op_parcela_dias"]);
                        }
                        else
                        {
                            parcela.op_parcela_dias = 0;
                        }

                        if (DBNull.Value != leitor_3["op_parcela_fp_id"])
                        {
                            parcela.op_parcela_fp_id = Convert.ToInt32(leitor_3["op_parcela_fp_id"]);
                        }
                        else
                        {
                            parcela.op_parcela_fp_id = 0;
                        }

                        if (DBNull.Value != leitor_3["op_parcela_op_id"])
                        {
                            parcela.op_parcela_op_id = Convert.ToInt32(leitor_3["op_parcela_op_id"]);
                        }
                        else
                        {
                            parcela.op_parcela_op_id = 0;
                        }
                        //--
                        if (DBNull.Value != leitor_3["op_parcela_valor_bruto"])
                        {
                            parcela.op_parcela_valor_bruto = Convert.ToDecimal(leitor_3["op_parcela_valor_bruto"]);
                        }
                        else
                        {
                            parcela.op_parcela_valor_bruto = 0;
                        }
                        if (DBNull.Value != leitor_3["op_parcela_ret_inss"])
                        {
                            parcela.op_parcela_ret_inss = Convert.ToDecimal(leitor_3["op_parcela_ret_inss"]);
                        }
                        else
                        {
                            parcela.op_parcela_ret_inss = 0;
                        }
                        if (DBNull.Value != leitor_3["op_parcela_ret_issqn"])
                        {
                            parcela.op_parcela_ret_issqn = Convert.ToDecimal(leitor_3["op_parcela_ret_issqn"]);
                        }
                        else
                        {
                            parcela.op_parcela_ret_issqn = 0;
                        }
                        if (DBNull.Value != leitor_3["op_parcela_ret_irrf"])
                        {
                            parcela.op_parcela_ret_irrf = Convert.ToDecimal(leitor_3["op_parcela_ret_irrf"]);
                        }
                        else
                        {
                            parcela.op_parcela_ret_irrf = 0;
                        }
                        if (DBNull.Value != leitor_3["op_parcela_ret_pis"])
                        {
                            parcela.op_parcela_ret_pis = Convert.ToDecimal(leitor_3["op_parcela_ret_pis"]);
                        }
                        else
                        {
                            parcela.op_parcela_ret_pis = 0;
                        }
                        if (DBNull.Value != leitor_3["op_parcela_ret_cofins"])
                        {
                            parcela.op_parcela_ret_cofins = Convert.ToDecimal(leitor_3["op_parcela_ret_cofins"]);
                        }
                        else
                        {
                            parcela.op_parcela_ret_cofins = 0;
                        }
                        if (DBNull.Value != leitor_3["op_parcela_ret_csll"])
                        {
                            parcela.op_parcela_ret_csll = Convert.ToDecimal(leitor_3["op_parcela_ret_csll"]);
                        }
                        else
                        {
                            parcela.op_parcela_ret_csll = 0;
                        }

                        parcela.op_parcela_obs = leitor_3["op_parcela_obs"].ToString();
                        parcela.controleEdit = "update";

                        parcelas.Add(parcela);
                    }

                    op.parcelas = parcelas;
                    leitor_3.Close();
                }
                
                Transacao.Commit();
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Operacao", "buscaOperacao", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return op;
        }

        //Alterar operação
        public string alterarOperacao(int usuario_id, int conta_id, Vm_operacao op)
        {
            string retorno = "Operação alterada com sucesso!";           

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;           

            try
            {
                MySqlDataReader leitor;

                MySqlCommand cmdp = conn.CreateCommand();
                cmdp.Connection = conn;
                cmdp.Transaction = Transacao;

                cmdp.CommandText = "SELECT * from op_parcelas_baixa as b where b.oppb_op_id = @op;";
                cmdp.Parameters.AddWithValue("@op", op.operacao.op_id);

                leitor = cmdp.ExecuteReader();

                if (leitor.HasRows)
                {
                    retorno = "Erro. Opercação não pode ser alterada devido a existencia de baixas nas parcelas. Exclua as baixas antes de efetuar alteração!";
                    leitor.Close();
                }
                else
                {
                    leitor.Close();
                    //Operação
                    comando.CommandText = "UPDATE operacao set op_data = @op_data, op_obs = @op_obs, op_previsao_entrega = @op_previsao_entrega, op_data_saida = @op_data_saida, op_categoria_id = @op_categoria_id, op_comNF = @op_comNF where operacao.op_conta_id = @conta_id and operacao.op_id = @op_id;";
                    comando.Parameters.AddWithValue("@op_id", op.operacao.op_id);
                    comando.Parameters.AddWithValue("@op_data", op.operacao.op_data);
                    comando.Parameters.AddWithValue("@conta_id", conta_id);
                    comando.Parameters.AddWithValue("@op_obs", op.operacao.op_obs);
                    comando.Parameters.AddWithValue("@op_previsao_entrega", op.operacao.op_previsao_entrega);
                    comando.Parameters.AddWithValue("@op_data_saida", op.operacao.op_data_saida);
                    comando.Parameters.AddWithValue("@op_categoria_id", op.operacao.op_categoria_id);
                    comando.Parameters.AddWithValue("@op_comNF", op.operacao.op_comNF);
                    comando.ExecuteNonQuery();                    

                    //participante
                    if(op.operacao.op_comParticipante)
                    {
                        if (op.participante.existe)
                        {
                            comando.CommandText = "UPDATE op_participante set op_paisesIBGE_codigo = @op_paisesIBGE_codigo, op_uf_ibge_codigo = @op_uf_ibge_codigo, op_part_participante_id = @op_part_participante_id, op_part_cep = @op_part_cep, op_part_nome = @op_part_nome, op_part_logradouro = @op_part_logradouro, op_part_numero = @op_part_numero, op_part_tipo = @op_part_tipo, op_part_cnpj_cpf = @op_part_cnpj_cpf, op_part_complemento = @op_part_complemento, op_part_bairro = @op_part_bairro, op_part_cidade = @op_part_cidade where op_participante.op_part_id = @op_part_id;";
                            comando.Parameters.AddWithValue("@op_part_nome", op.participante.op_part_nome);
                            comando.Parameters.AddWithValue("@op_part_tipo", op.participante.op_part_tipo);
                            comando.Parameters.AddWithValue("@op_part_cnpj_cpf", op.participante.op_part_cnpj_cpf);
                            comando.Parameters.AddWithValue("@op_part_cep", op.participante.op_part_cep);
                            comando.Parameters.AddWithValue("@op_part_cidade", op.participante.op_part_cidade);
                            comando.Parameters.AddWithValue("@op_part_bairro", op.participante.op_part_bairro);
                            comando.Parameters.AddWithValue("@op_part_logradouro", op.participante.op_part_logradouro);
                            comando.Parameters.AddWithValue("@op_part_numero", op.participante.op_part_numero);
                            comando.Parameters.AddWithValue("@op_part_complemento", op.participante.op_part_complemento);
                            comando.Parameters.AddWithValue("@op_paisesIBGE_codigo", op.participante.op_paisesIBGE_codigo);
                            comando.Parameters.AddWithValue("@op_uf_ibge_codigo", op.participante.op_uf_ibge_codigo);
                            comando.Parameters.AddWithValue("@op_part_participante_id", op.participante.op_part_participante_id);
                            comando.Parameters.AddWithValue("@op_part_id", op.participante.op_part_id);
                            comando.ExecuteNonQuery();
                        }
                        else
                        {
                            comando.CommandText = "insert into op_participante (op_part_nome, op_part_tipo, op_part_cnpj_cpf, op_part_cep, op_part_cidade, op_part_bairro, op_part_logradouro, op_part_numero, op_part_complemento, op_paisesIBGE_codigo, op_uf_ibge_codigo, op_id, op_part_participante_id) values (@op_part_nome, @op_part_tipo, @op_part_cnpj_cpf, @op_part_cep, @op_part_cidade, @op_part_bairro, @op_part_logradouro, @op_part_numero, @op_part_complemento, @op_paisesIBGE_codigo, @op_uf_ibge_codigo, @op_id, @op_part_participante_id);";
                            comando.Parameters.AddWithValue("@op_part_nome", op.participante.op_part_nome);
                            comando.Parameters.AddWithValue("@op_part_tipo", op.participante.op_part_tipo);
                            comando.Parameters.AddWithValue("@op_part_cnpj_cpf", op.participante.op_part_cnpj_cpf);
                            comando.Parameters.AddWithValue("@op_part_cep", op.participante.op_part_cep);
                            comando.Parameters.AddWithValue("@op_part_cidade", op.participante.op_part_cidade);
                            comando.Parameters.AddWithValue("@op_part_bairro", op.participante.op_part_bairro);
                            comando.Parameters.AddWithValue("@op_part_logradouro", op.participante.op_part_logradouro);
                            comando.Parameters.AddWithValue("@op_part_numero", op.participante.op_part_numero);
                            comando.Parameters.AddWithValue("@op_part_complemento", op.participante.op_part_complemento);
                            comando.Parameters.AddWithValue("@op_paisesIBGE_codigo", op.participante.op_paisesIBGE_codigo);
                            comando.Parameters.AddWithValue("@op_uf_ibge_codigo", op.participante.op_uf_ibge_codigo);
                            comando.Parameters.AddWithValue("@op_part_participante_id", op.participante.op_part_participante_id);
                            comando.Parameters.AddWithValue("@op_id", op.operacao.op_id);
                            comando.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (op.participante.existe)
                        {
                            comando.CommandText = "DELETE from op_participante where op_participante.op_part_id = @id_participante;";
                            comando.Parameters.AddWithValue("@id_participante", op.participante.op_part_id);
                            comando.ExecuteNonQuery();
                        }
                    }
                    

                    //Itens
                    if (op.itens.Count > 0)
                    {
                        for (int i = 0; i < op.itens.Count; i++)
                        {
                            MySqlCommand cmd = conn.CreateCommand();
                            cmd.Connection = conn;
                            cmd.Transaction = Transacao;

                            if (op.itens[i].controleEdit == "insert")
                            {
                                cmd.CommandText = "insert into op_itens (op_item_codigo, op_item_nome, op_item_unidade, op_item_preco, op_item_gtin_ean, op_item_gtin_ean_trib, op_item_obs, op_item_qtd, op_item_frete, op_item_seguros, op_item_desp_aces, op_item_desconto, op_item_op_id, op_item_vlr_ipi, op_item_vlr_icms_st, op_item_cod_fornecedor, op_item_produto_id, op_item_valor_total) values (@op_item_codigo, @op_item_nome, @op_item_unidade, @op_item_preco, @op_item_gtin_ean, @op_item_gtin_ean_trib, @op_item_obs, @op_item_qtd, @op_item_frete, @op_item_seguros, @op_item_desp_aces, @op_item_desconto, @op_item_op_id, @op_item_vlr_ipi, @op_item_vlr_icms_st, @op_item_cod_fornecedor, @op_item_produto_id, @op_item_valor_total);";
                                cmd.Parameters.AddWithValue("@op_item_op_id", op.operacao.op_id);
                                cmd.Parameters.AddWithValue("@op_item_codigo", op.itens[i].op_item_codigo);
                                cmd.Parameters.AddWithValue("@op_item_nome", op.itens[i].op_item_nome);
                                cmd.Parameters.AddWithValue("@op_item_unidade", op.itens[i].op_item_unidade);
                                cmd.Parameters.AddWithValue("@op_item_preco", op.itens[i].op_item_preco);
                                cmd.Parameters.AddWithValue("@op_item_gtin_ean", op.itens[i].op_item_gtin_ean);
                                cmd.Parameters.AddWithValue("@op_item_gtin_ean_trib", op.itens[i].op_item_gtin_ean_trib);
                                cmd.Parameters.AddWithValue("@op_item_obs", op.itens[i].op_item_obs);
                                cmd.Parameters.AddWithValue("@op_item_qtd", op.itens[i].op_item_qtd);
                                cmd.Parameters.AddWithValue("@op_item_frete", op.itens[i].op_item_frete);
                                cmd.Parameters.AddWithValue("@op_item_seguros", op.itens[i].op_item_seguros);
                                cmd.Parameters.AddWithValue("@op_item_desp_aces", op.itens[i].op_item_desp_aces);
                                cmd.Parameters.AddWithValue("@op_item_desconto", op.itens[i].op_item_desconto);
                                cmd.Parameters.AddWithValue("@op_item_vlr_ipi", op.itens[i].op_item_vlr_ipi);
                                cmd.Parameters.AddWithValue("@op_item_vlr_icms_st", op.itens[i].op_item_vlr_icms_st);
                                cmd.Parameters.AddWithValue("@op_item_cod_fornecedor", op.itens[i].op_item_cod_fornecedor);
                                cmd.Parameters.AddWithValue("@op_item_produto_id", op.itens[i].op_item_produto_id);
                                cmd.Parameters.AddWithValue("@op_item_valor_total", op.itens[i].op_item_valor_total);
                                cmd.ExecuteNonQuery();

                            }

                            if (op.itens[i].controleEdit == "update")
                            {
                                cmd.CommandText = "UPDATE op_itens set op_item_preco = @op_item_preco, op_item_qtd = @op_item_qtd, op_item_frete = @op_item_frete, op_item_seguros = @op_item_seguros, op_item_desp_aces = @op_item_desp_aces, op_item_desconto = @op_item_desconto, op_item_vlr_ipi = @op_item_vlr_ipi, op_item_vlr_icms_st = @op_item_vlr_icms_st, op_item_valor_total = @op_item_valor_total, op_item_produto_id = @op_item_produto_id, op_item_unidade = @op_item_unidade, op_item_nome = @op_item_nome, op_item_obs = @op_item_obs, op_item_codigo = @op_item_codigo, op_item_gtin_ean = @op_item_gtin_ean, op_item_gtin_ean_trib = @op_item_gtin_ean_trib, op_item_cod_fornecedor = @op_item_cod_fornecedor where op_itens.op_item_id = @op_item_id;";
                                cmd.Parameters.AddWithValue("@op_item_codigo", op.itens[i].op_item_codigo);
                                cmd.Parameters.AddWithValue("@op_item_nome", op.itens[i].op_item_nome);
                                cmd.Parameters.AddWithValue("@op_item_unidade", op.itens[i].op_item_unidade);
                                cmd.Parameters.AddWithValue("@op_item_preco", op.itens[i].op_item_preco);
                                cmd.Parameters.AddWithValue("@op_item_gtin_ean", op.itens[i].op_item_gtin_ean);
                                cmd.Parameters.AddWithValue("@op_item_gtin_ean_trib", op.itens[i].op_item_gtin_ean_trib);
                                cmd.Parameters.AddWithValue("@op_item_obs", op.itens[i].op_item_obs);
                                cmd.Parameters.AddWithValue("@op_item_qtd", op.itens[i].op_item_qtd);
                                cmd.Parameters.AddWithValue("@op_item_frete", op.itens[i].op_item_frete);
                                cmd.Parameters.AddWithValue("@op_item_seguros", op.itens[i].op_item_seguros);
                                cmd.Parameters.AddWithValue("@op_item_desp_aces", op.itens[i].op_item_desp_aces);
                                cmd.Parameters.AddWithValue("@op_item_desconto", op.itens[i].op_item_desconto);
                                cmd.Parameters.AddWithValue("@op_item_vlr_ipi", op.itens[i].op_item_vlr_ipi);
                                cmd.Parameters.AddWithValue("@op_item_vlr_icms_st", op.itens[i].op_item_vlr_icms_st);
                                cmd.Parameters.AddWithValue("@op_item_cod_fornecedor", op.itens[i].op_item_cod_fornecedor);
                                cmd.Parameters.AddWithValue("@op_item_produto_id", op.itens[i].op_item_produto_id);
                                cmd.Parameters.AddWithValue("@op_item_valor_total", op.itens[i].op_item_valor_total);
                                cmd.Parameters.AddWithValue("@op_item_id", op.itens[i].op_item_id);
                                cmd.ExecuteNonQuery();
                            }

                            if (op.itens[i].controleEdit == "delete")
                            {
                                cmd.CommandText = "DELETE from op_itens where op_itens.op_item_id = @op_item_id;";
                                cmd.Parameters.AddWithValue("@op_item_id", op.itens[i].op_item_id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    //totais
                    comando.CommandText = "UPDATE op_totais set op_totais_preco_itens = @op_totais_preco_itens, op_totais_frete = @op_totais_frete, op_totais_seguro = @op_totais_seguro, op_totais_desp_aces = @op_totais_desp_aces, op_totais_desconto = @op_totais_desconto, op_totais_qtd_itens = @op_totais_qtd_itens, op_totais_retencoes = @op_totais_retencoes, op_totais_total_op = @op_totais_total_op, op_totais_ipi = @op_totais_ipi, op_totais_icms_st = @op_totais_icms_st, op_totais_saldoLiquidacao = @op_totais_saldoLiquidacao, op_totais_itens = @op_totais_itens, op_totais_preco_servicos = @op_totais_preco_servicos where op_totais.op_totais_id = @op_totais_id;";
                    comando.Parameters.AddWithValue("@op_totais_id", op.totais.op_totais_id);
                    comando.Parameters.AddWithValue("@op_totais_preco_itens", op.totais.op_totais_preco_itens);
                    comando.Parameters.AddWithValue("@op_totais_frete", op.totais.op_totais_frete);
                    comando.Parameters.AddWithValue("@op_totais_seguro", op.totais.op_totais_seguro);
                    comando.Parameters.AddWithValue("@op_totais_desp_aces", op.totais.op_totais_desp_aces);
                    comando.Parameters.AddWithValue("@op_totais_desconto", op.totais.op_totais_desconto);
                    comando.Parameters.AddWithValue("@op_totais_itens", op.totais.op_totais_itens);
                    comando.Parameters.AddWithValue("@op_totais_qtd_itens", op.totais.op_totais_qtd_itens);
                    comando.Parameters.AddWithValue("@op_totais_retencoes", op.totais.op_totais_retencoes);
                    comando.Parameters.AddWithValue("@op_totais_total_op", op.totais.op_totais_total_op);
                    comando.Parameters.AddWithValue("@op_totais_ipi", op.totais.op_totais_ipi);
                    comando.Parameters.AddWithValue("@op_totais_icms_st", op.totais.op_totais_icms_st);
                    comando.Parameters.AddWithValue("@op_totais_saldoLiquidacao", op.totais.op_totais_saldoLiquidacao);
                    comando.Parameters.AddWithValue("@op_totais_preco_servicos", op.totais.op_totais_preco_servicos);
                    comando.ExecuteNonQuery();

                    //retenções
                    if (op.operacao.op_comRetencoes)
                    {
                        if (op.retencoes.existe)
                        {
                            comando.CommandText = "UPDATE op_retencoes set op_ret_pis = @op_ret_pis, op_ret_cofins = @op_ret_cofins, op_ret_csll = @op_ret_csll, op_ret_irrf = @op_ret_irrf, op_ret_inss = @op_ret_inss, op_ret_issqn = @op_ret_issqn, op_ret_id = @op_ret_id where op_retencoes.op_ret_id = @op_ret_id;";
                            comando.Parameters.AddWithValue("@op_ret_id", op.retencoes.op_ret_id);
                            comando.Parameters.AddWithValue("@op_ret_pis", op.retencoes.op_ret_pis);
                            comando.Parameters.AddWithValue("@op_ret_cofins", op.retencoes.op_ret_cofins);
                            comando.Parameters.AddWithValue("@op_ret_csll", op.retencoes.op_ret_csll);
                            comando.Parameters.AddWithValue("@op_ret_irrf", op.retencoes.op_ret_irrf);
                            comando.Parameters.AddWithValue("@op_ret_inss", op.retencoes.op_ret_inss);
                            comando.Parameters.AddWithValue("@op_ret_issqn", op.retencoes.op_ret_issqn);
                            comando.ExecuteNonQuery();
                        }
                        else
                        {
                            comando.CommandText = "insert into op_retencoes (op_ret_pis, op_ret_cofins, op_ret_csll, op_ret_irrf, op_ret_inss, op_ret_issqn, op_ret_op_id) values (@op_ret_pis, @op_ret_cofins, @op_ret_csll, @op_ret_irrf, @op_ret_inss, @op_ret_issqn, @op_ret_op_id);";
                            comando.Parameters.AddWithValue("@op_ret_op_id", op.operacao.op_id);
                            comando.Parameters.AddWithValue("@op_ret_pis", op.retencoes.op_ret_pis);
                            comando.Parameters.AddWithValue("@op_ret_cofins", op.retencoes.op_ret_cofins);
                            comando.Parameters.AddWithValue("@op_ret_csll", op.retencoes.op_ret_csll);
                            comando.Parameters.AddWithValue("@op_ret_irrf", op.retencoes.op_ret_irrf);
                            comando.Parameters.AddWithValue("@op_ret_inss", op.retencoes.op_ret_inss);
                            comando.Parameters.AddWithValue("@op_ret_issqn", op.retencoes.op_ret_issqn);
                            comando.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (op.retencoes.existe)
                        {
                            comando.CommandText = "DELETE from op_retencoes WHERE op_retencoes.op_ret_id = @id_retencoes;";
                            comando.Parameters.AddWithValue("@id_retencoes", op.retencoes.op_ret_id);
                            comando.ExecuteNonQuery();
                        }
                    }
                    

                    if (op.parcelas.Count > 0)
                    {
                        //Deletenado todas as parcelas da operação gravadas no banco

                        comando.CommandText = "DELETE from op_parcelas WHERE op_parcelas.op_parcela_op_id = @operacao_id;";
                        comando.Parameters.AddWithValue("@operacao_id", op.operacao.op_id);
                        comando.ExecuteNonQuery();

                        //Inserindo todas as novas parcelas no banco de dados
                        for (int i = 0; i < op.parcelas.Count; i++)
                        {
                            string contra_partida_tipo = "";
                            int contra_partidade_id = 0;

                            if (op.participante.op_part_participante_id != 0) //se operação possui um participante
                            {
                                contra_partida_tipo = "Participante";
                                contra_partidade_id = op.participante.op_part_participante_id;
                            }
                            else
                            {
                                contra_partida_tipo = "Categoria";
                                contra_partidade_id = op.operacao.op_categoria_id;
                            }

                            MySqlCommand cmd = conn.CreateCommand();
                            cmd.Connection = conn;
                            cmd.Transaction = Transacao;
                            

                            cmd.CommandText = "call pr_criaParcela (@op_parcela_dias, @op_parcela_vencimento, @op_parcela_fp_id, @op_parcela_op_id, @op_parcela_valor, @op_parcela_obs, @conta_id, @ccm_contra_partida_tipo, @ccm_contra_partida_id, @op_parcela_valor_bruto, @op_parcela_ret_inss, @op_parcela_ret_issqn, @op_parcela_ret_irrf, @op_parcela_ret_pis, @op_parcela_ret_cofins, @op_parcela_ret_csll);";
                            cmd.Parameters.AddWithValue("@conta_id", conta_id);
                            cmd.Parameters.AddWithValue("@op_parcela_dias", op.parcelas[i].op_parcela_dias);
                            cmd.Parameters.AddWithValue("@op_parcela_vencimento", op.parcelas[i].op_parcela_vencimento);
                            cmd.Parameters.AddWithValue("@op_parcela_fp_id", op.parcelas[i].op_parcela_fp_id);
                            cmd.Parameters.AddWithValue("@op_parcela_op_id", op.operacao.op_id);
                            cmd.Parameters.AddWithValue("@op_parcela_valor", op.parcelas[i].op_parcela_valor);
                            cmd.Parameters.AddWithValue("@op_parcela_obs", op.parcelas[i].op_parcela_obs);
                            cmd.Parameters.AddWithValue("@ccm_contra_partida_tipo", contra_partida_tipo);
                            cmd.Parameters.AddWithValue("@ccm_contra_partida_id", contra_partidade_id);
                            cmd.Parameters.AddWithValue("@op_parcela_valor_bruto", op.parcelas[i].op_parcela_valor_bruto);
                            cmd.Parameters.AddWithValue("@op_parcela_ret_inss", op.parcelas[i].op_parcela_ret_inss);
                            cmd.Parameters.AddWithValue("@op_parcela_ret_issqn", op.parcelas[i].op_parcela_ret_issqn);
                            cmd.Parameters.AddWithValue("@op_parcela_ret_irrf", op.parcelas[i].op_parcela_ret_irrf);
                            cmd.Parameters.AddWithValue("@op_parcela_ret_pis", op.parcelas[i].op_parcela_ret_pis);
                            cmd.Parameters.AddWithValue("@op_parcela_ret_cofins", op.parcelas[i].op_parcela_ret_cofins);
                            cmd.Parameters.AddWithValue("@op_parcela_ret_csll", op.parcelas[i].op_parcela_ret_csll);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //transportador
                    if (op.operacao.op_comTransportador)
                    {
                        if (op.transportador.existe)
                        {
                            comando.CommandText = "UPDATE op_transportador set op_transportador_volume_qtd = @op_transportador_volume_qtd, op_transportador_volume_peso_bruto = @op_transportador_volume_peso_bruto, op_transportador_participante_id = @op_transportador_participante_id, op_transportador_nome = @op_transportador_nome, op_transportador_modalidade_frete = @op_transportador_modalidade_frete, op_transportador_cnpj_cpf = @op_transportador_cnpj_cpf where op_transportador.op_transportador_id = @op_transportador_id;";
                            comando.Parameters.AddWithValue("@op_transportador_nome", op.transportador.op_transportador_nome);
                            comando.Parameters.AddWithValue("@op_transportador_cnpj_cpf", op.transportador.op_transportador_cnpj_cpf);
                            comando.Parameters.AddWithValue("@op_transportador_modalidade_frete", op.transportador.op_transportador_modalidade_frete);
                            comando.Parameters.AddWithValue("@op_transportador_volume_qtd", op.transportador.op_transportador_volume_qtd);
                            comando.Parameters.AddWithValue("@op_transportador_volume_peso_bruto", op.transportador.op_transportador_volume_peso_bruto);
                            comando.Parameters.AddWithValue("@op_transportador_id", op.transportador.op_transportador_id);
                            comando.Parameters.AddWithValue("@op_transportador_participante_id", op.transportador.op_transportador_participante_id);
                            comando.ExecuteNonQuery();
                        }
                        else
                        {
                            comando.CommandText = "INSERT into op_transportador (op_transportador_nome, op_transportador_cnpj_cpf, op_transportador_modalidade_frete, op_transportador_volume_qtd, op_transportador_volume_peso_bruto, op_transportador_op_id, op_transportador_participante_id) VALUES (@op_transportador_nome, @op_transportador_cnpj_cpf, @op_transportador_modalidade_frete, @op_transportador_volume_qtd, @op_transportador_volume_peso_bruto, @op_transportador_op_id, @op_transportador_participante_id);";
                            comando.Parameters.AddWithValue("@conta_id", conta_id);
                            comando.Parameters.AddWithValue("@op_transportador_nome", op.transportador.op_transportador_nome);
                            comando.Parameters.AddWithValue("@op_transportador_cnpj_cpf", op.transportador.op_transportador_cnpj_cpf);
                            comando.Parameters.AddWithValue("@op_transportador_modalidade_frete", op.transportador.op_transportador_modalidade_frete);
                            comando.Parameters.AddWithValue("@op_transportador_volume_qtd", op.transportador.op_transportador_volume_qtd);
                            comando.Parameters.AddWithValue("@op_transportador_volume_peso_bruto", op.transportador.op_transportador_volume_peso_bruto);
                            comando.Parameters.AddWithValue("@op_transportador_op_id", op.operacao.op_id);
                            comando.Parameters.AddWithValue("@op_transportador_participante_id", op.transportador.op_transportador_participante_id);
                            comando.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (op.transportador.existe)
                        {
                            comando.CommandText = "DELETE from op_transportador WHERE op_transportador.op_transportador_id = @id_transportador;";
                            comando.Parameters.AddWithValue("@id_transportador", op.transportador.op_transportador_id);
                            comando.ExecuteNonQuery();
                        }
                    }

                    //nota fiscal
                    if (op.operacao.op_comNF > 0)
                    {
                        if (op.nf.existe)
                        {
                            comando.CommandText = "UPDATE op_nf set op_nf_chave = @op_nf_chave, op_nf_data_emissao = @op_nf_data_emissao, op_nf_data_entrada_saida = @op_nf_data_entrada_saida, op_nf_serie = @op_nf_serie, op_nf_numero = @op_nf_numero where op_nf.op_nf_id = @op_nf_id;";
                            comando.Parameters.AddWithValue("@op_nf_id", op.nf.op_nf_id);
                            comando.Parameters.AddWithValue("@op_nf_chave", op.nf.op_nf_chave);
                            comando.Parameters.AddWithValue("@op_nf_data_emissao", op.nf.op_nf_data_emissao);
                            comando.Parameters.AddWithValue("@op_nf_data_entrada_saida", op.nf.op_nf_data_entrada_saida);
                            comando.Parameters.AddWithValue("@op_nf_serie", op.nf.op_nf_serie);
                            comando.Parameters.AddWithValue("@op_nf_numero", op.nf.op_nf_numero);
                            comando.ExecuteNonQuery();
                        }
                        else
                        {
                            comando.CommandText = "insert into op_nf (op_nf_op_id, op_nf_chave, op_nf_data_emissao, op_nf_data_entrada_saida, op_nf_serie, op_nf_numero) values (@op_nf_op_id, @op_nf_chave, @op_nf_data_emissao, @op_nf_data_entrada_saida, @op_nf_serie, @op_nf_numero);";
                            comando.Parameters.AddWithValue("@op_nf_op_id", op.operacao.op_id);
                            comando.Parameters.AddWithValue("@op_nf_chave", op.nf.op_nf_chave);
                            comando.Parameters.AddWithValue("@op_nf_data_emissao", op.nf.op_nf_data_emissao);
                            comando.Parameters.AddWithValue("@op_nf_data_entrada_saida", op.nf.op_nf_data_entrada_saida);
                            comando.Parameters.AddWithValue("@op_nf_serie", op.nf.op_nf_serie);
                            comando.Parameters.AddWithValue("@op_nf_numero", op.nf.op_nf_numero);
                            comando.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (op.nf.existe)
                        {
                            comando.CommandText = "DELETE from op_nf WHERE op_nf.op_nf_id = @op_nf_id;";
                            comando.Parameters.AddWithValue("@op_nf_id", op.nf.op_nf_id);
                            comando.ExecuteNonQuery();
                        }
                    }

                    if (op.operacao.op_tipo == "ServicoPrestado" || op.operacao.op_tipo == "ServicoTomado")
                    {
                        comando.CommandText = "UPDATE op_servico set p_servico_equipamento = @p_servico_equipamento, op_servico_nSerie = @op_servico_nSerie, op_servico_problema = @op_servico_problema, op_servico_obsReceb = @op_servico_obsReceb, op_servico_servico_executado = @op_servico_servico_executado, op_servico_valor = @op_servico_valor, op_servico_status = @op_servico_status WHERE op_servico.op_servico_id = @op_servico_id;";
                        comando.Parameters.AddWithValue("@op_servico_id", op.servico.op_servico_id);
                        comando.Parameters.AddWithValue("@op_servico_equipamento", op.servico.op_servico_equipamento);
                        comando.Parameters.AddWithValue("@op_servico_nSerie", op.servico.op_servico_nSerie);
                        comando.Parameters.AddWithValue("@op_servico_problema", op.servico.op_servico_problema);
                        comando.Parameters.AddWithValue("@op_servico_obsReceb", op.servico.op_servico_obsReceb);
                        comando.Parameters.AddWithValue("@op_servico_servico_executado", op.servico.op_servico_servico_executado);
                        comando.Parameters.AddWithValue("@op_servico_valor", op.servico.op_servico_valor);
                        comando.Parameters.AddWithValue("@op_servico_status", op.servico.op_servico_status);
                        comando.ExecuteNonQuery();
                    }


                    string msg = "Alteração de operação ID: " + op.operacao.op_id + " Alterada com sucesso";
                    log.log("Operacao", "cadastraOperacao", "Sucesso", msg, conta_id, usuario_id);

                }
                Transacao.Commit();
            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar a operação. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Operacao", "alterarOperacao", "Erro", msg, conta_id, usuario_id);
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
