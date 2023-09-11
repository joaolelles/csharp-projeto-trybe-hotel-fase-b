using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Services
{
    public class TokenGenerator
    {
        private readonly TokenOptions _tokenOptions;
        public TokenGenerator()
        {
            _tokenOptions = new TokenOptions
            {
                Secret = "4d82a63bbdc67c1e4784ed6587f3730c",
                ExpiresDay = 1
            };

        }
        public string Generate(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = AddClaims(user);

            if (claims != null)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenOptions.Secret)), SecurityAlgorithms.HmacSha256Signature),
                    Expires = DateTime.Now.AddDays(_tokenOptions.ExpiresDay)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);

            }
            else
            {
                throw new ApplicationException("Nenhuma reivindicação válida foi fornecida.");
            }
        }


        private ClaimsIdentity AddClaims(UserDto user)
        {
            var claims = new ClaimsIdentity();
            if (user.Email != null)
            {
                claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            }
            else
            {
                throw new ApplicationException("O e-mail do usuário é nulo.");
            }
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            if (user.UserType == "admin")
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            }

            return claims;
        }
    }
}