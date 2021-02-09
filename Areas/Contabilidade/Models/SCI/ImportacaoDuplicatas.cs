using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models.SCI
{
    public class ImportacaoDuplicatas
    {
        public int id_numero_parcela { get; set; }
        public string id_tipo_movimento { get; set; }
        public string id_cnpj_cpf { get; set; }
        public int id_inscricao_estadual { get; set; }
        public int id_numero_nf_inicial { get; set; }
        public int id_numero_nf_final { get; set; }
        public DateTime id_data_nf { get; set; } //Saídas: Data de Emissão ==> Entradas: Data de Entrada
        public string id_estado { get; set; } //UF do participante
        public string id_serie_nf { get; set; }
        public string id_especie_nf { get; set; }
        public string id_modelo_nf { get; set; }
        public string id_natureza_operacao { get; set; } //CFOP com sufixo
        public string id_conta_debito { get; set; } //Somente para tipo pagamento
        public string id_conta_credito { get; set; } //Somente para tipo pagamento
        public string id_historico_operacao { get; set; } //Somente para tipo pagamento
        public Decimal id_valor_duplicata { get; set; }
        public DateTime id_data_vencimento { get; set; } //Somente para tipo E/S/R
        public DateTime id_data_pagamento { get; set; } //Somente para pagamento
        public int id_numero_cheque { get; set; }
        public string id_numero_duplicata { get; set; } //Somente para tipo E/S/R
        public string id_numero_promissoria { get; set; } //Somente para tipo E/S/R
        public string id_numero_recibo { get; set; } //Somente para tipo E/S/R
        public Decimal id_valor_ret_pis { get; set; }
        public Decimal id_valor_ret_cofins { get; set; }
        public Decimal id_valor_ret_csll { get; set; }
        public Decimal id_valor_ret_ir { get; set; }
        public Decimal id_valor_ret_inss { get; set; }
        public Decimal id_valor_ret_iss { get; set; }
        public Decimal id_valor_ret_funrural { get; set; }
        public Decimal id_valor_ret_sest_senat { get; set; }
        public string id_tipo_baixa { get; set; }
        public int id_ordem_baixa { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ImportacaoDuplicatas()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();


        //Gerar duplicatas
        public SCI_IDs create(Filtros_Duplicatas fd, int conta_id, int usuario_id)
        {
            SCI_IDs ids = new SCI_IDs();
            ids.duplicatas = new List<ImportacaoDuplicatas>();
            ids.fd = new Filtros_Duplicatas();
            ids.fd = fd; //Devolver os filtros;
            
            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                MySqlDataReader reader;

                comando.CommandText = "SELECT op.op_tipo, COALESCE(par.op_part_cnpj_cpf, 'Sem Participante') as 'participante', COALESCE(nf.op_nf_numero, 'Não Informada') as 'nota', COALESCE(nf.op_nf_data_emissao, 'Não Informada') as 'data_nota_data_emissao', COALESCE(nf.op_nf_data_entrada_saida, 'Não Informada') as 'data_nota_data_saida_entrada', COALESCE(par.op_uf_ibge_codigo, 'Sem Participante') as 'estado_nf', COALESCE(nf.op_nf_serie,'Não Informada') as 'serie_nf', COALESCE(cc.ccorrente_masc_contabil, 'Conta corrente sem conta contábil') as 'conta_contabil_conta_correte', b.oppb_obs, b.oppb_valor, b.oppb_juros, b.oppb_multa, b.oppb_desconto, p.op_parcela_vencimento_alterado as 'data_vencimento', b.oppb_data as 'data_pagamento', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_pis)),2) as 'ret_pis', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_cofins)),2) as 'ret_cofins', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_csll)),2) as 'ret_csll', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_irrf)),2) as 'ret_ir', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_inss)),2) as 'ret_inss', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_issqn)),2) as 'ret_iss' from op_parcelas_baixa as b LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN operacao as op on op.op_id = b.oppb_op_id LEFT JOIN op_nf as nf on nf.op_nf_op_id = op.op_id LEFT JOIN op_participante as par on par.op_id = op.op_id LEFT JOIN conta_corrente as cc on cc.ccorrente_id = b.oppb_conta_corrente WHERE op.op_conta_id = @cliente_id and b.oppb_data BETWEEN @data_inicial and @data_final;";
                comando.Parameters.AddWithValue("@cliente_id", fd.cliente_id);
                comando.Parameters.AddWithValue("@data_inicial", fd.data_inicial);
                comando.Parameters.AddWithValue("@data_final", fd.data_final);

                reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ImportacaoDuplicatas id = new ImportacaoDuplicatas();




                        ids.duplicatas.Add(id);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 250);
                log.log("Memorando", "listMemorando", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return ids;
        }





    }

    public class SCI_IDs
    {
        public Filtros_Duplicatas fd { get; set; }
        public List<ImportacaoDuplicatas> duplicatas { get; set; }
        public string status { get; set; }
        public int qunatidade_erros { get; set; }
    }

    public class Filtros_Duplicatas
    {
        public int cliente_id { get; set; }
        public DateTime data_inicial { get; set; }
        public DateTime data_final { get; set; }
    }
}
