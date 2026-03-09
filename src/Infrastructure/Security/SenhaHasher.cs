namespace UserApi.Infrastructure.Security;

public class SenhaHasher
{
    public string GerarHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verificar(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}