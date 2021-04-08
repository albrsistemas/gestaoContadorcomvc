using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models.SCI;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [Authorize(Roles = "Contabilidade")]
    public class ImportacaoLancamentosContabeisController : Controller
    {
        public IActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);            

            Ilcs ilcs = new Ilcs();
            ilc_filter f = new ilc_filter();
            DateTime h = DateTime.Now;
            f.data_inicial = h.AddDays(-30);
            f.data_final = h;
            f.gerar_lancamentos_baixas = false;
            f.gerar_lancamentos_ccm = false;
            f.gerar_pgto_a_participante_categoria_fiscal_sem_nf_informada = false;
            ilcs.filtro = f;

            List<ImportacaoLancamentosContabeis> list_ilc = new List<ImportacaoLancamentosContabeis>();
            list_ilc = null;
            ilcs.list_ilc = list_ilc;

            return View(ilcs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(int cliente_id, DateTime data_inicial, DateTime data_final, bool gera_provisao_categoria_fiscal, bool gerar_lancamentos_baixas, bool gear_lancamentos_ccm)
        public ActionResult Create(Ilcs ilcs)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);                

                ImportacaoLancamentosContabeis ilc = new ImportacaoLancamentosContabeis();
                Ilcs new_ilcs = new Ilcs();
                //new_ilcs = ilc.create(user.usuario_id, user.usuario_conta_id,ilcs.filtro.cliente_id, ilcs.filtro.data_inicial, ilcs.filtro.data_final, ilcs.filtro.gerar_lancamentos_baixas, ilcs.filtro.gerar_lancamentos_ccm, ilcs.filtro.gerar_pgto_a_participante_categoria_fiscal_sem_nf_informada);
                new_ilcs = ilc.create(user.usuario_id, user.conta.conta_id,ilcs.filtro.cliente_id, ilcs.filtro.data_inicial, ilcs.filtro.data_final, ilcs.filtro.gerar_lancamentos_baixas, ilcs.filtro.gerar_lancamentos_ccm, ilcs.filtro.gerar_pgto_a_participante_categoria_fiscal_sem_nf_informada);
                new_ilcs.filtro = ilcs.filtro;                

                return View(new_ilcs);                
            }
            catch
            {
                TempData["ilc_retorno"] = "Erro ao gerar informação do relatório. Tente novamente, se persistir, entre em contato com o suporte!";
                
                Ilcs new_ilcs = new Ilcs();

                return View(ilcs);
            }
        }
    }
}
