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
            f.gera_provisao_categoria_fiscal = false;
            ilcs.filtro = f;

            List<ImportacaoLancamentosContabeis> list_ilc = new List<ImportacaoLancamentosContabeis>();
            list_ilc = null;
            ilcs.list_ilc = list_ilc;

            return View(ilcs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int cliente_id, DateTime data_inicial, DateTime data_final, bool gera_provisao_categoria_fiscal, bool gerar_lancamentos_baixas)
        {
            Ilcs ilcs = new Ilcs();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Selects select = new Selects();
                ViewBag.empresasContador = select.getEmpresasContador(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Selected = c.value == user.usuario_ultimoCliente }).ToList();

                ImportacaoLancamentosContabeis ilc = new ImportacaoLancamentosContabeis();                
                ilcs = ilc.create(user.usuario_id, user.usuario_conta_id, cliente_id, data_inicial, data_final, gera_provisao_categoria_fiscal, gerar_lancamentos_baixas);

                ilc_filter f = new ilc_filter();
                f.cliente_id = cliente_id;
                f.data_inicial = data_inicial;
                f.data_final = data_final;
                f.gera_provisao_categoria_fiscal = gera_provisao_categoria_fiscal;

                ilcs.filtro = f;

                return View(ilcs);

                //return Json(JsonConvert.SerializeObject(ilcs));
            }
            catch
            {
                ilcs.status = "Erro ao gerar informação do relatório. Tente novamente, se persistir, entre em contato com o suporte!";

                return Json(JsonConvert.SerializeObject(ilcs));
            }
        }
    }
}
