using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestaoContadorcomvc.Controllers.api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParticipanteController : ControllerBase
    {
        // GET: api/Participante
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Participante/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            string valor = "participante número: " + id;
            return valor;
        }

        // POST: api/Participante
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Participante/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
