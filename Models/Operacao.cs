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
            string retorno = "Sucesso";

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
                comando.CommandText = "call pr_operacao(@op_tipo, @op_data, @op_conta_id, @op_obs, @op_previsao_entrega, @op_data_saida, @op_categoria_id);";                
                comando.Parameters.AddWithValue("@op_tipo", op.operacao.op_tipo);
                comando.Parameters.AddWithValue("@op_data", op.operacao.op_data);
                comando.Parameters.AddWithValue("@op_conta_id", conta_id);
                comando.Parameters.AddWithValue("@op_obs", op.operacao.op_obs);                
                comando.Parameters.AddWithValue("@op_previsao_entrega", op.operacao.op_previsao_entrega);
                comando.Parameters.AddWithValue("@op_data_saida", op.operacao.op_data_saida);
                comando.Parameters.AddWithValue("@op_categoria_id", op.operacao.op_categoria_id);
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
                comando.CommandText = "insert into op_totais (op_totais_preco_itens, op_totais_frete, op_totais_seguro, op_totais_desp_aces, op_totais_desconto, op_totais_itens, op_totais_qtd_itens, op_totais_op_id, op_totais_retencoes, op_totais_total_op, op_totais_ipi, op_totais_icms_st, op_totais_saldoLiquidacao) values (@op_totais_preco_itens, @op_totais_frete, @op_totais_seguro, @op_totais_desp_aces, @op_totais_desconto, @op_totais_itens, @op_totais_qtd_itens, @op_totais_op_id, @op_totais_retencoes, @op_totais_total_op, @op_totais_ipi, @op_totais_icms_st, @op_totais_saldoLiquidacao);";
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
                comando.ExecuteNonQuery();

                //retenções
                comando.CommandText = "insert into op_retencoes (op_ret_pis, op_ret_cofins, op_ret_csll, op_ret_irrf, op_ret_inss, op_ret_issqn, op_ret_op_id) values (@op_ret_pis, @op_ret_cofins, @op_ret_csll, @op_ret_irrf, @op_ret_inss, @op_ret_issqn, @op_ret_op_id);";
                comando.Parameters.AddWithValue("@op_ret_op_id", id);
                comando.Parameters.AddWithValue("@op_ret_pis", op.retencoes.op_ret_pis);
                comando.Parameters.AddWithValue("@op_ret_cofins", op.retencoes.op_ret_cofins);
                comando.Parameters.AddWithValue("@op_ret_csll", op.retencoes.op_ret_csll);
                comando.Parameters.AddWithValue("@op_ret_irrf", op.retencoes.op_ret_irrf);
                comando.Parameters.AddWithValue("@op_ret_inss", op.retencoes.op_ret_inss);
                comando.Parameters.AddWithValue("@op_ret_issqn", op.retencoes.op_ret_issqn);
                comando.ExecuteNonQuery();

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


                        cmd.CommandText = "call pr_criaParcela (@op_parcela_dias, @op_parcela_vencimento, @op_parcela_fp_id, @op_parcela_op_id, @op_parcela_valor, @op_parcela_obs, @conta_id, @ccm_contra_partida_tipo, @ccm_contra_partida_id);";
                        cmd.Parameters.AddWithValue("@conta_id", conta_id);
                        cmd.Parameters.AddWithValue("@op_parcela_dias", op.parcelas[i].op_parcela_dias);
                        cmd.Parameters.AddWithValue("@op_parcela_vencimento", op.parcelas[i].op_parcela_vencimento);
                        cmd.Parameters.AddWithValue("@op_parcela_fp_id", op.parcelas[i].op_parcela_fp_id);
                        cmd.Parameters.AddWithValue("@op_parcela_op_id", id);
                        cmd.Parameters.AddWithValue("@op_parcela_valor", op.parcelas[i].op_parcela_valor);
                        cmd.Parameters.AddWithValue("@op_parcela_obs", op.parcelas[i].op_parcela_obs);
                        cmd.Parameters.AddWithValue("@ccm_contra_partida_tipo", contra_partida_tipo);
                        cmd.Parameters.AddWithValue("@ccm_contra_partida_id", contra_partidade_id);
                        cmd.ExecuteNonQuery();
                    }
                }

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




    }
}
