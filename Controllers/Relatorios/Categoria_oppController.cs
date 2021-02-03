using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.Relatorios;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.Relatorios
{
    [Authorize]
    public class Categoria_oppController : Controller
    {
        [Autoriza(permissao = "rCategoriasList")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Categoria_opp copp = new Categoria_opp();
            copp.lista = null;
            copp.user = user;
            copp.visao = "visao";
            DateTime hoje = DateTime.Today;
            copp.ano = hoje.ToString("yyyy");

            return View(copp);
        }

        // POST: Categoria_oppController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string ano, string visao, bool ignorarZeradas)
        {
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Categoria_opp copp = new Categoria_opp();
                copp = copp.gerarRelatorio(user.usuario_conta_id, ano, visao, ignorarZeradas);
                copp.user = user;
                copp.visao = visao;
                copp.ano = ano;

                return View(copp);
            }
            catch
            {
                return View();
            }
        }
    }
}
