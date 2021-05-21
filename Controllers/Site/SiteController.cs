using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Site;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers.Site
{
    public class SiteController : Controller
    {
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
                    Lead l = new Lead();
                    retorno = l.create(Convert.ToInt32(dados[1]), Convert.ToInt32(dados[0]), collection["lead_nome"], collection["lead_celular"], collection["lead_email"], collection["lead_tipo"], "Pendente", collection["lead_tipo"], collection["lead_contato_msg"]);
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


    }
}
