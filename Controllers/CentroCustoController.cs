using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.SoftwareHouse;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class CentroCustoController : Controller
    {
        [Autoriza(permissao = "centro_custoList")]
        public IActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Centro_custo c = new Centro_custo();

            c.centros_custo = c.listCentroCusto(user.conta.conta_id, user.usuario_id);
            c.user = user;

            return View(c);
        }

        [Autoriza(permissao = "centro_custoCreate")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Centro_custo c = new Centro_custo();

                retorno = c.cadastraCentroCusto(user.conta.conta_id, user.usuario_id, d["centro_custo_nome"]);

                TempData["msgCentroCusto"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao cadastrar o centro de custo. Tente novamente, se persistir, entre em contato com o suporte!";
                }

                TempData["msgCentroCusto"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "centro_custoEdit")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Centro_custo c = new Centro_custo();
            c = c.buscaCentroCusto(user.usuario_conta_id, user.usuario_id, id);
            c.user = user;

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int centro_custo_id, IFormCollection d)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Centro_custo c = new Centro_custo();

                retorno = c.alteraCentro_custo(user.usuario_conta_id, user.usuario_id, centro_custo_id, d["centro_custo_nome"]);

                TempData["msgCentroCusto"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao alterar o centro de custo. Tente novamente, se persistir, entre em contato com o suporte!";
                }
                TempData["msgCentroCusto"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "centro_custoDelete")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Centro_custo c = new Centro_custo();
            c = c.buscaCentroCusto(user.usuario_conta_id, user.usuario_id, id);
            c.user = user;

            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int centro_custo_id, IFormCollection collection)
        {
            string retorno = "";

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                Centro_custo c = new Centro_custo();

                retorno = c.deleteCentroCusto(user.usuario_conta_id, user.usuario_id, centro_custo_id);

                TempData["msgCentroCusto"] = retorno;

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                if (retorno == "")
                {
                    retorno = "Erro ao apagar o centro de custo. Tente novamente, se persistir, entre em contato com o suporte!";
                }
                TempData["msgCentroCusto"] = retorno;

                return RedirectToAction(nameof(Index));
            }
        }


    }
}
