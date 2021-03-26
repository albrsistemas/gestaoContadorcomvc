using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_rfm_details
    {
        public DateTime data { get; set; }
        public Decimal valor { get; set; }
        public string memorando { get; set; }
        public string origem { get; set; }


        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Vm_rfm_details()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }


        public List<Vm_rfm_details> rfm_details(int conta_id, DateTime data_inicio, DateTime data_fim, string categoria_classificacao)
        {
            List<Vm_rfm_details> lista = new List<Vm_rfm_details>();

            conn.Open();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            try
            {
                MySqlDataReader dr_1;
                MySqlCommand dr_1_c = conn.CreateCommand();
                dr_1_c.Connection = conn;
                dr_1_c.Transaction = Transacao;
                //dr_1_c.CommandText = "SELECT * from (SELECT b.oppb_data as 'data', (b.oppb_valor + b.oppb_juros + b.oppb_multa - b.oppb_desconto) as 'valor', op.op_obs as 'memorando', 'Baixas' as 'origem' from op_parcelas_baixa as b LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id WHERE op.op_conta_id = @conta_id and b.oppb_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT ccm.ccm_data as 'data', ccm.ccm_valor as 'valor', ccm.ccm_memorando as 'memorando', 'CCM' as 'origem' from conta_corrente_mov as ccm LEFT JOIN categoria as c_ on c_.categoria_id = ccm.ccm_contra_partida_id WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_data BETWEEN @data_inicial and @data_final and ccm.ccm_origem = 'CCM' and c_.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT op.op_data as 'data', p.op_parcela_valor as 'valor', op.op_obs as 'memorando', 'Cartão' as 'origem' from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id WHERE fp.fp_meio_pgto_nfe = '03' and op.op_data BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND c.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT op.op_data as 'data', p.op_parcela_valor as 'valor', op.op_obs as 'memorando', 'Cartão' as 'origem' from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id LEFT JOIN categoria as cc on cc.categoria_id = fp.fp_categoria_id_cartao WHERE fp.fp_meio_pgto_nfe = '03' and op.op_data BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND cc.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT m.mcc_data as 'data', m.mcc_valor as 'valor', m.mcc_descricao as 'memorando', 'Cartão' as 'origem' from movimentos_cartao_credito as m LEFT JOIN fatura_cartao_credito as f on f.fcc_id = m.mcc_fcc_id LEFT JOIN forma_pagamento as fp on fp.fp_id = f.fcc_forma_pagamento_id LEFT JOIN categoria as c on c.categoria_id = fp.fp_categoria_id_cartao_pagamento WHERE m.mcc_movimento = 'C' and f.fcc_conta_id = @conta_id and m.mcc_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT b.oppb_data as 'data', (b.oppb_valor + b.oppb_juros + b.oppb_multa - b.oppb_desconto) as 'valor', op.op_obs as 'memorando', 'Cartão' as 'origem' from op_parcelas_baixa as b LEFT JOIN operacao as op on op.op_id = b.oppb_op_id LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN fechamento_cartao as fc on fc.fc_op_id = op.op_id LEFT JOIN forma_pagamento as fpg on fpg.fp_id = fc.fc_forma_pagamento LEFT JOIN categoria as cfp on cfp.categoria_id = fpg.fp_categoria_id_cartao_pagamento WHERE op.op_tipo = 'Fechamento Cartão' and op.op_conta_id = @conta_id and b.oppb_data BETWEEN @data_inicial and @data_final and cfp.categoria_classificacao like concat(@categoria_classificacao,'%')) a order by data asc;";
                dr_1_c.CommandText = "SELECT * from (SELECT p.op_parcela_id as id, b.oppb_data as 'data', (b.oppb_valor + b.oppb_juros + b.oppb_multa - b.oppb_desconto) as 'valor', concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ',op.op_obs) as 'memorando', 'Baixas' as 'origem' from op_parcelas_baixa as b LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id WHERE op.op_conta_id = @conta_id and b.oppb_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT ccm.ccm_id as id, ccm.ccm_data as 'data', ccm.ccm_valor as 'valor', ccm.ccm_memorando as 'memorando', 'CCM' as 'origem' from conta_corrente_mov as ccm LEFT JOIN categoria as c_ on c_.categoria_id = ccm.ccm_contra_partida_id WHERE ccm.ccm_conta_id = @conta_id and ccm.ccm_data BETWEEN @data_inicial and @data_final and ccm.ccm_origem = 'CCM' and c_.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT p.op_parcela_id as id, op.op_data as 'data', p.op_parcela_valor as 'valor', concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ',op.op_obs) as 'memorando', 'Cartão' as 'origem' from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id WHERE fp.fp_meio_pgto_nfe = '03' and op.op_data BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND c.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT p.op_parcela_id as id, op.op_data as 'data', p.op_parcela_valor as 'valor', concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ',op.op_obs) as 'memorando', 'Cartão' as 'origem' from op_parcelas as p LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id LEFT JOIN categoria as c on c.categoria_id = op.op_categoria_id LEFT JOIN forma_pagamento as fp on fp.fp_id = p.op_parcela_fp_id LEFT JOIN categoria as cc on cc.categoria_id = fp.fp_categoria_id_cartao WHERE fp.fp_meio_pgto_nfe = '03' and op.op_data BETWEEN @data_inicial and @data_final and op.op_conta_id = @conta_id AND cc.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT m.mcc_id as id, m.mcc_data as 'data', m.mcc_valor as 'valor', m.mcc_descricao as 'memorando', 'Cartão' as 'origem' from movimentos_cartao_credito as m LEFT JOIN fatura_cartao_credito as f on f.fcc_id = m.mcc_fcc_id LEFT JOIN forma_pagamento as fp on fp.fp_id = f.fcc_forma_pagamento_id LEFT JOIN categoria as c on c.categoria_id = fp.fp_categoria_id_cartao_pagamento WHERE m.mcc_movimento = 'C' and f.fcc_conta_id = @conta_id and m.mcc_data BETWEEN @data_inicial and @data_final and c.categoria_classificacao like concat(@categoria_classificacao,'%') UNION ALL SELECT p.op_parcela_id as id, b.oppb_data as 'data', (b.oppb_valor + b.oppb_juros + b.oppb_multa - b.oppb_desconto) as 'valor', concat('Parcela ', p.op_parcela_numero, ' de ', p.op_parcela_numero_total, ' ref.: ',op.op_obs) as 'memorando', 'Cartão' as 'origem' from op_parcelas_baixa as b LEFT JOIN operacao as op on op.op_id = b.oppb_op_id LEFT JOIN op_parcelas as p on p.op_parcela_id = b.oppb_op_parcela_id LEFT JOIN fechamento_cartao as fc on fc.fc_op_id = op.op_id LEFT JOIN forma_pagamento as fpg on fpg.fp_id = fc.fc_forma_pagamento LEFT JOIN categoria as cfp on cfp.categoria_id = fpg.fp_categoria_id_cartao_pagamento WHERE op.op_tipo = 'Fechamento Cartão' and op.op_conta_id = @conta_id and b.oppb_data BETWEEN @data_inicial and @data_final and cfp.categoria_classificacao like concat(@categoria_classificacao,'%')) a order by data, id asc;";
                dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_1_c.Parameters.AddWithValue("data_inicial", data_inicio);
                dr_1_c.Parameters.AddWithValue("data_final", data_fim);
                dr_1_c.Parameters.AddWithValue("categoria_classificacao", categoria_classificacao);
                dr_1 = dr_1_c.ExecuteReader();

                if (dr_1.HasRows)
                {
                    while (dr_1.Read())
                    {
                        Vm_rfm_details d = new Vm_rfm_details();
                        if (DBNull.Value != dr_1["data"])
                        {
                            d.data = Convert.ToDateTime(dr_1["data"]);
                        }
                        else
                        {
                            d.data = new DateTime();
                        }

                        if (DBNull.Value != dr_1["valor"])
                        {
                            d.valor = Convert.ToDecimal(dr_1["valor"]);
                        }
                        else
                        {
                            d.valor = 0;
                        }

                        d.memorando = dr_1["memorando"].ToString();
                        d.origem = dr_1["origem"].ToString();

                        lista.Add(d);
                    }
                }
                dr_1.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return lista;
        }
    }
}
