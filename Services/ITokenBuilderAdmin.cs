namespace AuthenticationService.Services
{
    public interface ITokenBuilderAdmin
    {
        string BuildToken(string username);
    }
}
