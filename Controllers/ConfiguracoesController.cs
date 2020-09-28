using gestaoContadorcomvc.Filtros;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.GE
{
    [FiltroAutorizacao]
    public class ConfiguracoesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["bread"] = "Configurações";

            return View();
        }

        // GET: Categoria_v2Controller/Create        
        public ActionResult Contabilidade()
        {            
            return View();
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contabilidade(IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Categoria_v2Controller/Create        
        public ActionResult AddContabilidade()
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");
            Conta conta = new Conta();
            conta = conta.buscarConta(user.usuario_conta_id);

            Vm_contabilidade contabilidade = new Vm_contabilidade();

            contabilidade.cc_cnpj = "01050932000101";
            contabilidade.cc_nome = "Ozaki Contabildiade LTDA";
            contabilidade.conta = conta;

            //Produção do termo
            string termo = "Prezado cliente, \n" +
                           "Ao aceitar o compartilhamento de informações contábeis o contador designado por você no campo" +
                            "'Meu Contador', terá acesso a todas as suas informações com execessão a manipulação de usuários, senhas e atribuição de contabilidade.\n" +
                            "A contabilidade poderá visualizar suas categorias, caixa e bancos, relatórios, participantes, produtos, compras, vendas, serviços, " +
                            "controle de cartão de crédito, contas a pagar e receber.|" +
                            "Além de visuazar, a contabilidade poderá em alguns casos fazer alterações pertinentes a contabilização das informações tais como " +
                            "alteração da categoria informada nos cadastros e lançamentos e " +
                            "atribuir configurações extras para melhor desempenho da escrituração contábel, tais como " +
                            "atribuição da estrutura DRE(demonstrativo de resultado do exercício) nas categorias e atribuição de conta contábil nas categorias.|" +
                            "Além disso a contabiliade poderá restringir algumas funções do sistema para melhor controle das configurações contábeis, tais como, " +
                            "cadastro de nova categoria, novo caixa/banco e novo cartão de crédito.|" +
                            "Fique tranquilo, ao desvincular o contador a contabilidade perderá acesso as todas as suas informações.|" +
                            "Não nos reponsabilizamos pelo uso da informação fora do domínio do sistema.|" + 
                            "Eu " + contabilidade.cc_nomeUsuario + " usuário administrador da conta código: " + contabilidade.cc_conta_id + " nome: " + contabilidade.conta.conta_nome +
                            " registrado com documento número: " + contabilidade.conta.conta_dcto +
                            ", ciente das informações autorizo o compartilhamento de informações da minha empresa com a contabilidade de CNPJ número: " + contabilidade.cc_cnpj + ".";
            
            contabilidade.cc_termo = termo;

            
            
            return View(contabilidade);
        }

        // POST: Categoria_v2Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContabilidade(IFormCollection collection)
        {
            var user = HttpContext.Session.GetObjectFromJson<Usuario>("user");

            if (!ModelState.IsValid)
            {
                return View();
            }

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