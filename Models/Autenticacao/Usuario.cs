using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace gestaoContadorcomvc.Models.Autenticacao
{
    public class Usuario
    {        
        //Atributos
        public int usuario_id { get; set; }
        public int usuario_conta_id { get; set; }
        public string usuario_nome { get; set; }     
        public string usuario_dcto { get; set; }   
        public string usuario_user { get; set; }        
        public string usuario_senha { get; set; }        
        public string usuario_email { get; set; }
        public string Role { get; set; }
        public string permissoes { get; set; }
        public Conta conta { get; set; }
        public string usuario_forgt_token { get; set; }
        public DateTime usuario_forgt_data { get; set; }

        /*--------------------------*/
        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Usuario()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS
        //objeto de log para uso nos métodos
        Log log = new Log();

        //Localizar usuario pelo user e senha para verificação de login
        public Usuario buscaUsuarioLogin(string usuario, string senha)
        {
            Usuario user = new Usuario();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("select * from usuario where usuario_senha = md5(@senha) and (usuario_user = md5(@usuario) or usuario.usuario_email = @usuario) and usuario_status != 'Deletado';", conn);
                comando.Parameters.AddWithValue("@senha", senha);
                comando.Parameters.AddWithValue("@usuario", usuario);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        user.usuario_id = Convert.ToInt32(leitor["usuario_id"]);
                        user.usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]);
                        user.usuario_nome = leitor["usuario_nome"].ToString();
                        user.usuario_dcto = leitor["usuario_dcto"].ToString();
                        user.usuario_email = leitor["usuario_email"].ToString();                        
                        user.Role = leitor["Role"].ToString();
                        user.permissoes = leitor["usuario_permissoes"].ToString();                        
                    }
                }                

                conn.Close();
            }
            catch (Exception e)
            {
                string erro = e.ToString();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return user;
        }

        //Lista usuario pela conta (lista não traz o usuário 'adm' e o usuario que fez a requisição

        public List<Vm_usuario> listaUsuarios(int conta_id, int usuario_id)
        {
            List<Vm_usuario> lista = new List<Vm_usuario>();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_conta_id = @conta and Role = 'user' and usuario_id != @usuario_id and usuario_status != 'Deletado';", conn);
                comando.Parameters.AddWithValue("@conta", conta_id);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        lista.Add(new Vm_usuario
                        {
                            usuario_nome = leitor["usuario_nome"].ToString(),
                            usuario_email = leitor["usuario_email"].ToString(),
                            usuario_dcto = leitor["usuario_dcto"].ToString(),                            
                            usuario_id = Convert.ToInt32(leitor["usuario_id"]),                            
                        });
                    }
                }                

                conn.Close();
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

            return lista;
        }

        //Lista de usuários com exlusão do usuário 'adm' e o usuário requerente
        public List<Usuario> ListaUsuario(int conta_id, int usuario_id)
        {
            List<Usuario> usuarios = new List<Usuario>(usuario_id);            

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_conta_id = @conta and Role = 'user' and usuario_id != @usuario_id and usuario_status != 'Deletado';", conn);
                comando.Parameters.AddWithValue("@conta", conta_id);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        usuarios.Add(new gestaoContadorcomvc.Models.Autenticacao.Usuario
                        {
                            usuario_nome = leitor["usuario_nome"].ToString(),
                            usuario_email = leitor["usuario_email"].ToString(),
                            usuario_dcto = leitor["usuario_dcto"].ToString(),
                            usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]),
                            usuario_id = Convert.ToInt32(leitor["usuario_id"]),
                            Role = leitor["Role"].ToString(),
                            permissoes = leitor["usuario_permissoes"].ToString()
                        });
                    }
                }                

                conn.Close();
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

            return usuarios;
        }

        //Cadastrar novo usuário
        public string novoUsuario(string nome, string dcto, string usuario, string senha, int conta_id, string email, string permissoes, int usuario_id, Permissoes _permissoes)
        {
            string retorno = "Usuário cadastrado com sucesso !";
            
            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "call pr_novoUsuario (@usuario_nome, @usuario_dcto, @usuario_user, @usuario_senha, @usuario_conta_id, @usuario_permissoes, @usuario_email);";
                comando.Parameters.AddWithValue("@usuario_nome", nome);
                comando.Parameters.AddWithValue("@usuario_dcto", dcto);
                comando.Parameters.AddWithValue("@usuario_user", usuario);
                comando.Parameters.AddWithValue("@usuario_senha", senha);
                comando.Parameters.AddWithValue("@usuario_conta_id", conta_id);
                comando.Parameters.AddWithValue("@usuario_email", email);
                comando.Parameters.AddWithValue("@usuario_permissoes", permissoes);
                comando.ExecuteNonQuery();

                comando.CommandText = "insert into permissoes (permissoes_usuario_id, usuarioList, usuarioCreate, usuarioEdit, usuarioDelete, categoriaList, categoriaCreate, categoriaEdit, categoriaDelete, config, configContabilidade, planoContasList, planoContasCreate, planoContasEdit, planoContasDelete, contasContabeisList, contasContabeisCreate, contasContabeisEdit, contasContabeisDelete, planoCategoriasList, planoCategoriasCreate, planoCategoriasEdit, planoCategoriasDelete, categoriasPlanoList, categoriasPlanoCreate, categoriasPlanoEdit, categoriasPlanoDelete, clienteConfigList, clienteConfigCreate, clienteConfigEdit, clienteCategoriasList, clienteCategoriasCreate, clienteCategoriasEdit, clienteCategoriasDelete, clienteCopiaPlano, participanteList, participanteCreate, participanteEdit, participanteDelete, produtosList, produtosCreate, produtosEdit, produtosDelete, ccorrenteList, ccorrenteCreate, ccorrenteEdit, ccorrenteDelete, fpList, fpCreate, fpEdit, fpDelete, compraList, compraCreate, compraEdit, compraDelete, ContasPList, CCMList, baixaList, baixaCreate, baixaEdit, baixaDelete, vendaList, vendaCreate, vendaEdit, vendaDelete, CCMCreate, CCMEdit, CCMDelete, servicoPList, servicoPCreate, servicoPEdit, servicoPDelete, contasFList, contasFCreate, contasFEdit, contasFDelete,  rCategoriasList, servicoTList, servicoTCreate, servicoTEdit, servicoTDelete, operacaoList, operacaoCreate, operacaoEdit, operacaoDelete, ContasRList, memorandoList, memorandoCreate, memorandoEdit, memorandoDelete) VALUES (LAST_INSERT_ID(), @usuarioList, @usuarioCreate, @usuarioEdit, @usuarioDelete, @categoriaList, @categoriaCreate, @categoriaEdit, @categoriaDelete, @config, @configContabilidade, @planoContasList, @planoContasCreate, @planoContasEdit, @planoContasDelete, @contasContabeisList, @contasContabeisCreate, @contasContabeisEdit, @contasContabeisDelete, @planoCategoriasList, @planoCategoriasCreate, @planoCategoriasEdit, @planoCategoriasDelete, @categoriasPlanoList, @categoriasPlanoCreate, @categoriasPlanoEdit, @categoriasPlanoDelete, @clienteConfigList, @clienteConfigCreate, @clienteConfigEdit, @clienteCategoriasList, @clienteCategoriasCreate, @clienteCategoriasEdit, @clienteCategoriasDelete, @clienteCopiaPlano, @participanteList, @participanteCreate, @participanteEdit, @participanteDelete, @produtosList, @produtosCreate, @produtosEdit, @produtosDelete, @ccorrenteList, @ccorrenteCreate, @ccorrenteEdit, @ccorrenteDelete, @fpList, @fpCreate, @fpEdit, @fpDelete, @compraList, @compraCreate, @compraEdit, @compraDelete, @ContasPList, @CCMList, @baixaList, @baixaCreate, @baixaEdit, @baixaDelete, @vendaList, @vendaCreate, @vendaEdit, @vendaDelete, @CCMCreate, @CCMEdit, @CCMDelete,@servicoPList,@servicoPCreate,@servicoPEdit,@servicoPDelete,@contasFList,@contasFCreate,@contasFEdit,@contasFDelete, @rCategoriasList, @servicoTList, @servicoTCreate, @servicoTEdit, @servicoTDelete, @operacaoList, @operacaoCreate, @operacaoEdit, @operacaoDelete, @ContasRList, @memorandoList, @memorandoCreate, @memorandoEdit, @memorandoDelete);";
                comando.Parameters.AddWithValue("@usuarioList", _permissoes.usuarioList);
                comando.Parameters.AddWithValue("@usuarioCreate", _permissoes.usuarioCreate);
                comando.Parameters.AddWithValue("@usuarioEdit", _permissoes.usuarioEdit);
                comando.Parameters.AddWithValue("@usuarioDelete", _permissoes.usuarioDelete);
                comando.Parameters.AddWithValue("@categoriaList", _permissoes.categoriaList);
                comando.Parameters.AddWithValue("@categoriaCreate", _permissoes.categoriaCreate);
                comando.Parameters.AddWithValue("@categoriaEdit", _permissoes.categoriaEdit);
                comando.Parameters.AddWithValue("@categoriaDelete", _permissoes.categoriaDelete);
                comando.Parameters.AddWithValue("@config", _permissoes.config);
                comando.Parameters.AddWithValue("@configContabilidade", _permissoes.configContabilidade);
                comando.Parameters.AddWithValue("@planoContasList", _permissoes.planoContasList);
                comando.Parameters.AddWithValue("@planoContasCreate", _permissoes.planoContasCreate);
                comando.Parameters.AddWithValue("@planoContasEdit", _permissoes.planoContasEdit);
                comando.Parameters.AddWithValue("@planoContasDelete", _permissoes.planoContasDelete);
                comando.Parameters.AddWithValue("@contasContabeisList", _permissoes.contasContabeisList);
                comando.Parameters.AddWithValue("@contasContabeisCreate", _permissoes.contasContabeisCreate);
                comando.Parameters.AddWithValue("@contasContabeisEdit", _permissoes.contasContabeisEdit);
                comando.Parameters.AddWithValue("@contasContabeisDelete", _permissoes.contasContabeisDelete);
                comando.Parameters.AddWithValue("@planoCategoriasList", _permissoes.planoCategoriasList);
                comando.Parameters.AddWithValue("@planoCategoriasCreate", _permissoes.planoCategoriasCreate);
                comando.Parameters.AddWithValue("@planoCategoriasEdit", _permissoes.planoCategoriasEdit);
                comando.Parameters.AddWithValue("@planoCategoriasDelete", _permissoes.planoCategoriasDelete);
                comando.Parameters.AddWithValue("@categoriasPlanoList", _permissoes.categoriasPlanoList);
                comando.Parameters.AddWithValue("@categoriasPlanoCreate", _permissoes.categoriasPlanoCreate);
                comando.Parameters.AddWithValue("@categoriasPlanoEdit", _permissoes.categoriasPlanoEdit);
                comando.Parameters.AddWithValue("@categoriasPlanoDelete", _permissoes.categoriasPlanoDelete);
                comando.Parameters.AddWithValue("@clienteConfigList", _permissoes.clienteConfigList);
                comando.Parameters.AddWithValue("@clienteConfigCreate", _permissoes.clienteConfigCreate);
                comando.Parameters.AddWithValue("@clienteConfigEdit", _permissoes.clienteConfigEdit);
                comando.Parameters.AddWithValue("@clienteCategoriasList", _permissoes.clienteCategoriasList);
                comando.Parameters.AddWithValue("@clienteCategoriasCreate", _permissoes.clienteCategoriasCreate);
                comando.Parameters.AddWithValue("@clienteCategoriasEdit", _permissoes.clienteCategoriasEdit);
                comando.Parameters.AddWithValue("@clienteCategoriasDelete", _permissoes.clienteCategoriasDelete);
                comando.Parameters.AddWithValue("@clienteCopiaPlano", _permissoes.clienteCopiaPlano);
                comando.Parameters.AddWithValue("@participanteList", _permissoes.participanteList);
                comando.Parameters.AddWithValue("@participanteCreate", _permissoes.participanteCreate);
                comando.Parameters.AddWithValue("@participanteEdit", _permissoes.participanteEdit);
                comando.Parameters.AddWithValue("@participanteDelete", _permissoes.participanteDelete);
                comando.Parameters.AddWithValue("@produtosList", _permissoes.produtosList);
                comando.Parameters.AddWithValue("@produtosCreate", _permissoes.produtosCreate);
                comando.Parameters.AddWithValue("@produtosEdit", _permissoes.produtosEdit);
                comando.Parameters.AddWithValue("@produtosDelete", _permissoes.produtosDelete);
                comando.Parameters.AddWithValue("@ccorrenteList", _permissoes.ccorrenteList);
                comando.Parameters.AddWithValue("@ccorrenteCreate", _permissoes.ccorrenteCreate);
                comando.Parameters.AddWithValue("@ccorrenteEdit", _permissoes.ccorrenteEdit);
                comando.Parameters.AddWithValue("@ccorrenteDelete", _permissoes.ccorrenteDelete);
                comando.Parameters.AddWithValue("@fpList", _permissoes.fpList);
                comando.Parameters.AddWithValue("@fpCreate", _permissoes.fpCreate);
                comando.Parameters.AddWithValue("@fpEdit", _permissoes.fpEdit);
                comando.Parameters.AddWithValue("@fpDelete", _permissoes.fpDelete);
                comando.Parameters.AddWithValue("@compraList", _permissoes.compraList);
                comando.Parameters.AddWithValue("@compraCreate", _permissoes.compraCreate);
                comando.Parameters.AddWithValue("@compraEdit", _permissoes.compraEdit);
                comando.Parameters.AddWithValue("@compraDelete", _permissoes.compraDelete);
                comando.Parameters.AddWithValue("@ContasPList", _permissoes.ContasPList);
                comando.Parameters.AddWithValue("@CCMList", _permissoes.CCMList);
                comando.Parameters.AddWithValue("@baixaList", _permissoes.baixaList);
                comando.Parameters.AddWithValue("@baixaCreate", _permissoes.baixaCreate);
                comando.Parameters.AddWithValue("@baixaEdit", _permissoes.baixaEdit);
                comando.Parameters.AddWithValue("@baixaDelete", _permissoes.baixaDelete);
                comando.Parameters.AddWithValue("vendaList", _permissoes.vendaList);
                comando.Parameters.AddWithValue("vendaCreate", _permissoes.vendaCreate);
                comando.Parameters.AddWithValue("vendaEdit", _permissoes.vendaEdit);
                comando.Parameters.AddWithValue("vendaDelete", _permissoes.vendaDelete);
                comando.Parameters.AddWithValue("CCMCreate", _permissoes.CCMCreate);
                comando.Parameters.AddWithValue("CCMEdit", _permissoes.CCMEdit);
                comando.Parameters.AddWithValue("CCMDelete", _permissoes.CCMDelete);
                //Incluido em 28/12/2020
                comando.Parameters.AddWithValue("ContasRList", _permissoes.ContasRList);
                comando.Parameters.AddWithValue("servicoPList", _permissoes.servicoPList);
                comando.Parameters.AddWithValue("servicoPCreate", _permissoes.servicoPCreate);
                comando.Parameters.AddWithValue("servicoPEdit", _permissoes.servicoPEdit);
                comando.Parameters.AddWithValue("servicoPDelete", _permissoes.servicoPDelete);
                comando.Parameters.AddWithValue("contasFList", _permissoes.contasFList);
                comando.Parameters.AddWithValue("contasFCreate", _permissoes.contasFCreate);
                comando.Parameters.AddWithValue("contasFEdit", _permissoes.contasFEdit);
                comando.Parameters.AddWithValue("contasFDelete", _permissoes.contasFDelete);                
                comando.Parameters.AddWithValue("rCategoriasList", _permissoes.rCategoriasList); 
                comando.Parameters.AddWithValue("servicoTList", _permissoes.servicoTList);                
                comando.Parameters.AddWithValue("servicoTCreate", _permissoes.servicoTCreate);                
                comando.Parameters.AddWithValue("servicoTEdit", _permissoes.servicoTEdit);                
                comando.Parameters.AddWithValue("servicoTDelete", _permissoes.servicoTDelete);
                //01/02/2021                
                comando.Parameters.AddWithValue("operacaoList", _permissoes.operacaoList);
                comando.Parameters.AddWithValue("operacaoCreate", _permissoes.operacaoCreate);
                comando.Parameters.AddWithValue("operacaoEdit", _permissoes.operacaoEdit);
                comando.Parameters.AddWithValue("operacaoDelete", _permissoes.operacaoDelete);
                //02/02/2021
                comando.Parameters.AddWithValue("memorandoList", _permissoes.memorandoList);
                comando.Parameters.AddWithValue("memorandoCreate", _permissoes.memorandoCreate);
                comando.Parameters.AddWithValue("memorandoEdit", _permissoes.memorandoEdit);
                comando.Parameters.AddWithValue("memorandoDelete", _permissoes.memorandoDelete);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Tentativa de criar novo usuário de nome: " + nome + " Cadastrado com sucesso";
                log.log("Usuario", "novoUsuario", "Sucesso", msg, conta_id, usuario_id);
            }
            catch (Exception e)
            {
                string msg = "Tentativa de criar novo usuário de nome: " + nome + " fracassou" + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao cadastrar usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "novoUsuario", "Erro", msg, conta_id, usuario_id);
                Transacao.Rollback();                                
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

        //Busca um usuário por id
        public Vm_usuario BuscaUsuario(int usuario_id)
        {
            Vm_usuario usuario = new Vm_usuario();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario where usuario_id = @usuario_id;", conn);                
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        usuario.usuario_nome = leitor["usuario_nome"].ToString();
                        usuario.usuario_email = leitor["usuario_email"].ToString();
                        usuario.usuario_dcto = leitor["usuario_dcto"].ToString();
                        usuario.usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]);
                        usuario.usuario_id = Convert.ToInt32(leitor["usuario_id"]);
                        usuario.Role = leitor["Role"].ToString();
                        usuario.permissoes = leitor["usuario_permissoes"].ToString();
                        usuario.usuario_ultimoCliente = leitor["usuario_ultimoCliente"].ToString();
                    }
                }               

                conn.Close();
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

            Permissoes permissoes = new Permissoes();
            permissoes = permissoes.listaPermissoes(usuario_id);
            usuario._permissoes = permissoes;

            Conta conta = new Conta();
            conta = conta.buscarConta(usuario.usuario_conta_id);
            usuario.conta = conta;

            return usuario;
        }

        //Altera usuário
        public string alteraUsuario(string nome, string dcto, int conta_id, string email, string permissoes, int usuario_id, int usuarioLogado, Permissoes _permissoes)
        {
            string retorno = "Usuário alterado com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE usuario set usuario_nome = @usuario_nome, usuario_dcto = @usuario_dcto, usuario_email = @usuario_email, usuario_permissoes = @usuario_permissoes where usuario_id = @usuario_id;";
                comando.Parameters.AddWithValue("@usuario_nome", nome);
                comando.Parameters.AddWithValue("@usuario_dcto", dcto);                
                comando.Parameters.AddWithValue("@usuario_email", email);
                comando.Parameters.AddWithValue("@usuario_permissoes", permissoes);
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.ExecuteNonQuery();

                comando.CommandText = ("update permissoes set usuarioList = @usuarioList, usuarioCreate = @usuarioCreate, usuarioEdit = @usuarioEdit, usuarioDelete = @usuarioDelete, categoriaList = @categoriaList, categoriaCreate = @categoriaCreate, categoriaEdit = @categoriaEdit, categoriaDelete = @categoriaDelete, config = @config, configContabilidade = @configContabilidade, planoContasList = @planoContasList, planoContasCreate = @planoContasCreate, planoContasEdit = @planoContasEdit, planoContasDelete = @planoContasDelete, contasContabeisList = @contasContabeisList, contasContabeisCreate = @contasContabeisCreate, contasContabeisEdit = @contasContabeisEdit, contasContabeisDelete = @contasContabeisDelete, planoCategoriasList = @planoCategoriasList, planoCategoriasCreate = @planoCategoriasCreate, planoCategoriasEdit = @planoCategoriasEdit, planoCategoriasDelete = @planoCategoriasDelete, categoriasPlanoList = @categoriasPlanoList, categoriasPlanoCreate = @categoriasPlanoCreate, categoriasPlanoEdit = @categoriasPlanoEdit, categoriasPlanoDelete = @categoriasPlanoDelete, clienteConfigList = @clienteConfigList, clienteConfigCreate = @clienteConfigCreate, clienteConfigEdit = @clienteConfigEdit, clienteCategoriasList = @clienteCategoriasList, clienteCategoriasCreate = @clienteCategoriasCreate, clienteCategoriasEdit = @clienteCategoriasEdit, clienteCategoriasDelete = @clienteCategoriasDelete, clienteCopiaPlano = @clienteCopiaPlano, participanteList = @participanteList, participanteCreate = @participanteCreate, participanteEdit = @participanteEdit, participanteDelete = @participanteDelete, produtosList = @produtosList, produtosCreate = @produtosCreate, produtosEdit = @produtosEdit, produtosDelete = @produtosDelete, ccorrenteList = @ccorrenteList, ccorrenteCreate = @ccorrenteCreate, ccorrenteEdit = @ccorrenteEdit, ccorrenteDelete = @ccorrenteDelete, fpList = @fpList, fpCreate = @fpCreate, fpEdit = @fpEdit, fpDelete = @fpDelete, compraList = @compraList, compraCreate = @compraCreate, compraEdit = @compraEdit, compraDelete = @compraDelete, ContasPList = @ContasPList, CCMList = @CCMList, baixaList = @baixaList, baixaCreate = @baixaCreate, baixaEdit = @baixaEdit, baixaDelete = @baixaDelete, vendaList = @vendaList, vendaCreate = @vendaCreate, vendaEdit = @vendaEdit, vendaDelete = @vendaDelete, CCMCreate = @CCMCreate, CCMEdit = @CCMEdit, CCMDelete = @CCMDelete, ContasRList = @ContasRList, servicoPList = @servicoPList, servicoPCreate = @servicoPCreate, servicoPEdit = @servicoPEdit, servicoPDelete = @servicoPDelete, contasFList = @contasFList, contasFCreate = @contasFCreate, contasFEdit = @contasFEdit, contasFDelete = @contasFDelete, rCategoriasList = @rCategoriasList, servicoTList = @servicoTList, servicoTCreate = @servicoTCreate, servicoTEdit = @servicoTEdit, servicoTDelete = @servicoTDelete, operacaoList = @operacaoList, operacaoCreate = @operacaoCreate, operacaoEdit = @operacaoEdit, operacaoDelete = @operacaoDelete, memorandoList = @memorandoList, memorandoCreate = @memorandoCreate, memorandoEdit = @memorandoEdit, memorandoDelete = @memorandoDelete  where permissoes_usuario_id = @permissoes_usuario_id");
                comando.Parameters.AddWithValue("@usuarioList", _permissoes.usuarioList);
                comando.Parameters.AddWithValue("@usuarioCreate", _permissoes.usuarioCreate);
                comando.Parameters.AddWithValue("@usuarioEdit", _permissoes.usuarioEdit);
                comando.Parameters.AddWithValue("@usuarioDelete", _permissoes.usuarioDelete);
                comando.Parameters.AddWithValue("@categoriaList", _permissoes.categoriaList);
                comando.Parameters.AddWithValue("@categoriaCreate", _permissoes.categoriaCreate);
                comando.Parameters.AddWithValue("@categoriaEdit", _permissoes.categoriaEdit);
                comando.Parameters.AddWithValue("@categoriaDelete", _permissoes.categoriaDelete);
                comando.Parameters.AddWithValue("@config", _permissoes.config);
                comando.Parameters.AddWithValue("@configContabilidade", _permissoes.configContabilidade);
                comando.Parameters.AddWithValue("@planoContasList", _permissoes.planoContasList);
                comando.Parameters.AddWithValue("@planoContasCreate", _permissoes.planoContasCreate);
                comando.Parameters.AddWithValue("@planoContasEdit", _permissoes.planoContasEdit);
                comando.Parameters.AddWithValue("@planoContasDelete", _permissoes.planoContasDelete);
                comando.Parameters.AddWithValue("@contasContabeisList", _permissoes.contasContabeisList);
                comando.Parameters.AddWithValue("@contasContabeisCreate", _permissoes.contasContabeisCreate);
                comando.Parameters.AddWithValue("@contasContabeisEdit", _permissoes.contasContabeisEdit);
                comando.Parameters.AddWithValue("@contasContabeisDelete", _permissoes.contasContabeisDelete);
                comando.Parameters.AddWithValue("@planoCategoriasList", _permissoes.planoCategoriasList);
                comando.Parameters.AddWithValue("@planoCategoriasCreate", _permissoes.planoCategoriasCreate);
                comando.Parameters.AddWithValue("@planoCategoriasEdit", _permissoes.planoCategoriasEdit);
                comando.Parameters.AddWithValue("@planoCategoriasDelete", _permissoes.planoCategoriasDelete);
                comando.Parameters.AddWithValue("@categoriasPlanoList", _permissoes.categoriasPlanoList);
                comando.Parameters.AddWithValue("@categoriasPlanoCreate", _permissoes.categoriasPlanoCreate);
                comando.Parameters.AddWithValue("@categoriasPlanoEdit", _permissoes.categoriasPlanoEdit);
                comando.Parameters.AddWithValue("@categoriasPlanoDelete", _permissoes.categoriasPlanoDelete);
                comando.Parameters.AddWithValue("@clienteConfigList", _permissoes.clienteConfigList);
                comando.Parameters.AddWithValue("@clienteConfigCreate", _permissoes.clienteConfigCreate);
                comando.Parameters.AddWithValue("@clienteConfigEdit", _permissoes.clienteConfigEdit);
                comando.Parameters.AddWithValue("@clienteCategoriasList", _permissoes.clienteCategoriasList);
                comando.Parameters.AddWithValue("@clienteCategoriasCreate", _permissoes.clienteCategoriasCreate);
                comando.Parameters.AddWithValue("@clienteCategoriasEdit", _permissoes.clienteCategoriasEdit);
                comando.Parameters.AddWithValue("@clienteCategoriasDelete", _permissoes.clienteCategoriasDelete);
                comando.Parameters.AddWithValue("@clienteCopiaPlano", _permissoes.clienteCopiaPlano);
                comando.Parameters.AddWithValue("@permissoes_usuario_id", usuario_id);
                comando.Parameters.AddWithValue("@participanteList", _permissoes.participanteList);
                comando.Parameters.AddWithValue("@participanteCreate", _permissoes.participanteCreate);
                comando.Parameters.AddWithValue("@participanteEdit", _permissoes.participanteEdit);
                comando.Parameters.AddWithValue("@participanteDelete", _permissoes.participanteDelete);
                comando.Parameters.AddWithValue("@produtosList", _permissoes.produtosList);
                comando.Parameters.AddWithValue("@produtosCreate", _permissoes.produtosCreate);
                comando.Parameters.AddWithValue("@produtosEdit", _permissoes.produtosEdit);
                comando.Parameters.AddWithValue("@produtosDelete", _permissoes.produtosDelete);
                comando.Parameters.AddWithValue("@ccorrenteList", _permissoes.ccorrenteList);
                comando.Parameters.AddWithValue("@ccorrenteCreate", _permissoes.ccorrenteCreate);
                comando.Parameters.AddWithValue("@ccorrenteEdit", _permissoes.ccorrenteEdit);
                comando.Parameters.AddWithValue("@ccorrenteDelete", _permissoes.ccorrenteDelete);
                comando.Parameters.AddWithValue("@fpList", _permissoes.fpList);
                comando.Parameters.AddWithValue("@fpCreate", _permissoes.fpCreate);
                comando.Parameters.AddWithValue("@fpEdit", _permissoes.fpEdit);
                comando.Parameters.AddWithValue("@fpDelete", _permissoes.fpDelete);
                comando.Parameters.AddWithValue("@compraList", _permissoes.compraList);
                comando.Parameters.AddWithValue("@compraCreate", _permissoes.compraCreate);
                comando.Parameters.AddWithValue("@compraEdit", _permissoes.compraEdit);
                comando.Parameters.AddWithValue("@compraDelete", _permissoes.compraDelete);
                comando.Parameters.AddWithValue("@ContasPList", _permissoes.ContasPList);
                comando.Parameters.AddWithValue("@CCMList", _permissoes.CCMList);
                comando.Parameters.AddWithValue("@baixaList", _permissoes.baixaList);
                comando.Parameters.AddWithValue("@baixaCreate", _permissoes.baixaCreate);
                comando.Parameters.AddWithValue("@baixaEdit", _permissoes.baixaEdit);
                comando.Parameters.AddWithValue("@baixaDelete", _permissoes.baixaDelete);
                comando.Parameters.AddWithValue("vendaList", _permissoes.vendaList);
                comando.Parameters.AddWithValue("vendaCreate", _permissoes.vendaCreate);
                comando.Parameters.AddWithValue("vendaEdit", _permissoes.vendaEdit);
                comando.Parameters.AddWithValue("vendaDelete", _permissoes.vendaDelete);
                comando.Parameters.AddWithValue("CCMCreate", _permissoes.CCMCreate);
                comando.Parameters.AddWithValue("CCMEdit", _permissoes.CCMEdit);
                comando.Parameters.AddWithValue("CCMDelete", _permissoes.CCMDelete);
                //Incluido em 28/12/2020
                comando.Parameters.AddWithValue("ContasRList", _permissoes.ContasRList);
                comando.Parameters.AddWithValue("servicoPList", _permissoes.servicoPList);
                comando.Parameters.AddWithValue("servicoPCreate", _permissoes.servicoPCreate);
                comando.Parameters.AddWithValue("servicoPEdit", _permissoes.servicoPEdit);
                comando.Parameters.AddWithValue("servicoPDelete", _permissoes.servicoPDelete);
                comando.Parameters.AddWithValue("contasFList", _permissoes.contasFList);
                comando.Parameters.AddWithValue("contasFCreate", _permissoes.contasFCreate);
                comando.Parameters.AddWithValue("contasFEdit", _permissoes.contasFEdit);
                comando.Parameters.AddWithValue("contasFDelete", _permissoes.contasFDelete);
                comando.Parameters.AddWithValue("rCategoriasList", _permissoes.rCategoriasList);
                comando.Parameters.AddWithValue("servicoTList", _permissoes.servicoTList);
                comando.Parameters.AddWithValue("servicoTCreate", _permissoes.servicoTCreate);
                comando.Parameters.AddWithValue("servicoTEdit", _permissoes.servicoTEdit);
                comando.Parameters.AddWithValue("servicoTDelete", _permissoes.servicoTDelete);
                //01/02/2021                
                comando.Parameters.AddWithValue("operacaoList", _permissoes.operacaoList);
                comando.Parameters.AddWithValue("operacaoCreate", _permissoes.operacaoCreate);
                comando.Parameters.AddWithValue("operacaoEdit", _permissoes.operacaoEdit);
                comando.Parameters.AddWithValue("operacaoDelete", _permissoes.operacaoDelete);
                //02/02/2021
                comando.Parameters.AddWithValue("memorandoList", _permissoes.memorandoList);
                comando.Parameters.AddWithValue("memorandoCreate", _permissoes.memorandoCreate);
                comando.Parameters.AddWithValue("memorandoEdit", _permissoes.memorandoEdit);
                comando.Parameters.AddWithValue("memorandoDelete", _permissoes.memorandoDelete);
                comando.ExecuteNonQuery();

                Transacao.Commit();

                string msg = "Tentativa de alterar o usuário ID: " + usuario_id + " Alterado com sucesso";
                log.log("Usuario", "alteraUsuario", "Sucesso", retorno, conta_id, usuarioLogado);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de alterar o usuário ID: " + usuario_id + " fracassou" + e.Message.ToString().Substring(0, 300);

                retorno = "Erro ao alterar o usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "alteraUsuario", "Erro", msg, conta_id, usuarioLogado);
                Transacao.Rollback();
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

        //Altera usuário
        public string deletaUsuario(int conta_id, int usuario_id, int usuarioLogado)
        {
            string retorno = "Usuário apagado com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                Random randNum = new Random();
                int numero = randNum.Next(1,15000);
                comando.CommandText = "UPDATE usuario set usuario_status = 'Deletado', usuario.usuario_email = concat(usuario.usuario_email,'|',@numero), usuario.usuario_user = md5(@numero) where usuario_id = @usuario_id;";                
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.Parameters.AddWithValue("@numero", numero);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de apapar o usuário ID: " + usuario_id + " Apagado com Sucesso";
                log.log("Usuario", "deletaUsuario", "Sucesso", msg, conta_id, usuarioLogado);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de apapar o usuário ID: " + usuario_id + " fracassou" + e.Message.ToString().Substring(0, 300);
                retorno = "Erro ao apagar o usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "deletaUsuario", "Erro", msg , conta_id, usuarioLogado);
                Transacao.Rollback();
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

        //Altera usuário
        public string EditPassword(int conta_id, int usuario_id, int usuarioLogado, string usuario_user, string usuario_senha)
        {
            string retorno = "Senha do usuário alterada com sucesso !";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE usuario set usuario_user =  md5(@usuario_user), usuario_senha = md5(@usuario_senha) where usuario_id = @usuario_id;";
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.Parameters.AddWithValue("@usuario_user", usuario_user);
                comando.Parameters.AddWithValue("@usuario_senha", usuario_senha);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Tentativa de alterar os dados de acesso do usuário ID: " + usuario_id + " Alterado com Sucesso";
                log.log("Usuario", "EditPassword", "Sucesso", msg, conta_id, usuarioLogado);

            }
            catch (Exception e)
            {
                string msg = "Tentativa de alterar a senha do usuário ID: " + usuario_id + " fracassou" + e.Message.ToString().Substring(0, 300);
                retorno = "Erro ao alterar os dados de acesso do usuário, tente novamente. Se persistir entre em contato com o suporte !";
                log.log("Usuario", "EditPassword", "Erro", msg, conta_id, usuarioLogado);
                Transacao.Rollback();
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

        //Autualizar últomo cliente que usuário selecionou na área da contabilidade
        public void ultimoCliente(string cliente_id, int usuario_id)
        {
            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "UPDATE usuario set usuario_ultimoCliente = @cliente_id where usuario_id = @usuario_id;";
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.Parameters.AddWithValue("@cliente_id", cliente_id);                
                comando.ExecuteNonQuery();
                Transacao.Commit();
            }
            catch (Exception)
            {                
                Transacao.Rollback();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        //Busca usuário por e-mail
        public Vm_usuario BuscaUsuarioPorEmail(string email)
        {
            Vm_usuario usuario = new Vm_usuario();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT * from usuario WHERE usuario.usuario_email = @email;", conn);
                comando.Parameters.AddWithValue("@email", email);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        usuario.usuario_nome = leitor["usuario_nome"].ToString();
                        usuario.usuario_email = leitor["usuario_email"].ToString();
                        usuario.usuario_dcto = leitor["usuario_dcto"].ToString();
                        usuario.usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]);
                        usuario.usuario_id = Convert.ToInt32(leitor["usuario_id"]);
                        usuario.Role = leitor["Role"].ToString();
                        usuario.permissoes = leitor["usuario_permissoes"].ToString();
                        usuario.usuario_ultimoCliente = leitor["usuario_ultimoCliente"].ToString();
                    }

                    Permissoes permissoes = new Permissoes();
                    permissoes = permissoes.listaPermissoes(usuario_id);
                    usuario._permissoes = permissoes;

                    Conta conta = new Conta();
                    conta = conta.buscarConta(usuario.usuario_conta_id);
                    usuario.conta = conta;
                }
                else
                {
                    usuario = null;
                }
                    

                conn.Close();
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

            return usuario;
        }

        //Busca usuário por token
        public Vm_usuario BuscaUsuarioPorToken(string token)
        {
            Vm_usuario usuario = new Vm_usuario();

            try
            {
                conn.Open();
                MySqlCommand comando = new MySqlCommand("SELECT u.usuario_forgt_token, u.usuario_forgt_data from usuario as u WHERE u.usuario_forgt_token = @token;", conn);
                comando.Parameters.AddWithValue("@token", token);

                var leitor = comando.ExecuteReader();


                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        //usuario.usuario_nome = leitor["usuario_nome"].ToString();
                        //usuario.usuario_email = leitor["usuario_email"].ToString();
                        //usuario.usuario_dcto = leitor["usuario_dcto"].ToString();
                        //usuario.usuario_conta_id = Convert.ToInt32(leitor["usuario_conta_id"]);
                        //usuario.usuario_id = Convert.ToInt32(leitor["usuario_id"]);
                        //usuario.Role = leitor["Role"].ToString();
                        //usuario.permissoes = leitor["usuario_permissoes"].ToString();
                        //usuario.usuario_ultimoCliente = leitor["usuario_ultimoCliente"].ToString();
                        if (DBNull.Value != leitor["usuario_forgt_data"])
                        {
                            usuario.usuario_forgt_data = Convert.ToDateTime(leitor["usuario_forgt_data"]);
                        }
                        else
                        {
                            usuario.usuario_forgt_data = new DateTime();
                        }
                        
                        usuario.usuario_forgt_token = leitor["usuario_forgt_token"].ToString();
                    }

                    //Permissoes permissoes = new Permissoes();
                    //permissoes = permissoes.listaPermissoes(usuario_id);
                    //usuario._permissoes = permissoes;

                    //Conta conta = new Conta();
                    //conta = conta.buscarConta(usuario.usuario_conta_id);
                    //usuario.conta = conta;
                }
                else
                {
                    usuario = null;
                }


                conn.Close();
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

            return usuario;
        }

        //Altera token forgot usuário
        public string AtribuirTokenForgot(int usuario_id, int conta_id, string email, string token)
        {
            string retorno = "sucesso";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                DateTime hoje = DateTime.Today;

                comando.CommandText = "UPDATE usuario set usuario.usuario_forgt_token = @token, usuario.usuario_forgt_data = @data WHERE usuario.usuario_email = @email;";                
                comando.Parameters.AddWithValue("@email", email);
                comando.Parameters.AddWithValue("@token", token);
                comando.Parameters.AddWithValue("@data", hoje);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                string msg = "Solicitação de alteração de senha para o usuário ID: " + usuario_id + " enviado para e-mail: " + email;
                log.log("Usuario", "AtribuirTokenForgot", "Sucesso", msg, conta_id, usuario_id);

            }
            catch (Exception e)
            {
                string msg = "olicitação de alteração de senha para o usuário ID: " + usuario_id + " fracassou" + e.Message.ToString().Substring(0, 300);
                retorno = "Erro";
                log.log("Usuario", "EditPassword", "Erro", msg, conta_id, usuario_id);
                Transacao.Rollback();
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

        //Altera senha do usuário
        public string EditSenhaTokenForgot(string novasenha, string token)
        {
            string retorno = "Redefinição de senha realizada com sucesso";

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                DateTime hoje = DateTime.Today;

                comando.CommandText = "UPDATE usuario set usuario.usuario_senha = md5(@novasenha), usuario.usuario_forgt_token = '', usuario.usuario_forgt_data = '' WHERE usuario.usuario_forgt_token = @token;";
                comando.Parameters.AddWithValue("@novasenha", novasenha);
                comando.Parameters.AddWithValue("@token", token);                
                comando.ExecuteNonQuery();
                Transacao.Commit();

            }
            catch (Exception e)
            {   
                retorno = "Erro ao gravar a nova senha!";                

                Transacao.Rollback();
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
