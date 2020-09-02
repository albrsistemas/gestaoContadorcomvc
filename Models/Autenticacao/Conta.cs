using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Models.Autenticacao
{
    public class Conta
    {
        public int conta_id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        public string conta_dcto { get; set; }

        public string conta_tipo { get; set; }

        [Required]
        public string conta_email { get; set; }

    }
}
