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
using gestaoContadorcomvc.Models;


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
        public Ilcs create(int usuario_id, int conta_id, int cliente_id, DateTime data_inicial, DateTime data_final, bool gerar_lancamentos_baixas, bool gerar_lancamentos_ccm, bool gerar_pgto_a_participante_categoria_fiscal_sem_nf_informada)
        {
            Ilcs ilcs = new Ilcs();
            List<ImportacaoLancamentosContabeis> list_ilc = new List<ImportacaoLancamentosContabeis>();
            CategoriasPadrao cp = new CategoriasPadrao();
            cp.categoria_padrao(conta_id);




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

                comand_ccm.CommandText = "SELECT ccm.ccm_id, COALESCE(c.categoria_escopo, 'Sem Categoria') as 'escopo', COALESCE(c.categoria_categoria_fiscal, -1) as 'categoria_fiscal', ccm.ccm_data, ccm.ccm_data_competencia, COALESCE(c.categoria_conta_contabil,'Categoria sem conta contábil') as 'conta_contabil_categoria', COALESCE(cc.ccorrente_masc_contabil, 'Conta corrente sem conta contábil') as 'conta_contabil_conta_corrente', ccm.ccm_valor_principal, ccm.ccm_multa, ccm.ccm_juros, ccm.ccm_valor, ccm.ccm_memorando, COALESCE(nf.ccm_nf_numero,'Não Informada') as 'nota', COALESCE(p.participante_cnpj_cpf,'Sem Participante') as 'participante' from conta_corrente_mov as ccm LEFT JOIN categoria as c on c.categoria_id = ccm.ccm_contra_partida_id LEFT JOIN conta_corrente as cc on cc.ccorrente_id = ccm.ccm_ccorrente_id LEFT JOIN ccm_nf as nf on nf.ccm_nf_ccm_id = ccm.ccm_id LEFT JOIN participante as p on p.participante_id = ccm.ccm_participante_id WHERE ccm.ccm_origem in ('CCM') and ccm.ccm_conta_id = @cliente_id and ccm.ccm_data BETWEEN @data_inicial and @data_final;";
                comand_ccm.Parameters.AddWithValue("@cliente_id", cliente_id);
                comand_ccm.Parameters.AddWithValue("@data_inicial", data_inicial);
                comand_ccm.Parameters.AddWithValue("@data_final", data_final);

                reader_ccm = comand_ccm.ExecuteReader();                

                if (reader_ccm.HasRows)
                {
                    while (reader_ccm.Read())
                    {
                        DateTime data_lancamento = new DateTime();
                        string conta_contabil_categoria = "";                        
                        string conta_contabil_conta_corrente = "";                        
                        Decimal valor_lancamento = 0;                        
                        Decimal valor_multa = 0;
                        Decimal valor_juros = 0;                        
                        Decimal valor_principal = 0;                        
                        string complemento_historico = "";
                        string numero_documento = "";                                                
                        string participante = "";
                        string escopo = "";
                        int categoria_fiscal = -1; //Pode ser 0 (false), 1 (true) ou -1 (caso não tenha categoria informada);

                        if (DBNull.Value != reader_ccm["ccm_data"])
                        {
                            data_lancamento = Convert.ToDateTime(reader_ccm["ccm_data"]);
                        }
                        else
                        {
                            data_lancamento = new DateTime();
                        }

                        conta_contabil_categoria = reader_ccm["conta_contabil_categoria"].ToString();

                        if (DBNull.Value != reader_ccm["valor_lancamento"])
                        {
                            valor_lancamento = Convert.ToDecimal(reader_ccm["valor_lancamento"]);
                        }
                        else
                        {
                            valor_lancamento = 0;
                        }

                        if (DBNull.Value != reader_ccm["ccm_valor_principal"])
                        {
                            valor_principal = Convert.ToDecimal(reader_ccm["ccm_valor_principal"]);
                        }
                        else
                        {
                            valor_principal = 0;
                        }

                        if (DBNull.Value != reader_ccm["ccm_multa"])
                        {
                            valor_multa = Convert.ToDecimal(reader_ccm["ccm_multa"]);
                        }
                        else
                        {
                            valor_multa = 0;
                        }

                        if (DBNull.Value != reader_ccm["ccm_juros"])
                        {
                            valor_juros = Convert.ToDecimal(reader_ccm["ccm_juros"]);
                        }
                        else
                        {
                            valor_juros = 0;
                        }

                        conta_contabil_categoria = reader_ccm["conta_contabil_categoria"].ToString();
                        conta_contabil_conta_corrente = reader_ccm["conta_contabil_conta_corrente"].ToString();
                        complemento_historico = reader_ccm["ccm_memorando"].ToString();
                        numero_documento = reader_ccm["nota"].ToString();
                        participante = reader_ccm["participante"].ToString();
                        escopo = reader_ccm["escopo"].ToString();

                        if (DBNull.Value != reader_ccm["categoria_fiscal"])
                        {
                            categoria_fiscal = Convert.ToInt32(reader_ccm["categoria_fiscal"]);
                        }
                        else
                        {
                            valor_juros = -1;
                        }

                        //Lógica dos lançamentos
                        //CCM Lançamentos
                        if (gerar_lancamentos_ccm) //caso cliente opte por gerar os laçamentos de origem em ccm
                        {
                            if(categoria_fiscal == -1) //Se for -1 é porque não possui categoria. Gera um lançamento
                            {
                                list_ilc.Add(lancamento("CCM","Pagamento",false,false,data_lancamento,conta_contabil_categoria,conta_contabil_conta_corrente,valor_lancamento,"",complemento_historico,numero_documento,"",participante,participante,"S","","","","",""));
                            }
                            else
                            {
                                if (categoria_fiscal == 0) //Se for 0 então categoria NÃO é fiscal
                                {
                                    if(escopo == "Saída")
                                    {
                                        list_ilc.Add(lancamento("CCM", "Pagamento", false, false, data_lancamento, conta_contabil_categoria, conta_contabil_conta_corrente, valor_principal, "", complemento_historico, "", "", "", "", "A", "", "", "", "", ""));
                                        if(valor_juros > 0)
                                        {
                                            list_ilc.Add(lancamento("CCM", "Pagamento", false, false, data_lancamento, cp.juros_pagos.categoria_conta_contabil, conta_contabil_conta_corrente, valor_juros, "", ("Juros pagos ref.: " + complemento_historico), "", "", "", "", "A", "", "", "", "", ""));
                                        }
                                        if(valor_multa > 0)
                                        {
                                            list_ilc.Add(lancamento("CCM", "Pagamento", false, false, data_lancamento, cp.multas_pagas.categoria_conta_contabil, conta_contabil_conta_corrente, valor_multa, "", ("Multa paga ref.: " + complemento_historico), "", "", "", "", "A", "", "", "", "", ""));
                                        }
                                    }
                                    else
                                    {
                                        list_ilc.Add(lancamento("CCM", "Pagamento", false, false, data_lancamento, conta_contabil_conta_corrente, conta_contabil_categoria, valor_principal, "", complemento_historico, "", "", "", "", "A", "", "", "", "", ""));
                                        if (valor_juros > 0)
                                        {
                                            list_ilc.Add(lancamento("CCM", "Pagamento", false, false, data_lancamento, conta_contabil_conta_corrente, cp.juros_recebidos.categoria_conta_contabil , valor_juros, "", ("Juros recebidos ref.: " + complemento_historico), "", "", "", "", "A", "", "", "", "", ""));
                                        }
                                        if (valor_multa > 0)
                                        {
                                            list_ilc.Add(lancamento("CCM", "Pagamento", false, false, data_lancamento, conta_contabil_conta_corrente, cp.multas_recebidas.categoria_conta_contabil, valor_multa, "", ("Multa recebida ref.: " + complemento_historico), "", "", "", "", "A", "", "", "", "", ""));
                                        }
                                    }
                                }

                                if (categoria_fiscal == 1) //Se for 0 então categoria É fiscal
                                {
                                    //Requer nota fiscal
                                    //O cliente pode não informar, mas existe nota. 
                                    //Se não existe nota o lançamento é societário.
                                    //Se existe nota o lançamento é ambos.

                                    //verificar 'gerar pagamento contra participante das categorias fiscais independente da nota'
                                    if (gerar_pgto_a_participante_categoria_fiscal_sem_nf_informada)
                                    {
                                        if(escopo == "Saída")
                                        {
                                            list_ilc.Add(lancamento("CCM", "Pagamento", true, false, data_lancamento, "148", conta_contabil_conta_corrente, valor_lancamento, "", complemento_historico, "", "", participante, "", "A", "", "", "", "", ""));
                                            if (valor_juros > 0)
                                            {                                                
                                                list_ilc.Add(lancamento("CCM", "Provisão Juros", true, false, data_lancamento, cp.juros_pagos.categoria_conta_contabil, "148", valor_juros, "", ("Juros pagos ref.: " + complemento_historico), "", "", "", participante, "A", "", "", "", "", ""));
                                            }
                                            if (valor_multa > 0)
                                            {
                                                list_ilc.Add(lancamento("CCM", "Provisão Multa", true, false, data_lancamento, cp.multas_pagas.categoria_conta_contabil, "148", valor_multa, "", ("Multa paga ref.: " + complemento_historico), "", "", "", participante, "A", "", "", "", "", ""));
                                            }
                                        }
                                        else
                                        {
                                            list_ilc.Add(lancamento("CCM", "Pagamento", true, false, data_lancamento, conta_contabil_conta_corrente, "16", valor_lancamento, "", complemento_historico, "", "", "", participante, "A", "", "", "", "", ""));
                                            if (valor_juros > 0)
                                            {
                                                list_ilc.Add(lancamento("CCM", "Provisão Juros", true, false, data_lancamento, "16", cp.juros_recebidos.categoria_conta_contabil, valor_juros, "", ("Juros recebidos ref.: " + complemento_historico), "", "", participante, "", "A", "", "", "", "", ""));
                                            }
                                            if (valor_multa > 0)
                                            {
                                                list_ilc.Add(lancamento("CCM", "Provisão Multa", true, false, data_lancamento, "16", cp.multas_recebidas.categoria_conta_contabil, valor_multa, "", ("Multa recebida ref.: " + complemento_historico), "", "", participante, "", "A", "", "", "", "", ""));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if(numero_documento != "Não Informada") //Existe a informação da nota fiscal
                                        {
                                            if (escopo == "Saída")
                                            {
                                                list_ilc.Add(lancamento("CCM", "Pagamento", true, true, data_lancamento, "148", conta_contabil_conta_corrente, valor_lancamento, "", complemento_historico, ("DCTO" + numero_documento), "", participante, "", "A", "", "", "", "", ""));
                                                if (valor_juros > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Juros", true, false, data_lancamento, cp.juros_pagos.categoria_conta_contabil, "148", valor_juros, "", ("Juros pagos ref.: " + complemento_historico), "", "", "", participante, "A", "", "", "", "", ""));
                                                }
                                                if (valor_multa > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Multa", true, false, data_lancamento, cp.multas_pagas.categoria_conta_contabil, "148", valor_multa, "", ("Multa paga ref.: " + complemento_historico), "", "", "", participante, "A", "", "", "", "", ""));
                                                }
                                            }
                                            else
                                            {
                                                list_ilc.Add(lancamento("CCM", "Pagamento", true, true, data_lancamento, conta_contabil_conta_corrente, "16", valor_lancamento, "", complemento_historico, ("DCTO" + numero_documento), "", "", participante, "A", "", "", "", "", ""));
                                                if (valor_juros > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Juros", true, false, data_lancamento, "16", cp.juros_recebidos.categoria_conta_contabil, valor_juros, "", ("Juros recebidos ref.: " + complemento_historico), "", "", participante, "", "A", "", "", "", "", ""));
                                                }
                                                if (valor_multa > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Multa", true, false, data_lancamento, "16", cp.multas_recebidas.categoria_conta_contabil, valor_multa, "", ("Multa recebida ref.: " + complemento_historico), "", "", participante, "", "A", "", "", "", "", ""));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Se nota não informada lançamento é societário
                                            //Precisa lançar a provisão
                                            //A provisão será gerada em rotina separada
                                            //Nesta área está o lançamento do pagamento
                                            //PENDÊNCIA ==> provisão programar no futuro
                                            if (escopo == "Saída")
                                            {
                                                list_ilc.Add(lancamento("CCM", "Pagamento", true, false, data_lancamento, "148", conta_contabil_conta_corrente, valor_lancamento, "", complemento_historico, "", "", participante, "", "S", "", "", "", "", ""));
                                                if (valor_juros > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Juros", true, false, data_lancamento, cp.juros_pagos.categoria_conta_contabil, "148", valor_juros, "", ("Juros pagos ref.: " + complemento_historico), "", "", "", participante, "S", "", "", "", "", ""));
                                                }
                                                if (valor_multa > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Multa", true, false, data_lancamento, cp.multas_pagas.categoria_conta_contabil, "148", valor_multa, "", ("Multa paga ref.: " + complemento_historico), "", "", "", participante, "S", "", "", "", "", ""));
                                                }
                                            }
                                            else
                                            {
                                                list_ilc.Add(lancamento("CCM", "Pagamento", true, false, data_lancamento, conta_contabil_conta_corrente, "16", valor_lancamento, "", complemento_historico, "", "", "", participante, "S", "", "", "", "", ""));
                                                if (valor_juros > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Juros", true, false, data_lancamento, "16", cp.juros_recebidos.categoria_conta_contabil, valor_juros, "", ("Juros recebidos ref.: " + complemento_historico), "", "", participante, "", "S", "", "", "", "", ""));
                                                }
                                                if (valor_multa > 0)
                                                {
                                                    list_ilc.Add(lancamento("CCM", "Provisão Multa", true, false, data_lancamento, "16", cp.multas_recebidas.categoria_conta_contabil, valor_multa, "", ("Multa recebida ref.: " + complemento_historico), "", "", participante, "", "S", "", "", "", "", ""));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }                        
                    }
                }
                //FIM CCM Lançamentos
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

                //INICIO ==> Conta corrente movimento orgiem Baixas
                if (gerar_lancamentos_baixas)
                {                    
                    MySqlDataReader reader_baixas;
                    MySqlCommand comand_baixas = conn.CreateCommand();
                    comand_baixas.Connection = conn;
                    comand_baixas.Transaction = Transacao;

                    comand_baixas.CommandText = "SELECT ccm.ccm_data, ccm.ccm_movimento, COALESCE(cc.ccorrente_masc_contabil, 'Conta corrente sem conta contabil') as 'masc_conta_corrente', COALESCE(c.categoria_conta_contabil, 'Categoria sem conta contábil') as 'masc_categoria', baixa.oppb_valor, baixa.oppb_juros, baixa.oppb_multa, baixa.oppb_desconto, baixa.oppb_obs, ccm.ccm_memorando, baixa.oppb_data, COALESCE(nf.op_nf_numero, 'Não Informada') as nota, (baixa.oppb_valor + baixa.oppb_juros + baixa.oppb_multa - baixa.oppb_desconto) as 'valor_pagamento', COALESCE(part.op_part_cnpj_cpf, 'Sem Participante') as 'participante' from conta_corrente_mov as ccm LEFT JOIN conta_corrente as cc on cc.ccorrente_id = ccm.ccm_ccorrente_id LEFT JOIN operacao as op on op.op_id = ccm.ccm_op_id left JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN op_parcelas_baixa as baixa on baixa.oppb_id = ccm.ccm_oppb_id LEFT JOIN op_nf as nf on nf.op_nf_op_id = op.op_id LEFT JOIN op_participante as part on part.op_id = op.op_id WHERE ccm.ccm_origem = 'Baixa' and ccm.ccm_conta_id = @cliente_id and ccm.ccm_data BETWEEN @data_inicial and @data_final;";
                    comand_baixas.Parameters.AddWithValue("@cliente_id", cliente_id);
                    comand_baixas.Parameters.AddWithValue("@data_inicial", data_inicial);
                    comand_baixas.Parameters.AddWithValue("@data_final", data_final);

                    reader_baixas = comand_baixas.ExecuteReader();

                    if (reader_baixas.HasRows)
                    {
                        while (reader_baixas.Read())
                        {
                            //dados variáveis
                            string escopo = reader_baixas["ccm_movimento"].ToString(); //E ou S
                            string nota = reader_baixas["nota"].ToString(); //Número da nota ou 'Não Informada'
                            string ifrs = "A";
                            if(nota == "Não Informada")
                            {
                                ifrs = "S";
                            }                            
                            bool nota_obrigatoria = true;
                            if(ifrs == "S")
                            {
                                nota_obrigatoria = false;
                            }                            
                            DateTime data_baixa = new DateTime();
                            if (DBNull.Value != reader_baixas["oppb_data"])
                            {
                                data_baixa = Convert.ToDateTime(reader_baixas["oppb_data"]);
                            }
                            else
                            {
                                data_baixa = new DateTime();
                            }
                            string conta_contabil_cc = reader_baixas["masc_conta_corrente"].ToString();
                            Decimal valor_princial = 0;
                            Decimal multa = 0;
                            Decimal juros = 0;
                            Decimal desconto = 0;                            
                            Decimal valor_pagamento = 0;
                            if (DBNull.Value != reader_baixas["valor_pagamento"])
                            {
                                valor_pagamento = Convert.ToDecimal(reader_baixas["valor_pagamento"]);
                            }
                            else
                            {
                                valor_pagamento = 0;
                            }
                            if (DBNull.Value != reader_baixas["oppb_valor"])
                            {
                                valor_princial = Convert.ToDecimal(reader_baixas["oppb_valor"]);
                            }
                            else
                            {
                                valor_princial = 0;
                            }
                            if (DBNull.Value != reader_baixas["oppb_juros"])
                            {
                                juros = Convert.ToDecimal(reader_baixas["oppb_juros"]);
                            }
                            else
                            {
                                juros = 0;
                            }
                            if (DBNull.Value != reader_baixas["oppb_multa"])
                            {
                                multa = Convert.ToDecimal(reader_baixas["oppb_multa"]);
                            }
                            else
                            {
                                multa = 0;
                            }
                            if (DBNull.Value != reader_baixas["oppb_desconto"])
                            {
                                desconto = Convert.ToDecimal(reader_baixas["oppb_desconto"]);
                            }
                            else
                            {
                                desconto = 0;
                            }
                            string historico = reader_baixas["ccm_memorando"].ToString();
                            string participante = reader_baixas["participante"].ToString();
                            string dcto = "";
                            if(ifrs == "S")
                            {
                                dcto = "";
                            }
                            else
                            {
                                dcto = "DCTO" + nota;
                            }
                            
                            //Lançamentos
                            if (escopo == "S")
                            {
                                list_ilc.Add(lancamento("Baixa", "Pagamento", true, nota_obrigatoria, data_baixa, "148", conta_contabil_cc, valor_pagamento, "", historico, dcto, "", participante, "", ifrs, "", "", "", "", ""));
                                if (juros > 0)
                                {
                                    list_ilc.Add(lancamento("Baixa", "Provisão Juros", true, false, data_baixa, cp.juros_pagos.categoria_conta_contabil, "148", juros, "", ("Juros pagos ref.: " + historico), "", "", participante, "", ifrs, "", "", "", "", ""));
                                }
                                if (multa > 0)
                                {
                                    list_ilc.Add(lancamento("Baixa", "Provisão Multa", true, false, data_baixa, cp.multas_pagas.categoria_conta_contabil, "148", multa, "", ("Multa paga ref.: " + historico), "", "", participante, "", ifrs, "", "", "", "", ""));
                                }
                                if (desconto > 0)
                                {
                                    list_ilc.Add(lancamento("Baixa", "Provisão Desconto", true, false, data_baixa, "148", cp.descontos_obtidos.categoria_conta_contabil, desconto, "", ("Desconto obtido ref.: " + historico), "", "", participante, "", ifrs, "", "", "", "", ""));
                                }
                            }
                            else
                            {
                                list_ilc.Add(lancamento("Baixa", "Pagamento", true, nota_obrigatoria, data_baixa, conta_contabil_cc, "16", valor_pagamento, "", historico, dcto, "", participante, "", ifrs, "", "", "", "", ""));
                                if (juros > 0)
                                {
                                    list_ilc.Add(lancamento("Baixa", "Provisão Juros", true, false, data_baixa, "16", cp.juros_recebidos.categoria_conta_contabil, juros, "", ("Juros recebidos ref.: " + historico), "", "", participante, "", ifrs, "", "", "", "", ""));
                                }
                                if (multa > 0)
                                {
                                    list_ilc.Add(lancamento("Baixa", "Provisão Multa", true, false, data_baixa, "16", cp.multas_recebidas.categoria_conta_contabil, multa, "", ("Multa recebida ref.: " + historico), "", "", participante, "", ifrs, "", "", "", "", ""));
                                }
                                if (desconto > 0)
                                {
                                    list_ilc.Add(lancamento("Baixa", "Provisão Desconto", true, false, data_baixa, cp.descotos_concedidos.categoria_conta_contabil, "16", desconto, "", ("Desconto recebido ref.: " + historico), "", "", participante, "", ifrs, "", "", "", "", ""));
                                }
                            }
                        }
                    }
                    reader_baixas.Close();
                    //FIm ==> Conta corrente movimento orgiem Baixa
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

        //Gerar lançamento 
        public ImportacaoLancamentosContabeis lancamento(
            string origem,
            string tipo,
            bool participante_obrigatorio,
            bool nota_obrigatoria,
            DateTime ilc_data_lancamento,
            string ilc_conta_debito,
            string ilc_conta_credito,
            Decimal ilc_valor_lancamento,
            string ilc_codigo_historico,
            string ilc_complemento_historico,
            string ilc_numero_documento,
            string ilc_lote_lancamento,
            string ilc_cnpj_cpf_debito,
            string ilc_cnpj_cpf_credito,
            string ilc_contabilizacao_ifrs,
            string ilc_transacao_sped,
            string ilc_indicador_conciliacao,
            string ilc_indicador_pendencia_concialiacao,
            string ilc_obs_conciliacao_debito,
            string ilc_obs_conciliacao_credito            
            )
        {
            ImportacaoLancamentosContabeis ilc = new ImportacaoLancamentosContabeis();
            //start no status como regular
            ilc.status = "OK";
            ilc.mensagem = "";
            ilc.origem = origem;
            ilc.tipo = tipo;
            ilc.ilc_data_lancamento = ilc_data_lancamento;
            ilc.ilc_conta_debito = ilc_conta_debito;
            ilc.ilc_conta_credito = ilc_conta_credito;
            ilc.ilc_valor_lancamento = ilc_valor_lancamento;
            ilc.ilc_codigo_historico = ilc_codigo_historico;
            ilc.ilc_complemento_historico = ilc_complemento_historico;
            ilc.ilc_numero_documento = "DCTO" + ilc_numero_documento;
            ilc.ilc_lote_lancamento = ilc_lote_lancamento;
            ilc.ilc_cnpj_cpf_debito = ilc_cnpj_cpf_debito;
            ilc.ilc_cnpj_cpf_credito = ilc_cnpj_cpf_credito;
            ilc.ilc_contabilizacao_ifrs = ilc_contabilizacao_ifrs;
            ilc.ilc_transacao_sped = ilc_transacao_sped;
            ilc.ilc_indicador_conciliacao = ilc_indicador_conciliacao;
            ilc.ilc_indicador_pendencia_concialiacao = ilc_indicador_pendencia_concialiacao;
            ilc.ilc_obs_conciliacao_debito = ilc_obs_conciliacao_debito;
            ilc.ilc_obs_conciliacao_credito = ilc_obs_conciliacao_credito;

            //Validações
            if (
                ilc_conta_credito == "Categoria sem conta contábil" || ilc_conta_credito == "" || ilc_conta_credito == null || ilc_conta_debito == "Categoria sem conta contábil" || ilc_conta_debito == "" || ilc_conta_debito == null)
            {   
                ilc.status = "Erro";
                ilc.mensagem += "Lançamento sem categoria; ";
            }

            if(!CheckValidDateTime(ilc_data_lancamento.Year, ilc_data_lancamento.Month, ilc.ilc_data_lancamento.Day))
            {
                ilc.status = "Erro";
                ilc.mensagem += "Data do lançamento é inválida; ";
            }

            if(ilc_valor_lancamento <= 0)
            {
                ilc.status = "Erro";
                ilc.mensagem += "Valor do lançamento menor ou igual a zero; ";
            }

            if(ilc_complemento_historico.Length == 0)
            {
                ilc.status = "Erro";
                ilc.mensagem += "Sem complemento do histórico; ";
            }

            if(participante_obrigatorio && (ilc_cnpj_cpf_credito.Length <= 0 || ilc_cnpj_cpf_debito.Length <= 0 || ilc_cnpj_cpf_credito == null || ilc_cnpj_cpf_debito == null))
            {
                ilc.status = "Erro";
                ilc.mensagem += "Lançamento obrigatório informar participante; ";
            }

            if(nota_obrigatoria && (ilc_numero_documento.Length <= 0 || ilc_numero_documento == null || ilc_numero_documento == "Não Informada"))
            {
                ilc.status = "Erro";
                ilc.mensagem += "Lançamento obrigatório informar nota fiscal; ";
            }

            return ilc;
        }

        bool CheckValidDateTime(int year, int month, int day)
        {
            bool check = false;
            if (year <= DateTime.MaxValue.Year && year >= DateTime.MinValue.Year)
            {
                if (month >= 1 && month <= 12)
                {
                    if (DateTime.DaysInMonth(year, month) >= day && day >= 1)
                        check = true;
                }
            }
            return check;
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
        public string cliente_nome { get; set; }
        public DateTime data_inicial { get; set; }
        public DateTime data_final { get; set; }        
        public bool gerar_lancamentos_baixas { get; set; }
        public bool gerar_lancamentos_ccm { get; set; }
        public bool gerar_pgto_a_participante_categoria_fiscal_sem_nf_informada { get; set; }
    }
}
