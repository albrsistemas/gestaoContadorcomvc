using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace gestaoContadorcomvc.Controllers.Autenticacao
{
    [AllowAnonymous]
    public class LoginClienteController : Controller
    {
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

            if (user == null || user.usuario_id == 0)
            {
                return View(TempData["errorLogin"] = "Usuário ou senha inválidos");
            }

            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);

            if (conta.conta_tipo != "Contabilidade")
            {
                return View(TempData["errorLogin"] = "Área exclusiva para contador!");
            }

            Conta conta_cliente = new Conta();
            conta_cliente = conta_cliente.buscarContaPorDcto(collection["conta_dcto"]);

            if (conta.conta_id != conta_cliente.contador_id)
            {
                return View(TempData["errorLogin"] = "Você não é contador do cliente requerido!");
            }


            if (user != null && user.usuario_id > 0)
            {
                //if (conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
                //{
                //    if (collection["conta_tipo"] != "Contabilidade" && collection["conta_tipo"] != "Empresa")
                //    {
                //        return View(TempData["errorLogin"] = "Usuário não pertence ao tipo de conta especificado");
                //    }
                //}

                //if (conta.conta_tipo.ToUpper() != "CONTABILIDADE" && conta.conta_tipo != collection["conta_tipo"])
                //{
                //    return View(TempData["errorLogin"] = "Usuário não pertence ao tipo de conta especificado");
                //}

                var userClaims = new List<Claim>()
                {
                    //definindo o cookie
                    new Claim(ClaimTypes.Name, user.usuario_id.ToString() + ";" + conta_cliente.conta_id),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Role, conta_cliente.conta_tipo),
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

            //if (conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
            //{
            //    if (collection["conta_tipo"] == "Contabilidade")
            //    {
            //        return RedirectToAction("Index", "Home", new { area = "Contabilidade" });
            //    }
            //}

            return RedirectToAction("Index", "Home");
        }
    }
}
