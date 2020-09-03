using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gestaoContadorcomvc.Models.Autenticacao;
using gestaoContadorcomvc.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<dynamic>> Post([FromBody]Login model)
        {
            //Recupera o usuário
            Usuario user = new Usuario();
            user = user.buscaUsuarioLogin(model.usuario, model.senha);

            //Verifica se o usuário existe
            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos" });
            }

            //Gera o token
            var token = TokenService.GenerateToken(user);

            //Oculta a senha
            user.usuario_senha = "";
            user.usuario_user = "";

            //Retorna os dados
            return new
            {
                user = user,
                token = token
            };
        }
    }
}