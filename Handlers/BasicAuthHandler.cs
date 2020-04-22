using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cw5.DTOs;
using Cw5.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cw5.Handlers
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,IStudentDbService service) : base(options, logger, encoder, clock)
        {
            
        }
        
        private SqlServerStudentDbService _service;

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing auth header");

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialsBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(":");
            
            if(credentials.Length!=2)
                return AuthenticateResult.Fail("Incorrect auth header value");
            
            _service = new SqlServerStudentDbService();
            
            var login = new LoginRequestDto
            {
                Login = credentials[0],
                Haslo = credentials[1]
            };

            if(!_service.CheckUserPassword(login))
                return AuthenticateResult.Fail("User or password incorrect");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, credentials[0]),
                new Claim(ClaimTypes.Role, "employee"),
                new Claim(ClaimTypes.Role, "student"),
            };
            
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal,Scheme.Name);
            
            return AuthenticateResult.Success(ticket);
        }
    }
}