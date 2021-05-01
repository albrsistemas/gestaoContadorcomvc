using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Relatorios
{
    public class Riop //Relatório itens operação
    {
        public IEnumerable<Riop_model> lista { get; set; }
        public Riop_filtro filtro { get; set; }
        public Vm_usuario user { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Riop()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        public Riop create(Riop_filtro filtro, int conta_id)
        {
            Riop riop = new Riop();
            List<Riop_model> lista = new List<Riop_model>();
            riop.filtro = filtro;

            //Dados filtro em matriz
            string p = "";
            string it = "";
            string cc = "";
            if(filtro.participante != null && filtro.participante.Length > 0)
            {
                for (var i = 0; i < filtro.participante.Length; i++)
                {
                    if (i == 0)
                    {
                        p += filtro.participante[i];
                    }

                    if (i != 0)
                    {
                        p += "," + filtro.participante[i];
                    }
                }
            }

            if (filtro.item != null && filtro.item.Length > 0)
            {
                for (var i = 0; i < filtro.item.Length; i++)
                {
                    if (i == 0)
                    {
                        it += filtro.item[i];
                    }

                    if (i != 0)
                    {
                        it += "," + filtro.item[i];
                    }
                }
            }

            if (filtro.centro_custo != null && filtro.centro_custo.Length > 0)
            {
                for (var i = 0; i < filtro.centro_custo.Length; i++)
                {
                    if (i == 0)
                    {
                        cc += filtro.centro_custo[i];
                    }

                    if (i != 0)
                    {
                        cc += "," + filtro.centro_custo[i];
                    }
                }
            }



            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT op.op_data, op.op_tipo, now() as 'nf_data_ent_sai', nf.op_nf_data_emissao, nf.op_nf_numero, i.op_item_codigo, i.op_item_nome, nf.op_nf_chave, i.op_item_ncm, concat(i.op_item_origem,i.op_item_cst) as 'cst', i.op_item_cfop, part.op_part_nome, part.op_part_cnpj_cpf, i.op_item_qtd, i.op_item_unidade, i.op_item_preco, i.op_item_desconto, i.op_item_frete, i.op_item_seguros, i.op_item_desp_aces, i.op_item_valor_total, i.op_item_pIPI, i.op_item_vlr_ipi, i.op_item_vlr_icms_st, (i.op_item_valor_total + i.op_item_vlr_ipi + i.op_item_vlr_icms_st) as 'total', i.op_itens_centro_custo, (case WHEN i.op_itens_centro_custo = 0 THEN 'Geral' else cc.centro_custo_nome END) as 'centro_custo_nome'  from op_itens as i LEFT JOIN operacao as op on op.op_id = i.op_item_op_id LEFT JOIN op_nf as nf on nf.op_nf_op_id = op.op_id left JOIN op_participante as part on op.op_id = part.op_id left JOIN centro_custo as cc on cc.centro_custo_id = i.op_itens_centro_custo WHERE op.op_conta_id = @conta_id and (case WHEN @data_emissao_inicio <> '' THEN nf.op_nf_data_emissao BETWEEN @data_emissao_inicio and @data_emissao_final else true END) and (case WHEN @nf_numero <> '' THEN nf.op_nf_numero = @nf_numero ELSE true END) and (case WHEN @participante <> '' THEN FIND_IN_SET(part.op_part_participante_id, @participante) else true END) and (case WHEN @operacao_tipo = '1' THEN op.op_tipo = 'compra' WHEN @operacao_tipo = '2' THEN op.op_tipo = 'venda' else true END) and (case WHEN @item <> '' THEN FIND_IN_SET(i.op_item_produto_id, @item) else true END) and (case WHEN @centro_custo <> '' THEN FIND_IN_SET(i.op_itens_centro_custo, @centro_custo) else true END) ORDER BY op.op_id asc, op.op_data ASC;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@data_emissao_inicio", filtro.data_emissao_inicio);
                comando.Parameters.AddWithValue("@data_emissao_final", filtro.data_emissao_final);
                comando.Parameters.AddWithValue("@data_ent_sai_inicio", filtro.data_ent_sai_inicio);
                comando.Parameters.AddWithValue("@data_ent_sai_fim", filtro.data_ent_sai_fim);
                comando.Parameters.AddWithValue("@nf_numero", filtro.nf_numero);
                comando.Parameters.AddWithValue("@participante", p);
                comando.Parameters.AddWithValue("@operacao_tipo", filtro.operacao_tipo);
                comando.Parameters.AddWithValue("@item", it);
                comando.Parameters.AddWithValue("@centro_custo", cc);
                comando.ExecuteNonQuery();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Riop_model m = new Riop_model();

                        if (DBNull.Value != leitor["op_itens_centro_custo"])
                        {
                            m.item_centro_de_custo = Convert.ToInt32(leitor["op_itens_centro_custo"]);
                        }
                        else
                        {
                            m.item_centro_de_custo = 0;
                        }

                        if (DBNull.Value != leitor["op_data"])
                        {
                            m.operacao_data = Convert.ToDateTime(leitor["op_data"]);
                        }
                        else
                        {
                            m.operacao_data = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_nf_data_emissao"])
                        {
                            m.nf_data_emissao = Convert.ToDateTime(leitor["op_nf_data_emissao"]);
                        }
                        else
                        {
                            m.nf_data_emissao = new DateTime();
                        }

                        if (DBNull.Value != leitor["nf_data_ent_sai"])
                        {
                            m.nf_data_ent_sai = Convert.ToDateTime(leitor["nf_data_ent_sai"]);
                        }
                        else
                        {
                            m.nf_data_ent_sai = new DateTime();
                        }

                        if (DBNull.Value != leitor["op_item_qtd"])
                        {
                            m.item_quantidade = Convert.ToDecimal(leitor["op_item_qtd"]);
                        }
                        else
                        {
                            m.item_quantidade = 0;
                        }

                        if (DBNull.Value != leitor["op_item_preco"])
                        {
                            m.item_valor_unitario = Convert.ToDecimal(leitor["op_item_preco"]);
                        }
                        else
                        {
                            m.item_valor_unitario = 0;
                        }

                        if (DBNull.Value != leitor["op_item_desconto"])
                        {
                            m.item_desconto = Convert.ToDecimal(leitor["op_item_desconto"]);
                        }
                        else
                        {
                            m.item_desconto = 0;
                        }

                        if (DBNull.Value != leitor["op_item_frete"])
                        {
                            m.item_frete = Convert.ToDecimal(leitor["op_item_frete"]);
                        }
                        else
                        {
                            m.item_frete = 0;
                        }

                        if (DBNull.Value != leitor["op_item_seguros"])
                        {
                            m.item_seguros = Convert.ToDecimal(leitor["op_item_seguros"]);
                        }
                        else
                        {
                            m.item_seguros = 0;
                        }

                        if (DBNull.Value != leitor["op_item_desp_aces"])
                        {
                            m.item_desp_acessorias = Convert.ToDecimal(leitor["op_item_desp_aces"]);
                        }
                        else
                        {
                            m.item_desp_acessorias = 0;
                        }

                        if (DBNull.Value != leitor["op_item_pIPI"])
                        {
                            m.item_pIPI = Convert.ToDecimal(leitor["op_item_pIPI"]);
                        }
                        else
                        {
                            m.item_pIPI = 0;
                        }

                        if (DBNull.Value != leitor["op_item_vlr_ipi"])
                        {
                            m.item_vIPI = Convert.ToDecimal(leitor["op_item_vlr_ipi"]);
                        }
                        else
                        {
                            m.item_vIPI = 0;
                        }

                        if (DBNull.Value != leitor["op_item_vlr_icms_st"])
                        {
                            m.item_icms_st = Convert.ToDecimal(leitor["op_item_vlr_icms_st"]);
                        }
                        else
                        {
                            m.item_icms_st = 0;
                        }

                        if (DBNull.Value != leitor["total"])
                        {
                            m.item_valor_total = Convert.ToDecimal(leitor["total"]);
                        }
                        else
                        {
                            m.item_valor_total = 0;
                        }

                        m.nf_numero = leitor["op_nf_numero"].ToString();
                        m.item_codigo = leitor["op_item_codigo"].ToString();
                        m.item_descricao = leitor["op_item_nome"].ToString();
                        m.nf_chave_acesso = leitor["op_nf_chave"].ToString();
                        m.item_ncm = leitor["op_item_ncm"].ToString();
                        m.item_cst = leitor["cst"].ToString();
                        m.item_cfop = leitor["op_item_cfop"].ToString();
                        m.participante_nome = leitor["op_part_nome"].ToString();
                        m.participante_cnpj_cpf = leitor["op_part_cnpj_cpf"].ToString();
                        m.item_unidade = leitor["op_item_unidade"].ToString();
                        m.item_centro_de_custo_nome = leitor["centro_custo_nome"].ToString();
                        m.operacao_tipo = leitor["op_tipo"].ToString();

                        lista.Add(m);
                    }

                    riop.lista = lista;
                }
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

            return riop;
        }


    }

    public class Riop_model //Relatório itens operação modelo
    {
        public string operacao_tipo { get; set; }
        public DateTime operacao_data { get; set; }
        public DateTime nf_data_emissao { get; set; }
        public DateTime nf_data_ent_sai { get; set; }
        public string nf_numero { get; set; }
        public string item_codigo { get; set; }
        public string item_descricao { get; set; }
        public string nf_chave_acesso { get; set; }
        public string item_ncm { get; set; }
        public string item_cst { get; set; }
        public string item_cfop { get; set; }
        public string participante_nome { get; set; }
        public string participante_cnpj_cpf { get; set; }
        public Decimal item_quantidade { get; set; }
        public string item_unidade { get; set; }
        public Decimal item_valor_unitario { get; set; }
        public Decimal item_desconto { get; set; }
        public Decimal item_frete { get; set; }
        public Decimal item_seguros { get; set; }
        public Decimal item_desp_acessorias { get; set; }
        public Decimal item_pIPI { get; set; }
        public Decimal item_vIPI { get; set; }
        public Decimal item_icms_st { get; set; }
        public Decimal item_valor_total { get; set; }
        public int item_centro_de_custo { get; set; }
        public string item_centro_de_custo_nome { get; set; }
    }

    public class Riop_filtro
    {
        public string data_emissao_inicio { get; set; }
        public string data_emissao_final { get; set; }
        public string data_ent_sai_inicio { get; set; }
        public string data_ent_sai_fim { get; set; }
        public string nf_numero { get; set; }
        public int[] participante { get; set; }
        public string operacao_tipo { get; set; }
        public int[] item { get; set; }
        public int[] centro_custo { get; set; }
    }
}
