using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                registro.registro(collection["conta_dcto"].ToString().Replace(".","").Replace("-","").Replace("/",""),
                    collection["conta_tipo"],
                    collection["usuario_nome"],
                    collection["usuario_dcto"].ToString().Replace(".", "").Replace("-", "").Replace("/", ""),
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
            Selects select = new Selects();
            ViewBag.conta_tipo = select.getContaTipo().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Empresa" });

            return View();
        }

        [HttpPost]
        public IActionResult Login(IFormCollection collection)
        {
            //async Task<ActionResult<dynamic>>
            Selects select = new Selects();
            ViewBag.conta_tipo = select.getContaTipo().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "Empresa" });
            //Recupera usuario
            Usuario user = new Usuario();
            user = user.buscaUsuarioLogin(collection["usuario"], collection["senha"]);

            if(user == null || user.usuario_id == 0)
            {
                return View(TempData["errorLogin"] = "Usuário ou senha inválidos"); 
            }

            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);
                        
            if (user != null && user.usuario_id > 0)
            {
                if (conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
                {
                    if (collection["conta_tipo"] != "Contabilidade" && collection["conta_tipo"] != "Empresa")
                    {
                        return View(TempData["errorLogin"] = "Usuário não pertence ao tipo de conta especificado");
                    }
                }

                if (conta.conta_tipo.ToUpper() != "CONTABILIDADE" && conta.conta_tipo != collection["conta_tipo"])
                {
                    return View(TempData["errorLogin"] = "Usuário não pertence ao tipo de conta especificado");
                }

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

                var propriedadesDeAutenticacao = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IssuedUtc = DateTime.Now,
                    ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(4),
                    IsPersistent = true,
                };                

                //cria o cookie
                _ = HttpContext.SignInAsync(claimPrincipal, propriedadesDeAutenticacao);
            }

            TempData["user"] = user.usuario_nome;

            if (conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
            {
                if(collection["conta_tipo"] == "Contabilidade")
                {
                    return RedirectToAction("Index", "Home", new { area = "Contabilidade" });
                }
            }   
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {   
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Conta");
        }

        [HttpGet]
        public IActionResult AccessDeniedPath()
        {   
            return View();
        }

        public IActionResult userExiste(string usuario_user, int usuario_id)
        {
            Registro registro = new Registro();
            bool existe = registro.userExiste(usuario_user, usuario_id);

            return Json(!existe);
        }

        public IActionResult emailExiste(string conta_email, int usuario_id)
        {
            Registro registro = new Registro();
            bool existe = registro.emailExiste(conta_email, usuario_id);

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