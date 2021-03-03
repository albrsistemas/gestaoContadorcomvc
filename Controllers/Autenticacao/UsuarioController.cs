using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace gestaoContadorcomvc.Controllers.Autenticacao
{
    [Authorize]
    public class UsuarioController : Controller
    {
        [Autoriza(permissao = "usuarioList")]
        // GET: UsuarioController
        public ActionResult Index()
        {
            ViewData["bread"] = "Usuário";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            user.usuarios = usuario.listaUsuarios(user.usuario_conta_id, user.usuario_id);

            return View(user);
        }

        [Autoriza(permissao = "usuarioCreate")]
        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            ViewData["bread"] = "Usuário / Novo";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            user.usuario_id = 0;
            user.usuario_nome = "";
            user.usuario_dcto = "";
            user.usuario_email = "";
            user.usuario_user = "";

            return View(user);
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection dados, [Bind] Vm_usuario vm_user)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            string retorno = "";

            try
            {
                retorno = usuario.novoUsuario(dados["usuario_nome"], dados["usuario_dcto"], dados["usuario_user"], dados["usuario_senha"], user.usuario_conta_id, dados["usuario_email"], dados["permissoes"], user.usuario_id, vm_user._permissoes);

                TempData["novoUsuario"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao cadastrar usuário, tente novamente. Se persistir entre em contato com o suporte!";
                }

                TempData["novoUsuario"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "usuarioEdit")]
        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["bread"] = "Usuário / Alteração";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_usuario usuarioEdit = new Vm_usuario();

            usuarioEdit = usuario.BuscaUsuario_id(id);

            return View(usuarioEdit);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection dados, [Bind] Vm_usuario vm_user)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_usuario usuarioEdit = new Vm_usuario();
            usuarioEdit = usuario.BuscaUsuario_id(id);

            string retorno = "";

            if(id == user.usuario_id)
            {
                retorno = "Erro. Não pode ser alterado o próprio usuário.";

                return RedirectToAction(nameof(Index));
            }

            try
            {
                retorno =  usuario.alteraUsuario(dados["usuario_nome"], dados["usuario_dcto"], user.usuario_conta_id, dados["usuario_email"], dados["permissoes"], id, user.usuario_id, vm_user._permissoes);

                TempData["editUsuario"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if(retorno == "")
                {
                    retorno = "Erro ao alterar usuário, tente novamente. Se persistir entre em contato com o suporte!";
                }

                TempData["editUsuario"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "usuarioDelete")]
        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            ViewData["bread"] = "Usuário / Apagar";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_usuario usuarioDelete = new Vm_usuario();

            usuarioDelete = usuario.BuscaUsuario_id(id);

            return View(usuarioDelete);
            
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_usuario usuarioDelete = new Vm_usuario();
            usuarioDelete = usuario.BuscaUsuario_id(id);

            try
            {
                TempData["deleteUsuario"] = usuario.deletaUsuario(user.usuario_conta_id, id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["deleteUsuario"] = "Erro. Ocorreu um erro ao tentar apagar o usuário. Tente novamente. Se persistir, entre em contato com o suporte!";
                return RedirectToAction(nameof(Index));
            }
        }

        //Alterar senha do usuário (restrito ao adm)        
        [Authorize(Roles = "adm")]
        public ActionResult EditPassword(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_usuario us = new Vm_usuario();

            us = usuario.BuscaUsuario_id(id);

            return View(us);

        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(int id, IFormCollection dados)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Vm_usuario editpw = new Vm_usuario();
            editpw = usuario.BuscaUsuario_id(id);

            try
            {
                TempData["EditPasswordUsuario"] = usuario.EditPassword(user.usuario_conta_id, id, user.usuario_id, dados["usuario_user"], dados["usuario_senha"]);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(editpw);
            }
        }

        public IActionResult emailExiste(string usuario_email, int usuario_id)
        {
            Registro registro = new Registro();
            bool existe = registro.emailExiste(usuario_email, usuario_id);

            return Json(!existe);
        }

    }
}
