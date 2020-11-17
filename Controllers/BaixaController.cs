using System;
using System.Linq;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    public class BaixaController : Controller
    {
        // GET: BaixaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BaixaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BaixaController/Create
        public ActionResult Create(int parcela_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();
            Op_parcelas_baixa b = new Op_parcelas_baixa();
            vm_baixa = b.buscaDados_para_Baixa(user.usuario_conta_id, user.usuario_id, parcela_id);

            Selects select = new Selects();
            ViewBag.ccorrente = select.getContasCorrenteConta_id(user.usuario_conta_id).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            DateTime today = DateTime.Today;
            vm_baixa.data = today;

            return View(vm_baixa);
        }

        // POST: BaixaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            string retorno = "";
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Vm_op_parcelas_baixa vm_baixa = new Vm_op_parcelas_baixa();
                Op_parcelas_baixa b = new Op_parcelas_baixa();

                retorno = b.cadastrarBaixa(user.usuario_id, user.usuario_conta_id, Convert.ToInt32(collection["parcela_id"]), Convert.ToInt32(collection["contacorrente_id"]), Convert.ToDateTime(collection["data"]), Convert.ToDecimal(collection["valor"]), collection["obs"]);

                TempData["msgCP"] = retorno;

                return RedirectToAction("Index", collection["contexto"]);
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao gravar a baixa. Tente novamente. Se persistir entre em contato com o suporte!";
                }

                TempData["msgCP"] = retorno;

                return RedirectToAction("Index", collection["contexto"]);
            }
        }

        // GET: BaixaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BaixaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BaixaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BaixaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
