using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class MemorandoController : Controller
    {
        [Autoriza(permissao = "memorandoList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Memorando m = new Memorando();
            m.memorandos = m.listMemorando(user.usuario_conta_id, user.usuario_id);
            m.user = user;            

            return View(m);
        }


        [Autoriza(permissao = "memorandoCreate")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemorandoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Memorando m = new Memorando();

                retorno = m.cadastraMemorando(user.usuario_conta_id, user.usuario_id, d["memorando_codigo"], d["memorando_descricao"]);

                TempData["msgMemorando"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao cadastrar o memorando. Tente novamente, se persistir, entre em contato com o suporte!";
                }
                TempData["msgMemorando"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "memorandoEdit")]
        public ActionResult Edit(int memorando_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Memorando m = new Memorando();
            m = m.buscaMemorando(user.usuario_conta_id, user.usuario_id, memorando_id);
            m.user = user;

            return View(m);
        }

        // POST: MemorandoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int memorando_id, IFormCollection d)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Memorando m = new Memorando();

                retorno = m.alteraMemorando(user.usuario_conta_id, user.usuario_id, memorando_id, d["memorando_codigo"], d["memorando_descricao"]);

                TempData["msgMemorando"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao alterar o memorando. Tente novamente, se persistir, entre em contato com o suporte!";
                }
                TempData["msgMemorando"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "memorandoDelete")]
        public ActionResult Delete(int memorando_id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Memorando m = new Memorando();
            m = m.buscaMemorando(user.usuario_conta_id, user.usuario_id, memorando_id);
            m.user = user;

            return View(m);
        }

        // POST: MemorandoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int memorando_id, IFormCollection collection)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                Memorando m = new Memorando();

                retorno = m.deleteMemorando(user.usuario_conta_id, user.usuario_id, memorando_id);

                TempData["msgMemorando"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao apagar o memorando. Tente novamente, se persistir, entre em contato com o suporte!";
                }
                TempData["msgMemorando"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
