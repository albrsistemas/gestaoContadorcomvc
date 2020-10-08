using System.Collections.Generic;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            if(user == null)
            {
                return View(TempData["errorLogin"] = "Usuário ou senha inválidos"); 
            }

            //Gerando o login            
            HttpContext.Session.SetObjectAsJson("user", user);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("ID", user.usuario_id);
            HttpContext.Session.SetInt32("Conta", user.usuario_conta_id);
            HttpContext.Session.SetString("Permissoes", user.permissoes);

            TempData["user"] = user.usuario_nome;

            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);


            if (collection["area"].Equals("contabilidade") && conta.conta_tipo.ToUpper().Equals("CONTABILIDADE"))
            {
                return RedirectToAction("Index", "Home", new { area = "Contabilidade" });
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
//            HttpContext.Session.Clear();
            HttpContext.Session.Remove("user");

            return RedirectToAction("Login", "Conta");
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