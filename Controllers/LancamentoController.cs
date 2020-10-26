using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    public class LancamentoController : Controller
    {
        // GET: LancamentoController
        public ActionResult Index()
        {
            List<Vm_lancamento> lancamentos = new List<Vm_lancamento>();
            lancamentos.Add(new Vm_lancamento
            {
                lancamento_id = 1,
                lancamento_data = new DateTime(2020,09,14),
                lancamento_debito_descricao = "01.1.1.01.001 - Caixa",
                lancamento_credito_descricao = "04.2.1.03.001 - Aluguel",
                lancamento_valor = Convert.ToDecimal("835.9"),
                lancamento_tipo = "Lançamento Contábil"
            });
            lancamentos.Add(new Vm_lancamento
            {
                lancamento_id = 2,
                lancamento_data = new DateTime(2020, 09, 14),
                lancamento_debito_descricao = "01.1.1.01.001 - Caixa",
                lancamento_credito_descricao = "04.2.1.03.004 - Internet",
                lancamento_valor = Convert.ToDecimal("160.32"),
                lancamento_tipo = "Lançamento Contábil"
            });
            lancamentos.Add(new Vm_lancamento
            {
                lancamento_id = 3,
                lancamento_data = new DateTime(2020, 09, 14),
                lancamento_debito_descricao = "01.1.1.01.001 - Caixa",
                lancamento_credito_descricao = "04.2.1.03.007 - Material de limpeza",
                lancamento_valor = Convert.ToDecimal("259.60"),
                lancamento_tipo = "Gestão de Compras"
            });
            lancamentos.Add(new Vm_lancamento
            {
                lancamento_id = 4,
                lancamento_data = new DateTime(2020, 09, 14),
                lancamento_debito_descricao = "01.1.1.01.001 - Caixa",
                lancamento_credito_descricao = "04.2.1.03.009 - Manutenção e reparos",
                lancamento_valor = Convert.ToDecimal("532.5"),
                lancamento_tipo = "Gestão de Compras"
            });
            lancamentos.Add(new Vm_lancamento
            {
                lancamento_id = 5,
                lancamento_data = new DateTime(2020, 09, 14),
                lancamento_debito_descricao = "01.1.1.01.001 - Caixa",
                lancamento_credito_descricao = "04.2.1.03.003 - Energia elétrica",
                lancamento_valor = Convert.ToDecimal("168.50"),
                lancamento_tipo = "Gestão de Compras"
            });

            ViewData["bread"] = "Lançamento Contábil";

            return View(lancamentos);
        }

        // GET: LancamentoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LancamentoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LancamentoController/Create
        [HttpPost]       
        public ActionResult Create(IFormCollection collection)
        {
            //lanc dados
            try
            {


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LancamentoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LancamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LancamentoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LancamentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
