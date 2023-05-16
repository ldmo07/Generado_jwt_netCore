namespace GENERADOR_JWT_UNIMINUTO.Helper
{
    public interface IJwtGenerador
    {
        string crearToken(string username,string password);
        string ValidateToken(string token);
    }
}
