﻿using gestaoContadorcomvc.Models.SoftwareHouse;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models
{
    public class Permissoes
    {
        public int permissoes_id { get; set; }
        public int permissoes_usuario_id { get; set; }
        public bool usuarioList { get; set; }
        public bool usuarioCreate { get; set; }
        public bool usuarioEdit { get; set; }
        public bool usuarioDelete { get; set; }
        public bool categoriaList { get; set; }
        public bool categoriaCreate { get; set; }
        public bool categoriaEdit { get; set; }
        public bool categoriaDelete { get; set; }
        public bool config { get; set; }
        public bool configContabilidade { get; set; }
        public bool planoContasList { get; set; }
        public bool planoContasCreate { get; set; }
        public bool planoContasEdit { get; set; }
        public bool planoContasDelete { get; set; }
        public bool contasContabeisList { get; set; }
        public bool contasContabeisCreate { get; set; }
        public bool contasContabeisEdit { get; set; }
        public bool contasContabeisDelete { get; set; }
        public bool planoCategoriasList { get; set; }
        public bool planoCategoriasCreate { get; set; }
        public bool planoCategoriasEdit { get; set; }
        public bool planoCategoriasDelete { get; set; }
        public bool categoriasPlanoList { get; set; }
        public bool categoriasPlanoCreate { get; set; }
        public bool categoriasPlanoEdit { get; set; }
        public bool categoriasPlanoDelete { get; set; }
        public bool clienteConfigList { get; set; }
        public bool clienteConfigCreate { get; set; }
        public bool clienteConfigEdit { get; set; }
        public bool clienteCategoriasList { get; set; }
        public bool clienteCategoriasCreate { get; set; }
        public bool clienteCategoriasEdit { get; set; }
        public bool clienteCategoriasDelete { get; set; }
        public bool clienteCopiaPlano { get; set; }

        //Métodos para pegar a string de conexão do arquivo appsettings.json e gerar conexão no MySql.      
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        //Método para gerar a conexão
        MySqlConnection conn;
        public Permissoes()
        {
            var configuration = GetConfiguration();
            conn = new MySqlConnection(configuration.GetSection("ConnectionStrings").GetSection("conexaocvc").Value);
        }

        //MÉTODOS

        //objeto de log para uso nos métodos
        Log log = new Log();

        //Buscar permissões por usuário
        public Permissoes listaPermissoes(int usuario_id)
        {
            Permissoes permissao = new Permissoes();

            conn.Open();
            MySqlCommand comando = conn.CreateCommand();
            MySqlTransaction Transacao;
            Transacao = conn.BeginTransaction();
            comando.Connection = conn;
            comando.Transaction = Transacao;

            try
            {
                comando.CommandText = "SELECT * from permissoes where permissoes.permissoes_usuario_id = @usuario_id;";
                comando.Parameters.AddWithValue("@usuario_id", usuario_id);
                comando.ExecuteNonQuery();
                Transacao.Commit();

                var leitor = comando.ExecuteReader();

                if (leitor.HasRows)
                {
                    while (leitor.Read())
                    {
                        if (DBNull.Value != leitor["permissoes_id"])
                        {
                            permissao.permissoes_id = Convert.ToInt32(leitor["permissoes_id"]);
                        }
                        else
                        {
                            permissao.permissoes_id = 0;
                        }

                        if (DBNull.Value != leitor["permissoes_usuario_id"])
                        {
                            permissao.permissoes_usuario_id = Convert.ToInt32(leitor["permissoes_usuario_id"]);
                        }
                        else
                        {
                            permissao.permissoes_usuario_id = 0;
                        }

                        if (DBNull.Value != leitor["usuarioList"])
                        {
                            permissao.usuarioList = Convert.ToBoolean(leitor["usuarioList"]);
                        }
                        else
                        {
                            permissao.usuarioList = false;
                        }

                        //----
                        if (DBNull.Value != leitor["usuarioCreate"])
                        {
                            permissao.usuarioCreate = Convert.ToBoolean(leitor["usuarioCreate"]);
                        }
                        else
                        {
                            permissao.usuarioCreate = false;
                        }
                        if (DBNull.Value != leitor["usuarioEdit"])
                        {
                            permissao.usuarioEdit = Convert.ToBoolean(leitor["usuarioEdit"]);
                        }
                        else
                        {
                            permissao.usuarioEdit = false;
                        }
                        if (DBNull.Value != leitor["usuarioDelete"])
                        {
                            permissao.usuarioDelete = Convert.ToBoolean(leitor["usuarioDelete"]);
                        }
                        else
                        {
                            permissao.usuarioDelete = false;
                        }
                        if (DBNull.Value != leitor["categoriaList"])
                        {
                            permissao.categoriaList = Convert.ToBoolean(leitor["categoriaList"]);
                        }
                        else
                        {
                            permissao.categoriaList = false;
                        }
                        if (DBNull.Value != leitor["categoriaCreate"])
                        {
                            permissao.categoriaCreate = Convert.ToBoolean(leitor["categoriaCreate"]);
                        }
                        else
                        {
                            permissao.categoriaCreate = false;
                        }
                        if (DBNull.Value != leitor["categoriaEdit"])
                        {
                            permissao.categoriaEdit = Convert.ToBoolean(leitor["categoriaEdit"]);
                        }
                        else
                        {
                            permissao.categoriaEdit = false;
                        }
                        if (DBNull.Value != leitor["categoriaDelete"])
                        {
                            permissao.categoriaDelete = Convert.ToBoolean(leitor["categoriaDelete"]);
                        }
                        else
                        {
                            permissao.categoriaDelete = false;
                        }
                        if (DBNull.Value != leitor["config"])
                        {
                            permissao.config = Convert.ToBoolean(leitor["config"]);
                        }
                        else
                        {
                            permissao.config = false;
                        }
                        if (DBNull.Value != leitor["configContabilidade"])
                        {
                            permissao.configContabilidade = Convert.ToBoolean(leitor["configContabilidade"]);
                        }
                        else
                        {
                            permissao.configContabilidade = false;
                        }
                        if (DBNull.Value != leitor["planoContasList"])
                        {
                            permissao.planoContasList = Convert.ToBoolean(leitor["planoContasList"]);
                        }
                        else
                        {
                            permissao.planoContasList = false;
                        }
                        if (DBNull.Value != leitor["planoContasCreate"])
                        {
                            permissao.planoContasCreate = Convert.ToBoolean(leitor["planoContasCreate"]);
                        }
                        else
                        {
                            permissao.planoContasCreate = false;
                        }
                        if (DBNull.Value != leitor["planoContasEdit"])
                        {
                            permissao.planoContasEdit = Convert.ToBoolean(leitor["planoContasEdit"]);
                        }
                        else
                        {
                            permissao.planoContasEdit = false;
                        }
                        if (DBNull.Value != leitor["planoContasDelete"])
                        {
                            permissao.planoContasDelete = Convert.ToBoolean(leitor["planoContasDelete"]);
                        }
                        else
                        {
                            permissao.planoContasDelete = false;
                        }
                        if (DBNull.Value != leitor["contasContabeisList"])
                        {
                            permissao.contasContabeisList = Convert.ToBoolean(leitor["contasContabeisList"]);
                        }
                        else
                        {
                            permissao.contasContabeisList = false;
                        }
                        if (DBNull.Value != leitor["contasContabeisCreate"])
                        {
                            permissao.contasContabeisCreate = Convert.ToBoolean(leitor["contasContabeisCreate"]);
                        }
                        else
                        {
                            permissao.contasContabeisCreate = false;
                        }
                        if (DBNull.Value != leitor["contasContabeisEdit"])
                        {
                            permissao.contasContabeisEdit = Convert.ToBoolean(leitor["contasContabeisEdit"]);
                        }
                        else
                        {
                            permissao.contasContabeisEdit = false;
                        }
                        if (DBNull.Value != leitor["contasContabeisDelete"])
                        {
                            permissao.contasContabeisDelete = Convert.ToBoolean(leitor["contasContabeisDelete"]);
                        }
                        else
                        {
                            permissao.contasContabeisDelete = false;
                        }
                        if (DBNull.Value != leitor["planoCategoriasList"])
                        {
                            permissao.planoCategoriasList = Convert.ToBoolean(leitor["planoCategoriasList"]);
                        }
                        else
                        {
                            permissao.planoCategoriasList = false;
                        }
                        if (DBNull.Value != leitor["planoCategoriasCreate"])
                        {
                            permissao.planoCategoriasCreate = Convert.ToBoolean(leitor["planoCategoriasCreate"]);
                        }
                        else
                        {
                            permissao.planoCategoriasCreate = false;
                        }
                        if (DBNull.Value != leitor["planoCategoriasEdit"])
                        {
                            permissao.planoCategoriasEdit = Convert.ToBoolean(leitor["planoCategoriasEdit"]);
                        }
                        else
                        {
                            permissao.planoCategoriasEdit = false;
                        }
                        if (DBNull.Value != leitor["planoCategoriasDelete"])
                        {
                            permissao.planoCategoriasDelete = Convert.ToBoolean(leitor["planoCategoriasDelete"]);
                        }
                        else
                        {
                            permissao.planoCategoriasDelete = false;
                        }
                        if (DBNull.Value != leitor["categoriasPlanoList"])
                        {
                            permissao.categoriasPlanoList = Convert.ToBoolean(leitor["categoriasPlanoList"]);
                        }
                        else
                        {
                            permissao.categoriasPlanoList = false;
                        }
                        if (DBNull.Value != leitor["categoriasPlanoCreate"])
                        {
                            permissao.categoriasPlanoCreate = Convert.ToBoolean(leitor["categoriasPlanoCreate"]);
                        }
                        else
                        {
                            permissao.categoriasPlanoCreate = false;
                        }
                        if (DBNull.Value != leitor["categoriasPlanoEdit"])
                        {
                            permissao.categoriasPlanoEdit = Convert.ToBoolean(leitor["categoriasPlanoEdit"]);
                        }
                        else
                        {
                            permissao.categoriasPlanoEdit = false;
                        }
                        if (DBNull.Value != leitor["categoriasPlanoDelete"])
                        {
                            permissao.categoriasPlanoDelete = Convert.ToBoolean(leitor["categoriasPlanoDelete"]);
                        }
                        else
                        {
                            permissao.categoriasPlanoDelete = false;
                        }
                        if (DBNull.Value != leitor["clienteConfigList"])
                        {
                            permissao.clienteConfigList = Convert.ToBoolean(leitor["clienteConfigList"]);
                        }
                        else
                        {
                            permissao.clienteConfigList = false;
                        }
                        if (DBNull.Value != leitor["clienteConfigCreate"])
                        {
                            permissao.clienteConfigCreate = Convert.ToBoolean(leitor["clienteConfigCreate"]);
                        }
                        else
                        {
                            permissao.clienteConfigCreate = false;
                        }
                        if (DBNull.Value != leitor["clienteConfigEdit"])
                        {
                            permissao.clienteConfigEdit = Convert.ToBoolean(leitor["clienteConfigEdit"]);
                        }
                        else
                        {
                            permissao.clienteConfigEdit = false;
                        }
                        if (DBNull.Value != leitor["clienteCategoriasList"])
                        {
                            permissao.clienteCategoriasList = Convert.ToBoolean(leitor["clienteCategoriasList"]);
                        }
                        else
                        {
                            permissao.clienteCategoriasList = false;
                        }
                        if (DBNull.Value != leitor["clienteCategoriasCreate"])
                        {
                            permissao.clienteCategoriasCreate = Convert.ToBoolean(leitor["clienteCategoriasCreate"]);
                        }
                        else
                        {
                            permissao.clienteCategoriasCreate = false;
                        }
                        if (DBNull.Value != leitor["clienteCategoriasEdit"])
                        {
                            permissao.clienteCategoriasEdit = Convert.ToBoolean(leitor["clienteCategoriasEdit"]);
                        }
                        else
                        {
                            permissao.clienteCategoriasEdit = false;
                        }
                        if (DBNull.Value != leitor["clienteCategoriasDelete"])
                        {
                            permissao.clienteCategoriasDelete = Convert.ToBoolean(leitor["clienteCategoriasDelete"]);
                        }
                        else
                        {
                            permissao.clienteCategoriasDelete = false;
                        }
                        if (DBNull.Value != leitor["clienteCopiaPlano"])
                        {
                            permissao.clienteCopiaPlano = Convert.ToBoolean(leitor["clienteCopiaPlano"]);
                        }
                        else
                        {
                            permissao.clienteCopiaPlano = false;
                        }
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

            return permissao;
        }


    }
}