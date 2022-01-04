using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class FaturaCartaoCredito
    {
        public int fcc_id { get; set; }
        public int fcc_forma_pagamento_id { get; set; }
        public string fcc_situacao { get; set; }
        public string fcc_competencia { get; set; }
        public DateTime fcc_data_corte { get; set; }
        public DateTime fcc_data_vencimento { get; set; }
        public List<MovimentosCartaoCredito> fcc_movimentos { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public FaturaCartaoCredito()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public Vm_FaturaCartaoCredito buscaFaturaCartao(int conta_id, int usuario_id, string contexto, int fcc_id, DateTime fcc_data_corte, DateTime fcc_data_vencimento, int fcc_forma_pagamento_id)
        {
            Vm_FaturaCartaoCredito vm_fcc = new Vm_FaturaCartaoCredito();
            vm_fcc.fcc_movimentos = new List<MovimentosCartaoCredito>();
            vm_fcc.fcc_data_corte = fcc_data_corte;
            vm_fcc.fcc_data_vencimento = fcc_data_vencimento;
            vm_fcc.fcc_forma_pagamento_id = fcc_forma_pagamento_id;
            ParcelamentoFaturaCartaoCredito parcelamento = new ParcelamentoFaturaCartaoCredito();

            if (contexto == "open")
            {
                FormaPagamento fp = new FormaPagamento();
                Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();
                vm_fp = fp.buscaFormasPagamento(conta_id, usuario_id, fcc_forma_pagamento_id);

                DateTime hj = DateTime.Today;

                vm_fcc.fcc_id = fcc_id;                

                vm_fcc.fcc_data_corte = new DateTime(hj.Year, hj.Month, vm_fp.fp_dia_fechamento_cartao);
                
                if(vm_fp.fp_dia_fechamento_cartao > vm_fp.fp_dia_vencimento_cartao)
                {
                    DateTime v = new DateTime(hj.Year, hj.Month, vm_fp.fp_dia_vencimento_cartao);
                    vm_fcc.fcc_data_vencimento = v.AddMonths(1);
                }
                else
                {
                    vm_fcc.fcc_data_vencimento = new DateTime(hj.Year, hj.Month, vm_fp.fp_dia_vencimento_cartao);
                }
                
                vm_fcc.fcc_forma_pagamento_id = fcc_forma_pagamento_id;
            }

            if (contexto == "previous")
            {
                vm_fcc.fcc_id = fcc_id;
                vm_fcc.fcc_data_corte = fcc_data_corte.AddMonths(-1);
                vm_fcc.fcc_data_vencimento = fcc_data_vencimento.AddMonths(-1);
                vm_fcc.fcc_forma_pagamento_id = fcc_forma_pagamento_id;
            }

            if (contexto == "next")
            {
                vm_fcc.fcc_id = fcc_id;
                vm_fcc.fcc_data_corte = fcc_data_corte.AddMonths(1);
                vm_fcc.fcc_data_vencimento = fcc_data_vencimento.AddMonths(1);
                vm_fcc.fcc_forma_pagamento_id = fcc_forma_pagamento_id;
            }

            conn.Open();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();

            try
            {
                string comp = vm_fcc.fcc_data_corte.Month.ToString().PadLeft(2, '0') + "/" + vm_fcc.fcc_data_corte.Year.ToString();
                //Verifica se fatura cartão existe
                MySqlDataReader dr_1;
                MySqlCommand dr_1_c = conn.CreateCommand();
                dr_1_c.Connection = conn;
                dr_1_c.Transaction = Transacao;
                dr_1_c.CommandText = "SELECT fp.fp_nome, fatura_cartao_credito.*, COALESCE(p.pfcc_id,0) as 'parcelamento', p.*, c.categoria_nome from fatura_cartao_credito LEFT JOIN forma_pagamento as fp on fp.fp_id = fatura_cartao_credito.fcc_forma_pagamento_id LEFT JOIN parcelamentofaturacartaocredito as p on p.pfcc_fcc_id = fatura_cartao_credito.fcc_id LEFT JOIN categoria as c on c.categoria_id = p.pfcc_categoria_id WHERE fatura_cartao_credito.fcc_forma_pagamento_id = @fcc_forma_pagamento_id and fatura_cartao_credito.fcc_competencia = @fcc_competencia and fatura_cartao_credito.fcc_conta_id = @conta_id;";
                dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_1_c.Parameters.AddWithValue("fcc_competencia", comp);
                dr_1_c.Parameters.AddWithValue("fcc_forma_pagamento_id", vm_fcc.fcc_forma_pagamento_id);
                dr_1 = dr_1_c.ExecuteReader();

                if (dr_1.HasRows)
                {
                    while (dr_1.Read())
                    {
                        if (DBNull.Value != dr_1["fcc_id"])
                        {
                            vm_fcc.fcc_id = Convert.ToInt32(dr_1["fcc_id"]);
                        }
                        else
                        {
                            vm_fcc.fcc_id = 0;
                        }

                        if (DBNull.Value != dr_1["fcc_conta_id"])
                        {
                            vm_fcc.fcc_conta_id = Convert.ToInt32(dr_1["fcc_conta_id"]);
                        }
                        else
                        {
                            vm_fcc.fcc_conta_id = 0;
                        }

                        if (DBNull.Value != dr_1["fcc_forma_pagamento_id"])
                        {
                            vm_fcc.fcc_forma_pagamento_id = Convert.ToInt32(dr_1["fcc_forma_pagamento_id"]);
                        }
                        else
                        {
                            vm_fcc.fcc_forma_pagamento_id = 0;
                        }

                        if(contexto == "open" || contexto == "previous" || contexto == "next")
                        {
                            if (DBNull.Value != dr_1["fcc_data_corte"])
                            {
                                vm_fcc.fcc_data_corte = Convert.ToDateTime(dr_1["fcc_data_corte"]);
                            }
                            else
                            {
                                vm_fcc.fcc_data_corte = new DateTime();
                            }

                            if (DBNull.Value != dr_1["fcc_data_vencimento"])
                            {
                                vm_fcc.fcc_data_vencimento = Convert.ToDateTime(dr_1["fcc_data_vencimento"]);
                            }
                            else
                            {
                                vm_fcc.fcc_data_vencimento = new DateTime();
                            }
                        }
                        vm_fcc.fcc_situacao = dr_1["fcc_situacao"].ToString();
                        vm_fcc.fcc_competencia = dr_1["fcc_competencia"].ToString();
                        vm_fcc.fcc_nome_cartao = dr_1["fp_nome"].ToString();

                        
                        vm_fcc.parcelamento = parcelamento;

                        if (DBNull.Value != dr_1["pfcc_id"])
                        {
                            vm_fcc.parcelamento.pfcc_id = Convert.ToInt32(dr_1["pfcc_id"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_id = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_fcc_id"])
                        {
                            vm_fcc.parcelamento.pfcc_fcc_id = Convert.ToInt32(dr_1["pfcc_fcc_id"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_fcc_id = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_categoria_id"])
                        {
                            vm_fcc.parcelamento.pfcc_categoria_id = Convert.ToInt32(dr_1["pfcc_categoria_id"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_categoria_id = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_numero_parcelas"])
                        {
                            vm_fcc.parcelamento.pfcc_numero_parcelas = Convert.ToInt32(dr_1["pfcc_numero_parcelas"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_numero_parcelas = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_total_fatura"])
                        {
                            vm_fcc.parcelamento.pfcc_total_fatura = Convert.ToDecimal(dr_1["pfcc_total_fatura"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_total_fatura = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_valor_parcelado"])
                        {
                            vm_fcc.parcelamento.pfcc_valor_parcelado = Convert.ToDecimal(dr_1["pfcc_valor_parcelado"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_valor_parcelado = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_valor_parcela"])
                        {
                            vm_fcc.parcelamento.pfcc_valor_parcela = Convert.ToDecimal(dr_1["pfcc_valor_parcela"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_valor_parcela = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_juros"])
                        {
                            vm_fcc.parcelamento.pfcc_juros = Convert.ToDecimal(dr_1["pfcc_juros"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_juros = 0;
                        }

                        if (DBNull.Value != dr_1["pfcc_data_parcelamento"])
                        {
                            vm_fcc.parcelamento.pfcc_data_parcelamento = Convert.ToDateTime(dr_1["pfcc_data_parcelamento"]);
                        }
                        else
                        {
                            vm_fcc.parcelamento.pfcc_data_parcelamento = new DateTime();
                        }

                        vm_fcc.parcelamento.categoria_nome = dr_1["categoria_nome"].ToString();
                    }
                }
                else
                {
                    vm_fcc.fcc_situacao = "Aberta";
                    vm_fcc.parcelamento = parcelamento;
                    vm_fcc.parcelamento.pfcc_id = 0;
                }
                dr_1.Close();

                //Datas
                DateTime corte_data_fim = vm_fcc.fcc_data_corte;
                DateTime corte_data_inicio = vm_fcc.fcc_data_corte.AddDays(-30);
                DateTime venc_data_fim = vm_fcc.fcc_data_vencimento;
                DateTime venc_data_inicio = vm_fcc.fcc_data_vencimento.AddDays(-30);

                //Pesquisando a data de corete fatura antetior
                DateTime mes_anterior_corte = vm_fcc.fcc_data_corte.AddMonths(-1);
                string comp_anter = mes_anterior_corte.Month.ToString().PadLeft(2, '0') + "/" + mes_anterior_corte.Year.ToString();
                //Verifica se fatura cartão existe
                MySqlDataReader dr_8;
                MySqlCommand dr_8_c = conn.CreateCommand();
                dr_8_c.Connection = conn;
                dr_8_c.Transaction = Transacao;
                dr_8_c.CommandText = "SELECT fp.fp_nome, fatura_cartao_credito.* from fatura_cartao_credito LEFT JOIN forma_pagamento as fp on fp.fp_id = fatura_cartao_credito.fcc_forma_pagamento_id WHERE fatura_cartao_credito.fcc_forma_pagamento_id = @fcc_forma_pagamento_id and fatura_cartao_credito.fcc_competencia = @fcc_competencia and fatura_cartao_credito.fcc_conta_id = @conta_id;";
                dr_8_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_8_c.Parameters.AddWithValue("fcc_competencia", comp_anter);
                dr_8_c.Parameters.AddWithValue("fcc_forma_pagamento_id", vm_fcc.fcc_forma_pagamento_id);
                dr_8 = dr_8_c.ExecuteReader();

                DateTime corte_mes_antetior = new DateTime();

                if (dr_8.HasRows)
                {
                    while (dr_8.Read())
                    {
                        if (DBNull.Value != dr_8["fcc_data_corte"])
                        {
                            corte_mes_antetior = Convert.ToDateTime(dr_8["fcc_data_corte"]);
                        }
                        else
                        {
                            corte_mes_antetior = new DateTime();
                        }

                        corte_data_inicio = corte_mes_antetior.AddDays(1);
                    }
                }                
                dr_8.Close();               

                MySqlDataReader dr_2;
                MySqlCommand dr_2_c = conn.CreateCommand();
                dr_2_c.Connection = conn;
                dr_2_c.Transaction = Transacao;
                if (vm_fcc.fcc_situacao == "Fechada")
                {
                    dr_2_c.CommandText = "SELECT * from movimentos_cartao_credito as mcc LEFT JOIN fatura_cartao_credito as f on f.fcc_id = mcc.mcc_fcc_id WHERE f.fcc_conta_id = @conta_id and mcc.mcc_fcc_id = @fcc_id ORDER by Date(mcc_data) ASC;";
                    dr_2_c.Parameters.AddWithValue("conta_id", conta_id);
                    dr_2_c.Parameters.AddWithValue("fcc_id", vm_fcc.fcc_id);
                }
                else
                {
                    dr_2_c.CommandText = "SELECT (0) as 'mcc_id', ('op_parcelas') as 'mcc_tipo', p.op_parcela_id as 'mcc_tipo_id', (0) as 'mcc_fcc_id', op.op_data as 'mcc_data', SUBSTRING((case WHEN op.op_tipo = 'Contas Financeiras' THEN concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ', cf.cf_nome) WHEN op.op_tipo <> 'ContasFinanceiras' THEN concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ', op.op_obs)END),1,150) as 'mcc_descricao', p.op_parcela_valor as 'mcc_valor', ('D') as 'mcc_movimento' from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id left JOIN categoria as c on c.categoria_id = op.op_categoria_id left JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id LEFT JOIN contas_financeiras as cf on cf.cf_op_id = op.op_id WHERE op.op_conta_id = @conta_id and fp.fp_id = @fp_id and fp.fp_identificacao = 'Pagamento' and p.op_parcelas_cartao = 0 and (op.op_data <= @corte_data_fim and p.op_parcela_vencimento BETWEEN @venc_data_inicio AND @venc_data_fim) UNION ALL SELECT mcc.* from movimentos_cartao_credito as mcc LEFT JOIN fatura_cartao_credito as f on f.fcc_id = mcc.mcc_fcc_id WHERE f.fcc_conta_id = @conta_id AND mcc.mcc_fcc_id = @fcc_id ORDER by Date(mcc_data) ASC;";                    
                    dr_2_c.Parameters.AddWithValue("conta_id", conta_id);
                    dr_2_c.Parameters.AddWithValue("corte_data_fim", corte_data_fim);
                    dr_2_c.Parameters.AddWithValue("venc_data_inicio", corte_data_inicio);
                    dr_2_c.Parameters.AddWithValue("venc_data_fim", corte_data_fim);
                    dr_2_c.Parameters.AddWithValue("fp_id", vm_fcc.fcc_forma_pagamento_id);
                    dr_2_c.Parameters.AddWithValue("fcc_id", vm_fcc.fcc_id);
                }
                dr_2 = dr_2_c.ExecuteReader();

                if (dr_2.HasRows)
                {
                    while (dr_2.Read())
                    {
                        MovimentosCartaoCredito mcc = new MovimentosCartaoCredito();
                        if (DBNull.Value != dr_2["mcc_id"])
                        {
                            mcc.mcc_id = Convert.ToInt32(dr_2["mcc_id"]);
                        }
                        else
                        {
                            mcc.mcc_id = 0;
                        }

                        if (DBNull.Value != dr_2["mcc_tipo_id"])
                        {
                            mcc.mcc_tipo_id = Convert.ToInt32(dr_2["mcc_tipo_id"]);
                        }
                        else
                        {
                            mcc.mcc_tipo_id = 0;
                        }

                        if (DBNull.Value != dr_2["mcc_fcc_id"])
                        {
                            mcc.mcc_fcc_id = Convert.ToInt32(dr_2["mcc_fcc_id"]);
                        }
                        else
                        {
                            mcc.mcc_fcc_id = 0;
                        }

                        if (DBNull.Value != dr_2["mcc_data"])
                        {
                            mcc.mcc_data = Convert.ToDateTime(dr_2["mcc_data"]);
                        }
                        else
                        {
                            mcc.mcc_data = new DateTime();
                        }

                        if (DBNull.Value != dr_2["mcc_valor"])
                        {
                            mcc.mcc_valor = Convert.ToDecimal(dr_2["mcc_valor"]);
                        }
                        else
                        {
                            mcc.mcc_valor = 0;
                        }

                        mcc.mcc_tipo = dr_2["mcc_tipo"].ToString();
                        mcc.mcc_descricao = dr_2["mcc_descricao"].ToString();
                        mcc.mcc_movimento = dr_2["mcc_movimento"].ToString();

                        vm_fcc.fcc_movimentos.Add(mcc);
                    }
                }
                dr_2.Close();
            }
            catch (Exception e)
            {
                string erro = e.Message;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return vm_fcc;
        }

        //Alteração de competência
        public string alocacaoCompetencia(int conta_id, int usuario_id, FaturaCartaoCredito fcc, DateTime competencia)
        {
            string retorno = "";
            //string comp = competencia.Month.ToString().PadLeft(2,'0') + "/" + competencia.Year.ToString();
            string comp = fcc.fcc_data_corte.Month.ToString().PadLeft(2, '0') + "/" + fcc.fcc_data_corte.Year.ToString();

            try
            {
                if (fcc.fcc_movimentos.Count > 1)
                {
                    retorno += " Erro, requisição com mais de uma movimento do cartão de crédito.";

                    return retorno;
                }
                else
                {
                    conn.Open();
                    MySqlTransaction Transacao;
                    Transacao = conn.BeginTransaction();
                    MySqlCommand comando = conn.CreateCommand();
                    comando.Connection = conn;
                    comando.Transaction = Transacao;
                                        
                    //Verifica se fatura cartão existe
                    MySqlDataReader dr_1;
                    MySqlCommand dr_1_c = conn.CreateCommand();
                    dr_1_c.Connection = conn;
                    dr_1_c.Transaction = Transacao;
                    dr_1_c.CommandText = "SELECT * from fatura_cartao_credito WHERE fatura_cartao_credito.fcc_forma_pagamento_id = @fcc_forma_pagamento_id and fatura_cartao_credito.fcc_competencia = @fcc_competencia and fatura_cartao_credito.fcc_conta_id = @conta_id;";
                    dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                    dr_1_c.Parameters.AddWithValue("fcc_competencia", comp);
                    dr_1_c.Parameters.AddWithValue("fcc_forma_pagamento_id", fcc.fcc_forma_pagamento_id);
                    dr_1 = dr_1_c.ExecuteReader();

                    int id_fcc = 0;
                    string situacao_fcc = "Aberta";

                    if (dr_1.HasRows)
                    {
                        while (dr_1.Read())
                        {
                            if (DBNull.Value != dr_1["fcc_id"])
                            {
                                id_fcc = Convert.ToInt32(dr_1["fcc_id"]);
                            }
                            else
                            {
                                id_fcc = 0;
                            }
                            situacao_fcc = dr_1["fcc_situacao"].ToString();
                        }
                    }                    
                    dr_1.Close();

                    if (id_fcc == 0) //Criar o fcc e depois o movimento
                    {
                        //Criando fcc                                                            
                        comando.CommandText = "INSERT into fatura_cartao_credito (fcc_conta_id, fcc_forma_pagamento_id, fcc_competencia, fcc_situacao, fcc_data_corte, fcc_data_vencimento) VALUES (@fcc_conta_id, @fcc_forma_pagamento_id, @fcc_competencia, @fcc_situacao, @fcc_data_corte, @fcc_data_vencimento);";
                        comando.Parameters.AddWithValue("fcc_conta_id", conta_id);
                        comando.Parameters.AddWithValue("fcc_forma_pagamento_id", fcc.fcc_forma_pagamento_id);
                        comando.Parameters.AddWithValue("fcc_competencia", comp);
                        comando.Parameters.AddWithValue("fcc_situacao", "Aberta");
                        comando.Parameters.AddWithValue("fcc_data_corte", fcc.fcc_data_corte);
                        comando.Parameters.AddWithValue("fcc_data_vencimento", fcc.fcc_data_vencimento);
                        comando.ExecuteNonQuery();

                        //recuperando id da fcc
                        int id = 0;
                        MySqlDataReader myReader;
                        comando.CommandText = "SELECT LAST_INSERT_ID();";
                        myReader = comando.ExecuteReader();
                        while (myReader.Read())
                        {
                            id = myReader.GetInt32(0);
                        }
                        myReader.Close();

                        int id_mcc = 0;

                        if (fcc.fcc_movimentos[0].mcc_id == 0)
                        {
                            //Criando o movimento de fcc
                            comando.CommandText = "INSERT into movimentos_cartao_credito (mcc_fcc_id, mcc_tipo, mcc_tipo_id, mcc_data, mcc_descricao, mcc_valor, mcc_movimento) values (@mcc_fcc_id, @mcc_tipo, @mcc_tipo_id, @mcc_data, @mcc_descricao, @mcc_valor, @mcc_movimento);";
                            comando.Parameters.AddWithValue("mcc_fcc_id", id);
                            comando.Parameters.AddWithValue("mcc_tipo", "mcc_op_parcelas");
                            comando.Parameters.AddWithValue("mcc_tipo_id", fcc.fcc_movimentos[0].mcc_tipo_id);
                            comando.Parameters.AddWithValue("mcc_data", fcc.fcc_movimentos[0].mcc_data);
                            comando.Parameters.AddWithValue("mcc_descricao", fcc.fcc_movimentos[0].mcc_descricao);
                            comando.Parameters.AddWithValue("mcc_valor", fcc.fcc_movimentos[0].mcc_valor);
                            comando.Parameters.AddWithValue("mcc_movimento", fcc.fcc_movimentos[0].mcc_movimento);
                            comando.ExecuteNonQuery();

                            MySqlDataReader reader_mcc;
                            comando.CommandText = "SELECT LAST_INSERT_ID();";
                            reader_mcc = comando.ExecuteReader();
                            while (reader_mcc.Read())
                            {
                                id_mcc = reader_mcc.GetInt32(0);
                            }
                            reader_mcc.Close();
                        }
                        else
                        {
                            comando.CommandText = "UPDATE movimentos_cartao_credito set movimentos_cartao_credito.mcc_fcc_id = @mcc_fcc_id WHERE movimentos_cartao_credito.mcc_id = @mcc_id;"; 
                            comando.Parameters.AddWithValue("mcc_fcc_id", id);
                            comando.Parameters.AddWithValue("mcc_id", fcc.fcc_movimentos[0].mcc_id);
                            comando.ExecuteNonQuery();
                        }                        

                        //Atualizando a parcela para informar o número da fatura que pertence
                        comando.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcelas_cartao = @id, op_parcelas_cartao_mcc_tipo_id = @id_mcc WHERE op_parcelas.op_parcela_id = @tipo_id;";
                        comando.Parameters.AddWithValue("id", id);
                        comando.Parameters.AddWithValue("id_mcc", id_mcc);
                        comando.Parameters.AddWithValue("tipo_id", fcc.fcc_movimentos[0].mcc_tipo_id);
                        comando.ExecuteNonQuery();

                    }
                    else //Se tipo for 'op_parcelas' então criar o movimento. Se for tipo 'mcc' então fazer update
                    {
                        if (situacao_fcc == "Fechada")
                        {
                            retorno = "Erro. A fatura destino está fechada.";
                            return retorno;
                        }

                        if (fcc.fcc_movimentos[0].mcc_tipo == "op_parcelas")
                        {
                            int id_mcc = 0;

                            //Criando o movimento de fcc
                            comando.CommandText = "INSERT into movimentos_cartao_credito (mcc_fcc_id, mcc_tipo, mcc_tipo_id, mcc_data, mcc_descricao, mcc_valor, mcc_movimento) values (@mcc_fcc_id, @mcc_tipo, @mcc_tipo_id, @mcc_data, @mcc_descricao, @mcc_valor, @mcc_movimento);";
                            comando.Parameters.AddWithValue("mcc_fcc_id", id_fcc);
                            comando.Parameters.AddWithValue("mcc_tipo", "mcc_op_parcelas");
                            comando.Parameters.AddWithValue("mcc_tipo_id", fcc.fcc_movimentos[0].mcc_tipo_id);
                            comando.Parameters.AddWithValue("mcc_data", fcc.fcc_movimentos[0].mcc_data);
                            comando.Parameters.AddWithValue("mcc_descricao", fcc.fcc_movimentos[0].mcc_descricao);
                            comando.Parameters.AddWithValue("mcc_valor", fcc.fcc_movimentos[0].mcc_valor);
                            comando.Parameters.AddWithValue("mcc_movimento", fcc.fcc_movimentos[0].mcc_movimento);
                            comando.ExecuteNonQuery();

                            MySqlDataReader reader_mcc;
                            comando.CommandText = "SELECT LAST_INSERT_ID();";
                            reader_mcc = comando.ExecuteReader();
                            while (reader_mcc.Read())
                            {
                                id_mcc = reader_mcc.GetInt32(0);
                            }
                            reader_mcc.Close();

                            //Atualizando a parcela para informar o número da fatura que pertence
                            comando.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcelas_cartao = @id, op_parcelas_cartao_mcc_tipo_id = @id_mcc WHERE op_parcelas.op_parcela_id = @tipo_id;";
                            comando.Parameters.AddWithValue("id", id_fcc);
                            comando.Parameters.AddWithValue("id_mcc", id_mcc);
                            comando.Parameters.AddWithValue("tipo_id", fcc.fcc_movimentos[0].mcc_tipo_id);
                            comando.ExecuteNonQuery();
                        }

                        if (fcc.fcc_movimentos[0].mcc_tipo == "mcc_op_parcelas")
                        {
                            comando.CommandText = "UPDATE movimentos_cartao_credito set movimentos_cartao_credito.mcc_fcc_id = @id_fcc WHERE movimentos_cartao_credito.mcc_id = @mcc_id;";
                            comando.Parameters.AddWithValue("id_fcc", id_fcc);
                            comando.Parameters.AddWithValue("mcc_id", fcc.fcc_movimentos[0].mcc_id);
                            comando.ExecuteNonQuery();

                            //Atualizando a parcela para informar o número da fatura que pertence
                            comando.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcelas_cartao = @id WHERE op_parcelas.op_parcela_id = @tipo_id;";
                            comando.Parameters.AddWithValue("id", id_fcc);                            
                            comando.Parameters.AddWithValue("tipo_id", fcc.fcc_movimentos[0].mcc_tipo_id);
                            comando.ExecuteNonQuery();
                        }
                    }

                    Transacao.Commit();
                    retorno = "Competência alterada com sucesso!";
                }
            }
            catch (Exception e)
            {
                string erro = e.Message;
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

        public AjustaParcelasCartao ajustaParcelasCartao_pesquisa (int conta_id, int usuario_id, string mcc_tipo, int mcc_tipo_id)
        {
            AjustaParcelasCartao apc = new AjustaParcelasCartao();
            apc.parcelas = new List<AjustaParcelasCartao_Parcelas>();
            apc.mcc_tipo = mcc_tipo;
            apc.mcc_tipo_id = mcc_tipo_id;


            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT op.op_id, COALESCE(cf.cf_valor_operacao,t.op_totais_total_op) as 'op_totais_total_op', p.op_parcela_id as 'parcela_id', p.op_parcela_vencimento as 'parcela_data', op.op_data as 'parcela_data_operacao', p.op_parcelas_cartao_mcc_tipo_id as 'parcela_fatura_cartao_mcc_tipo_id', p.op_parcela_valor as 'parcela_valor', p.op_parcelas_cartao as 'parcela_fatura_cartao', COALESCE(f.fcc_situacao,'Sem Fatura') as 'parcela_fatura_status', (p.op_parcela_ret_pis + p.op_parcela_ret_cofins + p.op_parcela_ret_csll + p.op_parcela_ret_inss + p.op_parcela_ret_issqn + p.op_parcela_ret_irrf) as 'parcela_retencoes', (0) as 'baixas', p.op_parcela_numero, p.op_parcela_numero_total from op_parcelas as p left JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN op_totais as t on t.op_totais_op_id = op.op_id LEFT JOIN fatura_cartao_credito as f on f.fcc_id = p.op_parcelas_cartao LEFT JOIN contas_financeiras as cf on cf.cf_op_id = op.op_id WHERE op.op_conta_id = @conta_id and p.op_parcela_op_id = (SELECT op_parcelas.op_parcela_op_id from op_parcelas WHERE op_parcelas.op_parcela_id = @parcela_id) order BY p.op_parcela_vencimento ASC;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@parcela_id", mcc_tipo_id);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        AjustaParcelasCartao_Parcelas apcp = new AjustaParcelasCartao_Parcelas();

                        if (DBNull.Value != leitor["parcela_id"])
                        {
                            apcp.parcela_id = Convert.ToInt32(leitor["parcela_id"]);
                        }
                        else
                        {
                            apcp.parcela_id = 0;
                        }

                        if (DBNull.Value != leitor["parcela_fatura_cartao"])
                        {
                            apcp.parcela_fatura_cartao = Convert.ToInt32(leitor["parcela_fatura_cartao"]);
                        }
                        else
                        {
                            apcp.parcela_fatura_cartao = 0;
                        }
                        
                        if (DBNull.Value != leitor["parcela_fatura_cartao_mcc_tipo_id"])
                        {
                            apcp.parcela_fatura_cartao_mcc_tipo_id = Convert.ToInt32(leitor["parcela_fatura_cartao_mcc_tipo_id"]);
                        }
                        else
                        {
                            apcp.parcela_fatura_cartao_mcc_tipo_id = 0;
                        }

                        if (DBNull.Value != leitor["parcela_data"])
                        {
                            apcp.parcela_data = Convert.ToDateTime(leitor["parcela_data"]);
                        }
                        else
                        {
                            apcp.parcela_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["parcela_data_operacao"])
                        {
                            apcp.parcela_data_operacao = Convert.ToDateTime(leitor["parcela_data_operacao"]);
                        }
                        else
                        {
                            apcp.parcela_data_operacao = new DateTime();
                        }

                        if (DBNull.Value != leitor["parcela_valor"])
                        {
                            apcp.parcela_valor = Convert.ToDecimal(leitor["parcela_valor"]);
                        }
                        else
                        {
                            apcp.parcela_valor = 0;
                        }

                        if (DBNull.Value != leitor["parcela_retencoes"])
                        {
                            apcp.parcela_retencoes = Convert.ToDecimal(leitor["parcela_retencoes"]);
                        }
                        else
                        {
                            apcp.parcela_retencoes = 0;
                        }

                        if (DBNull.Value != leitor["baixas"])
                        {
                            apcp.parcela_baixas = Convert.ToDecimal(leitor["baixas"]);
                        }
                        else
                        {
                            apcp.parcela_baixas = 0;
                        }

                        apcp.parcela_fatura_status = leitor["parcela_fatura_status"].ToString();
                        apc.parcelas.Add(apcp);

                        //Dados da operação
                        if (DBNull.Value != leitor["op_id"])
                        {
                            apc.operacao_id = Convert.ToInt32(leitor["op_id"]);
                        }
                        else
                        {
                            apc.operacao_id = 0;
                        }

                        if (DBNull.Value != leitor["op_totais_total_op"])
                        {
                            apc.operacao_valor_total = Convert.ToDecimal(leitor["op_totais_total_op"]);
                        }
                        else
                        {
                            apc.operacao_valor_total = 0;
                        }

                        if (DBNull.Value != leitor["op_parcela_numero"])
                        {
                            apcp.op_parcela_numero = Convert.ToInt32(leitor["op_parcela_numero"]);
                        }
                        else
                        {
                            apcp.op_parcela_numero = 0;
                        }

                        if (DBNull.Value != leitor["op_parcela_numero_total"])
                        {
                            apcp.op_parcela_numero_total = Convert.ToInt32(leitor["op_parcela_numero_total"]);
                        }
                        else
                        {
                            apcp.op_parcela_numero_total = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("FaturaCartaoCredito", "ajustaParcelasCartao_pesquisa", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return apc;
        }

        public List<string> ajustaParcelasCartao_gravar(AjustaParcelasCartao apc, int usuario_id, int conta_id)
        {
            List<string> retorno = new List<string>();
            string ids = "";
            Log logando = new Log();

            Decimal retencoes = 0;
            for (var i = 0; i < apc.parcelas.Count; i++)
            {
                retencoes += apc.parcelas[i].parcela_retencoes;
                ids += apc.parcelas[i].parcela_id.ToString() + " ";
            }

            if(retencoes > 0)
            {
                retorno.Add("Conjunto de parcelas possui retenções. Não pode ser alterado.");                
            }
            else
            {
                try
                {
                    conn.Open();
                    MySqlTransaction Transacao;
                    Transacao = conn.BeginTransaction();

                    for (var i = 0; i < apc.parcelas.Count; i++)
                    {
                        MySqlCommand cmd = conn.CreateCommand();
                        cmd.Connection = conn;
                        cmd.Transaction = Transacao;

                        cmd.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcela_valor = @op_parcela_valor, op_parcelas.op_parcela_valor_bruto = @op_parcela_valor WHERE op_parcelas.op_parcela_id = @op_parcela_id;";
                        cmd.Parameters.AddWithValue("@op_parcela_valor", apc.parcelas[i].parcela_valor);
                        cmd.Parameters.AddWithValue("@op_parcela_id", apc.parcelas[i].parcela_id);
                        cmd.ExecuteNonQuery();

                        if (apc.parcelas[i].parcela_fatura_cartao_mcc_tipo_id > 0)
                        {
                            cmd.CommandText = "UPDATE movimentos_cartao_credito set movimentos_cartao_credito.mcc_valor = @valor WHERE movimentos_cartao_credito.mcc_id = @mcc_id;";
                            cmd.Parameters.AddWithValue("@valor", apc.parcelas[i].parcela_valor);
                            cmd.Parameters.AddWithValue("@mcc_id", apc.parcelas[i].parcela_fatura_cartao_mcc_tipo_id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    Transacao.Commit();
                    logando.log("FaturaCartaoCredito", "ajustaParcelasCartao_gravar", "Sucesso", "As parcelas com os ID´s: " + ids + " foi ajustados os valores com sucesso", conta_id, usuario_id);

                }
                catch (Exception e)
                {
                    logando.log("FaturaCartaoCredito", "ajustaParcelasCartao_gravar", "Erro", "As parcelas com os ID´s: " + ids + " retornou falha na tentativa de ajustar o valor", conta_id, usuario_id);                    
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }

            return retorno;
        }

        //Pagamento
        public string PagamentoFatura(int conta_id, int usuario_id, Decimal valorPgto, DateTime dataPgto, int conta_corrente_Pgto, FaturaCartaoCredito fcc)
        {
            string retorno = "Pagamento realizado com sucesso";
            Decimal debito = 0;
            Decimal credito = 0;
            for(var i = 0; i < fcc.fcc_movimentos.Count; i++)
            {
                if(fcc.fcc_movimentos[i].mcc_movimento == "D")
                {
                    debito += fcc.fcc_movimentos[i].mcc_valor;
                }

                if (fcc.fcc_movimentos[i].mcc_movimento == "C")
                {
                    credito += fcc.fcc_movimentos[i].mcc_valor;
                }
            }
            bool d = true;
            DateTime dataTemp;
            d = DateTime.TryParse(dataPgto.ToShortDateString(), out dataTemp);

            if(valorPgto > debito || valorPgto > (debito - credito) || valorPgto < 0 || d == false)
            {
                retorno = "Erro, valor do pagamento inválido. Não pode ser menor que zero ou maior que o débito ou saldo. ";
                if(d == false)
                {
                    retorno += "Erro, data Inválida!";
                }
                
                return retorno;
            }

            string comp = fcc.fcc_data_corte.Month.ToString().PadLeft(2, '0') + "/" + fcc.fcc_data_corte.Year.ToString();

            try
            {
                conn.Open();
                MySqlTransaction Transacao;
                Transacao = conn.BeginTransaction();
                MySqlCommand comando = conn.CreateCommand();
                comando.Connection = conn;
                comando.Transaction = Transacao;

                //Verifica se fatura cartão existe                
                MySqlDataReader dr_1;
                MySqlCommand dr_1_c = conn.CreateCommand();
                dr_1_c.Connection = conn;
                dr_1_c.Transaction = Transacao;
                dr_1_c.CommandText = "SELECT * from fatura_cartao_credito WHERE fatura_cartao_credito.fcc_forma_pagamento_id = @fcc_forma_pagamento_id and fatura_cartao_credito.fcc_competencia = @fcc_competencia and fatura_cartao_credito.fcc_conta_id = @conta_id;";
                dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_1_c.Parameters.AddWithValue("fcc_competencia", comp);
                dr_1_c.Parameters.AddWithValue("fcc_forma_pagamento_id", fcc.fcc_forma_pagamento_id);
                dr_1 = dr_1_c.ExecuteReader();

                int id_fcc = 0;

                if (dr_1.HasRows)
                {
                    while (dr_1.Read())
                    {
                        if (DBNull.Value != dr_1["fcc_id"])
                        {
                            id_fcc = Convert.ToInt32(dr_1["fcc_id"]);
                        }
                        else
                        {
                            id_fcc = 0;
                        }
                    }
                }
                dr_1.Close();

                if(id_fcc == 0)                {
                    //Criando fcc                                                            
                    comando.CommandText = "INSERT into fatura_cartao_credito (fcc_conta_id, fcc_forma_pagamento_id, fcc_competencia, fcc_situacao, fcc_data_corte, fcc_data_vencimento) VALUES (@fcc_conta_id, @fcc_forma_pagamento_id, @fcc_competencia, @fcc_situacao, @fcc_data_corte, @fcc_data_vencimento);";
                    comando.Parameters.AddWithValue("fcc_conta_id", conta_id);
                    comando.Parameters.AddWithValue("fcc_forma_pagamento_id", fcc.fcc_forma_pagamento_id);
                    comando.Parameters.AddWithValue("fcc_competencia", comp);
                    comando.Parameters.AddWithValue("fcc_situacao", "Aberta");
                    comando.Parameters.AddWithValue("fcc_data_corte", fcc.fcc_data_corte);
                    comando.Parameters.AddWithValue("fcc_data_vencimento", fcc.fcc_data_vencimento);
                    comando.ExecuteNonQuery();

                    //recuperando id da fcc                    
                    MySqlDataReader myReader;
                    comando.CommandText = "SELECT LAST_INSERT_ID();";
                    myReader = comando.ExecuteReader();
                    while (myReader.Read())
                    {
                        id_fcc = myReader.GetInt32(0);
                    }
                    myReader.Close();
                }

                comando.CommandText = "INSERT into movimentos_cartao_credito (mcc_tipo, mcc_tipo_id, mcc_fcc_id, mcc_data,mcc_descricao, mcc_valor, mcc_movimento) values ('Pagamento',0, @id_fcc, @dataPgto, concat('Pagamento realizado'),@valorPgto,'C');";
                comando.Parameters.AddWithValue("id_fcc", id_fcc);
                comando.Parameters.AddWithValue("dataPgto", dataPgto);                
                comando.Parameters.AddWithValue("valorPgto", valorPgto);
                comando.ExecuteNonQuery();

                int id_mov = 0;
                
                //recuperando id do movimento                    
                MySqlDataReader myReader2;
                comando.CommandText = "SELECT LAST_INSERT_ID();";
                myReader2 = comando.ExecuteReader();
                while (myReader2.Read())
                {
                    id_mov = myReader2.GetInt32(0);
                }
                myReader2.Close();

                if(id_mov > 0)
                {
                    //Cria movimento de caixa
                    comando.CommandText = "INSERT into conta_corrente_mov (ccm_conta_id, ccm_ccorrente_id, ccm_movimento, ccm_contra_partida_tipo, ccm_contra_partida_id, ccm_data, ccm_data_competencia, ccm_valor, ccm_memorando, ccm_origem, ccm_origem_id) values (@conta_id_1, @ccm_ccorrente_id, 'S', 'Fatura Cartão', @ccm_contra_partida_id, @ccm_data, @ccm_data_competencia, @ccm_valor, concat('Pagamento fatura do cartão de crédito ref.: ', @comp_1),'Mov. Cartão', @ccm_origem_id);";
                    comando.Parameters.AddWithValue("conta_id_1", conta_id);
                    comando.Parameters.AddWithValue("ccm_ccorrente_id", conta_corrente_Pgto);
                    comando.Parameters.AddWithValue("ccm_contra_partida_id", id_fcc);
                    comando.Parameters.AddWithValue("ccm_data", dataPgto);
                    comando.Parameters.AddWithValue("ccm_data_competencia", fcc.fcc_data_corte);
                    comando.Parameters.AddWithValue("ccm_valor", valorPgto);
                    comando.Parameters.AddWithValue("comp_1", comp);
                    comando.Parameters.AddWithValue("ccm_origem_id", id_mov);
                    comando.ExecuteNonQuery();
                }

                if (id_mov > 0 && id_fcc > 0)
                {
                    Transacao.Commit();
                }
                else
                {
                    retorno = "Erro na gravação dos dados no banco!";
                }

                
                log.log("FaturaCartaoCredito", "PagamentoFatura", "Sucesso", "Pagamento da fatura: " + comp + " realizada com sucesso", conta_id, usuario_id);

            }
            catch (Exception e)
            {
                log.log("FaturaCartaoCredito", "PagamentoFatura", "Erro", "Pagamento da fatura: " + comp + " retornou falha", conta_id, usuario_id);
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

        public string fechar_abrir_cartao(int conta_id, int usuario_id, string contexto, FaturaCartaoCredito fcc)
        {
            string retorno = "";

            string comp = fcc.fcc_data_corte.Month.ToString().PadLeft(2, '0') + "/" + fcc.fcc_data_corte.Year.ToString();

            try
            {
                conn.Open();
                MySqlTransaction Transacao;
                Transacao = conn.BeginTransaction();
                MySqlCommand comando = conn.CreateCommand();
                comando.Connection = conn;
                comando.Transaction = Transacao;

                //Verifica se fatura cartão existe                
                MySqlDataReader dr_1;
                MySqlCommand dr_1_c = conn.CreateCommand();
                dr_1_c.Connection = conn;
                dr_1_c.Transaction = Transacao;
                dr_1_c.CommandText = "SELECT * from fatura_cartao_credito WHERE fatura_cartao_credito.fcc_forma_pagamento_id = @fcc_forma_pagamento_id and fatura_cartao_credito.fcc_competencia = @fcc_competencia and fatura_cartao_credito.fcc_conta_id = @conta_id;";
                dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_1_c.Parameters.AddWithValue("fcc_competencia", comp);
                dr_1_c.Parameters.AddWithValue("fcc_forma_pagamento_id", fcc.fcc_forma_pagamento_id);
                dr_1 = dr_1_c.ExecuteReader();

                int id_fcc = 0;

                if (dr_1.HasRows)
                {
                    while (dr_1.Read())
                    {
                        if (DBNull.Value != dr_1["fcc_id"])
                        {
                            id_fcc = Convert.ToInt32(dr_1["fcc_id"]);
                        }
                        else
                        {
                            id_fcc = 0;
                        }
                    }
                }
                dr_1.Close();

                if (id_fcc == 0)
                {
                    //Criando fcc                                                            
                    comando.CommandText = "INSERT into fatura_cartao_credito (fcc_conta_id, fcc_forma_pagamento_id, fcc_competencia, fcc_situacao, fcc_data_corte, fcc_data_vencimento) VALUES (@fcc_conta_id, @fcc_forma_pagamento_id1, @fcc_competencia1, @fcc_situacao, @fcc_data_corte, @fcc_data_vencimento);";
                    comando.Parameters.AddWithValue("fcc_conta_id", conta_id);
                    comando.Parameters.AddWithValue("fcc_forma_pagamento_id1", fcc.fcc_forma_pagamento_id);
                    comando.Parameters.AddWithValue("fcc_competencia1", comp);
                    comando.Parameters.AddWithValue("fcc_situacao", "Fechada");
                    comando.Parameters.AddWithValue("fcc_data_corte", fcc.fcc_data_corte);
                    comando.Parameters.AddWithValue("fcc_data_vencimento", fcc.fcc_data_vencimento);
                    comando.ExecuteNonQuery();

                    //recuperando id da fcc                    
                    MySqlDataReader myReader;
                    comando.CommandText = "SELECT LAST_INSERT_ID();";
                    myReader = comando.ExecuteReader();
                    while (myReader.Read())
                    {
                        id_fcc = myReader.GetInt32(0);
                    }
                    myReader.Close();
                }

                if (id_fcc > 0)
                {
                    if (contexto == "abrir")
                    {
                        comando.CommandText = "UPDATE fatura_cartao_credito set fatura_cartao_credito.fcc_situacao = 'Aberta', fatura_cartao_credito.fcc_data_corte = @fcc_data_corte, fatura_cartao_credito.fcc_data_vencimento = @fcc_data_vencimento WHERE fatura_cartao_credito.fcc_id = @id_fcc and fatura_cartao_credito.fcc_conta_id = @conta_id_2;";
                        comando.Parameters.AddWithValue("conta_id_2", conta_id);
                        comando.Parameters.AddWithValue("id_fcc", id_fcc);
                        comando.Parameters.AddWithValue("fcc_data_corte", fcc.fcc_data_corte);
                        comando.Parameters.AddWithValue("fcc_data_vencimento", fcc.fcc_data_vencimento);
                        comando.ExecuteNonQuery();

                        retorno = "Fatura aberta com sucesso!";
                    }

                    if (contexto == "fechar")
                    {
                        for (var i = 0; i < fcc.fcc_movimentos.Count; i++)
                        {
                            MySqlCommand cmd = conn.CreateCommand();
                            cmd.Connection = conn;
                            cmd.Transaction = Transacao;

                            if (fcc.fcc_movimentos[i].mcc_tipo == "op_parcelas")
                            {
                                int id_mcc = 0;

                                //Criando o movimento de fcc
                                cmd.CommandText = "INSERT into movimentos_cartao_credito (mcc_fcc_id, mcc_tipo, mcc_tipo_id, mcc_data, mcc_descricao, mcc_valor, mcc_movimento) values (@mcc_fcc_id1, @mcc_tipo, @mcc_tipo_id, @mcc_data, @mcc_descricao, @mcc_valor, @mcc_movimento);";
                                cmd.Parameters.AddWithValue("mcc_fcc_id1", id_fcc);
                                cmd.Parameters.AddWithValue("mcc_tipo", "mcc_op_parcelas");
                                cmd.Parameters.AddWithValue("mcc_tipo_id", fcc.fcc_movimentos[i].mcc_tipo_id);
                                cmd.Parameters.AddWithValue("mcc_data", fcc.fcc_movimentos[i].mcc_data);
                                cmd.Parameters.AddWithValue("mcc_descricao", fcc.fcc_movimentos[i].mcc_descricao);
                                cmd.Parameters.AddWithValue("mcc_valor", fcc.fcc_movimentos[i].mcc_valor);
                                cmd.Parameters.AddWithValue("mcc_movimento", fcc.fcc_movimentos[i].mcc_movimento);
                                cmd.ExecuteNonQuery();

                                MySqlDataReader reader_mcc;
                                cmd.CommandText = "SELECT LAST_INSERT_ID();";
                                reader_mcc = cmd.ExecuteReader();
                                while (reader_mcc.Read())
                                {
                                    id_mcc = reader_mcc.GetInt32(0);
                                }
                                reader_mcc.Close();

                                //Atualizando a parcela para informar o número da fatura que pertence
                                cmd.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcelas_cartao = @id, op_parcelas_cartao_mcc_tipo_id = @id_mcc WHERE op_parcelas.op_parcela_id = @tipo_id;";
                                cmd.Parameters.AddWithValue("id", id_fcc);
                                cmd.Parameters.AddWithValue("id_mcc", id_mcc);
                                cmd.Parameters.AddWithValue("tipo_id", fcc.fcc_movimentos[i].mcc_tipo_id);
                                cmd.ExecuteNonQuery();
                            }

                            if (fcc.fcc_movimentos[i].mcc_tipo == "mcc_op_parcelas")
                            {
                                cmd.CommandText = "UPDATE movimentos_cartao_credito set movimentos_cartao_credito.mcc_fcc_id = @id_fcc2 WHERE movimentos_cartao_credito.mcc_id = @mcc_id;";
                                cmd.Parameters.AddWithValue("id_fcc2", id_fcc);
                                cmd.Parameters.AddWithValue("mcc_id", fcc.fcc_movimentos[i].mcc_id);
                                cmd.ExecuteNonQuery();

                                //Atualizando a parcela para informar o número da fatura que pertence
                                cmd.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcelas_cartao = @id WHERE op_parcelas.op_parcela_id = @tipo_id1;";
                                cmd.Parameters.AddWithValue("id", id_fcc);
                                cmd.Parameters.AddWithValue("tipo_id1", fcc.fcc_movimentos[i].mcc_tipo_id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        comando.CommandText = "UPDATE fatura_cartao_credito set fatura_cartao_credito.fcc_situacao = 'Fechada' WHERE fatura_cartao_credito.fcc_id = @id_fcc3 and fatura_cartao_credito.fcc_conta_id = @conta_id3;";
                        comando.Parameters.AddWithValue("conta_id3", conta_id);
                        comando.Parameters.AddWithValue("id_fcc3", id_fcc);
                        comando.ExecuteNonQuery();

                        retorno = "Fatura fechada com sucesso!";
                    }

                    Transacao.Commit();

                    log.log("FaturaCartaoCredito", "fechar_abrir_cartao", "Sucessso", "Sucesso ao tentar: " + contexto + " a fatura do cartão de crédito ref.: " + comp, conta_id, usuario_id);
                }
                else
                {
                    retorno = "Erro na pesquisa da fatura. Fatura não localizada!";
                }
            }
            catch (Exception e)
            {
                retorno = "Erro ao processa a solicitação!" + e.Message;

                log.log("FaturaCartaoCredito", "fechar_abrir_cartao", "Erro", "Falha ao tentar: " + contexto + " a fatura do cartão de crédito ref.: " + comp, conta_id, usuario_id);
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

        public string deletePagamento(int conta_id, int usuario_id, int mcc_id)
        {
            string retorno = "";

            conn.Open();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            MySqlCommand comando = conn.CreateCommand();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "DELETE FROM movimentos_cartao_credito WHERE movimentos_cartao_credito.mcc_id = @mcc_id1;";
                comando.Parameters.AddWithValue("mcc_id1", mcc_id);
                comando.ExecuteNonQuery();
                comando.CommandText = "DELETE from conta_corrente_mov WHERE conta_corrente_mov.ccm_origem_id = @mcc_id";
                comando.Parameters.AddWithValue("mcc_id", mcc_id);                
                comando.ExecuteNonQuery();

                Transacao.Commit();

                retorno = "Pagamento excluído com sucesso!";

                log.log("FaturaCartaoCredito", "deletePagamento", "Sucesso", "Pagamento ref. mcc_id: " + mcc_id + " excluído com sucesso.", conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao escluir o pagamento. " + e.Message;

                log.log("FaturaCartaoCredito", "deletePagamento", "Erro", "Falha ao tentar excluir o pagamento mcc_id: " + mcc_id, conta_id, usuario_id);
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

        public string edit_datas_cartao(DateTime data_fechamento, DateTime data_vencimento, int fcc_id, int conta_id)
        {
            string retorno = "";

            conn.Open();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            MySqlCommand comando = conn.CreateCommand();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE fatura_cartao_credito set fatura_cartao_credito.fcc_data_corte = @data_fechamento, fatura_cartao_credito.fcc_data_vencimento = @data_vencimento WHERE fatura_cartao_credito.fcc_conta_id = @conta_id and fatura_cartao_credito.fcc_id = @fcc_id;";
                comando.Parameters.AddWithValue("data_fechamento", data_fechamento);                
                comando.Parameters.AddWithValue("data_vencimento", data_vencimento);                
                comando.Parameters.AddWithValue("conta_id", conta_id);                
                comando.Parameters.AddWithValue("fcc_id", fcc_id);                
                comando.ExecuteNonQuery();

                Transacao.Commit();

                retorno = "Data alterada com sucesso!";
            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar a data! ";
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

    public class AjustaParcelasCartao
    {
        public string validacao { get; set; }
        public List<String> retorno { get; set; }        
        public int operacao_id { get; set; }
        public Decimal operacao_valor_total { get; set; }
        public string mcc_tipo { get; set; }
        public int mcc_tipo_id { get; set; }
        public List<AjustaParcelasCartao_Parcelas> parcelas { get; set; }
    }
    public class AjustaParcelasCartao_Parcelas
    {
        public int parcela_id { get; set; }
        public DateTime parcela_data { get; set; }
        public DateTime parcela_data_operacao { get; set; }
        public Decimal parcela_valor { get; set; }
        public int parcela_fatura_cartao { get; set; }
        public int parcela_fatura_cartao_mcc_tipo_id { get; set; }
        public int op_parcela_numero { get; set; }
        public int op_parcela_numero_total { get; set; }
        public string parcela_fatura_status { get; set; }
        public Decimal parcela_retencoes { get; set; }
        public Decimal parcela_baixas { get; set; }
    }
}

