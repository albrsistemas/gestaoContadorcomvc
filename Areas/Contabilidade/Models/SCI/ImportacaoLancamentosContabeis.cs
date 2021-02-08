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
    public class ImportacaoLancamentosContabeis
    {
        public int ilc_sequencia { get; set; }
        public DateTime ilc_data_lancamento { get; set; }
        public string ilc_conta_debito { get; set; }
        public string ilc_conta_credito { get; set; }
        public Decimal ilc_valor_lancamento { get; set; }
        public string ilc_codigo_historico { get; set; }
        public string ilc_complemento_historico { get; set; }
        public string ilc_numero_documento { get; set; }
        public string ilc_lote_lancamento { get; set; }
        public string ilc_cnpj_cpf_debito { get; set; }
        public string ilc_cnpj_cpf_credito { get; set; }
        public string ilc_contabilizacao_ifrs { get; set; }
        public string ilc_transacao_sped { get; set; }
        public string ilc_indicador_conciliacao { get; set; }
        public string ilc_indicador_pendencia_concialiacao { get; set; }
        public string ilc_obs_conciliacao_debito { get; set; }
        public string ilc_obs_conciliacao_credito { get; set; }
        //atributos de controle
        public string status { get; set; }
        public string mensagem { get; set; }
        public string origem { get; set; }
        public string tipo { get; set; }


        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ImportacaoLancamentosContabeis()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Gerar arquivo ilc
        public Ilcs create(int usuario_id, int conta_id, int cliente_id, DateTime data_inicial, DateTime data_final, bool gera_provisao_categoria_fiscal, bool gerar_lancamentos_baixas)
        {
            Ilcs ilcs = new Ilcs();
            List<ImportacaoLancamentosContabeis> list_ilc = new List<ImportacaoLancamentosContabeis>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                //INICIO ==> Conta corrente movimento origem CCM
                MySqlDataReader reader_ccm;
                MySqlCommand comand_ccm = conn.CreateCommand();
                comand_ccm.Connection = conn;
                comand_ccm.Transaction = Transacao;

                comand_ccm.CommandText = "SELECT ccm.ccm_id, COALESCE(c.categoria_escopo, 'Sem Categoria') as 'escopo', c.categoria_categoria_fiscal, ccm.ccm_data, ccm.ccm_data_competencia, COALESCE(c.categoria_conta_contabil,'Categoria sem conta contábil') as 'conta_contabil_categoria', COALESCE(cc.ccorrente_masc_contabil, 'Conta corrente sem conta contábil') as 'conta_contabil_conta_corrente', ccm.ccm_valor, ccm.ccm_memorando, COALESCE(nf.ccm_nf_id,'Não Informada') as 'nota', nf.ccm_nf_numero, COALESCE(p.participante_cnpj_cpf,'Sem Participante') as 'participante' from conta_corrente_mov as ccm LEFT JOIN categoria as c on c.categoria_id = ccm.ccm_contra_partida_id LEFT JOIN conta_corrente as cc on cc.ccorrente_id = ccm.ccm_ccorrente_id LEFT JOIN ccm_nf as nf on nf.ccm_nf_ccm_id = ccm.ccm_id LEFT JOIN participante as p on p.participante_id = ccm.ccm_participante_id WHERE ccm.ccm_origem in ('CCM') and ccm.ccm_conta_id = @cliente_id and ccm.ccm_data BETWEEN @data_inicial and @data_final;";
                comand_ccm.Parameters.AddWithValue("@cliente_id", cliente_id);
                comand_ccm.Parameters.AddWithValue("@data_inicial", data_inicial);
                comand_ccm.Parameters.AddWithValue("@data_final", data_final);

                reader_ccm = comand_ccm.ExecuteReader();                

                if (reader_ccm.HasRows)
                {
                    while (reader_ccm.Read())
                    {
                        ImportacaoLancamentosContabeis ilc = new ImportacaoLancamentosContabeis();
                        ImportacaoLancamentosContabeis ilc_provisao = new ImportacaoLancamentosContabeis();

                        //start no status como regular
                        ilc.status = "OK";
                        ilc.mensagem += "";
                        ilc.origem += "CCML";
                        ilc.tipo = "Pagamento";

                        if (reader_ccm["escopo"].ToString() == "Sem Categoria")
                        {
                            ilcs.qunatidade_erros += 1;
                            ilc.status = "Erro";
                            ilc.mensagem += "Lançamento sem categoria; ";
                        }

                        if (reader_ccm["conta_contabil_conta_corrente"].ToString() == "Conta corrente sem conta contábil" || reader_ccm["conta_contabil_conta_corrente"].ToString() == "")
                        {
                            ilcs.qunatidade_erros += 1;
                            ilc.status = "Erro";
                            ilc.mensagem += "Conta corrente sem conta contábil; ";
                        }

                        if (reader_ccm["conta_contabil_categoria"].ToString() == "Categoria sem conta contábil" || reader_ccm["conta_contabil_categoria"].ToString() == "")
                        {
                            ilcs.qunatidade_erros += 1;
                            ilc.status = "Erro";
                            ilc.mensagem += "Categoria sem conta contábil; ";
                        }

                        if (Convert.ToBoolean(reader_ccm["categoria_categoria_fiscal"]) == false) //Se categoria não for fiscal
                        {
                            ilc.ilc_data_lancamento = Convert.ToDateTime(reader_ccm["ccm_data"]);
                            
                            if (reader_ccm["escopo"].ToString() == "Entrada")
                            {   
                                ilc.ilc_conta_debito = reader_ccm["conta_contabil_conta_corrente"].ToString();
                                ilc.ilc_conta_credito = reader_ccm["conta_contabil_categoria"].ToString();
                                //participante
                                if (reader_ccm["participante"].ToString() == "Sem Participante")
                                {
                                    ilc.ilc_cnpj_cpf_debito = "";
                                    ilc.ilc_cnpj_cpf_credito = "";
                                }
                                else
                                {
                                    ilc.ilc_cnpj_cpf_debito = "";
                                    ilc.ilc_cnpj_cpf_credito = reader_ccm["participante"].ToString();
                                }
                            }
                            if (reader_ccm["escopo"].ToString() == "Saída")
                            {
                                ilc.ilc_conta_debito = reader_ccm["conta_contabil_categoria"].ToString();
                                ilc.ilc_conta_credito = reader_ccm["conta_contabil_conta_corrente"].ToString();
                                //participante
                                if (reader_ccm["participante"].ToString() == "Sem Participante")
                                {
                                    ilc.ilc_cnpj_cpf_debito = "";
                                    ilc.ilc_cnpj_cpf_credito = "";
                                }
                                else
                                {
                                    ilc.ilc_cnpj_cpf_debito = reader_ccm["participante"].ToString();
                                    ilc.ilc_cnpj_cpf_credito = "";
                                }
                            }
                            ilc.ilc_valor_lancamento = Convert.ToDecimal(reader_ccm["ccm_valor"]);
                            ilc.ilc_codigo_historico = "";
                            ilc.ilc_complemento_historico = reader_ccm["ccm_memorando"].ToString();
                            ilc.ilc_numero_documento = "DCTO";
                            ilc.ilc_lote_lancamento = "";
                            ilc.ilc_contabilizacao_ifrs = "A";
                            ilc.ilc_transacao_sped = "";
                            ilc.ilc_indicador_conciliacao = "";
                            ilc.ilc_indicador_pendencia_concialiacao = "";
                            ilc.ilc_obs_conciliacao_credito = "";
                            ilc.ilc_obs_conciliacao_debito = "";

                        }
                        else //Se cagoria for fiscal
                        {
                            if (gera_provisao_categoria_fiscal) //Se for true
                            {
                                ilc_provisao.status = "OK";
                                ilc_provisao.mensagem += "";
                                ilc_provisao.origem += "CCML";
                                ilc_provisao.tipo = "Provisão";

                                if (reader_ccm["participante"].ToString() == "Sem Participante")
                                {
                                    ilcs.qunatidade_erros += 1;
                                    ilc_provisao.status = "Erro";
                                    ilc_provisao.mensagem += "Lançamento sem Participante; ";
                                }

                                ilc_provisao.ilc_data_lancamento = Convert.ToDateTime(reader_ccm["ccm_data_competencia"]);

                                if (reader_ccm["escopo"].ToString() == "Entrada")
                                {
                                    ilc_provisao.ilc_conta_debito = "16";
                                    ilc_provisao.ilc_conta_credito = reader_ccm["conta_contabil_categoria"].ToString();
                                    ilc_provisao.ilc_cnpj_cpf_debito = reader_ccm["participante"].ToString();
                                    ilc_provisao.ilc_cnpj_cpf_credito = "";
                                }
                                if (reader_ccm["escopo"].ToString() == "Saída")
                                {
                                    ilc_provisao.ilc_conta_debito = reader_ccm["conta_contabil_categoria"].ToString();
                                    ilc_provisao.ilc_conta_credito = "148";
                                    ilc_provisao.ilc_cnpj_cpf_debito = "";
                                    ilc_provisao.ilc_cnpj_cpf_credito = reader_ccm["participante"].ToString();
                                }
                                ilc_provisao.ilc_valor_lancamento = Convert.ToDecimal(reader_ccm["ccm_valor"]);
                                ilc_provisao.ilc_codigo_historico = "";
                                ilc_provisao.ilc_complemento_historico = reader_ccm["ccm_memorando"].ToString();

                                if (reader_ccm["nota"].ToString() == "Não Informada")
                                {
                                    ilc_provisao.ilc_contabilizacao_ifrs = "S";
                                    ilc.ilc_contabilizacao_ifrs = "S"; //Se a provisão é societária o pagamento também deve ser societário
                                }
                                else
                                {
                                    ilc_provisao.ilc_contabilizacao_ifrs = "A";
                                    ilc.ilc_contabilizacao_ifrs = "A"; //Se a provisão é societária o pagamento também deve ser societário
                                }

                                ilc_provisao.ilc_numero_documento = "DCTO" + reader_ccm["ccm_nf_numero"].ToString();
                                ilc_provisao.ilc_lote_lancamento = "";
                                ilc_provisao.ilc_transacao_sped = "";
                                ilc_provisao.ilc_indicador_conciliacao = "";
                                ilc_provisao.ilc_indicador_pendencia_concialiacao = "";
                                ilc_provisao.ilc_obs_conciliacao_credito = "";
                                ilc_provisao.ilc_obs_conciliacao_debito = "";
                                
                                list_ilc.Add(ilc_provisao);
                            }                            

                            //Pagamento da categoria fiscal
                            ilc.ilc_data_lancamento = Convert.ToDateTime(reader_ccm["ccm_data"]);
                            if (reader_ccm["escopo"].ToString() == "Entrada")
                            {
                                ilc.ilc_conta_debito = reader_ccm["conta_contabil_conta_corrente"].ToString();
                                ilc.ilc_conta_credito = reader_ccm["conta_contabil_categoria"].ToString();
                                //participante
                                if (reader_ccm["participante"].ToString() == "Sem Participante")
                                {
                                    ilc.ilc_cnpj_cpf_debito = "";
                                    ilc.ilc_cnpj_cpf_credito = "";
                                }
                                else
                                {
                                    ilc.ilc_cnpj_cpf_debito = "";
                                    ilc.ilc_cnpj_cpf_credito = reader_ccm["participante"].ToString();
                                }
                            }
                            if (reader_ccm["escopo"].ToString() == "Saída")
                            {
                                ilc.ilc_conta_debito = reader_ccm["conta_contabil_categoria"].ToString();
                                ilc.ilc_conta_credito = reader_ccm["conta_contabil_conta_corrente"].ToString();
                                //participante
                                if (reader_ccm["participante"].ToString() == "Sem Participante")
                                {
                                    ilc.ilc_cnpj_cpf_debito = "";
                                    ilc.ilc_cnpj_cpf_credito = "";
                                }
                                else
                                {
                                    ilc.ilc_cnpj_cpf_debito = reader_ccm["participante"].ToString();
                                    ilc.ilc_cnpj_cpf_credito = "";
                                }
                            }
                            ilc.ilc_valor_lancamento = Convert.ToDecimal(reader_ccm["ccm_valor"]);
                            ilc.ilc_codigo_historico = "";
                            ilc.ilc_complemento_historico = reader_ccm["ccm_memorando"].ToString();
                            ilc.ilc_numero_documento = "DCTO";
                            ilc.ilc_lote_lancamento = "";                            
                            ilc.ilc_transacao_sped = "";
                            ilc.ilc_indicador_conciliacao = "";
                            ilc.ilc_indicador_pendencia_concialiacao = "";
                            ilc.ilc_obs_conciliacao_credito = "";
                            ilc.ilc_obs_conciliacao_debito = "";
                        }

                        list_ilc.Add(ilc);
                    }
                }
                reader_ccm.Close();
                //FIM ==> Conta corrente movimento origem CCM
                //INIIO ==> Conta corrente movimento orgiem Transferências
                MySqlDataReader reader_ccm_t;
                MySqlCommand comand_ccm_t = conn.CreateCommand();
                comand_ccm_t.Connection = conn;
                comand_ccm_t.Transaction = Transacao;

                comand_ccm_t.CommandText = "SELECT de.ccm_id, COALESCE(ccde.ccorrente_masc_contabil,'Sem Conta Contabil') as 'masc_de', COALESCE(ccpara.ccorrente_masc_contabil, 'Sem Conta Contabil') as 'masc_para', de.ccm_valor, de.ccm_data, de.ccm_memorando from conta_corrente_mov as de LEFT JOIN conta_corrente_mov as para on para.ccm_origem_id = de.ccm_id LEFT JOIN conta_corrente as ccde on ccde.ccorrente_id = de.ccm_ccorrente_id LEFT JOIN conta_corrente as ccpara on ccpara.ccorrente_id = para.ccm_ccorrente_id WHERE de.ccm_origem = 'Transferencia' and de.ccm_movimento = 'S' and de.ccm_conta_id = @cliente_id and de.ccm_data BETWEEN @data_inicial and @data_final;";
                comand_ccm_t.Parameters.AddWithValue("@cliente_id", cliente_id);
                comand_ccm_t.Parameters.AddWithValue("@data_inicial", data_inicial);
                comand_ccm_t.Parameters.AddWithValue("@data_final", data_final);

                reader_ccm_t = comand_ccm_t.ExecuteReader();

                if (reader_ccm_t.HasRows)
                {
                    while (reader_ccm_t.Read())
                    {
                        ImportacaoLancamentosContabeis ilc = new ImportacaoLancamentosContabeis();

                        //start no status como regular
                        ilc.status = "OK";
                        ilc.mensagem += "";
                        ilc.origem += "CCMT";
                        ilc.tipo = "Transferência";

                        if(reader_ccm_t["masc_de"].ToString() == "Sem Conta Contabil" || reader_ccm_t["masc_para"].ToString() == "Sem Conta Contabil")
                        {
                            ilcs.qunatidade_erros += 1;
                            ilc.status = "Erro";
                            ilc.mensagem += "Conta corrente sem conta contábil; ";
                        }

                        ilc.ilc_data_lancamento = Convert.ToDateTime(reader_ccm_t["ccm_data"]);
                        ilc.ilc_conta_debito = reader_ccm_t["masc_para"].ToString();
                        ilc.ilc_conta_credito = reader_ccm_t["masc_de"].ToString();
                        ilc.ilc_valor_lancamento = Convert.ToDecimal(reader_ccm_t["ccm_valor"]);
                        ilc.ilc_codigo_historico = "";
                        ilc.ilc_complemento_historico = reader_ccm_t["ccm_memorando"].ToString();
                        ilc.ilc_numero_documento = "DCTO";
                        ilc.ilc_lote_lancamento = "";
                        ilc.ilc_contabilizacao_ifrs = "A";
                        ilc.ilc_transacao_sped = "";
                        ilc.ilc_indicador_conciliacao = "";
                        ilc.ilc_indicador_pendencia_concialiacao = "";
                        ilc.ilc_obs_conciliacao_credito = "";
                        ilc.ilc_obs_conciliacao_debito = "";

                        list_ilc.Add(ilc);
                    }
                }
                reader_ccm_t.Close();
                //FIm ==> Conta corrente movimento orgiem Transferências
                //INIIO ==> Conta corrente movimento orgiem Baixas
                if (gerar_lancamentos_baixas)
                {                    
                    MySqlDataReader reader_baixas;
                    MySqlCommand comand_baixas = conn.CreateCommand();
                    comand_baixas.Connection = conn;
                    comand_baixas.Transaction = Transacao;

                    comand_baixas.CommandText = "SELECT ccm.ccm_data, ccm.ccm_movimento, COALESCE(cc.ccorrente_masc_contabil, 'Conta corrente sem conta contabil') as 'masc_conta_corrente', COALESCE(c.categoria_conta_contabil, 'Categoria sem conta contábil') as 'masc_categoria', baixa.oppb_valor, baixa.oppb_juros, baixa.oppb_multa, baixa.oppb_desconto, baixa.oppb_obs, ccm.ccm_memorando, baixa.oppb_data, COALESCE(nf.op_nf_numero, 'Não Informada') as nota, (baixa.oppb_valor + baixa.oppb_juros + baixa.oppb_multa - baixa.oppb_desconto) as 'valor_pagamento' from conta_corrente_mov as ccm LEFT JOIN conta_corrente as cc on cc.ccorrente_id = ccm.ccm_ccorrente_id LEFT JOIN operacao as op on op.op_id = ccm.ccm_op_id left JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN op_parcelas_baixa as baixa on baixa.oppb_id = ccm.ccm_oppb_id LEFT JOIN op_nf as nf on nf.op_nf_op_id = op.op_id WHERE ccm.ccm_origem = 'Baixa' and ccm.ccm_conta_id = @cliente_id and ccm.ccm_data BETWEEN @data_inicial and @data_final;";
                    comand_baixas.Parameters.AddWithValue("@cliente_id", cliente_id);
                    comand_baixas.Parameters.AddWithValue("@data_inicial", data_inicial);
                    comand_baixas.Parameters.AddWithValue("@data_final", data_final);

                    reader_baixas = comand_baixas.ExecuteReader();

                    if (reader_baixas.HasRows)
                    {
                        while (reader_baixas.Read())
                        {
                            ImportacaoLancamentosContabeis ilc = new ImportacaoLancamentosContabeis();

                            //start no status como regular
                            ilc.status = "OK";
                            ilc.mensagem += "";
                            ilc.origem += "Baixa";
                            ilc.tipo = "Baixa";

                            if (reader_baixas["masc_conta_corrente"].ToString() == "Conta corrente sem conta contabil")
                            {
                                ilcs.qunatidade_erros += 1;
                                ilc.status = "Erro";
                                ilc.mensagem += "Conta corrente sem conta contábil; ";
                            }

                            //A categoria nãop importar, pois está sendo importando o pagamento da baixa. A provisão provem do único ou da provisão de operações a ser feita no futuro.
                            //if (reader_baixas["masc_categoria"].ToString() == "Categoria sem conta contábil")
                            //{
                            //    ilcs.qunatidade_erros += 1;
                            //    ilc.status = "Erro";
                            //    ilc.mensagem += "Categoria sem conta contábil; ";
                            //}

                            ilc.ilc_data_lancamento = Convert.ToDateTime(reader_baixas["oppb_data"]);

                            if(reader_baixas["ccm_movimento"].ToString() == "E")
                            {
                                ilc.ilc_conta_debito = reader_baixas["masc_conta_corrente"].ToString();
                                ilc.ilc_conta_credito = "16";
                            }
                            else
                            {
                                ilc.ilc_conta_debito = "148";
                                ilc.ilc_conta_credito = reader_baixas["masc_conta_corrente"].ToString();
                            }
                                                    
                            ilc.ilc_valor_lancamento = Convert.ToDecimal(reader_baixas["valor_pagamento"]);
                            ilc.ilc_codigo_historico = "";
                            ilc.ilc_complemento_historico = reader_baixas["ccm_memorando"].ToString();
                            
                            //Não está sendo computado erro se na operação não foi informada nota fiscal.                            
                            if(reader_baixas["nota"].ToString() == "Não Informada")
                            {
                                ilc.ilc_numero_documento = "DCTO";
                            }
                            else
                            {
                                ilc.ilc_numero_documento = "DCTO" + reader_baixas["nota"].ToString();
                            }                            
                            ilc.ilc_lote_lancamento = "";
                            ilc.ilc_contabilizacao_ifrs = "A";
                            ilc.ilc_transacao_sped = "";
                            ilc.ilc_indicador_conciliacao = "";
                            ilc.ilc_indicador_pendencia_concialiacao = "";
                            ilc.ilc_obs_conciliacao_credito = "";
                            ilc.ilc_obs_conciliacao_debito = "";

                            list_ilc.Add(ilc);

                            //Lançamento dos juros da baixa

                            //Lançamento da multa da baixa

                            //Lançamento do desconto da baixa
                        }
                    }
                    reader_baixas.Close();
                    //FIm ==> Conta corrente movimento orgiem Baixas
                }



                ilcs.status = "Sucesso";
                ilcs.list_ilc = list_ilc;                
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

            return ilcs;
        }
    }

    public class Ilcs
    {
        public string status { get; set; }
        public int qunatidade_erros { get; set; }
        public List<ImportacaoLancamentosContabeis> list_ilc { get; set; }
        public ilc_filter filtro { get; set; }
    }

    public class ilc_filter
    {
        public int cliente_id { get; set; }
        public DateTime data_inicial { get; set; }
        public DateTime data_final { get; set; }
        public bool gera_provisao_categoria_fiscal { get; set; }
        public bool gerar_lancamentos_baixas { get; set; }
    }
}
