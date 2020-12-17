using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Selects
    {
        //Atributos
        public string value { get; set; }
        public string text { get; set; }
        public bool disabled { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Selects()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //Grupos das contas padrão
        public List<Selects> getGrupoContas()
        {
            List<Selects> contas = new List<Selects>();
            contas.Add(new Selects
            {
                value = "Ativo",
                text = "Ativo"
            });
            contas.Add(new Selects
            {
                value = "Passico",
                text = "Passivo"
            });
            contas.Add(new Selects
            {
                value = "Receita",
                text = "Receita"
            });
            contas.Add(new Selects
            {
                value = "Despesa",
                text = "Despesa"
            });

            return contas;
        }

        //Empresas contador
        public List<Selects> getEmpresasContador(int conta_id_contador)
        {
            List<Selects> empresas = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT conta.conta_id, conta.conta_nome, contacontabilidade.cc_conta_id_contador from conta left join contacontabilidade on conta.conta_contador = contacontabilidade.cc_id where contacontabilidade.cc_conta_id_contador = @conta_id_contador;";
                comando.Parameters.AddWithValue("@conta_id_contador", conta_id_contador);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        empresas.Add(new Selects
                        {
                            value = leitor["conta_id"].ToString(),
                            text = leitor["conta_nome"].ToString()
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return empresas;
        }

        //Plano de contas do contador
        public List<Selects> getPlanosContador(int conta_id_contador)
        {
            List<Selects> empresas = new List<Selects>();
            empresas.Add(new Selects {
                value = "",
                text = "Selecione um plano de contas",
                disabled = true
            });

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from planocontas WHERE planocontas.plano_conta_id = @conta_id_contador and planocontas.plano_status = 'Ativo'";
                comando.Parameters.AddWithValue("@conta_id_contador", conta_id_contador);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        empresas.Add(new Selects
                        {
                            value = leitor["plano_id"].ToString(),
                            text = leitor["plano_nome"].ToString(),
                            disabled = false
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return empresas;
        }

        //Contas contábeis do plano especificado
        public List<Selects> getPlanoContas(int plano_id)
        {
            List<Selects> planoContas = new List<Selects>();           

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from contacontabil WHERE contacontabil.ccontabil_plano_id = @plano_id and contacontabil.ccontabil_status = 'Ativo'";
                comando.Parameters.AddWithValue("@plano_id", plano_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    var nivelconta = "";
                    while (leitor.Read())
                    {
                        Selects conta = new Selects();
                        conta.text = leitor["ccontabil_classificacao"].ToString() + "  " + leitor["ccontabil_nome"].ToString();
                        conta.value = leitor["ccontabil_id"].ToString();
                        nivelconta = leitor["ccontabil_nivel"].ToString();
                        if (nivelconta.Equals("5"))
                        {
                            conta.disabled = false;
                        }
                        else
                        {
                            conta.disabled = true;
                        }

                        planoContas.Add(conta);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return planoContas;
        }

        //Plano de categorias do contador
        public List<Selects> getPlanosCategoriaContador(int conta_id_contador)
        {
            List<Selects> planos = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT pc_id, pc_nome, pc_conta_id from planocategorias WHERE planocategorias.pc_conta_id = @contador_id and planocategorias.pc_status = 'Ativo'";
                comando.Parameters.AddWithValue("@contador_id", conta_id_contador);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        planos.Add(new Selects
                        {
                            value = leitor["pc_id"].ToString(),
                            text = leitor["pc_nome"].ToString(),
                            disabled = false
                        });
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return planos;
        }

        //Categorias do cliente
        public List<Selects> getCategoriasCliente(int conta_id, bool semCategoria)
        {
            List<Selects> categorias = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT c.categoria_id, c.categoria_classificacao, c.categoria_nome, c.categoria_tipo from categoria as c WHERE c.categoria_conta_id = @conta_id and c.categoria_status = 'Ativo' order by c.categoria_classificacao;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();
                Selects semCat = new Selects();
                //add select 'Sem Categoria'
                if (semCategoria)
                {
                    semCat.value = "0";
                    semCat.text = "Sem Categoria";
                    semCat.disabled = false;

                    categorias.Add(semCat);
                }


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects categoria = new Selects();

                        string texto = leitor["categoria_classificacao"].ToString() + " - " + leitor["categoria_nome"].ToString();
                        categoria.value = leitor["categoria_id"].ToString();
                        categoria.text = texto;
                        string tipo = leitor["categoria_tipo"].ToString();
                        if (tipo.Equals("Sintetica"))
                        {
                            categoria.disabled = true;
                        }
                        else
                        {
                            categoria.disabled = false;
                        }

                        categorias.Add(categoria);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return categorias;
        }

        //Lista uf igbe (origem: arquivo txt sped fiscal)
        public List<Selects> getUF_ibge()
        {
            List<Selects> ufs_ibge = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT u.uf_ibge_codigo, u.uf_ibge_sigla from uf_ibge as u WHERE u.uf_ibge_data_fim IS NULL;";                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects uf_ibge = new Selects();

                        uf_ibge.value = leitor["uf_ibge_codigo"].ToString();
                        uf_ibge.text = leitor["uf_ibge_sigla"].ToString();
                        uf_ibge.disabled = false;

                        ufs_ibge.Add(uf_ibge);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return ufs_ibge;
        }

        //Tipo contribuinte de acordo com o layout nf-e
        public List<Selects> getTipoContribuinte()
        {
            List<Selects> tipoContribuinte = new List<Selects>();
            tipoContribuinte.Add(new Selects
            {
                value = "1",
                text = "Contribuinte ICMS"
            });
            tipoContribuinte.Add(new Selects
            {
                value = "2",
                text = "Contribuinte isento de Inscrição no cadastro de Contribuintes do ICMS"
            });
            tipoContribuinte.Add(new Selects
            {
                value = "9",
                text = "Não Contribuinte, que pode ou não possuir Inscrição Estadual no Cadastro de Contribuintes do ICMS"
            });

            return tipoContribuinte;
        }

        //regimes de tributação
        public List<Selects> getRegimes()
        {
            List<Selects> regimes = new List<Selects>();
            regimes.Add(new Selects
            {
                value = "1",
                text = "Simples Nacional"
            });
            regimes.Add(new Selects
            {
                value = "2",
                text = "Simples Nacional, excesso sublimite de receita bruta"
            });
            regimes.Add(new Selects
            {
                value = "3",
                text = "Regime Normal"
            });

            return regimes;
        }

        //regimes de tributação
        public List<Selects> getTipoPessoa()
        {
            List<Selects> tipoPessoa = new List<Selects>();
            tipoPessoa.Add(new Selects
            {
                value = "1",
                text = "Pessoa Física"
            });
            tipoPessoa.Add(new Selects
            {
                value = "2",
                text = "Pessoa Jurídica"
            });
            tipoPessoa.Add(new Selects
            {
                value = "3",
                text = "Estrangeiro"
            });

            return tipoPessoa;
        }

        //Lista paises igbe (origem: arquivo txt sped fiscal)
        public List<Selects> getPaises_ibge()
        {
            List<Selects> paises = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT p.paisesIBGE_codigo, p.paisesIBGE_nome from paisesibge as p WHERE p.paisesIBGE_data_fim IS NULL;";
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects pais = new Selects();

                        pais.value = leitor["paisesIBGE_codigo"].ToString();
                        pais.text = leitor["paisesIBGE_nome"].ToString();
                        pais.disabled = false;

                        paises.Add(pais);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return paises;
        }

        //Status de cadastros
        public List<Selects> getStatus()
        {
            List<Selects> status = new List<Selects>();
            status.Add(new Selects
            {
                value = "Ativo",
                text = "Ativo"
            });
            status.Add(new Selects
            {
                value = "Inativo",
                text = "Inativo"
            });            

            return status;
        }

        //Metodo para lista de origem mercadoria
        public List<Selects> getOrigemMercadoria()
        {
            List<Selects> origem = new List<Selects>();
            origem.Add(new Selects
            {
                value = "0",
                text = "0 - Nacional, exceto as indicadas nos códigos 3, 4, 5 e 8"
            });
            origem.Add(new Selects
            {
                value = "1",
                text = "1 - Estrangeira - Importação direta, exceto a indicada no código 6"
            });
            origem.Add(new Selects
            {
                value = "2",
                text = "2 - Estrangeira - Adquirida no mercado interno, exceto a indicada no código 7"
            });
            origem.Add(new Selects
            {
                value = "3",
                text = "3 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 40% e inferior ou igual a 70%"
            });
            origem.Add(new Selects
            {
                value = "4",
                text = "4 - Nacional, cuja produção tenha sido feita em conformidade com os processos produtivos básicos de que tratam as legislações citadas nos Ajustes"
            });
            origem.Add(new Selects
            {
                value = "5",
                text = "5 - Nacional, mercadoria ou bem com Conteúdo de Importação inferior ou igual a 40% "
            });
            origem.Add(new Selects
            {
                value = "6",
                text = "6 - Estrangeira - Importação direta, sem similar nacional, constante em lista da CAMEX e gás natural"
            });
            origem.Add(new Selects
            {
                value = "7",
                text = "7 - Estrangeira - Adquirida no mercado interno, sem similar nacional, constante lista CAMEX e gás natural"
            });
            origem.Add(new Selects
            {
                value = "8",
                text = "8 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 70%"
            });

            return origem;
        }

        //Tipo item
        public List<Selects> getTipoItem()
        {
            List<Selects> status = new List<Selects>();
            status.Add(new Selects
            {
                value = "00",
                text = "Mercadoria para Revenda"
            });
            status.Add(new Selects
            {
                value = "01",
                text = "Matéria-Prima"
            });
            status.Add(new Selects
            {
                value = "02",
                text = "Embalagem"
            });
            status.Add(new Selects
            {
                value = "03",
                text = "Produto em Processo"
            });
            status.Add(new Selects
            {
                value = "04",
                text = "Produto Acabado"
            });
            status.Add(new Selects
            {
                value = "05",
                text = "Subproduto"
            });
            status.Add(new Selects
            {
                value = "06",
                text = "Produto Intermediário"
            });
            status.Add(new Selects
            {
                value = "07",
                text = "Material de Uso e Consumo"
            });
            status.Add(new Selects
            {
                value = "08",
                text = "Ativo Imobilizado"
            });
            status.Add(new Selects
            {
                value = "09",
                text = "Serviços"
            });
            status.Add(new Selects
            {
                value = "10",
                text = "Outros insumos"
            });
            status.Add(new Selects
            {
                value = "99",
                text = "Outras"
            });

            return status;
        }

        //Tipos conta corrente
        public List<Selects> getTiposContaCorrente()
        {
            List<Selects> tipo = new List<Selects>();
            tipo.Add(new Selects
            {
                value = "Caixa",
                text = "Caixa",
                disabled = false
            });
            tipo.Add(new Selects
            {
                value = "Banco",
                text = "Banco",
                disabled = false
            });
            tipo.Add(new Selects
            {
                value = "Maquininha de Cartão",
                text = "Maquininha de Cartão",
                disabled = false
            });
            tipo.Add(new Selects
            {
                value = "Vale",
                text = "Vale",
                disabled = true
            });

            return tipo;
        }

        //Lista meios de pagamento nfe
        public List<Selects> getMeioPagamento()
        {
            List<Selects> mps = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from meio_pgto WHERE meio_pgto_status = 'Ativo';";
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects mp = new Selects();

                        mp.value = leitor["meio_pgto_codigo"].ToString();
                        mp.text = leitor["meio_pgto_descricao"].ToString();
                        mp.disabled = false;

                        mps.Add(mp);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return mps;
        }

        //Identificação forma de pagamento
        public List<Selects> getTipoFormaPgto()
        {
            List<Selects> tipo = new List<Selects>();
            tipo.Add(new Selects
            {
                value = "Pagamento",
                text = "Pagamento",
                disabled = false
            });
            tipo.Add(new Selects
            {
                value = "Recebimento",
                text = "Recebimento",
                disabled = false
            });            

            return tipo;
        }

        //Lista contas correntes do cliente // Para uso na forma de pagamentos
        public List<Selects> getContasCorrente(int conta_id)
        {
            List<Selects> selects = new List<Selects>();            

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT cc.ccorrente_id, cc.ccorrente_nome from conta_corrente as cc WHERE cc.ccorrente_conta_id = @conta_id and cc.ccorrente_status = 'Ativo';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["ccorrente_id"].ToString();
                        select.text = leitor["ccorrente_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }

                Selects selectPadrao = new Selects();
                selectPadrao.value = "0";
                selectPadrao.text = "Diversos";
                selectPadrao.disabled = false;
                selects.Add(selectPadrao);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        //Lista contas correntes do cliente
        public List<Selects> getContasCorrenteConta_id(int conta_id)
        {
            List<Selects> selects = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT cc.ccorrente_id, cc.ccorrente_nome from conta_corrente as cc WHERE cc.ccorrente_conta_id = @conta_id and cc.ccorrente_status = 'Ativo';";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["ccorrente_id"].ToString();
                        select.text = leitor["ccorrente_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        //Identificação forma de pagamento
        public List<Selects> getIntegracaoCartao()
        {
            List<Selects> tipo = new List<Selects>();
            tipo.Add(new Selects
            {
                value = "TEF",
                text = "TEF",
                disabled = false
            });
            tipo.Add(new Selects
            {
                value = "POS",
                text = "POS",
                disabled = false
            });

            return tipo;
        }

        //Lista bandeiras cartão
        public List<Selects> getBandeirasCartao()
        {
            List<Selects> selects = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from bandeira_cartao WHERE bd_status = 'Ativo';";                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["bd_cod"].ToString();
                        select.text = leitor["bd_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        //Lista contas correntes do cliente com filtro por tipo de conta
        public List<Selects> getContasCorrenteTipo(int conta_id, string tipo)
        {
            List<Selects> selects = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT cc.ccorrente_id, cc.ccorrente_nome, cc.ccorrente_tipo from conta_corrente as cc WHERE cc.ccorrente_conta_id = @conta_id and cc.ccorrente_status = 'Ativo' and cc.ccorrente_tipo = @tipo;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@tipo", tipo);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["ccorrente_id"].ToString();
                        select.text = leitor["ccorrente_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }               
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        //Natureza contábil das contas
        public List<Selects> getNaturezaContabil()
        {
            List<Selects> tipo = new List<Selects>();
            tipo.Add(new Selects
            {
                value = "1",
                text = "Devedora",
                disabled = false
            });
            tipo.Add(new Selects
            {
                value = "-1",
                text = "Credora",
                disabled = false
            });

            return tipo;
        }

        //Lista formas de pagamento
        public List<Selects> getFormaPgto(int conta_id, string identificacao)
        {
            List<Selects> selects = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT f.fp_id, f.fp_nome from forma_pagamento as f WHERE f.fp_conta_id = @conta_id and f.fp_status = 'Ativo' and f.fp_identificacao = @identificacao;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@identificacao", identificacao);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["fp_id"].ToString();
                        select.text = leitor["fp_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        //Método para listar modalidades de frete
        public List<Selects> getModFrete()
        {
            List<Selects> modFrete = new List<Selects>();
            modFrete.Add(new Selects
            {
                value = "0",
                text = "Contratação do Frete por conta do Remetente (CIF)"
            });
            modFrete.Add(new Selects
            {
                value = "1",
                text = "Contratação do Frete por conta do Destinatário (FOB)"
            });
            modFrete.Add(new Selects
            {
                value = "2",
                text = "Contratação do Frete por conta de Terceiros"
            });
            modFrete.Add(new Selects
            {
                value = "3",
                text = "Transporte Próprio por conta do Remetente"
            });
            modFrete.Add(new Selects
            {
                value = "4",
                text = "Transporte Próprio por conta do Destinatário"
            });
            modFrete.Add(new Selects
            {
                value = "9",
                text = "Sem Ocorrência de Transporte"
            });

            return modFrete;
        }

        public List<Selects> getTipoOperacaoCCM()
        {
            List<Selects> TipoOperacao = new List<Selects>();
            TipoOperacao.Add(new Selects
            {
                value = "0",
                text = "Todas"
            });
            TipoOperacao.Add(new Selects
            {
                value = "1",
                text = "Compra"
            });
            TipoOperacao.Add(new Selects
            {
                value = "2",
                text = "Venda"
            });
            TipoOperacao.Add(new Selects
            {
                value = "3",
                text = "Prestação de Serviço"
            });
            TipoOperacao.Add(new Selects
            {
                value = "4",
                text = "Serviço Tomado"
            });            

            return TipoOperacao;
        }

        public List<Selects> getStatusServico()
        {
            List<Selects> TipoOperacao = new List<Selects>();
            TipoOperacao.Add(new Selects
            {
                value = "Orçamento",
                text = "Orçamento"
            });
            TipoOperacao.Add(new Selects
            {
                value = "Aprovado e em execução",
                text = "Aprovado e em execução"
            });            
            TipoOperacao.Add(new Selects
            {
                value = "Aprovado e concluído",
                text = "Aprovado e concluído"
            });
            TipoOperacao.Add(new Selects
            {
                value = "Não aprovado a devolver",
                text = "Não aprovado a devolver"
            });
            TipoOperacao.Add(new Selects
            {
                value = "Não aprovado devolvido sem conclusão",
                text = "Não aprovado devolvido sem conclusão"
            });

            return TipoOperacao;
        }

        public List<Selects> getSituacaoContas()
        {
            List<Selects> TipoOperacao = new List<Selects>();
            TipoOperacao.Add(new Selects
            {
                value = "0",
                text = "Todas"
            });
            TipoOperacao.Add(new Selects
            {
                value = "1",
                text = "Em aberto"
            });
            TipoOperacao.Add(new Selects
            {
                value = "2",
                text = "Pagas"
            });            

            return TipoOperacao;
        }

        public List<Selects> getTipoOperacao(string contexto)
        {
            List<Selects> TipoOperacao = new List<Selects>();
            TipoOperacao.Add(new Selects
            {
                value = "0",
                text = "Todas"
            });

            if(contexto == "Pagamento")
            {
                TipoOperacao.Add(new Selects
                {
                    value = "1",
                    text = "Compra"
                });
                TipoOperacao.Add(new Selects
                {
                    value = "4",
                    text = "Serviço Tomado"
                });
                TipoOperacao.Add(new Selects
                {
                    value = "5",
                    text = "Contas Financeiras"
                });
            }
            if (contexto == "Rcebimento")
            {
                TipoOperacao.Add(new Selects
                {
                    value = "2",
                    text = "Venda"
                });
                TipoOperacao.Add(new Selects
                {
                    value = "3",
                    text = "Prestação de Serviço"
                });
                TipoOperacao.Add(new Selects
                {
                    value = "5",
                    text = "Contas Financeiras"
                });
            }

            return TipoOperacao;
        }

        public List<Selects> getFormaPgtoContas(int conta_id, string identificacao)
        {
            List<Selects> selects = new List<Selects>();
            //Select padrão
            selects.Add(new Selects
            {
                value = "0",
                text = "Todas"
            });

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT f.fp_id, f.fp_nome from forma_pagamento as f WHERE f.fp_conta_id = @conta_id and f.fp_status = 'Ativo' and f.fp_identificacao = @identificacao;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@identificacao", identificacao);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["fp_id"].ToString();
                        select.text = leitor["fp_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        public List<Selects> getVencimentoContas()
        {
            List<Selects> TipoOperacao = new List<Selects>();
            TipoOperacao.Add(new Selects
            {
                value = "0",
                text = "Todas"
            });
            TipoOperacao.Add(new Selects
            {
                value = "1",
                text = "Hoje"
            });
            TipoOperacao.Add(new Selects
            {
                value = "2",
                text = "Atrasadas"
            });
            TipoOperacao.Add(new Selects
            {
                value = "3",
                text = "A Vencer"
            });

            return TipoOperacao;
        }

        //Forma de pagamento para cartão de crédito (necessário forma pgto tipo bolet bancário)
        public List<Selects> getFormaPgto_boletoBancario(int conta_id, string identificacao)
        {
            List<Selects> selects = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT f.fp_id, f.fp_nome from forma_pagamento as f WHERE f.fp_conta_id = @conta_id and f.fp_status = 'Ativo' and f.fp_identificacao = @identificacao and f.fp_meio_pgto_nfe = 15;";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@identificacao", identificacao);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["fp_id"].ToString();
                        select.text = leitor["fp_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        public List<Selects> getTipoCtaFinanceira()
        {
            List<Selects> selects = new List<Selects>();
            selects.Add(new Selects
            {
                value = "Realizada",
                text = "Realizada"
            });
            selects.Add(new Selects
            {
                value = "Realizar",
                text = "À Realizar"
            });

            return selects;
        }

        public List<Selects> getRecorrencias()
        {
            List<Selects> selects = new List<Selects>();
            selects.Add(new Selects
            {
                value = "Unica",
                text = "Única"
            });
            selects.Add(new Selects
            {
                value = "Semanal",
                text = "Semanal"
            });
            selects.Add(new Selects
            {
                value = "Quinzenal",
                text = "Quinzenal"
            });
            selects.Add(new Selects
            {
                value = "Mensal",
                text = "Mensal"
            });
            selects.Add(new Selects
            {
                value = "Bimestral",
                text = "Bimestral"
            });
            selects.Add(new Selects
            {
                value = "Trimestral",
                text = "Trimestral"
            });
            selects.Add(new Selects
            {
                value = "Semestral",
                text = "Semestral"
            });
            selects.Add(new Selects
            {
                value = "Anual",
                text = "Anual"
            });

            return selects;
        }

        //Lista formas de pagamento de acrodo com o contexto da categoria
        public List<Selects> getFormaPgto_categoria_id(int conta_id, int categoria_id)
        {
            List<Selects> selects = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_formaPgto_categoria(@conta_id,@categoria_id);";
                comando.Parameters.AddWithValue("@conta_id", conta_id);
                comando.Parameters.AddWithValue("@categoria_id", categoria_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["fp_id"].ToString();
                        select.text = leitor["fp_nome"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }

        //Lista tipos documentos tipo_nf
        public List<Selects> getTipoNF()
        {
            List<Selects> selects = new List<Selects>();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from tipo_nf WHERE tipo_nf.tipo_nf_status = 'Ativo';";                
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        Selects select = new Selects();

                        select.value = leitor["tipo_nf_codigo"].ToString();
                        select.text = leitor["tipo_nf_descricao"].ToString();
                        select.disabled = false;

                        selects.Add(select);
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return selects;
        }


    }
}
