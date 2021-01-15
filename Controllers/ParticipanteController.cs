using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace gestaoContadorcomvc.Controllers
{
    [Authorize]
    public class ParticipanteController : Controller
    {
        [Autoriza(permissao = "participanteList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Participante participante = new Participante();
            Vm_participante vm_part = new Vm_participante();

            vm_part.user = user;
            vm_part.participantes = participante.listaParticipantes(user.usuario_id, user.usuario_conta_id);

            return View(vm_part);
        }


        [Autoriza(permissao = "participanteCreate")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));



            Selects select = new Selects();
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1" });
            ViewBag.regimeTributario = select.getRegimes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            ViewBag.icmsContribuinte = select.getTipoContribuinte().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "35" });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "1058" });
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });

            return View();
        }

        // POST: ParticipanteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d)
        {
            Participante participante = new Participante();            

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                if (!ModelState.IsValid)
                {
                    TempData["msgParticipante"] = "Informação incorreta no preechimento do formulário!";

                    return RedirectToAction(nameof(Index));
                }
                                
                TempData["msgParticipante"] = participante.cadastrarParticipante(user.usuario_id, user.usuario_conta_id, Convert.ToDateTime(d["participante_clienteDesde"]), Convert.ToInt32(d["participante_contribuinte"]),
                    Convert.ToInt32(d["participante_uf"]), Convert.ToInt32(d["participante_categoria"]),user.usuario_conta_id, Convert.ToInt32(d["participante_pais"]),
                    d["participante_cep"],d["participante_nome"],d["participante_logradouro"],d["participante_rg"],d["participante_orgaoEmissor"],d["participante_numero"],
                    d["participante_codigo"],d["participante_tipoPessoa"],d["participante_inscricaoEstadual"],d["participante_cnpj_cpf"],d["participante_complemento"],
                    d["participante_obs"],d["participante_bairro"],d["participante_cidade"],d["participante_fantasia"],d["participante_insc_municipal"],Convert.ToInt32(d["participante_regime_tributario"]),d["participante_suframa"]);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgParticipante"] = "Erro ao gravar os dados do participante!";

                return View();
            }
        }

        [Autoriza(permissao = "participanteEdit")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Participante participante = new Participante();
            Vm_participante vm_part = new Vm_participante();
            vm_part = participante.buscarParticipantes(user.usuario_id, user.usuario_conta_id, id);

            Selects select = new Selects();
            ViewBag.tipoPessoa = select.getTipoPessoa().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_part.participante_tipoPessoa });
            ViewBag.regimeTributario = select.getRegimes().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_part.participante_regime_tributario.ToString() });
            ViewBag.icmsContribuinte = select.getTipoContribuinte().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_part.participante_contribuinte.ToString() });
            ViewBag.ufIbge = select.getUF_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_part.participante_uf.ToString() });
            ViewBag.paisesIbge = select.getPaises_ibge().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_part.participante_pais.ToString() });
            ViewBag.categoria = select.getCategoriasCliente(user.usuario_conta_id, true).Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_part.participante_categoria.ToString() });
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_part.participante_status });

            return View(vm_part);
        }

        // POST: ParticipanteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int participante_id, IFormCollection d)
        {
            Participante participante = new Participante();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                if (!ModelState.IsValid)
                {
                    TempData["msgParticipante"] = "Informação incorreta no preechimento do formulário!";

                    return RedirectToAction(nameof(Index));
                }

                TempData["msgParticipante"] = participante.alterarParticipante(user.usuario_id, user.usuario_conta_id, participante_id, Convert.ToDateTime(d["participante_clienteDesde"]), Convert.ToInt32(d["participante_contribuinte"]),
                    Convert.ToInt32(d["participante_uf"]), Convert.ToInt32(d["participante_categoria"]), Convert.ToInt32(d["participante_pais"]),
                    d["participante_cep"], d["participante_nome"], d["participante_logradouro"], d["participante_rg"], d["participante_orgaoEmissor"], d["participante_numero"],
                    d["participante_codigo"], d["participante_tipoPessoa"], d["participante_inscricaoEstadual"], d["participante_cnpj_cpf"], d["participante_complemento"],
                    d["participante_obs"], d["participante_bairro"], d["participante_cidade"], d["participante_fantasia"], d["participante_status"], d["participante_insc_municipal"], Convert.ToInt32(d["participante_regime_tributario"]), d["participante_suframa"]);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgParticipante"] = "Erro ao gravar os dados do participante!";

                return RedirectToAction(nameof(Index));
            }
        }

        [Autoriza(permissao = "participanteDelete")]
        public ActionResult Delete(int id)
        {
            Participante participante = new Participante();
            
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Vm_participante vm_part = new Vm_participante();
            vm_part = participante.buscarParticipantes(user.usuario_id, user.usuario_conta_id, id);

            return View(vm_part);
        }

        // POST: ParticipanteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection d)
        {
            Participante participante = new Participante();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

                TempData["msgParticipante"] = participante.deletarParticipante(user.usuario_id, user.usuario_conta_id, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgParticipante"] = "Erro ao tentar apagar o participante!";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult consultaParticipante(IFormCollection d)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(Convert.ToInt32(HttpContext.User.Identity.Name));

            Participante p = new Participante();
            List<Vm_participante> vm_p = new List<Vm_participante>();
            vm_p = p.listaParticipantesPorTermo(user.usuario_id, user.usuario_conta_id, d["termo"]);

            return Json(JsonConvert.SerializeObject(vm_p));
        }
    }
}
