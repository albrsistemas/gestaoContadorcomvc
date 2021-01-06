using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using gestaoContadorcomvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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

        public IActionResult redefinir_senha(string email)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuarioPorEmail(email);

            if (user == null)
            {
                return View(TempData["error"] = "Conta não localizada!");
            }

            //try
            //{
            //    envioEmailRefefinirSenha(conta.conta_email, email.Assunto, email.Mensagem).GetAwaiter();
            //    return RedirectToAction("EmailEnviado");
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("EmailFalhou");
            //}



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
