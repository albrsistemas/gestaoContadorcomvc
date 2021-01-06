using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using gestaoContadorcomvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    [AllowAnonymous]
    public class ForgotPasswordController : Controller
    {
        private readonly IEmailSender _emailSender;
        public ForgotPasswordController(IEmailSender emailSender, IHostingEnvironment env)
        {
            _emailSender = emailSender;
        }

        public IActionResult redefinir_senha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult redefinir_senha(IFormCollection collection)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuarioPorEmail(collection["email"]);

            if (user == null)
            {
                TempData["error"] = "Usuário não localizado!";

                return RedirectToAction("redefinir_senha");
            }

            try
            {
                usuario.usuario_nome = user.usuario_nome;
                usuario.usuario_conta_id = user.usuario_conta_id;
                usuario.Role = user.Role;
                var token = TokenService.GenerateToken(usuario);

                string retorno = usuario.AtribuirTokenForgot(user.usuario_id, user.usuario_conta_id, user.usuario_email, token);

                if (retorno.Contains("Erro"))
                {
                    TempData["error"] = "Erro ao processar a solicitação!";

                    return RedirectToAction("redefinir_senha");
                }

                string msg = "<!DOCTYPE html><html><head><meta name=\"viewport\" content=\"width = device - width\"/><title>Redefinir Senha</title><style> html, body{ margin: 0; padding: 0; } .container { width: 100%; height: 100%; } .box { width: 100%; height: auto; background: #fff; padding-bottom: 5px; } .conteudo{ text-align: center; padding: 10px; } .box input { text-align: center; } .box input:hover { color: #495057; background-color: #fff; border-color: #80bdff; outline: 0; box-shadow: 0 0 0 0.2rem rgba(0,123,255,.25); } .faixa { width: 100%; height: 35px; border-bottom: 3px solid #ff4400; background-color: #060040; } .login { text-align: center; font-family: sans-serif; font-weight: bold; font-size: 32px; margin-top: 35px; margin-bottom: 40px; } </style> </head> <body> <div class=\"container\"><div class=\"box\"><div class=\"faixa\" style=\"padding-top: 17px; padding-left: 5px; font-family: sans - serif;\"><strong><span style=\"color: white\">Contador</span><span style=\"color: #ff4400;\">com</span><span style=\"color: white\">vc</span></strong></div> <div class=\"conteudo\"><p>Prezado(a) "+ user.usuario_nome + ", recebemos sua solicitação para redefinição de senha.</p> <p><a href=\"https://localhost:44339/ForgotPassword/forgot_password?token=" + token + "\">Clique aqui para redefiir sua senha</a></p></div></div></div></body></html>";

                envioEmailRefefinirSenha(user.usuario_email, "Redefinição de Senha", msg).GetAwaiter();

                TempData["error"] = null;
                TempData["falha"] = null;
                TempData["sucesso"] = "O e-mail para redefinição de senha foi enviado com sucesso!";              

                return RedirectToAction("redefinir_senha");
            }
            catch (Exception)
            {
                TempData["error"] = null;
                TempData["sucesso"] = null;
                TempData["falha"] = "Erro no envio do e-mail!";

                return RedirectToAction("redefinir_senha");
            }
        }

        public IActionResult forgot_password(string token)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();

            user = usuario.BuscaUsuarioPorToken(token);

            if(user == null)
            {
                TempData["token_invalido"] = "Solicitação com token inválido!";

                return RedirectToAction("token_invalido");
            }

            DateTime hoje = DateTime.Today;

            if(user.usuario_forgt_data < hoje.AddDays(-2))
            {
                TempData["token_invalido"] = "Sua solitiação ultrapassou dois dias. Solicite novamente a redefinição de senha!";

                return RedirectToAction("token_invalido");
            }

            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult forgot_password(IFormCollection collection)
        {
            Usuario usuario = new Usuario();

            string retorno = usuario.EditSenhaTokenForgot(collection["senha"], collection["token"]);

            TempData["forgot_retorno"] = retorno;

            return RedirectToAction("Login", "Conta");
        }

        public IActionResult token_invalido()
        {
            return View();
        }

        public async Task envioEmailRefefinirSenha(string email, string assunto, string mensagem)
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



    }
}
