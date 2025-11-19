using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CadastroUsuarios.Api.Repositories;
using CadastroUsuarios.Shared.Interfaces;
using CadastroUsuarios.Shared.Models;

namespace CadastroUsuarios.Api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;

        // Construtor padrão (produção)
        public UsuarioService() : this(new UsuarioRepositorySql())
        {
        }

        // Construtor com injeção (para testes)
        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Usuario> Listar()
        {
            return _repo.Listar();
        }

        public Usuario Obter(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido.");

            return _repo.Obter(id);
        }

        public void Adicionar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            ValidarUsuario(usuario);

            _repo.Adicionar(usuario);
        }

        public void Atualizar(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (usuario.Id <= 0)
                throw new ArgumentException("Id inválido para atualização.");

            ValidarUsuario(usuario);

            _repo.Atualizar(usuario);
        }

        public void Excluir(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido para exclusão.");

            _repo.Excluir(id);
        }

        private void ValidarUsuario(Usuario usuario)
        {
            var context = new ValidationContext(usuario);
            Validator.ValidateObject(usuario, context, validateAllProperties: true);

            // Aqui NÃO validamos Telefone via DataAnnotations.
            // Se quiser alguma regra extra de negócio, coloca aqui.
        }
    }
}
