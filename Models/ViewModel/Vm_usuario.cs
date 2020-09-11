﻿using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.ViewModel
{
    public class Vm_usuario
    {
        //Atributos
        public int usuario_id { get; set; }

        public int usuario_conta_id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome")]
        public string usuario_nome { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O cpf é obrigatório.")]
        [RegularExpression(@"[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}",
         ErrorMessage = "Quantidade de dígitos do cnpj ou cpf inválido.")]
        public string usuario_dcto { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "É obrigatório definir um usuário.")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Mínimo de 4 caracteres e máximo de 40")]
        [Remote("userExiste", "Conta", ErrorMessage = "Usuário já existe")]
        public string usuario_user { get; set; }

        [Required(ErrorMessage = "É obrigatório definir uma senha.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Mínimo de 4 caracteres e máximo de 10")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string usuario_senha { get; set; }

        [Compare("usuario_senha", ErrorMessage = "A senha não confere")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação de Senha")]
        public string confirmaSenha { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [Remote("emailExiste", "Conta", ErrorMessage = "E-mail já cadastrado")]
        public string usuario_email { get; set; }

        public string Role { get; set; }
        public string permissoes { get; set; }
    }
}
