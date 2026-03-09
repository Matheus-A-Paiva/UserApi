using UserApi.Application.DTOs;
using UserApi.Domain.Entities;
using UserApi.Domain.Interfaces;
using UserApi.Infrastructure.Security;

public class AuthService
{
    private readonly IUsuarioRepository _repositorioUsuario;
    private readonly ITokenService _tokenService;
    private readonly SenhaHasher _senhaHasher;

    public AuthService(IUsuarioRepository repositorioUsuario, ITokenService tokenService, SenhaHasher senhaHasher)
    {
        _repositorioUsuario = repositorioUsuario;
        _tokenService = tokenService;
        _senhaHasher = senhaHasher;
    }

    public async Task<string> Login(string email, string senha)
    {
        var usuario = await _repositorioUsuario.ObterPorEmailAsync(email);

        if (usuario == null)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        if (!usuario.Ativo)
            throw new UnauthorizedAccessException("Usuário inativo");

        var senhaValida = _senhaHasher.Verificar(senha, usuario.SenhaHash);

        if (!senhaValida)
        throw new UnauthorizedAccessException("Credenciais inválidas");

        return _tokenService.GerarToken(usuario);
    }

    public async Task<string> Registrar(RegistrarUsuarioDto usuarioDto)
    {
        var existente = await _repositorioUsuario.ObterPorEmailAsync(usuarioDto.Email);
        
        if (existente is not null)
            throw new InvalidOperationException("E-mail já cadastrado.");
            
        Usuario usuario = new Usuario
        {
            Nome = usuarioDto.Nome,
            Email = usuarioDto.Email,
            SenhaHash = _senhaHasher.GerarHash(usuarioDto.Senha)
        };
        await _repositorioUsuario.AdicionarAsync(usuario);

        return _tokenService.GerarToken(usuario);
    }
}