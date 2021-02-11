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

                comando.CommandText = "SELECT op.op_tipo, COALESCE(p.op_parcela_numero,0) as 'numero_parcela', COALESCE(par.op_part_cnpj_cpf, 'Sem Participante') as 'participante', COALESCE(nf.op_nf_numero, -1) as 'nota', COALESCE(nf.op_nf_data_emissao, 'Não Informada') as 'data_nota_data_emissao', COALESCE(nf.op_nf_data_entrada_saida, 'Não Informada') as 'data_nota_data_saida_entrada', COALESCE(par.op_uf_ibge_codigo, 'Sem Participante') as 'estado_nf', COALESCE(uf_ibge.uf_ibge_sigla, 'Sem Participante') as 'estado_nf_sigla', COALESCE(nf.op_nf_serie,'Não Informada') as 'serie_nf', COALESCE(cc.ccorrente_masc_contabil, 'Conta corrente sem conta contábil') as 'conta_contabil_conta_corrente', b.oppb_obs, round(b.oppb_valor,2) as 'oppb_valor', b.oppb_juros, b.oppb_multa, b.oppb_desconto, p.op_parcela_vencimento_alterado as 'data_vencimento', b.oppb_data as 'data_pagamento', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_pis)),2) as 'ret_pis', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_cofins)),2) as 'ret_cofins', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_csll)),2) as 'ret_csll', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_irrf)),2) as 'ret_ir', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_inss)),2) as 'ret_inss', round(((b.oppb_valor / p.op_parcela_valor * p.op_parcela_ret_issqn)),2) as 'ret_iss' from op_parcelas_baixa as b LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN operacao as op on op.op_id = b.oppb_op_id LEFT JOIN op_nf as nf on nf.op_nf_op_id = op.op_id LEFT JOIN op_participante as par on par.op_id = op.op_id LEFT JOIN conta_corrente as cc on cc.ccorrente_id = b.oppb_conta_corrente LEFT JOIN uf_ibge on uf_ibge.uf_ibge_codigo = par.op_uf_ibge_codigo WHERE op.op_conta_id = @cliente_id and b.oppb_data BETWEEN @data_inicial and @data_final;";
                comando.Parameters.AddWithValue("@cliente_id", fd.cliente_id);
                comando.Parameters.AddWithValue("@data_inicial", fd.data_inicial);
                comando.Parameters.AddWithValue("@data_final", fd.data_final);

                reader = comando.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ImportacaoDuplicatas id = new ImportacaoDuplicatas();
                        string tipo = reader["op_tipo"].ToString();
                        //Definição da natureza da operação (padrão)
                        string natureza = "Sem Definição";
                        if(tipo == "ServicoPrestado")
                        {
                            natureza = "5933000";
                        }
                        int numero_p = Convert.ToInt32(reader["numero_parcela"]);
                        if (numero_p == 0)
                        {
                            numero_p = 1;
                        }
                        id.id_numero_parcela = numero_p;
                        id.id_tipo_movimento = tipo_movimento(reader["op_tipo"].ToString());
                        id.id_cnpj_cpf = reader["participante"].ToString();
                        id.id_numero_nf_inicial = Convert.ToInt32(reader["nota"]);
                        id.id_numero_nf_final = Convert.ToInt32(reader["nota"]);
                        if(tipo == "Compra")
                        {
                            if (DBNull.Value != reader["data_nota_data_saida_entrada"])
                            {
                                id.id_data_nf = Convert.ToDateTime(reader["data_nota_data_saida_entrada"]);
                            }
                            else
                            {
                                id.id_data_nf = new DateTime();
                            }
                        }
                        else
                        {
                            if (DBNull.Value != reader["data_nota_data_emissao"])
                            {
                                id.id_data_nf = Convert.ToDateTime(reader["data_nota_data_emissao"]);
                            }
                            else
                            {
                                id.id_data_nf = new DateTime();
                            }
                        }
                        id.id_estado = reader["estado_nf_sigla"].ToString();
                        id.id_serie_nf = reader["serie_nf"].ToString();
                        id.id_especie_nf = "";
                        id.id_modelo_nf = "";
                        id.id_natureza_operacao = natureza;
                        if(tipo == "ServicoPrestado" || tipo == "Venda")
                        {
                            id.id_conta_debito = reader["conta_contabil_conta_corrente"].ToString();
                            id.id_conta_credito = "";
                        }
                        else
                        {
                            id.id_conta_debito = "";
                            id.id_conta_credito = reader["conta_contabil_conta_corrente"].ToString();
                        }                        

                        id.id_historico_operacao = reader["oppb_obs"].ToString();

                        if (DBNull.Value != reader["oppb_valor"])
                        {
                            id.id_valor_duplicata = Convert.ToDecimal(reader["oppb_valor"]);
                        }
                        else
                        {
                            id.id_valor_duplicata = 0;
                        }
                        if (DBNull.Value != reader["data_vencimento"])
                        {
                            id.id_data_vencimento = Convert.ToDateTime(reader["data_vencimento"]);
                        }
                        else
                        {
                            id.id_data_vencimento = new DateTime();
                        }
                        if (DBNull.Value != reader["data_pagamento"])
                        {
                            id.id_data_pagamento = Convert.ToDateTime(reader["data_pagamento"]);
                        }
                        else
                        {
                            id.id_data_pagamento = new DateTime();
                        }
                        id.id_numero_cheque = 0;
                        id.id_numero_duplicata = "";
                        id.id_numero_promissoria = "";
                        id.id_numero_recibo = "";

                        if (DBNull.Value != reader["ret_pis"])
                        {
                            id.id_valor_ret_pis = Convert.ToDecimal(reader["ret_pis"]);
                        }
                        else
                        {
                            id.id_valor_ret_pis = 0;
                        }

                        if (DBNull.Value != reader["ret_cofins"])
                        {
                            id.id_valor_ret_cofins = Convert.ToDecimal(reader["ret_cofins"]);
                        }
                        else
                        {
                            id.id_valor_ret_cofins = 0;
                        }

                        if (DBNull.Value != reader["ret_csll"])
                        {
                            id.id_valor_ret_csll = Convert.ToDecimal(reader["ret_csll"]);
                        }
                        else
                        {
                            id.id_valor_ret_csll = 0;
                        }

                        if (DBNull.Value != reader["ret_ir"])
                        {
                            id.id_valor_ret_ir = Convert.ToDecimal(reader["ret_ir"]);
                        }
                        else
                        {
                            id.id_valor_ret_ir = 0;
                        }

                        if (DBNull.Value != reader["ret_inss"])
                        {
                            id.id_valor_ret_inss = Convert.ToDecimal(reader["ret_inss"]);
                        }
                        else
                        {
                            id.id_valor_ret_inss = 0;
                        }

                        if (DBNull.Value != reader["ret_iss"])
                        {
                            id.id_valor_ret_iss = Convert.ToDecimal(reader["ret_iss"]);
                        }
                        else
                        {
                            id.id_valor_ret_iss = 0;
                        }
                        id.id_valor_ret_funrural = 0;
                        id.id_valor_ret_sest_senat = 0;
                        id.id_tipo_baixa = "P";                        
                        id.id_ordem_baixa = 0;

                        //Somando retenções
                        ids.retencao_pis_total += id.id_valor_ret_pis;
                        ids.retencao_cofins_total += id.id_valor_ret_cofins;
                        ids.retencao_csll_total += id.id_valor_ret_csll;
                        ids.retencao_irrf_total += id.id_valor_ret_ir;
                        ids.retencao_inss_total += id.id_valor_ret_inss;
                        ids.retencao_iss_total += id.id_valor_ret_iss;

                        ids.duplicatas.Add(id);

                        //Se hoouver juros , multa, descontos
                        Decimal juros = 0;
                        Decimal multa = 0;
                        Decimal desconto = 0;
                        if (DBNull.Value != reader["oppb_juros"])
                        {
                            juros = Convert.ToDecimal(reader["oppb_juros"]);
                        }
                        else
                        {
                            juros = 0;
                        }

                        if (DBNull.Value != reader["oppb_multa"])
                        {
                            multa = Convert.ToDecimal(reader["oppb_multa"]);
                        }
                        else
                        {
                            multa = 0;
                        }

                        if (DBNull.Value != reader["oppb_desconto"])
                        {
                            desconto = Convert.ToDecimal(reader["oppb_desconto"]);
                        }
                        else
                        {
                            desconto = 0;
                        }

                        if(juros > 0)
                        {
                            ImportacaoDuplicatas juro = new ImportacaoDuplicatas();
                            juro.id_numero_parcela = id.id_numero_parcela;
                            juro.id_tipo_movimento = id.id_tipo_movimento;
                            juro.id_cnpj_cpf = id.id_cnpj_cpf;
                            juro.id_numero_nf_inicial = id.id_numero_nf_inicial;
                            juro.id_numero_nf_final = id.id_numero_nf_final;
                            juro.id_data_nf = id.id_data_nf;
                            juro.id_estado = id.id_estado;
                            juro.id_serie_nf = id.id_serie_nf;
                            juro.id_especie_nf = id.id_especie_nf;
                            juro.id_modelo_nf = id.id_modelo_nf;
                            juro.id_natureza_operacao = id.id_natureza_operacao;
                            juro.id_conta_debito = id.id_conta_debito;
                            juro.id_conta_credito = id.id_conta_credito;
                            juro.id_historico_operacao = "Juros ref.: " + id.id_historico_operacao;
                            juro.id_valor_duplicata = juros;
                            juro.id_data_vencimento = id.id_data_vencimento;
                            juro.id_data_pagamento = id.id_data_pagamento;
                            juro.id_numero_cheque = id.id_numero_cheque;
                            juro.id_numero_duplicata = id.id_numero_duplicata;
                            juro.id_numero_promissoria = id.id_numero_promissoria;
                            juro.id_numero_recibo = id.id_numero_recibo;
                            juro.id_valor_ret_pis = 0;
                            juro.id_valor_ret_cofins = 0;
                            juro.id_valor_ret_csll = 0;
                            juro.id_valor_ret_ir = 0;
                            juro.id_valor_ret_inss = 0;
                            juro.id_valor_ret_iss = 0;
                            juro.id_valor_ret_funrural = 0;
                            juro.id_valor_ret_sest_senat = 0;
                            juro.id_tipo_baixa = "J";
                            juro.id_ordem_baixa = 0;

                            ids.duplicatas.Add(juro);

                        }

                        if (multa > 0)
                        {
                            ImportacaoDuplicatas multas = new ImportacaoDuplicatas();
                            multas.id_numero_parcela = id.id_numero_parcela;
                            multas.id_tipo_movimento = id.id_tipo_movimento;
                            multas.id_cnpj_cpf = id.id_cnpj_cpf;
                            multas.id_numero_nf_inicial = id.id_numero_nf_inicial;
                            multas.id_numero_nf_final = id.id_numero_nf_final;
                            multas.id_data_nf = id.id_data_nf;
                            multas.id_estado = id.id_estado;
                            multas.id_serie_nf = id.id_serie_nf;
                            multas.id_especie_nf = id.id_especie_nf;
                            multas.id_modelo_nf = id.id_modelo_nf;
                            multas.id_natureza_operacao = id.id_natureza_operacao;
                            multas.id_conta_debito = id.id_conta_debito;
                            multas.id_conta_credito = id.id_conta_credito;
                            multas.id_historico_operacao = "multa ref.: " + id.id_historico_operacao;
                            multas.id_valor_duplicata = multa;
                            multas.id_data_vencimento = id.id_data_vencimento;
                            multas.id_data_pagamento = id.id_data_pagamento;
                            multas.id_numero_cheque = id.id_numero_cheque;
                            multas.id_numero_duplicata = id.id_numero_duplicata;
                            multas.id_numero_promissoria = id.id_numero_promissoria;
                            multas.id_numero_recibo = id.id_numero_recibo;
                            multas.id_valor_ret_pis = 0;
                            multas.id_valor_ret_cofins = 0;
                            multas.id_valor_ret_csll = 0;
                            multas.id_valor_ret_ir = 0;
                            multas.id_valor_ret_inss = 0;
                            multas.id_valor_ret_iss = 0;
                            multas.id_valor_ret_funrural = 0;
                            multas.id_valor_ret_sest_senat = 0;
                            multas.id_tipo_baixa = "M";
                            multas.id_ordem_baixa = 0;

                            ids.duplicatas.Add(multas);
                        }

                        if (desconto > 0)
                        {
                            ImportacaoDuplicatas descontos = new ImportacaoDuplicatas();
                            descontos.id_numero_parcela = id.id_numero_parcela;
                            descontos.id_tipo_movimento = id.id_tipo_movimento;
                            descontos.id_cnpj_cpf = id.id_cnpj_cpf;
                            descontos.id_numero_nf_inicial = id.id_numero_nf_inicial;
                            descontos.id_numero_nf_final = id.id_numero_nf_final;
                            descontos.id_data_nf = id.id_data_nf;
                            descontos.id_estado = id.id_estado;
                            descontos.id_serie_nf = id.id_serie_nf;
                            descontos.id_especie_nf = id.id_especie_nf;
                            descontos.id_modelo_nf = id.id_modelo_nf;
                            descontos.id_natureza_operacao = id.id_natureza_operacao;
                            descontos.id_conta_debito = id.id_conta_debito;
                            descontos.id_conta_credito = id.id_conta_credito;
                            descontos.id_historico_operacao = "desconto ref.: " + id.id_historico_operacao;
                            descontos.id_valor_duplicata = desconto;
                            descontos.id_data_vencimento = id.id_data_vencimento;
                            descontos.id_data_pagamento = id.id_data_pagamento;
                            descontos.id_numero_cheque = id.id_numero_cheque;
                            descontos.id_numero_duplicata = id.id_numero_duplicata;
                            descontos.id_numero_promissoria = id.id_numero_promissoria;
                            descontos.id_numero_recibo = id.id_numero_recibo;
                            descontos.id_valor_ret_pis = 0;
                            descontos.id_valor_ret_cofins = 0;
                            descontos.id_valor_ret_csll = 0;
                            descontos.id_valor_ret_ir = 0;
                            descontos.id_valor_ret_inss = 0;
                            descontos.id_valor_ret_iss = 0;
                            descontos.id_valor_ret_funrural = 0;
                            descontos.id_valor_ret_sest_senat = 0;
                            descontos.id_tipo_baixa = "D";
                            descontos.id_ordem_baixa = 0;

                            ids.duplicatas.Add(descontos);
                        }                        
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 250);
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

        public string tipo_movimento(string tipo)
        {
            string tm = "";
            if(tipo == "ServicoPrestado" || tipo == "ServicoTomado")
            {
                tm = "PR";
            }
            else
            {
                if (tipo == "Compra")
                {
                    tm = "PE";
                }
                else
                {
                    if (tipo == "Venda")
                    {
                        tm = "PS";
                    }
                    else
                    {
                        tm = "Erro, tipo inválido";
                    }
                }
            }

            return tm;
        }        
    }

    public class SCI_IDs
    {
        public Filtros_Duplicatas fd { get; set; }
        public List<ImportacaoDuplicatas> duplicatas { get; set; }
        public string status { get; set; }
        public int qunatidade_erros { get; set; }
        public Decimal retencao_pis_total { get; set; }
        public Decimal retencao_cofins_total { get; set; }
        public Decimal retencao_csll_total { get; set; }
        public Decimal retencao_irrf_total { get; set; }
        public Decimal retencao_inss_total { get; set; }
        public Decimal retencao_iss_total { get; set; }
    }

    public class Filtros_Duplicatas
    {
        public int cliente_id { get; set; }
        public string cliente_nome { get; set; }
        public DateTime data_inicial { get; set; }
        public DateTime data_final { get; set; }
    }
}
