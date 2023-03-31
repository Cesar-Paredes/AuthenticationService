namespace AuthenticationService.Services
{
    public interface ITokenBuilderUser
    {
        string BuildToken(string username);
    }
}
