namespace AuthApi.Core.Utils;

public static class Senha
{
    public static string GerarHash(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public static bool VerificarHash(string senha, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, hash);
    }
}
