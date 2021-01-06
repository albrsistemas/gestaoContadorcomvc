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
    }
}
