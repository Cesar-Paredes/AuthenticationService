namespace AuthenticationService.Services
{
    public interface ITokenBuilderCSRAgent
    {
        string BuildToken(string username);
    }
}
