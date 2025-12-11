namespace EventPlannerServer.Services
{
    public interface IAuthorizationService
    {
        public (bool, string) Registration(string login, string password);
        public (bool, string) Authorization(string login, string password);
    }
}
