using System.ComponentModel.DataAnnotations;

namespace CadastroUsuarios.Shared.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "Nome pode ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos.")]
        public string Cpf { get; set; } = string.Empty;

        // Sem DataAnnotations, conforme você pediu
        public string Telefone { get; set; } = string.Empty;
    }
}

