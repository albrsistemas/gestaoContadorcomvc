using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Areas.Contabilidade.Models;
using gestaoContadorcomvc.Areas.Contabilidade.Models.ViewModel;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Areas.Contabilidade.Controllers
{
    [Area("Contabilidade")]
    [Route("Contabilidade/[controller]/[action]")]
    [Authorize(Roles = "Contabilidade")]
    public class BalanceteController : Controller
    {
        // GET: BalanceteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BalanceteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BalanceteController/Create
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
            Conta contexto = new Conta();
            contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

            Config_contador_cliente config = new Config_contador_cliente();
            vm_ConfigContadorCliente vm_config = new vm_ConfigContadorCliente();
            vm_config = config.buscaCCC(user.usuario_id, contexto.conta_id, user.usuario_conta_id);

            vm_balancete vm_b = new vm_balancete();
            vm_b.balancete = null;
            vm_b.vm_config = vm_config;            

            return View(vm_b);
        }

        // POST: BalanceteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["balancete"] = "Erro no preenchimento das informações do formulário!";

                    return RedirectToAction(nameof(Create));
                }

                DateTime data_inical = Convert.ToDateTime(collection["data_inicial"]);
                DateTime data_final = Convert.ToDateTime(collection["data_final"]);

                if(data_inical.Year.ToString() != data_final.Year.ToString())
                {
                    TempData["balancete"] = "Erro, as datas inicial e final precisam compreender o mesmo exercício!";

                    return RedirectToAction(nameof(Create));
                }

                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
                Conta contexto = new Conta();
                contexto = contexto.contextoCliente(Convert.ToInt32(user.usuario_ultimoCliente));

                Config_contador_cliente config = new Config_contador_cliente();
                vm_ConfigContadorCliente vm_config = new vm_ConfigContadorCliente();
                vm_config = config.buscaCCC(user.usuario_id, contexto.conta_id, user.usuario_conta_id);

                vm_balancete vm_b = new vm_balancete();
                vm_b.user = user;
                vm_b.vm_config = vm_config;
                vm_b.data_inicial = data_inical;
                vm_b.data_final = data_final;

                Balancete balancete = new Balancete();

                vm_b.balancete = balancete.gerarBalancete(data_inical, data_final, vm_config.ccc_planoContasVigente, contexto.conta_id, user.usuario_conta_id, user.usuario_id);                

                return View(vm_b);
            }
            catch
            {
                TempData["balancete"] = "Erro ao gerar o balancete!";

                return RedirectToAction(nameof(Create));
            }
        }

        // GET: BalanceteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BalanceteController/Edit/5
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

        // GET: BalanceteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BalanceteController/Delete/5
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
