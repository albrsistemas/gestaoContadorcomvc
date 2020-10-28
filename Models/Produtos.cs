using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.IO;

namespace gestaoContadorcomvc.Models
{
    public class Produtos
    {
        public int produtos_id { get; set; }
        public string produtos_codigo { get; set; }
        public string produtos_nome { get; set; }
        public DateTime produtos_dataCriacao { get; set; }
        public string produtos_formato { get; set; }
        public string produtos_status { get; set; }
        public string produtos_unidade { get; set; }
        public Decimal produtos_preco_venda { get; set; }
        public string produtos_gtin_ean { get; set; }
        public string produtos_gtin_ean_trib { get; set; }
        public Decimal produtos_estoque_min { get; set; }
        public Decimal produtos_estoque_max { get; set; }
        public Decimal produtos_estoque_qtd_inicial { get; set; }
        public Decimal produtos_estoque_preco_compra { get; set; }
        public Decimal produtos_estoque_custo_compra { get; set; }
        public string produtos_obs { get; set; }
        public int produtos_origem { get; set; }
        public string produtos_ncm { get; set; }
        public string produtos_cest { get; set; }
        public string produtos_tipo_item { get; set; }
        public Decimal produtos_perc_tributos { get; set; }
        public int produtos_conta_id { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Produtos()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Listar produtos
        public List<Vm_produtos> listaProdutos(int usuario_id, int conta_id)
        {
            List<Vm_produtos> produtos = new List<Vm_produtos>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from produtos where produtos_conta_id = @conta_id and produtos_status = 'Ativo';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Vm_produtos produto = new Vm_produtos();

                        if (DBNull.Value != leitor["produtos_id"])
                        {
                            produto.produtos_id = Convert.ToInt32(leitor["produtos_id"]);
                        }
                        else
                        {
                            produto.produtos_id = 0;
                        }

                        if (DBNull.Value != leitor["produtos_origem"])
                        {
                            produto.produtos_origem = Convert.ToInt32(leitor["produtos_origem"]);
                        }
                        else
                        {
                            produto.produtos_origem = 0;
                        }

                        if (DBNull.Value != leitor["produtos_conta_id"])
                        {
                            produto.produtos_conta_id = Convert.ToInt32(leitor["produtos_conta_id"]);
                        }
                        else
                        {
                            produto.produtos_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["produtos_dataCriacao"])
                        {
                            produto.produtos_dataCriacao = Convert.ToDateTime(leitor["produtos_dataCriacao"]);
                        }
                        else
                        {
                            produto.produtos_dataCriacao = new DateTime();
                        }

                        //Decimal
                        if (DBNull.Value != leitor["produtos_preco_venda"])
                        {
                            produto.produtos_preco_venda = Convert.ToDecimal(leitor["produtos_preco_venda"]);
                        }
                        else
                        {
                            produto.produtos_preco_venda = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_min"])
                        {
                            produto.produtos_estoque_min = Convert.ToDecimal(leitor["produtos_estoque_min"]);
                        }
                        else
                        {
                            produto.produtos_estoque_min = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_max"])
                        {
                            produto.produtos_estoque_max = Convert.ToDecimal(leitor["produtos_estoque_max"]);
                        }
                        else
                        {
                            produto.produtos_estoque_max = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_qtd_inicial"])
                        {
                            produto.produtos_estoque_qtd_inicial = Convert.ToDecimal(leitor["produtos_estoque_qtd_inicial"]);
                        }
                        else
                        {
                            produto.produtos_estoque_qtd_inicial = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_preco_compra"])
                        {
                            produto.produtos_estoque_preco_compra = Convert.ToDecimal(leitor["produtos_estoque_preco_compra"]);
                        }
                        else
                        {
                            produto.produtos_estoque_preco_compra = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_custo_compra"])
                        {
                            produto.produtos_estoque_custo_compra = Convert.ToDecimal(leitor["produtos_estoque_custo_compra"]);
                        }
                        else
                        {
                            produto.produtos_estoque_custo_compra = 0;
                        }

                        if (DBNull.Value != leitor["produtos_perc_tributos"])
                        {
                            produto.produtos_perc_tributos = Convert.ToDecimal(leitor["produtos_perc_tributos"]);
                        }
                        else
                        {
                            produto.produtos_perc_tributos = 0;
                        }

                        produto.produtos_codigo = leitor["produtos_codigo"].ToString();
                        produto.produtos_nome = leitor["produtos_nome"].ToString();
                        produto.produtos_formato = leitor["produtos_formato"].ToString();
                        produto.produtos_status = leitor["produtos_status"].ToString();
                        produto.produtos_unidade = leitor["produtos_unidade"].ToString();
                        produto.produtos_gtin_ean = leitor["produtos_gtin_ean"].ToString();
                        produto.produtos_gtin_ean_trib = leitor["produtos_gtin_ean_trib"].ToString();
                        produto.produtos_obs = leitor["produtos_obs"].ToString();
                        produto.produtos_ncm = leitor["produtos_ncm"].ToString();
                        produto.produtos_cest = leitor["produtos_cest"].ToString();
                        produto.produtos_tipo_item = leitor["produtos_tipo_item"].ToString();

                        produtos.Add(produto);
                    }
                }

            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Produtos", "listaProdutos", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return produtos;
        }

        //Cadastrar produto
        public string cadastraProduto(
            int conta_id,
            int usuario_id,
            string produtos_codigo,
            string produtos_nome,            
            string produtos_formato,            
            string produtos_unidade,
            Decimal produtos_preco_venda,
            string produtos_gtin_ean,
            string produtos_gtin_ean_trib,
            Decimal produtos_estoque_min,
            Decimal produtos_estoque_max,
            Decimal produtos_estoque_qtd_inicial,
            Decimal produtos_estoque_preco_compra,
            Decimal produtos_estoque_custo_compra,
            string produtos_obs,
            int produtos_origem,
            string produtos_ncm,
            string produtos_cest,
            string produtos_tipo_item,
            Decimal produtos_perc_tributos
            )
        {
            string retorno = "Produto cadastrado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "insert into produtos (produtos_codigo, produtos_nome, produtos_formato, produtos_unidade, produtos_preco_venda, produtos_gtin_ean, produtos_gtin_ean_trib, produtos_estoque_min, produtos_estoque_max, produtos_estoque_qtd_inicial, produtos_estoque_preco_compra, produtos_estoque_custo_compra, produtos_obs, produtos_origem, produtos_ncm, produtos_cest, produtos_tipo_item, produtos_perc_tributos, produtos_conta_id) values (@produtos_codigo, @produtos_nome, @produtos_formato, @produtos_unidade, @produtos_preco_venda, @produtos_gtin_ean, @produtos_gtin_ean_trib, @produtos_estoque_min, @produtos_estoque_max, @produtos_estoque_qtd_inicial, @produtos_estoque_preco_compra, @produtos_estoque_custo_compra, @produtos_obs, @produtos_origem, @produtos_ncm, @produtos_cest, @produtos_tipo_item, @produtos_perc_tributos, @produtos_conta_id)";
                comando.Parameters.AddWithValue("@produtos_codigo", produtos_codigo);
                comando.Parameters.AddWithValue("@produtos_nome", produtos_nome);
                comando.Parameters.AddWithValue("@produtos_formato", produtos_formato);
                comando.Parameters.AddWithValue("@produtos_unidade", produtos_unidade);
                comando.Parameters.AddWithValue("@produtos_preco_venda", produtos_preco_venda);
                comando.Parameters.AddWithValue("@produtos_gtin_ean", produtos_gtin_ean);
                comando.Parameters.AddWithValue("@produtos_gtin_ean_trib", produtos_gtin_ean_trib);
                comando.Parameters.AddWithValue("@produtos_estoque_min", produtos_estoque_min);
                comando.Parameters.AddWithValue("@produtos_estoque_max", produtos_estoque_max);
                comando.Parameters.AddWithValue("@produtos_estoque_qtd_inicial", produtos_estoque_qtd_inicial);
                comando.Parameters.AddWithValue("@produtos_estoque_preco_compra", produtos_estoque_preco_compra);
                comando.Parameters.AddWithValue("@produtos_estoque_custo_compra", produtos_estoque_custo_compra);
                comando.Parameters.AddWithValue("@produtos_obs", produtos_obs);
                comando.Parameters.AddWithValue("@produtos_origem", produtos_origem);
                comando.Parameters.AddWithValue("@produtos_ncm", produtos_ncm);
                comando.Parameters.AddWithValue("@produtos_cest", produtos_cest);
                comando.Parameters.AddWithValue("@produtos_tipo_item", produtos_tipo_item);
                comando.Parameters.AddWithValue("@produtos_perc_tributos", produtos_perc_tributos);
                comando.Parameters.AddWithValue("@produtos_conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Cadastro de novo produto nome: " + produtos_nome + " Cadastrado com sucesso";
                log.log("Produto", "cadastrarProduto", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao cadastrar o produto. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Produto", "cadastraProduto", "Erro", msg, conta_id, usuario_id);
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

        //Buscar produto por id
        public Vm_produtos buscaProduto(int usuario_id, int conta_id, int produto_id)
        {
            Vm_produtos produto = new Vm_produtos();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from produtos where produtos_conta_id = @conta_id and produtos_id = @produtos_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@produtos_id", produto_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["produtos_id"])
                        {
                            produto.produtos_id = Convert.ToInt32(leitor["produtos_id"]);
                        }
                        else
                        {
                            produto.produtos_id = 0;
                        }

                        if (DBNull.Value != leitor["produtos_origem"])
                        {
                            produto.produtos_origem = Convert.ToInt32(leitor["produtos_origem"]);
                        }
                        else
                        {
                            produto.produtos_origem = 0;
                        }

                        if (DBNull.Value != leitor["produtos_conta_id"])
                        {
                            produto.produtos_conta_id = Convert.ToInt32(leitor["produtos_conta_id"]);
                        }
                        else
                        {
                            produto.produtos_conta_id = 0;
                        }

                        if (DBNull.Value != leitor["produtos_dataCriacao"])
                        {
                            produto.produtos_dataCriacao = Convert.ToDateTime(leitor["produtos_dataCriacao"]);
                        }
                        else
                        {
                            produto.produtos_dataCriacao = new DateTime();
                        }

                        //Decimal
                        if (DBNull.Value != leitor["produtos_preco_venda"])
                        {
                            produto.produtos_preco_venda = Convert.ToDecimal(leitor["produtos_preco_venda"]);
                        }
                        else
                        {
                            produto.produtos_preco_venda = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_min"])
                        {
                            produto.produtos_estoque_min = Convert.ToDecimal(leitor["produtos_estoque_min"]);
                        }
                        else
                        {
                            produto.produtos_estoque_min = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_max"])
                        {
                            produto.produtos_estoque_max = Convert.ToDecimal(leitor["produtos_estoque_max"]);
                        }
                        else
                        {
                            produto.produtos_estoque_max = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_qtd_inicial"])
                        {
                            produto.produtos_estoque_qtd_inicial = Convert.ToDecimal(leitor["produtos_estoque_qtd_inicial"]);
                        }
                        else
                        {
                            produto.produtos_estoque_qtd_inicial = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_preco_compra"])
                        {
                            produto.produtos_estoque_preco_compra = Convert.ToDecimal(leitor["produtos_estoque_preco_compra"]);
                        }
                        else
                        {
                            produto.produtos_estoque_preco_compra = 0;
                        }

                        if (DBNull.Value != leitor["produtos_estoque_custo_compra"])
                        {
                            produto.produtos_estoque_custo_compra = Convert.ToDecimal(leitor["produtos_estoque_custo_compra"]);
                        }
                        else
                        {
                            produto.produtos_estoque_custo_compra = 0;
                        }

                        if (DBNull.Value != leitor["produtos_perc_tributos"])
                        {
                            produto.produtos_perc_tributos = Convert.ToDecimal(leitor["produtos_perc_tributos"]);
                        }
                        else
                        {
                            produto.produtos_perc_tributos = 0;
                        }

                        produto.produtos_codigo = leitor["produtos_codigo"].ToString();
                        produto.produtos_nome = leitor["produtos_nome"].ToString();
                        produto.produtos_formato = leitor["produtos_formato"].ToString();
                        produto.produtos_status = leitor["produtos_status"].ToString();
                        produto.produtos_unidade = leitor["produtos_unidade"].ToString();
                        produto.produtos_gtin_ean = leitor["produtos_gtin_ean"].ToString();
                        produto.produtos_gtin_ean_trib = leitor["produtos_gtin_ean_trib"].ToString();
                        produto.produtos_obs = leitor["produtos_obs"].ToString();
                        produto.produtos_ncm = leitor["produtos_ncm"].ToString();
                        produto.produtos_cest = leitor["produtos_cest"].ToString();
                        produto.produtos_tipo_item = leitor["produtos_tipo_item"].ToString();
                    }
                }

            }
            catch (Exception e)
            {
                string msg = e.Message.Substring(0, 300);
                log.log("Produtos", "listaProdutos", "Erro", msg, conta_id, usuario_id);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return produto;
        }

        //Alterar produto
        public string alteraProduto(
            int usuario_id,
            int conta_id,
            int produtos_id,
            string produtos_codigo,
            string produtos_nome,
            string produtos_formato,
            string produtos_status,
            string produtos_unidade,
            Decimal produtos_preco_venda,
            string produtos_gtin_ean,
            string produtos_gtin_ean_trib,
            Decimal produtos_estoque_min,
            Decimal produtos_estoque_max,
            Decimal produtos_estoque_qtd_inicial,
            Decimal produtos_estoque_preco_compra,
            Decimal produtos_estoque_custo_compra,
            string produtos_obs,
            int produtos_origem,
            string produtos_ncm,
            string produtos_cest,
            string produtos_tipo_item,
            Decimal produtos_perc_tributos
            )
        {
            string retorno = "Produto alterado com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update produtos set produtos_codigo = @produtos_codigo, produtos_nome = @produtos_nome, produtos_formato = @produtos_formato, produtos_status = @produtos_status, produtos_unidade = @produtos_unidade, produtos_preco_venda = @produtos_preco_venda, produtos_gtin_ean = @produtos_gtin_ean, produtos_gtin_ean_trib = @produtos_gtin_ean_trib, produtos_estoque_min = @produtos_estoque_min, produtos_estoque_max = @produtos_estoque_max, produtos_estoque_qtd_inicial = @produtos_estoque_qtd_inicial, produtos_estoque_preco_compra = @produtos_estoque_preco_compra, produtos_estoque_custo_compra = @produtos_estoque_custo_compra, produtos_obs = @produtos_obs, produtos_origem = @produtos_origem, produtos_ncm = @produtos_ncm, produtos_cest = @produtos_cest, produtos_tipo_item = @produtos_tipo_item, produtos_perc_tributos = @produtos_perc_tributos where produtos_id = @produtos_id and produtos_conta_id = @conta_id";
                comando.Parameters.AddWithValue("@produtos_codigo", produtos_codigo);
                comando.Parameters.AddWithValue("@produtos_nome", produtos_nome);
                comando.Parameters.AddWithValue("@produtos_formato", produtos_formato);
                comando.Parameters.AddWithValue("@produtos_unidade", produtos_unidade);
                comando.Parameters.AddWithValue("@produtos_preco_venda", produtos_preco_venda);
                comando.Parameters.AddWithValue("@produtos_gtin_ean", produtos_gtin_ean);
                comando.Parameters.AddWithValue("@produtos_gtin_ean_trib", produtos_gtin_ean_trib);
                comando.Parameters.AddWithValue("@produtos_estoque_min", produtos_estoque_min);
                comando.Parameters.AddWithValue("@produtos_estoque_max", produtos_estoque_max);
                comando.Parameters.AddWithValue("@produtos_estoque_qtd_inicial", produtos_estoque_qtd_inicial);
                comando.Parameters.AddWithValue("@produtos_estoque_preco_compra", produtos_estoque_preco_compra);
                comando.Parameters.AddWithValue("@produtos_estoque_custo_compra", produtos_estoque_custo_compra);
                comando.Parameters.AddWithValue("@produtos_obs", produtos_obs);
                comando.Parameters.AddWithValue("@produtos_origem", produtos_origem);
                comando.Parameters.AddWithValue("@produtos_ncm", produtos_ncm);
                comando.Parameters.AddWithValue("@produtos_cest", produtos_cest);
                comando.Parameters.AddWithValue("@produtos_tipo_item", produtos_tipo_item);
                comando.Parameters.AddWithValue("@produtos_perc_tributos", produtos_perc_tributos);                
                comando.Parameters.AddWithValue("@produtos_id", produtos_id);
                comando.Parameters.AddWithValue("@produtos_status", produtos_status);
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Alteração do produto nome: " + produtos_nome + " Alterado com sucesso";
                log.log("Produto", "alteraProduto", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao alterar o produto. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Produtos", "alteraProduto", "Erro", msg, conta_id, usuario_id);
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

        //Exluir participante
        public string deletarProduto(int usuario_id, int conta_id, int produtos_id)
        {
            string retorno = "Produto excluído com sucesso!";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "update produtos set produtos_status = 'Deletado' where produtos_conta_id = @conta_id and produtos_id = @produtos_id;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@produtos_id", produtos_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Exclusão do produto ID: " + produtos_id + " Excluído com sucesso";
                log.log("Produto", "deletarProduto", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                retorno = "Erro ao excluir o produto. Tente novamente. Se persistir, entre em contato com o suporte!";

                string msg = e.Message.Substring(0, 300);
                log.log("Produto", "deletarProduto", "Erro", msg, conta_id, usuario_id);
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
