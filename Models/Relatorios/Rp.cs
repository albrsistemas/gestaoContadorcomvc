using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Relatorios
{
    public class Rp
    {
        public int participante_id { get; set; }
        public string participante_codigo { get; set; }
        public string participante_nome { get; set; }
        public Decimal jan { get; set; }
        public Decimal fev { get; set; }
        public Decimal marc { get; set; }
        public Decimal abr { get; set; }
        public Decimal mai { get; set; }
        public Decimal jun { get; set; }
        public Decimal jul { get; set; }
        public Decimal ago { get; set; }
        public Decimal sete { get; set; }
        public Decimal outu { get; set; }
        public Decimal nov { get; set; }
        public Decimal dez { get; set; }
        public Decimal total { get; set; }       

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Rp()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        public Relatorio_participante create(int conta_id, string ano, bool ignorar_zerados, bool ocultar_nomes, int[] tipos_particiapnte)
        {
            string p = "";            
            if(tipos_particiapnte != null)
            {
                if (tipos_particiapnte.Length > 0)
                {
                    for (var i = 0; i < tipos_particiapnte.Length; i++)
                    {
                        if (i == 0)
                        {
                            p += tipos_particiapnte[i];
                        }

                        if (i != 0)
                        {
                            p += "," + tipos_particiapnte[i];
                        }
                    }
                }
            }

            Relatorio_participante relatorio_Participante = new Relatorio_participante();
            Rp total = new Rp();
            List<Rp> rps = new List<Rp>();

            conn.Open();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            try
            {   
                MySqlDataReader dr_1;
                MySqlCommand dr_1_c = conn.CreateCommand();
                dr_1_c.Connection = conn;
                dr_1_c.Transaction = Transacao;
                if (tipos_particiapnte != null)
                {
                    dr_1_c.CommandText = "SELECT pa.participante_id, pa.participante_codigo, pa.participante_nome, (round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-1','-1') and concat(@ano,'-1','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-1','-1') and concat(@ano,'-1','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'jan', (round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-2','-1') and concat(@ano,'-2','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-2','-1') and concat(@ano,'-2','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'fev',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-3','-1') and concat(@ano,'-3','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-3','-1') and concat(@ano,'-3','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'marc',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-4','-1') and concat(@ano,'-4','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-4','-1') and concat(@ano,'-4','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'abr',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-5','-1') and concat(@ano,'-5','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-5','-1') and concat(@ano,'-5','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'mai',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-6','-1') and concat(@ano,'-6','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-6','-1') and concat(@ano,'-6','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'jun',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-7','-1') and concat(@ano,'-7','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-7','-1') and concat(@ano,'-7','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'jul',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-8','-1') and concat(@ano,'-8','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-8','-1') and concat(@ano,'-8','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'ago',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-9','-1') and concat(@ano,'-9','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-9','-1') and concat(@ano,'-9','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'sete',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-10','-1') and concat(@ano,'-10','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-10','-1') and concat(@ano,'-10','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'outu',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-11','-1') and concat(@ano,'-11','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-11','-1') and concat(@ano,'-11','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'nov',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-12','-1') and concat(@ano,'-12','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-12','-1') and concat(@ano,'-12','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'dez' from participante as pa WHERE pa.participante_conta_id = @conta_id and pa.participante_status = 'Ativo' and FIND_IN_SET(pa.participante_tipo, @p);";
                    dr_1_c.Parameters.AddWithValue("@p", p);
                }
                else
                {
                    dr_1_c.CommandText = "SELECT pa.participante_id, pa.participante_codigo, pa.participante_nome, (round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-1','-1') and concat(@ano,'-1','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-1','-1') and concat(@ano,'-1','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'jan', (round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-2','-1') and concat(@ano,'-2','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-2','-1') and concat(@ano,'-2','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'fev',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-3','-1') and concat(@ano,'-3','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-3','-1') and concat(@ano,'-3','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'marc',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-4','-1') and concat(@ano,'-4','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-4','-1') and concat(@ano,'-4','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'abr',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-5','-1') and concat(@ano,'-5','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-5','-1') and concat(@ano,'-5','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'mai',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-6','-1') and concat(@ano,'-6','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-6','-1') and concat(@ano,'-6','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'jun',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-7','-1') and concat(@ano,'-7','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-7','-1') and concat(@ano,'-7','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'jul',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-8','-1') and concat(@ano,'-8','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-8','-1') and concat(@ano,'-8','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'ago',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-9','-1') and concat(@ano,'-9','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-9','-1') and concat(@ano,'-9','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'sete',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-10','-1') and concat(@ano,'-10','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-10','-1') and concat(@ano,'-10','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'outu',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-11','-1') and concat(@ano,'-11','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-11','-1') and concat(@ano,'-11','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'nov',(round(COALESCE((SELECT sum(ccm.ccm_valor) from conta_corrente_mov as ccm WHERE ccm.ccm_participante_id = pa.participante_id and ccm.ccm_data BETWEEN concat(@ano,'-12','-1') and concat(@ano,'-12','-31')),0)+COALESCE((SELECT sum(mcc.mcc_valor) from movimentos_cartao_credito as mcc LEFT JOIN op_parcelas as p on p.op_parcela_id = mcc.mcc_tipo_id LEFT JOIN op_participante as opp on opp.op_id = p.op_parcela_op_id LEFT JOIN operacao as op on op.op_id = p.op_parcela_op_id WHERE mcc.mcc_tipo = 'mcc_op_parcelas' and op.op_conta_id = @conta_id and op.op_data BETWEEN concat(@ano,'-12','-1') and concat(@ano,'-12','-31') and opp.op_part_participante_id = pa.participante_id),0),2)) as 'dez' from participante as pa WHERE pa.participante_conta_id = @conta_id and pa.participante_status = 'Ativo';";
                }
                dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                dr_1_c.Parameters.AddWithValue("ano", ano);
                dr_1 = dr_1_c.ExecuteReader();

                if (dr_1.HasRows)
                {
                    while (dr_1.Read())
                    {
                        Rp rp = new Rp();

                        rp.participante_id = Convert.ToInt32(dr_1["participante_id"]);
                        rp.participante_codigo = dr_1["participante_codigo"].ToString();
                        if (ocultar_nomes)
                        {
                            rp.participante_nome = "-";
                        }
                        else
                        {
                            rp.participante_nome = dr_1["participante_nome"].ToString();
                        }                        
                        rp.jan = Convert.ToDecimal(dr_1["jan"]);
                        rp.fev = Convert.ToDecimal(dr_1["fev"]);
                        rp.marc = Convert.ToDecimal(dr_1["marc"]);
                        rp.abr = Convert.ToDecimal(dr_1["abr"]);
                        rp.mai = Convert.ToDecimal(dr_1["mai"]);
                        rp.jun = Convert.ToDecimal(dr_1["jun"]);
                        rp.jul = Convert.ToDecimal(dr_1["jul"]);
                        rp.ago = Convert.ToDecimal(dr_1["ago"]);
                        rp.sete = Convert.ToDecimal(dr_1["sete"]);
                        rp.outu = Convert.ToDecimal(dr_1["outu"]);
                        rp.nov = Convert.ToDecimal(dr_1["nov"]);
                        rp.dez = Convert.ToDecimal(dr_1["dez"]);
                        rp.total = rp.jan + rp.fev + rp.marc + rp.abr + rp.mai + rp.jun + rp.jul + rp.ago + rp.sete + rp.outu + rp.nov + rp.dez;

                        total.jan += rp.jan;
                        total.fev += rp.fev;
                        total.marc += rp.marc;
                        total.abr += rp.abr;
                        total.mai += rp.mai;
                        total.jun += rp.jun;
                        total.jul += rp.jul;
                        total.ago += rp.ago;
                        total.sete += rp.sete;
                        total.outu += rp.outu;
                        total.nov += rp.nov;
                        total.dez += rp.dez;
                        total.total += rp.total;

                        if(ignorar_zerados)
                        {
                            if(rp.total > 0)
                            {
                                rps.Add(rp);
                            }
                        }
                        else
                        {
                            rps.Add(rp);
                        }
                    }
                }
                relatorio_Participante.rps = rps;
                relatorio_Participante.total = total;
                dr_1.Close();
            }
            catch (Exception e)
            {
                string t = "";
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return relatorio_Participante;
        }
    }

    public class Rp_filtro
    {
        public string ano { get; set; }
        public bool ignorar_zerados { get; set; }
        public bool ocultar_nomes { get; set; }
        public int[] tipos_participante { get; set; }
    }

    public class Relatorio_participante
    {
        public IEnumerable<Rp> rps { get; set; }
        public Rp total { get; set; }
        public Rp_filtro filtro { get; set; }
        public string retorno { get; set; }
    }
}
