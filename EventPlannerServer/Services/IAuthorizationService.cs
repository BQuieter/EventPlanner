namespace EventPlannerServer.Services
{
    public interface IAuthorizationService
    {
        public (ErrorMessage?, string) Registration(string login, string password);
        public (ErrorMessage?, string, string) Authorization(string login, string password);
        public (ErrorMessage?, string, string) RefreshJWTToken(string jwtToken, string refreshToken);
    }
}
