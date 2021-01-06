using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Autenticacao
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Obrigatório informar um e-mail")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "É obrigatório definir uma senha.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Mínimo de 4 caracteres e máximo de 10")]
        [DataType(DataType.Password)]
        [Display(Name = "usuario_senha")]
        public string senha { get; set; }

        [Compare("senha", ErrorMessage = "A senha não confere")]
        [DataType(DataType.Password)]
        public string confirmaSenha { get; set; }

    }
}
