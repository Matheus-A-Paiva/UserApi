using System.ComponentModel.DataAnnotations;

namespace UserApi.Application.DTOs;

public class AtualizarUsuarioDto
{
    public string? Nome { get; set; }
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string? Email { get; set; }
    public bool? Ativo { get; set; }
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    public string? Senha { get; set; }
}

