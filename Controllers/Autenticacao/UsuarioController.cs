using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace gestaoContadorcomvc.Controllers.Autenticacao
{
    [FiltroAutenticacao]
    public class UsuarioController : Controller
    {
        // GET: UsuarioController
        public ActionResult Index()
        {
            ViewData["bread"] = "Usuário";

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            TempData["user"] = user;

            return View(user.listaUsuarios(user.usuario_conta_id, user.usuario_id));
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            ViewData["bread"] = "Usuário / Novo";

            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection dados)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            string retorno = "Erro ao cadastrar usuário, tente novamente. Se persistir entre em contato com o suporte !";

            try
            {
                retorno = user.novoUsuario(dados["usuario_nome"], dados["usuario_dcto"], dados["usuario_user"], dados["usuario_senha"], user.usuario_conta_id, dados["usuario_email"], dados["permissoes"], user.usuario_id);

                TempData["novoUsuario"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["novoUsuario"] = retorno;

                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["bread"] = "Usuário / Alteração";

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Vm_usuario usuario = new Vm_usuario();

            usuario = user.BuscaUsuario(id);

            return View(usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection dados)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Vm_usuario usuario = new Vm_usuario();
            usuario = user.BuscaUsuario(id);

            try
            {
                TempData["editUsuario"] = user.alteraUsuario(dados["usuario_nome"], dados["usuario_dcto"], user.usuario_conta_id, dados["usuario_email"], dados["permissoes"], id, user.usuario_id);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuario);
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            ViewData["bread"] = "Usuário / Apagar";

            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Vm_usuario usuario = new Vm_usuario();

            usuario = user.BuscaUsuario(id);

            return View(usuario);
            
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Vm_usuario usuario = new Vm_usuario();
            usuario = user.BuscaUsuario(id);

            try
            {
                TempData["deleteUsuario"] = user.deletaUsuario(user.usuario_conta_id, id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuario);
            }
        }

        //Alterar senha do usuário (restrito ao adm)
        public ActionResult EditPassword(int id)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            Vm_usuario usuario = new Vm_usuario();

            usuario = user.BuscaUsuario(id);

            return View(usuario);

        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(int id, IFormCollection dados)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Vm_usuario usuario = new Vm_usuario();
            usuario = user.BuscaUsuario(id);

            try
            {
                TempData["EditPasswordUsuario"] = user.EditPassword(user.usuario_conta_id, id, user.usuario_id, dados["usuario_user"], dados["usuario_senha"]);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuario);
            }
        }

    }
}
