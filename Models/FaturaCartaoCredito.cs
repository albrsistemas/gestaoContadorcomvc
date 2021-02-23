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

            if (contexto == "open")
            {
                FormaPagamento fp = new FormaPagamento();
                Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();
                vm_fp = fp.buscaFormasPagamento(conta_id, usuario_id, fcc_forma_pagamento_id);

                DateTime hj = DateTime.Today;
                vm_fcc.fcc_id = fcc_id;
                vm_fcc.fcc_data_corte = new DateTime(hj.Year, hj.Month, vm_fp.fp_dia_fechamento_cartao);
                vm_fcc.fcc_data_vencimento = new DateTime(hj.Year, hj.Month, vm_fp.fp_dia_vencimento_cartao);
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
                dr_1_c.CommandText = "SELECT * from fatura_cartao_credito WHERE fatura_cartao_credito.fcc_forma_pagamento_id = @fcc_forma_pagamento_id and fatura_cartao_credito.fcc_competencia = @fcc_competencia and fatura_cartao_credito.fcc_conta_id = @conta_id;";
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
                        vm_fcc.fcc_situacao = dr_1["fcc_situacao"].ToString();
                        vm_fcc.fcc_competencia = dr_1["fcc_competencia"].ToString();
                    }
                }
                else
                {
                    vm_fcc.fcc_situacao = "Aberta";
                }
                dr_1.Close();
                

                DateTime corte_data_fim = vm_fcc.fcc_data_corte;
                DateTime corte_data_inicio = vm_fcc.fcc_data_corte.AddDays(-30);
                DateTime venc_data_fim = vm_fcc.fcc_data_vencimento;
                DateTime venc_data_inicio = vm_fcc.fcc_data_vencimento.AddDays(-30);

                MySqlDataReader dr_2;
                MySqlCommand dr_2_c = conn.CreateCommand();
                dr_2_c.Connection = conn;
                dr_2_c.Transaction = Transacao;
                if (vm_fcc.fcc_situacao == "Fechada")
                {
                    dr_2_c.CommandText = "SELECT * from movimentos_cartao_credito as mcc WHERE mcc.mcc_fcc_id = @fcc_id ORDER by Date(mcc_data) ASC;";
                    dr_2_c.Parameters.AddWithValue("fcc_id", vm_fcc.fcc_id);
                }
                else
                {
                    dr_2_c.CommandText = "SELECT (0) as 'mcc_id', ('op_parcelas') as 'mcc_tipo', p.op_parcela_id as 'mcc_tipo_id', (0) as 'mcc_fcc_id', op.op_data as 'mcc_data', SUBSTRING((case WHEN op.op_tipo = 'Contas Financeiras' THEN concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ', cf.cf_nome) WHEN op.op_tipo <> 'ContasFinanceiras' THEN concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ', op.op_obs)END),1,150) as 'mcc_descricao', p.op_parcela_valor as 'mcc_valor', ('D') as 'mcc_movimento' from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id left JOIN categoria as c on c.categoria_id = op.op_categoria_id left JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id LEFT JOIN contas_financeiras as cf on cf.cf_op_id = op.op_id WHERE op.op_conta_id = @conta_id and fp.fp_id = @fp_id and fp.fp_identificacao = 'Pagamento' and p.op_parcelas_cartao = 0 and (op.op_data <= @corte_data_fim and p.op_parcela_vencimento BETWEEN @venc_data_inicio AND @venc_data_fim) UNION ALL SELECT * from movimentos_cartao_credito as mcc WHERE mcc.mcc_fcc_id = @fcc_id ORDER by Date(mcc_data) ASC;";
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
                    MySqlCommand comando = conn.CreateCommand();
                    MySqlTransaction Transacao;
                    Transacao = conn.BeginTransaction();
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

                        if(fcc.fcc_movimentos[0].mcc_id == 0)
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
                        }
                        else
                        {
                            comando.CommandText = "UPDATE movimentos_cartao_credito set movimentos_cartao_credito.mcc_fcc_id = @mcc_fcc_id;";
                            comando.Parameters.AddWithValue("mcc_fcc_id", id);
                            comando.ExecuteNonQuery();
                        }                        

                        //Atualizando a parcela para informar o número da fatura que pertence
                        comando.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcelas_cartao = @id WHERE op_parcelas.op_parcela_id = @tipo_id;";
                        comando.Parameters.AddWithValue("id", id);
                        comando.Parameters.AddWithValue("tipo_id", fcc.fcc_movimentos[0].mcc_tipo_id);
                        comando.ExecuteNonQuery();

                    }
                    else //Se tipo for 'op_parcelas' então criar o movimento. Se for tipo 'mcc' então fazer update
                    {
                        if (fcc.fcc_movimentos[0].mcc_tipo == "op_parcelas")
                        {
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

                            //Atualizando a parcela para informar o número da fatura que pertence
                            comando.CommandText = "UPDATE op_parcelas set op_parcelas.op_parcelas_cartao = @id WHERE op_parcelas.op_parcela_id = @tipo_id;";
                            comando.Parameters.AddWithValue("id", id_fcc);
                            comando.Parameters.AddWithValue("tipo_id", fcc.fcc_movimentos[0].mcc_tipo_id);
                            comando.ExecuteNonQuery();
                        }

                        if (fcc.fcc_movimentos[0].mcc_tipo == "mcc_op_parcelas")
                        {
                            comando.CommandText = "UPDATE movimentos_cartao_credito set movimentos_cartao_credito.mcc_fcc_id = @id_fcc WHERE movimentos_cartao_credito.mcc_id = @mcc_id;";
                            comando.Parameters.AddWithValue("id_fcc", id_fcc);
                            comando.Parameters.AddWithValue("mcc_id", fcc.fcc_movimentos[0].mcc_id);
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



    }
}
