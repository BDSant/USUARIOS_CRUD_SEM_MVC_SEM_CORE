using CadastroUsuarios.Shared.Models;
using System.Collections.Generic;

namespace CadastroUsuarios.Shared.Interfaces
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> Listar();
        Usuario Obter(int id);
        void Adicionar(Usuario usuario);
        void Atualizar(Usuario usuario);
        void Excluir(int id);
    }
}
