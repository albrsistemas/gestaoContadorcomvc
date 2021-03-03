using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_rfm
    {
        public IEnumerable<Vm_rfm_saldos_contas_correntes> vm_rfm_saldo_inicial_contas { get; set; }
        public IEnumerable<Vm_rfm_saldos_contas_correntes> vm_rfm_saldo_final_contas { get; set; }
        public IEnumerable<Vm_rfm_categorias> vm_rfm_categorias { get; set; }
        public Vm_rfm_filtro filtro { get; set; }
        public string status_processamento { get; set; }
        public string erro_processaento { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Vm_rfm()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        public Vm_rfm create(int conta_id, Vm_rfm_filtro filtro)
        {
            Vm_rfm rfm = new Vm_rfm();
            List<Vm_rfm_saldos_contas_correntes> vm_rfm_saldo_inicial_contas = new List<Vm_rfm_saldos_contas_correntes>();
            List<Vm_rfm_saldos_contas_correntes> vm_rfm_saldo_final_contas = new List<Vm_rfm_saldos_contas_correntes>();
            List<Vm_rfm_categorias> vm_rfm_categorias = new List<Vm_rfm_categorias>();

            conn.Open();            
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            try
            {
                //Busca saldo inicial das contas correntes
                MySqlDataReader dr_1;
                MySqlCommand dr_1_c = conn.CreateCommand();
                dr_1_c.Connection = conn;
                dr_1_c.Transaction = Transacao;
                dr_1_c.CommandText = "SELECT cc.ccorrente_id, cc.ccorrente_nome, cc.ccorrente_saldo_abertura, (((SELECT COALESCE(SUM(if(ccm.ccm_movimento = 'E',ccm.ccm_valor, -ccm.ccm_valor)),0.00) from conta_corrente_mov as ccm WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_ccorrente_id = cc.ccorrente_id and ccm.ccm_data < @data_inicial)) + cc.ccorrente_saldo_abertura) as 'saldo' from conta_corrente as cc WHERE cc.ccorrente_conta_id = @conta_id and cc.ccorrente_status = 'Ativo';";
                dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_1_c.Parameters.AddWithValue("data_inicial", filtro.data_inicio);
                dr_1 = dr_1_c.ExecuteReader();

                if (dr_1.HasRows)
                {
                    while (dr_1.Read())
                    {
                        Vm_rfm_saldos_contas_correntes si = new Vm_rfm_saldos_contas_correntes();
                        si.conta_corrente_id = Convert.ToInt32(dr_1["ccorrente_id"]);
                        si.conta_corrente_nome = dr_1["ccorrente_nome"].ToString();
                        si.conta_corrente_saldo = Convert.ToDecimal(dr_1["saldo"]);
                        vm_rfm_saldo_inicial_contas.Add(si);
                    }
                }
                dr_1.Close();
                //Fim

                //Busca saldo final das contas correntes
                MySqlDataReader dr_2;
                MySqlCommand dr_2_c = conn.CreateCommand();
                dr_2_c.Connection = conn;
                dr_2_c.Transaction = Transacao;
                //dr_2_c.CommandText = "SELECT cc.ccorrente_id, cc.ccorrente_nome, cc.ccorrente_saldo_abertura,(((SELECT COALESCE(SUM(ccm.ccm_valor),0) from conta_corrente_mov as ccm WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_movimento = 'E' and ccm.ccm_ccorrente_id = cc.ccorrente_id and ccm.ccm_data <= @data_final) - (SELECT COALESCE(SUM(ccm.ccm_valor),0) from conta_corrente_mov as ccm WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_movimento = 'S' and ccm.ccm_ccorrente_id = cc.ccorrente_id and ccm.ccm_data <= @data_final)) + cc.ccorrente_saldo_abertura) as 'saldo' from conta_corrente as cc WHERE cc.ccorrente_conta_id = @conta_id and cc.ccorrente_status = 'Ativo';";
                dr_2_c.CommandText = "SELECT cc.ccorrente_id, cc.ccorrente_nome, cc.ccorrente_saldo_abertura, (((SELECT COALESCE(SUM(if(ccm.ccm_movimento = 'E',ccm.ccm_valor, -ccm.ccm_valor)),0.00) from conta_corrente_mov as ccm WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_ccorrente_id = cc.ccorrente_id and ccm.ccm_data <= @data_final)) + cc.ccorrente_saldo_abertura) as 'saldo' from conta_corrente as cc WHERE cc.ccorrente_conta_id = @conta_id and cc.ccorrente_status = 'Ativo';";
                dr_2_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_2_c.Parameters.AddWithValue("data_final", filtro.data_fim);
                dr_2 = dr_2_c.ExecuteReader();

                if (dr_2.HasRows)
                {
                    while (dr_2.Read())
                    {   
                        Vm_rfm_saldos_contas_correntes sf = new Vm_rfm_saldos_contas_correntes();
                        sf.conta_corrente_id = Convert.ToInt32(dr_2["ccorrente_id"]);
                        sf.conta_corrente_nome = dr_2["ccorrente_nome"].ToString();
                        sf.conta_corrente_saldo = Convert.ToDecimal(dr_2["saldo"]);
                        vm_rfm_saldo_final_contas.Add(sf);
                    }
                }
                dr_2.Close();
                //Fim

                //Busca lista de categorias e valores
                MySqlDataReader dr_3;
                MySqlCommand dr_3_c = conn.CreateCommand();
                dr_3_c.Connection = conn;
                dr_3_c.Transaction = Transacao;
                if(filtro.vm_rfm_filtros_visao == "caixa")
                {
                    //dr_3_c.CommandText = "SELECT cg.categoria_id, cg.categoria_classificacao, cg.categoria_nome, (COALESCE((SELECT sum(b.oppb_valor + b.oppb_juros + b.oppb_multa - b.oppb_desconto) as 'caixa' from op_parcelas_baixa as b LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id WHERE op.op_conta_id = @conta_id and b.oppb_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0) + COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm LEFT JOIN categoria as c_ on c_.categoria_id = ccm.ccm_contra_partida_id WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_data BETWEEN @data_inicial and @data_final and ccm.ccm_origem = 'CCM' and c_.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0) + COALESCE((SELECT sum(p.op_parcela_valor) from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id WHERE fp.fp_meio_pgto_nfe = '03' and p.op_parcela_vencimento BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND c.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)+ COALESCE((SELECT sum(p.op_parcela_valor) from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id LEFT JOIN categoria as cc on cc.categoria_id = fp.fp_categoria_id_cartao WHERE fp.fp_meio_pgto_nfe = '03' and p.op_parcela_vencimento BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND cc.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)+COALESCE((SELECT sum(m.mcc_valor) from movimentos_cartao_credito as m LEFT JOIN fatura_cartao_credito as f on f.fcc_id = m.mcc_fcc_id LEFT JOIN forma_pagamento as fp on fp.fp_id = f.fcc_forma_pagamento_id LEFT JOIN categoria as c on c.categoria_id = fp.fp_categoria_id_cartao_pagamento WHERE m.mcc_movimento = 'C' and f.fcc_conta_id = @conta_id and m.mcc_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)) as 'valor' from categoria as cg WHERE cg.categoria_conta_id = @conta_id and cg.categoria_dePlano = 'Não' order by cg.categoria_classificacao ASC;";
                    dr_3_c.CommandText = "SELECT cg.categoria_id, cg.categoria_classificacao, cg.categoria_nome, (round((COALESCE((SELECT sum(b.oppb_valor + b.oppb_juros + b.oppb_multa - b.oppb_desconto) as 'caixa' from op_parcelas_baixa as b LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id WHERE op.op_conta_id = @conta_id and b.oppb_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)+ COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm LEFT JOIN categoria as c_ on c_.categoria_id = ccm.ccm_contra_partida_id WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_data BETWEEN @data_inicial and @data_final and ccm.ccm_origem = 'CCM' and c_.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)+ COALESCE((SELECT sum(p.op_parcela_valor) from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id WHERE fp.fp_meio_pgto_nfe = '03' and op.op_data BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND c.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)+ COALESCE((SELECT sum(p.op_parcela_valor) from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id LEFT JOIN categoria as cc on cc.categoria_id = fp.fp_categoria_id_cartao WHERE fp.fp_meio_pgto_nfe = '03' and op.op_data BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND cc.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)+COALESCE((SELECT sum(m.mcc_valor) from movimentos_cartao_credito as m LEFT JOIN fatura_cartao_credito as f on f.fcc_id = m.mcc_fcc_id LEFT JOIN forma_pagamento as fp on fp.fp_id = f.fcc_forma_pagamento_id LEFT JOIN categoria as c on c.categoria_id = fp.fp_categoria_id_cartao_pagamento WHERE m.mcc_movimento = 'C' and f.fcc_conta_id = @conta_id and m.mcc_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(cg.categoria_classificacao,'%')),0)),2)) as 'valor' from categoria as cg WHERE cg.categoria_conta_id = @conta_id and cg.categoria_dePlano = 'Não' order by cg.categoria_classificacao ASC;";
                }
                else
                {
                    dr_3_c.CommandText = "SELECT c.categoria_id, c.categoria_classificacao, c.categoria_nome, (SELECT COALESCE(sum(round(op_parcelas.op_parcela_valor,2)),0) from op_parcelas LEFT JOIN operacao on operacao.op_id = op_parcelas.op_parcela_op_id left JOIN categoria on categoria.categoria_id = operacao.op_categoria_id WHERE categoria.categoria_classificacao LIKE concat(c.categoria_classificacao,'%') and op_parcelas.op_parcela_vencimento BETWEEN @data_inicial and @data_final AND operacao.op_conta_id = @conta_id) as 'valor' from categoria as c WHERE c.categoria_conta_id = @conta_id and c.categoria_status = 'Ativo' and c.categoria_dePlano = 'Não' ORDER by c.categoria_classificacao ASC;";
                }
                dr_3_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_3_c.Parameters.AddWithValue("data_inicial", filtro.data_inicio);
                dr_3_c.Parameters.AddWithValue("data_final", filtro.data_fim);
                dr_3 = dr_3_c.ExecuteReader();

                if (dr_3.HasRows)
                {
                    while (dr_3.Read())
                    {
                        Vm_rfm_categorias frm_c = new Vm_rfm_categorias();
                        frm_c.rfmc_categoria_id = Convert.ToInt32(dr_3["categoria_id"]);
                        frm_c.rfmc_categoria_descricao = dr_3["categoria_nome"].ToString();
                        frm_c.rfmc_categoria_classificacao = dr_3["categoria_classificacao"].ToString();
                        frm_c.rfmc_categoria_valor = Convert.ToDecimal(dr_3["valor"]);
                        if (filtro.vm_rfm_ignorar_categorias_zeradas)
                        {
                            if(frm_c.rfmc_categoria_valor > 0)
                            {
                                vm_rfm_categorias.Add(frm_c);
                            }
                        }
                        else
                        {
                            vm_rfm_categorias.Add(frm_c);
                        }
                    }
                }
                dr_3.Close();
                //Fim
                Transacao.Commit();

                rfm.filtro = filtro;
                rfm.vm_rfm_saldo_inicial_contas = vm_rfm_saldo_inicial_contas;
                rfm.vm_rfm_saldo_final_contas = vm_rfm_saldo_final_contas;
                rfm.vm_rfm_categorias = vm_rfm_categorias;
                rfm.status_processamento = "Ok";
            }
            catch (Exception e)
            {
                rfm.status_processamento = "Erro";
                rfm.erro_processaento = e.Message;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return rfm;
        }
    }
    
    //Classe saldos das contas
    public class Vm_rfm_saldos_contas_correntes
    {
        public int conta_corrente_id { get; set; }
        public string conta_corrente_nome { get; set; }
        public Decimal conta_corrente_saldo { get; set; }
    }

    //Categorias
    public class Vm_rfm_categorias
    {
        public int rfmc_categoria_id { get; set; }
        public string rfmc_categoria_classificacao { get; set; }
        public string rfmc_categoria_descricao { get; set; }        
        public Decimal rfmc_categoria_valor { get; set; }
    }

    //Filtros
    public class Vm_rfm_filtro
    {
        public string vm_rfm_filtros_visao { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_fim { get; set; }        
        public bool vm_rfm_ignorar_categorias_zeradas { get; set; }
    }
}
