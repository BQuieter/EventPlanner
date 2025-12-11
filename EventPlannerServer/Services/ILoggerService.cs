namespace EventPlannerServer.Services
{
    public interface ILoggerService
    {
        public Task Log(string login, string action, string? description, int? eventId);
    }
}
