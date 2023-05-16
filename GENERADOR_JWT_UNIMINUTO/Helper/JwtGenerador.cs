using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GENERADOR_JWT_UNIMINUTO.Helper
{
    public class JwtGenerador : IJwtGenerador
    {
        private readonly IConfiguration _configuration;
        public JwtGenerador(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string crearToken(string username, string password)
        {

            var issuer = _configuration.GetSection("Jwt").GetSection("Issuer").Value;
            var audience = _configuration.GetSection("Jwt").GetSection("Audience").Value;

            var key = Encoding.ASCII.GetBytes
            (_configuration.GetSection("Jwt").GetSection("Key").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        public string ValidateToken(string token)
        {
            if (token == null)
                return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt").GetSection("Key").Value!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.First(x => x.Type == "email").Value;

                // return user id from JWT token if validation successful
                return email;
            }
            catch
            {
                // return null if validation fails
                return string.Empty;
            }
        }

    }
}
