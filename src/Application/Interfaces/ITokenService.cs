using UserApi.Domain.Entities;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}