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
    public class ParcelamentoFaturaCartaoCredito
    {
        public int pfcc_id { get; set; }
        public int pfcc_fcc_id { get; set; }
        public Decimal pfcc_total_fatura { get; set; }
        public Decimal pfcc_valor_parcelado { get; set; }
        public int pfcc_numero_parcelas { get; set; }
        public Decimal pfcc_valor_parcela { get; set; }
        public Decimal pfcc_juros { get; set; }
        public int pfcc_categoria_id { get; set; }
        public DateTime pfcc_data_parcelamento { get; set; }
        public string categoria_nome { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public ParcelamentoFaturaCartaoCredito()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        public string criarParcelamento(int conta_id, int usuario_id, int pfcc_fcc_id, Decimal pfcc_total_fatura, Decimal pfcc_valor_parcelado, int pfcc_numero_parcelas, Decimal pfcc_valor_parcela, Decimal pfcc_juros, int pfcc_categoria_id, DateTime pfcc_data_parcelamento, string[] competencias, string competencia, int fcc_forma_pagamento_id)
        {
            string retorno = "";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                //Pesquisando dados do cartão para uso a frente
                FormaPagamento fp = new FormaPagamento();
                Vm_forma_pagamento vm_fp = new Vm_forma_pagamento();
                vm_fp = fp.buscaFormasPagamento(conta_id, usuario_id, fcc_forma_pagamento_id);

                //Definindo as data de fechamento e vencimento com base nos dias padrão da forma de pagamento para a competência parcelada.
                string dia_fechamento = vm_fp.fp_dia_fechamento_cartao + "/" + competencia;
                string dia_vencimento = vm_fp.fp_dia_vencimento_cartao + "/" + competencia;
                DateTime data_fechamento = DateTime.Parse(dia_fechamento);
                DateTime data_vencimento = DateTime.Parse(dia_fechamento);
                if (vm_fp.fp_dia_vencimento_cartao < vm_fp.fp_dia_fechamento_cartao)
                {
                    data_vencimento = data_vencimento.AddMonths(1);
                }


                comando.CommandText = "insert into parcelamentofaturacartaocredito (parcelamentofaturacartaocredito.pfcc_fcc_id, parcelamentofaturacartaocredito.pfcc_total_fatura, parcelamentofaturacartaocredito.pfcc_valor_parcelado, parcelamentofaturacartaocredito.pfcc_numero_parcelas, parcelamentofaturacartaocredito.pfcc_valor_parcela, parcelamentofaturacartaocredito.pfcc_juros, parcelamentofaturacartaocredito.pfcc_categoria_id, parcelamentofaturacartaocredito.pfcc_data_parcelamento) values (@pfcc_fcc_id,@pfcc_total_fatura,@pfcc_valor_parcelado,@pfcc_numero_parcelas,@pfcc_valor_parcela,@pfcc_juros,@pfcc_categoria_id,@pfcc_data_parcelamento);";
                comando.Parameters.AddWithValue("@pfcc_fcc_id", pfcc_fcc_id);
                comando.Parameters.AddWithValue("@pfcc_total_fatura", pfcc_total_fatura);
                comando.Parameters.AddWithValue("@pfcc_valor_parcelado", pfcc_valor_parcelado);
                comando.Parameters.AddWithValue("@pfcc_numero_parcelas", pfcc_numero_parcelas);
                comando.Parameters.AddWithValue("@pfcc_valor_parcela", pfcc_valor_parcela);
                comando.Parameters.AddWithValue("@pfcc_juros", pfcc_juros);
                comando.Parameters.AddWithValue("@pfcc_categoria_id", pfcc_categoria_id);
                comando.Parameters.AddWithValue("@pfcc_data_parcelamento", pfcc_data_parcelamento);
                comando.ExecuteNonQuery();

                //recuperando id do parcelamento
                MySqlDataReader myReader;
                int parcelamento_id = 0;
                comando.CommandText = "SELECT LAST_INSERT_ID();";
                myReader = comando.ExecuteReader();
                while (myReader.Read())
                {
                    parcelamento_id = myReader.GetInt32(0);
                }
                myReader.Close();

                for (int i = 0; i < competencias.Length; i++)
                {
                    //Adiantnado um mês a cada competência nas datas de fechamento e vencimento para se precisar criar fatura cartão
                    data_fechamento = data_fechamento.AddMonths(1);
                    data_vencimento = data_vencimento.AddMonths(1);

                    //Criando memorando do movimento do cartão de crédito
                    string memorando = "Parcela " + (i + 1) + " de " + competencias.Length + " ref.: Parcelamento da fatura da competência " + competencia;
                    
                    int fcc_id = 0;

                    //Verifica se fatura cartão existe
                    MySqlDataReader dr_1;
                    MySqlCommand dr_1_c = conn.CreateCommand();
                    dr_1_c.Connection = conn;
                    dr_1_c.Transaction = Transacao;
                    dr_1_c.CommandText = "SELECT f.fcc_id from fatura_cartao_credito as f WHERE f.fcc_conta_id = @conta_id and f.fcc_competencia = @competencia;";
                    dr_1_c.Parameters.AddWithValue("conta_id", conta_id);
                    dr_1_c.Parameters.AddWithValue("competencia", competencias[i]);                    
                    dr_1 = dr_1_c.ExecuteReader();
                    
                    bool linha = dr_1.HasRows;


                    if (dr_1.HasRows)
                    {
                        while (dr_1.Read())
                        {
                            if (DBNull.Value != dr_1["fcc_id"])
                            {
                                fcc_id = Convert.ToInt32(dr_1["fcc_id"]);
                            }
                            else
                            {
                                fcc_id = 0;
                            }
                        }
                    }
                    dr_1.Close();
                                     

                    if (linha == true) //Foi localizada a fatura
                    {
                        //Gravar o movimento na fatura
                        dr_1_c.CommandText = "INSERT into movimentos_cartao_credito (mcc_tipo, mcc_tipo_id, mcc_fcc_id, mcc_data, mcc_descricao, mcc_valor, mcc_movimento) VALUES('Parcelamento', @parcelamento_id, @fcc_id, @pfcc_data_parcelamento1, @memorando, @pfcc_valor_parcela1, 'D');";
                        dr_1_c.Parameters.AddWithValue("@parcelamento_id", parcelamento_id);
                        dr_1_c.Parameters.AddWithValue("@fcc_id", fcc_id);
                        dr_1_c.Parameters.AddWithValue("@pfcc_data_parcelamento1", pfcc_data_parcelamento);
                        dr_1_c.Parameters.AddWithValue("@memorando", memorando);
                        dr_1_c.Parameters.AddWithValue("@pfcc_valor_parcela1", pfcc_valor_parcela);
                        dr_1_c.ExecuteNonQuery();
                        
                    }
                    

                    if (linha == false) //Não foi localizado fatura                     
                    {
                        //Criar a fatura
                        MySqlCommand dr_4_c = conn.CreateCommand();
                        dr_4_c.Connection = conn;
                        dr_4_c.Transaction = Transacao;

                        dr_4_c.CommandText = "INSERT into fatura_cartao_credito (fcc_data_corte, fcc_data_vencimento, fcc_conta_id, fcc_situacao, fcc_forma_pagamento_id, fcc_competencia) values (@data_fechamento, @data_vencimento, @conta_id, 'Aberta', @fcc_forma_pagamento_id, @fcc_competencia);";
                        dr_4_c.Parameters.AddWithValue("@data_fechamento", data_fechamento);
                        dr_4_c.Parameters.AddWithValue("@data_vencimento", data_vencimento);
                        dr_4_c.Parameters.AddWithValue("@conta_id", conta_id);
                        dr_4_c.Parameters.AddWithValue("@fcc_forma_pagamento_id", fcc_forma_pagamento_id);
                        dr_4_c.Parameters.AddWithValue("@fcc_competencia", competencias[i]);
                        dr_4_c.ExecuteNonQuery();

                        //Recuperar o ID da Fatura
                        //recuperando id do parcelamento
                        MySqlDataReader myReader2;
                        MySqlCommand dr_2_c = conn.CreateCommand();
                        dr_2_c.Connection = conn;
                        dr_2_c.Transaction = Transacao;
                        int fatura_id = 0;
                        dr_2_c.CommandText = "SELECT LAST_INSERT_ID();";
                        myReader2 = dr_2_c.ExecuteReader();
                        while (myReader2.Read())
                        {
                            fatura_id = myReader2.GetInt32(0);
                        }

                        myReader2.Close();

                        //Gravar o movimento na fatura
                        dr_2_c.CommandText = "INSERT into movimentos_cartao_credito (mcc_tipo, mcc_tipo_id, mcc_fcc_id, mcc_data, mcc_descricao, mcc_valor, mcc_movimento) VALUES('Parcelamento', @parcelamento_id1, @fcc_id, @pfcc_data_parcelamento2, @memorando1, @pfcc_valor_parcela2, 'D');";
                        dr_2_c.Parameters.AddWithValue("@parcelamento_id1", parcelamento_id);
                        dr_2_c.Parameters.AddWithValue("@fcc_id", fatura_id);
                        dr_2_c.Parameters.AddWithValue("@pfcc_data_parcelamento2", pfcc_data_parcelamento);
                        dr_2_c.Parameters.AddWithValue("@memorando1", memorando);
                        dr_2_c.Parameters.AddWithValue("@pfcc_valor_parcela2", pfcc_valor_parcela);
                        dr_2_c.ExecuteNonQuery();                        
                    }                    
                }
                //Inserindo o movimento de crédito
                string m = "Crédito Ref. Parcelamento da Fatura " + competencia;
                MySqlCommand dr_5_c = conn.CreateCommand();
                dr_5_c.Connection = conn;
                dr_5_c.Transaction = Transacao;

                dr_5_c.CommandText = "INSERT into movimentos_cartao_credito (mcc_tipo, mcc_tipo_id, mcc_fcc_id, mcc_data, mcc_descricao, mcc_valor, mcc_movimento) VALUES('Crédito Parcelamento', @parcelamento_id5, @fcc_id5, @pfcc_data_parcelamento5, @memorando5, @valor_parcelado, 'C');";
                dr_5_c.Parameters.AddWithValue("@parcelamento_id5", parcelamento_id);
                dr_5_c.Parameters.AddWithValue("@fcc_id5", pfcc_fcc_id);
                dr_5_c.Parameters.AddWithValue("@pfcc_data_parcelamento5", pfcc_data_parcelamento);
                dr_5_c.Parameters.AddWithValue("@memorando5", m);
                dr_5_c.Parameters.AddWithValue("@valor_parcelado", pfcc_valor_parcelado);
                dr_5_c.ExecuteNonQuery();


                Transacao.Commit();

                retorno = "Parcelamento criado com sucesso!";
            }
            catch (Exception e)
            {
                retorno = "Erro ao criar o parcelamento! (" + e.Message + ").";
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

        public string excluirParcelamento(int conta_id, int pfcc_id)
        {
            string retorno = "";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "DELETE from movimentos_cartao_credito WHERE (movimentos_cartao_credito.mcc_tipo = 'Parcelamento' or movimentos_cartao_credito.mcc_tipo = 'Crédito Parcelamento') and movimentos_cartao_credito.mcc_tipo_id = @pfcc_id;";
                comando.Parameters.AddWithValue("@pfcc_id", pfcc_id);
                comando.ExecuteNonQuery();
                comando.CommandText = "DELETE from parcelamentofaturacartaocredito WHERE parcelamentofaturacartaocredito.pfcc_id = @pfcc_id_2";                
                comando.Parameters.AddWithValue("@pfcc_id_2", pfcc_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                retorno = "Parcelamento excluído com sucesso!";
            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir o parcelamento! (" + e.Message + ").";
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
