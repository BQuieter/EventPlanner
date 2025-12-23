using BCrypt.Net;
using EventPlannerServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventPlannerServer.Services
{
    public class AuthorizationService: IAuthorizationService
    {
        private EventPlannerDbContext dbContext;
        private readonly Random random = new();
        public AuthorizationService(EventPlannerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public (ErrorMessage?, string) Registration(string login, string password)
        {
            if (dbContext.Users.ToList<User>().Find((user) => user.Login == login) != null)
                return (new() { Message = "Пользователь с данным логином уже существует", ErrorCode = 409 }, string.Empty);
            var user = new User() { Login = login, Password = HashString(password)};
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return (null, GetJWTToken(login));
        }   

        public (ErrorMessage?, string, string) Authorization(string login, string password)
        {
            var user = dbContext.Users.ToList().FirstOrDefault(user => user.Login == login && VerifyString(password, user.Password));
            if (user is not null)
                return (null, GetJWTToken(login), GetRefreshToken(user.Id));
            return (new() { Message = "Неправильный логин или пароль", ErrorCode = 401 }, string.Empty, string.Empty);
        }

        public (ErrorMessage?, string, string) RefreshJWTToken(string jwtToken, string refreshToken)
        {
            var claims = GetPrincipalFromExpiredToken(jwtToken).Claims;
            if (claims is null || claims.Count() == 0)
                return (new ErrorMessage() { Message = "Что-то не так с прошлым токеном", ErrorCode = 403 }, string.Empty, string.Empty);
            string login = claims.FirstOrDefault(c => c.ValueType == ClaimTypes.Name).Value;
            var user = dbContext.Users.Include(user => user.RefreshTokens).FirstOrDefault(user => user.Login == login);
            if (user is null)
                return (new() { Message = "Пользователь с таким логином не найден", ErrorCode = 401 }, string.Empty, string.Empty);
            RefreshToken tokenInDb = user.RefreshTokens.First();
            if (tokenInDb is null)
                return (new ErrorMessage() { Message = "Не существует токена обновления для данного пользователя", ErrorCode = 401 }, string.Empty, string.Empty);
            if (!VerifyString(refreshToken, tokenInDb.Token))
                return (new ErrorMessage() { Message = "Неверный токен обновления", ErrorCode = 401 }, string.Empty, string.Empty);
            if (DateTime.Compare(DateTime.UtcNow, tokenInDb.ExpiredAt) > 0)
                return (new ErrorMessage() { Message = "Время функционирования токена обновления закончилось", ErrorCode = 403 }, string.Empty, string.Empty);
            
            var newJwtToken = GetJWTToken(login, claims);
            var newRefreshToken = GetRefreshToken(user.Id);
            return (null, newJwtToken, newRefreshToken);
        }

        private string GetJWTToken(string login, IEnumerable<Claim>? expiredClaims = null)
        {
            var claims = expiredClaims is null ? new List<Claim> { new Claim(ClaimTypes.Name, login) } : expiredClaims;
            Console.WriteLine(AuthOptions.Expires);
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: AuthOptions.Expires,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            SymmetricSecurityKey key = AuthOptions.GetSymmetricSecurityKey();
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = key,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Неправильный токен");
            return principal;
        }

        private string GetRefreshToken(int userId)
        {
            RefreshToken tokenInDb = dbContext.RefreshTokens.Include(t => t.User).FirstOrDefault(t => t.UserId == userId);
            if (tokenInDb is null)
                tokenInDb = dbContext.RefreshTokens.Add(new RefreshToken() { UserId = userId }).Entity;
            var byteArray = new byte[32];
            random.NextBytes(byteArray);
            string refreshToken = Convert.ToHexString(byteArray);
            tokenInDb.Token = HashString(refreshToken);
            tokenInDb.ExpiredAt = DateTime.UtcNow.AddDays(15);
            dbContext.SaveChanges();
            return refreshToken;
        }

        private string HashString(string str) =>
           BCrypt.Net.BCrypt.HashPassword(str);

        private bool VerifyString(string str, string hashedString) =>
            BCrypt.Net.BCrypt.Verify(str, hashedString);
    }
}
