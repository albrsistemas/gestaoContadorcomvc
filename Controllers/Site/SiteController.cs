using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Site;
using gestaoContadorcomvc.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers.Site
{
    public class SiteController : Controller
    {
        private readonly IEmailSender _emailSender;
        public SiteController(IEmailSender emailSender, IHostingEnvironment env)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PrestadorServico()
        {
            return View();
        }

        public IActionResult AreaSaude()
        {
            return View();
        }

        public IActionResult MarketingPublicidade()
        {
            return View();
        }

        public IActionResult DesenvSistemas()
        {
            return View();
        }

        public IActionResult Academias()
        {
            return View();
        }

        public IActionResult Coaching()
        {
            return View();
        }

        public IActionResult EngenheirosArquitetos()
        {
            return View();
        }

        public IActionResult SeguradorasCorretoras()
        {
            return View();
        }

        public IActionResult Escolas()
        {
            return View();
        }

        public IActionResult Representantes()
        {
            return View();
        }

        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult Planos()
        {
            return View();
        }

        public IActionResult Planos_ozaki()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(IFormCollection collection)
        {
            string retorno = "";
            try
            {
                Codificacao c = new Codificacao();
                string decode = c.DecodeFrom64(collection["atendente_id"]);
                string[] dados = decode.Split(",");

                if(dados[0] == "0" || dados[1] == "0")
                {
                    retorno = "Desculpe, infelizmente tivemos um problema com o envio do contato. Favor tentar novamente.";
                }
                else
                {                   

                    try
                    {
                        //Gravar no banco de dados  
                        Lead l = new Lead();
                        retorno = l.create(Convert.ToInt32(dados[1]), Convert.ToInt32(dados[0]), collection["lead_nome"], collection["lead_celular"], collection["lead_email"], collection["lead_tipo"], "Pendente", collection["lead_tipo"], collection["lead_contato_msg"], "Contadorcomvocê");

                        DateTime hj = DateTime.Now;
                        //Enviar e-mail
                        string msg = "<!DOCTYPE html> <html>  <head> <meta name=\"viewport\" content=\"width=device - width\" /> <title>Lead´s</title> <style> html, body { margin: 0; padding: 0; }  .titulo { width: 100%; height: 50px; line-height: 50px; background-color: #C00000; padding-left: 15px; font-family: sans-serif; font-size: 25px; font-weight: bold; color: white; }  .container { width: 100%; height: auto; padding: 15px; }  ul { list-style-type: none; }  li { margin-bottom: 10px; font-family: sans-serif; } </style> </head>  <body> <div class=\"titulo\"> <span>LEAD - Contadorcomvocê</span> </div> <div class=\"container\"> <ul> <li><strong>Nome: </strong>" + collection["lead_nome"] + "</li> <li><strong>Celular: </strong>" + collection["lead_celular"] + "</li> <li><strong>E-mail: </strong>" + collection["lead_email"] + "</li> <li><strong>Plano: </strong>" + collection["lead_plano_selecionado"] + "</li><li><strong>Tipo: </strong>" + collection["lead_tipo"] + "</li><li><strong>Mensagem: </strong>" + collection["lead_contato_msg"] + "</li></ul> <span class=\"timest\">" + hj.ToLongDateString() + "</span> </div>  </body>  </html>";
                        string assunto = "Lead Contadorcomvocê: " + hj.ToString();

                        enviarEmail("fernando_servicos@ozaki.com.br", assunto, msg).GetAwaiter();
                        enviarEmail("manoel_servicos@ozaki.com.br", assunto, msg).GetAwaiter();
                        enviarEmail("ariel_fiscal@ozaki.com.br", assunto, msg).GetAwaiter();
                        retorno = "Contato enviado com sucesso. Em breve retornaremos seu contato.";
                    }
                    catch
                    {
                        retorno = "Erro. Falha no envio da mensagem!";
                    }                   

                }                

                return Json(JsonConvert.SerializeObject(retorno));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Desculpe, infelizmente tivemos um problema com o envio do contato. Favor tentar novamente.";
                }

                return Json(JsonConvert.SerializeObject(retorno));
            }
        }

        public async Task enviarEmail(string email, string assunto, string mensagem)
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
