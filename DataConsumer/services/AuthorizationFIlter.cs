using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text;
using System.Threading.Tasks;

public class BasicAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {

        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
        if (authHeader.StartsWith("Basic "))
        {
            var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

            var parts = decodedUsernamePassword.Split(":");
            Console.WriteLine(parts[0]);

            if (parts[0] == "teste01" && parts[1] == "password1")
            {
                return;
            }
        }

        context.Result = new UnauthorizedResult();
    }
}
