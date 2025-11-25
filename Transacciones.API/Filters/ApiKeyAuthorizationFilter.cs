using Microsoft.AspNetCore.Mvc.Filters;
using Transacciones.Core.Exceptions;

namespace Transacciones.API.Filters
{
    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        private const string HeaderName = "Authorization";

        public ApiKeyAuthorizationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var http = context.HttpContext;
            var path = http.Request.Path.Value ?? string.Empty;

            // Skip Swagger
            if (path.IndexOf("/swagger", StringComparison.OrdinalIgnoreCase) >= 0)
                return;

            if (!http.Request.Headers.TryGetValue(HeaderName, out var headerValue) ||
                !headerValue.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedException("Missing or malformed Authorization header");
            }

            var incomingKey = headerValue.ToString().Substring("Bearer ".Length).Trim();
            var expectedKey = _configuration["Security:ApiKey"];

            if (string.IsNullOrWhiteSpace(expectedKey) || !string.Equals(incomingKey, expectedKey, StringComparison.Ordinal))
            {
                throw new UnauthorizedException("Invalid Api Key");
            }
        }
    }
}
