using EventPlannerServer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;

namespace EventPlannerServer.Services
{
    //Сделать, чтоб токен обновлялся и мало длился
    public class AuthorizationService: IAuthorizationService
    {
        private EventPlannerDbContext dbContext;
        public AuthorizationService(EventPlannerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public (bool, string) Registration(string login, string password)
        {
            if (dbContext.Users.ToList<User>().Find((user) => user.Login == login) != null)
                return (false, "Данный пользователь существует");
            var user = new User() { Login = login, Password = HashPassword(password)};
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return (true, GetJWTToken(login));
        }   

        public (bool, string) Authorization(string login, string password)
        {
            if (dbContext.Users.ToList<User>().Find((user) => user.Login == login && VerifyPassword(password, user.Password)) != null)
                return (true, GetJWTToken(login));
            return (false, "Неверный логин или пароль");
        }

        private string GetJWTToken(string login)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, login) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string HashPassword(string password) =>
           BCrypt.Net.BCrypt.HashPassword(password);

        private bool VerifyPassword(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
