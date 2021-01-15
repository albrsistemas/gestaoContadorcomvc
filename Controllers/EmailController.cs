using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using gestaoContadorcomvc.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender, IHostingEnvironment env)
        {
            _emailSender = emailSender;
        }

        public IActionResult EnviaEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnviaEmail(EmailModel email)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            string msg = "<!DOCTYPE html><html><head><meta name=\"viewport\" content=\"width = device - width\"/><title>Redefinir Senha</title><style> html, body{ margin: 0; padding: 0; } .container { width: 100%; height: 100%; } .box { width: 100%; height: auto; background: #fff; padding-bottom: 5px; } .conteudo{ text-align: center; padding: 10px; } .box input { text-align: center; } .box input:hover { color: #495057; background-color: #fff; border-color: #80bdff; outline: 0; box-shadow: 0 0 0 0.2rem rgba(0,123,255,.25); } .faixa { width: 100%; height: 35px; border-bottom: 3px solid #ff4400; background-color: #060040; } .login { text-align: center; font-family: sans-serif; font-weight: bold; font-size: 32px; margin-top: 35px; margin-bottom: 40px; } </style> </head> <body> <div class=\"container\"><div class=\"box\"><div class=\"faixa\" style=\"padding-top: 17px; padding-left: 5px; font-family: sans - serif;\"><strong><span style=\"color: white\">Contador</span><span style=\"color: #ff4400;\">com</span><span style=\"color: white\">vc</span></strong></div> <div class=\"conteudo\"><p>Cliente: " + user.usuario_nome + "</p><p>Ref.: Conta: " + user.conta.conta_nome + "</p><p>Ref.: ID: " + user.conta.conta_id + "</p> <p><strong>Mensagem:</strong></br>"+ email.Mensagem + "</p></div></div></div></body></html>";

            if (ModelState.IsValid)
            {
                try
                {
                    TesteEnvioEmail("albrsistemas@gmail.com", email.Assunto, msg).GetAwaiter();
                    return RedirectToAction("EmailEnviado");
                }
                catch (Exception)
                {
                    return RedirectToAction("EmailFalhou");
                }
            }
            return View(email);
        }

        public async Task TesteEnvioEmail(string email, string assunto, string mensagem)
        {
            try
            {
                //email destino, assunto do email, mensagem a enviar
                await _emailSender.SendEmailAsync(email, assunto, mensagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EmailEnviado()
        {
            return View();
        }

        public ActionResult EmailFalhou()
        {
            return View();
        }
    }
}
