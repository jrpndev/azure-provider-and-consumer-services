using System.Threading.Tasks;

public interface IBasicAuthenticationService
{
    Task<string> GenerateBasicAuthTokenAsync(string username, string password);
}
