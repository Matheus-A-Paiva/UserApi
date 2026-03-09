using UserApi.Application.DTOs;
using UserApi.Domain.Entities;
using UserApi.Domain.Interfaces;
using UserApi.Infrastructure.Security;

namespace UserApi.Application.Services;

public class UsuarioService
{
    private readonly IUsuarioRepository _repositorioUsuario;
    private readonly SenhaHasher _senhaHasher;

    public UsuarioService(IUsuarioRepository repositorioUsuario, SenhaHasher senhaHasher)
    {
        _repositorioUsuario = repositorioUsuario;
        _senhaHasher = senhaHasher;
    }

    public async Task<IEnumerable<Usuario>> ObterTodosAsync()
    {
        return await _repositorioUsuario.ObterTodosAsync();
    }

    public async Task<Usuario> ObterPorIdAsync(Guid id)
    {
        return await _repositorioUsuario.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");
    }

    public async Task<Usuario> AdicionarAsync(RegistrarUsuarioDto dto)
    {
        var existente = await _repositorioUsuario.ObterPorEmailAsync(dto.Email);

        if (existente is not null)
            throw new InvalidOperationException("E-mail já cadastrado.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = _senhaHasher.GerarHash(dto.Senha)
        };

        await _repositorioUsuario.AdicionarAsync(usuario);

        return usuario;
    }

    public async Task<Usuario> AtualizarAsync(Guid id, AtualizarUsuarioDto dto)
    {
        var usuario = await _repositorioUsuario.ObterPorIdAsync(id);

        if (usuario is null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            !dto.Email.Equals(usuario.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existente = await _repositorioUsuario.ObterPorEmailAsync(dto.Email);

            if (existente is not null)
                throw new InvalidOperationException("E-mail já cadastrado.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            usuario.Nome = dto.Nome;

        if (!string.IsNullOrWhiteSpace(dto.Email))
            usuario.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Senha))
            usuario.SenhaHash = _senhaHasher.GerarHash(dto.Senha);

        if (dto.Ativo.HasValue)
            usuario.Ativo = dto.Ativo.Value;

        usuario.DataAtualizacao = DateTime.UtcNow;

        await _repositorioUsuario.AtualizarAsync(usuario);

        return usuario;
    }

    public async Task DesativarAsync(Guid id)
    {
        var usuario = await _repositorioUsuario.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        if (!usuario.Ativo)
            return;

        usuario.Ativo = false;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await _repositorioUsuario.AtualizarAsync(usuario);
    }
}