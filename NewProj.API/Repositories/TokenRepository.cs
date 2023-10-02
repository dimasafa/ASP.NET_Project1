using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewProj.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            // Создание утверждений(Утверждения (claims) - это факты о пользователе, которые будут включены в JWT. Каждое утверждение представляет собой пару "ключ-значение" и может содержать информацию о пользователе, такую как идентификатор, имя, роли, и т. д.)
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // пробегаемся в цикле по ролям и добавляем необходимую роль в claims
            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // nehmen key from appsettings.json. .UTF8.GetBytes - преобразуем ключ в байты, т.к SymmetricSecurityKey ожидает результат в виде байт. Он создает симметричный ключ для дальнейшего ипользования в JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            // создаем элемент SigningCredentials, который создается из key и алгоритма хэщирования key. Далее используется для создания jWT.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Erstellen von Token
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            // создает и возвращает строковое представление JWT-токена.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
