using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Autenticacao
{
    public class Login
    {
        [Required]
        public string usuario { get; set; }

        [Required]
        public string senha { get; set; }
    }
}
