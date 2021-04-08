using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProdutosController : Controller
    {
        [Autoriza(permissao = "produtosList")]
        public ActionResult Index()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Produtos produto = new Produtos();
            Vm_produtos vm_prod = new Vm_produtos();

            vm_prod.user = user;
            vm_prod.produtos = produto.listaProdutos(user.usuario_id, user.usuario_conta_id);

            return View(vm_prod);
        }


        [Autoriza(permissao = "produtosCreate")]
        public ActionResult Create()
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Selects select = new Selects();
            ViewBag.origem = select.getOrigemMercadoria().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            ViewBag.tipoItem = select.getTipoItem().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "00" });

            Produtos produto = new Produtos();
            Vm_produtos vm_prod = new Vm_produtos();

            vm_prod.user = user;

            return View(vm_prod);
        }

        // POST: ProdutosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection d)
        {
            Vm_produtos vm_prod = new Vm_produtos();
            
            Selects select = new Selects();
            ViewBag.origem = select.getOrigemMercadoria().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "0" });
            ViewBag.tipoItem = select.getTipoItem().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == "00" });

            try
            {

                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
                
                vm_prod.produtos_nome = d["produtos_nome"];
                vm_prod.produtos_status = d["produtos_status"];
                vm_prod.produtos_unidade = d["produtos_unidade"];
                vm_prod.produtos_preco_venda = Convert.ToDecimal(d["produtos_preco_venda"].ToString().Replace(".", ""));
                vm_prod.produtos_obs = d["produtos_obs"];
                vm_prod.produtos_codigo = d["produtos_codigo"];

                if (user.conta.conta_tipo != "Entidade")
                {   
                    vm_prod.produtos_formato = d["produtos_formato"];
                    vm_prod.produtos_gtin_ean = d["produtos_gtin_ean"];
                    vm_prod.produtos_gtin_ean_trib = d["produtos_gtin_ean_trib"];
                    vm_prod.produtos_estoque_min = Convert.ToDecimal(d["produtos_estoque_min"].ToString().Replace(".", ""));
                    vm_prod.produtos_estoque_max = Convert.ToDecimal(d["produtos_estoque_max"].ToString().Replace(".", ","));
                    vm_prod.produtos_estoque_qtd_inicial = Convert.ToDecimal(d["produtos_estoque_qtd_inicial"].ToString().Replace(".", ""));
                    vm_prod.produtos_estoque_preco_compra = Convert.ToDecimal(d["produtos_estoque_preco_compra"].ToString().Replace(".", ""));
                    vm_prod.produtos_estoque_custo_compra = Convert.ToDecimal(d["produtos_estoque_custo_compra"].ToString().Replace(".", ""));
                    vm_prod.produtos_origem = Convert.ToInt32(d["produtos_origem"]);
                    vm_prod.produtos_ncm = d["produtos_ncm"];
                    vm_prod.produtos_cest = d["produtos_cest"];
                    vm_prod.produtos_tipo_item = d["produtos_tipo_item"];
                    vm_prod.produtos_perc_tributos = Convert.ToDecimal(d["produtos_perc_tributos"].ToString().Replace(".", ""));
                }
                else
                {   
                    vm_prod.produtos_formato = "Simples";
                    vm_prod.produtos_gtin_ean = "";
                    vm_prod.produtos_gtin_ean_trib = "";
                    vm_prod.produtos_estoque_min = 0;
                    vm_prod.produtos_estoque_max = 0;
                    vm_prod.produtos_estoque_qtd_inicial = 0;
                    vm_prod.produtos_estoque_preco_compra = 0;
                    vm_prod.produtos_estoque_custo_compra = 0;
                    vm_prod.produtos_origem = 0;
                    vm_prod.produtos_ncm = "";
                    vm_prod.produtos_cest = "";
                    vm_prod.produtos_tipo_item = "99";
                    vm_prod.produtos_perc_tributos = 0;
                }                               

                if (!ModelState.IsValid)
                {
                    TempData["msgProdutos"] = "Formulário com informação incorreta (Erro)!";

                    return View(vm_prod);
                }

                Produtos produto = new Produtos();

                TempData["msgProdutos"] = produto.cadastraProduto(user.usuario_conta_id, user.usuario_id, vm_prod.produtos_codigo, vm_prod.produtos_nome, vm_prod.produtos_formato, vm_prod.produtos_unidade, vm_prod.produtos_preco_venda, vm_prod.produtos_gtin_ean, vm_prod.produtos_gtin_ean_trib, vm_prod.produtos_estoque_min, vm_prod.produtos_estoque_max, vm_prod.produtos_estoque_qtd_inicial, vm_prod.produtos_estoque_preco_compra, vm_prod.produtos_estoque_custo_compra, vm_prod.produtos_obs, vm_prod.produtos_origem, vm_prod.produtos_ncm, vm_prod.produtos_cest, vm_prod.produtos_tipo_item, vm_prod.produtos_perc_tributos);

                if (TempData["msgProdutos"].ToString().Contains("Erro"))
                {
                    return View(vm_prod);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgProdutos"] = "Erro na gravação dos dados do produto!";

                return View(vm_prod);
            }
        }

        [Autoriza(permissao = "produtosEdit")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Produtos produto = new Produtos();
            Vm_produtos vm_prod = new Vm_produtos();

            vm_prod = produto.buscaProduto(user.usuario_id, user.usuario_conta_id, id);
            vm_prod.user = user;

            Selects select = new Selects();
            ViewBag.origem = select.getOrigemMercadoria().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_prod.produtos_origem.ToString() });
            ViewBag.tipoItem = select.getTipoItem().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_prod.produtos_tipo_item });
            ViewBag.status = select.getStatus().Select(c => new SelectListItem() { Text = c.text, Value = c.value, Disabled = c.disabled, Selected = c.value == vm_prod.produtos_status });

            return View(vm_prod);
        }

        // POST: ProdutosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection d)
        {
            Vm_produtos vm_prod = new Vm_produtos();

            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                vm_prod.produtos_nome = d["produtos_nome"];
                vm_prod.produtos_status = d["produtos_status"];
                vm_prod.produtos_unidade = d["produtos_unidade"];
                vm_prod.produtos_preco_venda = Convert.ToDecimal(d["produtos_preco_venda"].ToString().Replace(".", ""));
                vm_prod.produtos_obs = d["produtos_obs"];
                vm_prod.produtos_id = Convert.ToInt32(d["produtos_id"]);
                vm_prod.produtos_codigo = d["produtos_codigo"];

                if (user.conta.conta_tipo != "Entidade")
                {   
                    vm_prod.produtos_formato = d["produtos_formato"];                    
                    vm_prod.produtos_gtin_ean = d["produtos_gtin_ean"];
                    vm_prod.produtos_gtin_ean_trib = d["produtos_gtin_ean_trib"];
                    vm_prod.produtos_estoque_min = Convert.ToDecimal(d["produtos_estoque_min"].ToString().Replace(".", ""));
                    vm_prod.produtos_estoque_max = Convert.ToDecimal(d["produtos_estoque_max"].ToString().Replace(".", ""));
                    vm_prod.produtos_estoque_qtd_inicial = Convert.ToDecimal(d["produtos_estoque_qtd_inicial"].ToString().Replace(".", ""));
                    vm_prod.produtos_estoque_preco_compra = Convert.ToDecimal(d["produtos_estoque_preco_compra"].ToString().Replace(".", ""));
                    vm_prod.produtos_estoque_custo_compra = Convert.ToDecimal(d["produtos_estoque_custo_compra"].ToString().Replace(".", ""));
                    vm_prod.produtos_origem = Convert.ToInt32(d["produtos_origem"]);
                    vm_prod.produtos_ncm = d["produtos_ncm"];
                    vm_prod.produtos_cest = d["produtos_cest"];
                    vm_prod.produtos_tipo_item = d["produtos_tipo_item"];
                    vm_prod.produtos_perc_tributos = Convert.ToDecimal(d["produtos_perc_tributos"].ToString().Replace(".", ""));                    
                    
                }
                else
                {   
                    vm_prod.produtos_formato = "Simples";
                    vm_prod.produtos_gtin_ean = "";
                    vm_prod.produtos_gtin_ean_trib = "";
                    vm_prod.produtos_estoque_min = 0;
                    vm_prod.produtos_estoque_max = 0;
                    vm_prod.produtos_estoque_qtd_inicial = 0;
                    vm_prod.produtos_estoque_preco_compra = 0;
                    vm_prod.produtos_estoque_custo_compra = 0;
                    vm_prod.produtos_origem = 0;
                    vm_prod.produtos_ncm = "";
                    vm_prod.produtos_cest = "";
                    vm_prod.produtos_tipo_item = "99";
                    vm_prod.produtos_perc_tributos = 0;
                }


                if (!ModelState.IsValid)
                {
                    TempData["msgProdutos"] = "Formulário com informação incorreta (Erro)!";

                    return View(vm_prod);
                }

                Produtos produto = new Produtos();

                TempData["msgProdutos"] = produto.alteraProduto(user.usuario_id, user.usuario_conta_id, vm_prod.produtos_id, vm_prod.produtos_codigo, vm_prod.produtos_nome, vm_prod.produtos_formato, vm_prod.produtos_status, vm_prod.produtos_unidade, vm_prod.produtos_preco_venda, vm_prod.produtos_gtin_ean, vm_prod.produtos_gtin_ean_trib, vm_prod.produtos_estoque_min, vm_prod.produtos_estoque_max, vm_prod.produtos_estoque_qtd_inicial, vm_prod.produtos_estoque_preco_compra, vm_prod.produtos_estoque_custo_compra, vm_prod.produtos_obs, vm_prod.produtos_origem, vm_prod.produtos_ncm, vm_prod.produtos_cest, vm_prod.produtos_tipo_item, vm_prod.produtos_perc_tributos);

                if (TempData["msgProdutos"].ToString().Contains("Erro"))
                {
                    return View(vm_prod);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgProdutos"] = "Erro na gravação dos dados do produto!";

                return View(vm_prod);
            }
        }

        [Autoriza(permissao = "produtosDelete")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

            Produtos produto = new Produtos();
            Vm_produtos vm_prod = new Vm_produtos();

            vm_prod = produto.buscaProduto(user.usuario_id, user.usuario_conta_id, id);
            vm_prod.user = user;

            return View(vm_prod);
        }

        // POST: ProdutosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int produtos_id, IFormCollection d)
        {
            Produtos prod = new Produtos();
            try
            {
                Usuario usuario = new Usuario();
                Vm_usuario user = new Vm_usuario();
                user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);

                TempData["msgProdutos"] = prod.deletarProduto(user.usuario_id, user.usuario_conta_id, produtos_id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["msgProdutos"] = "Erro ao tentar apagar o produto!";

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult consultaProdutos(IFormCollection d)
        {
            Usuario usuario = new Usuario();
            Vm_usuario user = new Vm_usuario();
            user = usuario.BuscaUsuario(HttpContext.User.Identity.Name);
            
            Produtos p = new Produtos();
            List<Vm_produtos> vm_p = new List<Vm_produtos>();
            vm_p = p.listaProdutosPorTermo(user.usuario_id, user.usuario_conta_id, d["termo"]);

            return Json(JsonConvert.SerializeObject(vm_p));
        }
    }
}
