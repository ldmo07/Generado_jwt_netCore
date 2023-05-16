using GENERADOR_JWT_UNIMINUTO.Helper;
using System.Net;
using System.Text.Json;

namespace GENERADOR_JWT_UNIMINUTO.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtGenerador jwtUtils)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var email = jwtUtils.ValidateToken(token);
                if (!string.IsNullOrEmpty(email))
                {
                    context.Items["email"] = email;
                }

                await _next(context);

            }
            catch (Exception)
            {
                var statusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType= "application/json";
                context.Response.StatusCode = statusCode;


                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var json = JsonSerializer.Serialize(statusCode, options);

                await context.Response.WriteAsync(json);
               
            }
           
        }
    }
}
