using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace gestaoContadorcomvc.Controllers.Autenticacao
{
    [AllowAnonymous]    
    public class ContaController : Controller
    {
        // GET: Conta
        public ActionResult Registro()
        {
            return View();
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                Registro registro = new Registro();
                registro.registro(collection["conta_dcto"],
                    "Cliente",
                    collection["usuario_nome"],
                    collection["usuario_dcto"],
                    collection["usuario_user"],
                    collection["usuario_senha"],
                    collection["conta_email"],
                    collection["conta_nome"]);
                
                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if(user != null)
            {
                TempData["user"] = user.usuario_nome;
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> Login(IFormCollection collection)
        {
            //Recupera usuario
            Usuario user = new Usuario();
            user = user.buscaUsuarioLogin(collection["usuario"], collection["senha"]);

            if(user == null || user.usuario_id == 0)
            {
                return View(TempData["errorLogin"] = "Usuário ou senha inválidos"); 
            }

            //Gerando o login     
            
            HttpContext.Session.SetObjectAsJson("user", user);
            HttpContext.Session.SetInt32("cliente_id", 0);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("ID", user.usuario_id);
            HttpContext.Session.SetInt32("Conta", user.usuario_conta_id);
            HttpContext.Session.SetString("Permissoes", user.permissoes);

            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);

            if (user != null && user.usuario_id > 0)
            {
                var userClaims = new List<Claim>()
                {
                    //definindo o cookie
                    new Claim(ClaimTypes.Name, user.usuario_id.ToString()),
                    new Claim(ClaimTypes.Email, user.usuario_email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Role, conta.conta_tipo),                   
                };

                var minhaIdentity = new ClaimsIdentity(userClaims, "Usuario");
                var claimPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });
                //cria o cookie
                _ = HttpContext.SignInAsync(claimPrincipal);
            }

            TempData["user"] = user.usuario_nome;

            if (collection["area"].Equals("contabilidade") && conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
            {
                return RedirectToAction("Index", "Home", new { area = "Contabilidade" });
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //            HttpContext.Session.Clear();
            HttpContext.Session.Remove("user");

            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Conta");
        }

        [HttpGet]
        public IActionResult AccessDeniedPath()
        {   
            return View();
        }

        public IActionResult userExiste(string usuario_user)
        {
            Registro registro = new Registro();
            bool existe = registro.userExiste(usuario_user);

            return Json(!existe);
        }

        public IActionResult emailExiste(string conta_email)
        {
            Registro registro = new Registro();
            bool existe = registro.emailExiste(conta_email);

            return Json(!existe);
        }

        public IActionResult dctoExiste(string conta_dcto)
        {
            Registro registro = new Registro();
            bool existe = registro.dctoExiste(conta_dcto);

            return Json(!existe);
        }    
    }
}