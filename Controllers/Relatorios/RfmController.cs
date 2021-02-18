using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.Relatorios;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers.Relatorios
{
    [Authorize]
    public class RfmController : Controller
    {
        [Autoriza(permissao = "rCategoriasList")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            DateTime data = DateTime.Today;
            //DateTime com o primeiro dia do mês
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            //DateTime com o último dia do mês
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            Vm_rfm rfm = new Vm_rfm();
            Vm_rfm_filtro filtro = new Vm_rfm_filtro();
            filtro.data_inicio = primeiroDiaDoMes;
            filtro.data_fim = ultimoDiaDoMes;
            filtro.vm_rfm_ignorar_categorias_zeradas = true;
            filtro.vm_rfm_filtros_visao = "caixa";
            rfm.filtro = filtro;

            return View(rfm);
        }

        // POST: Categoria_oppController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Vm_rfm_filtro filtro)
        {
            Vm_rfm rfm = new Vm_rfm();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
                
                rfm = rfm.create(user.conta.conta_id, filtro);

                return Json(JsonConvert.SerializeObject(rfm));
            }
            catch
            {
                rfm.status_processamento = "Erro";
                rfm.erro_processaento = "Ocorreu um erro no processamento do relatório!";
                
                return Json(JsonConvert.SerializeObject(rfm));
            }
        }
    }
}
