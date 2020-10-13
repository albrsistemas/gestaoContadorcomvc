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
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
            user.usuarios = usuario.listaUsuarios(user.usuario_conta_id, user.usuario_id);

            return View(user);
        }

        [Autoriza(permissao = "usuarioCreate")]
        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            ViewData["bread"] = "Usuário / Novo";

            Usuario user = new Usuario();
            Vm_usuario usuario = new Vm_usuario();
            usuario = user.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));
            usuario.usuario_id = 0;
            usuario.usuario_nome = "";
            usuario.usuario_dcto = "";
            usuario.usuario_email = "";
            usuario.usuario_user = "";

            return View(usuario);
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection dados, [Bind] Vm_usuario vm_user)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            string retorno = "Erro ao cadastrar usuário, tente novamente. Se persistir entre em contato com o suporte !";

            try
            {
                retorno = usuario.novoUsuario(dados["usuario_nome"], dados["usuario_dcto"], dados["usuario_user"], dados["usuario_senha"], user.usuario_conta_id, dados["usuario_email"], dados["permissoes"], user.usuario_id, vm_user._permissoes);

                TempData["novoUsuario"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["novoUsuario"] = retorno;

                return View();
            }
        }

        [Autoriza(permissao = "usuarioEdit")]
        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["bread"] = "Usuário / Alteração";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_usuario usuarioEdit = new Vm_usuario();

            usuarioEdit = usuario.BuscaUsuario(id);

            return View(usuarioEdit);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection dados, [Bind] Vm_usuario vm_user)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_usuario usuarioEdit = new Vm_usuario();
            usuarioEdit = usuario.BuscaUsuario(id);

            try
            {
                TempData["editUsuario"] = usuario.alteraUsuario(dados["usuario_nome"], dados["usuario_dcto"], user.usuario_conta_id, dados["usuario_email"], dados["permissoes"], id, user.usuario_id, vm_user._permissoes);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuarioEdit);
            }
        }

        [Autoriza(permissao = "usuarioDelete")]
        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            ViewData["bread"] = "Usuário / Apagar";

            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_usuario usuarioDelete = new Vm_usuario();

            usuarioDelete = usuario.BuscaUsuario(id);

            return View(usuarioDelete);
            
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_usuario usuarioDelete = new Vm_usuario();
            usuarioDelete = usuario.BuscaUsuario(id);

            try
            {
                TempData["deleteUsuario"] = usuario.deletaUsuario(user.usuario_conta_id, id, user.usuario_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(usuarioDelete);
            }
        }

        //Alterar senha do usuário (restrito ao adm)        
        [Authorize(Roles = "adm")]
        public ActionResult EditPassword(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_usuario us = new Vm_usuario();

            us = usuario.BuscaUsuario(id);

            return View(us);

        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(int id, IFormCollection dados)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_usuario editpw = new Vm_usuario();
            editpw = usuario.BuscaUsuario(id);

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

    }
}
