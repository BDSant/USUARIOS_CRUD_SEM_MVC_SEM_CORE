using CadastroUsuarios.Api.Services;
using CadastroUsuarios.Shared.Interfaces;
using CadastroUsuarios.Shared.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace CadastroUsuarios.Api.Controllers
{
    [RoutePrefix("api/usuarios")]
    //[EnableCors(origins: "https://localhost:44304", headers: "*", methods: "*")]
    public class UsuariosController : ApiController
    {
        private readonly IUsuarioService _service;

        // Produção
        public UsuariosController() : this(new UsuarioService())
        {
        }

        // Testes / injeção
        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        // GET api/usuarios
        [HttpGet]
        [Route("")]
        public IEnumerable<Usuario> Get()
        {
            return _service.Listar();
        }

        // GET api/usuarios/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var usuario = _service.Obter(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // POST api/usuarios
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Usuário inválido.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _service.Adicionar(usuario);
            return Created($"api/usuarios/{usuario.Id}", usuario);
        }

        // PUT api/usuarios/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Usuário inválido.");

            usuario.Id = id;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _service.Atualizar(usuario);
            return Ok(usuario);
        }

        // DELETE api/usuarios/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            _service.Excluir(id);
            return Ok();
        }
    }
}
