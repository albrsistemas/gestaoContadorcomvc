using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers
{
    public class CategoriaController : Controller
    {
        // GET: Categoria
        public ActionResult Index()
        {
            List<Vm_categoria> categorias = new List<Vm_categoria>();
            categorias.Add(new Vm_categoria
            {
                categoria_id = 1,
                categoria_nome = "Caixa",
                categoria_classificacao = "01.1.1.01.001",
                categoria_apelido = "CX",
                categoria_titulo = 4,
                categoria_tipo = "Padrão"
            });
            categorias.Add(new Vm_categoria
            {
                categoria_id = 2,
                categoria_nome = "Banco do Brasil",
                categoria_classificacao = "01.1.1.02.001",
                categoria_apelido = "BB",
                categoria_titulo = 6,
                categoria_tipo = "Contabilidade"
            });
            categorias.Add(new Vm_categoria
            {
                categoria_id = 2,
                categoria_nome = "Caixa Econômica Federal",
                categoria_classificacao = "01.1.1.02.002",
                categoria_apelido = "CEF",
                categoria_titulo = 6,
                categoria_tipo = "Própria (cliente)"
            });


            return View(categorias);
        }

        // GET: Categoria/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Categoria/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: Categoria/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Categoria/Edit/5
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

        // GET: Categoria/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Categoria/Delete/5
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
