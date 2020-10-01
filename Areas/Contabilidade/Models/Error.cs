using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gestaoContadorcomvc.Areas.Contabilidade.Models
{
    public class Error
    {        
        public string controller { get; set; }
        public string action { get; set; }
    }
}
