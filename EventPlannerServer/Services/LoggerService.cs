using EventPlannerServer.Models;
using System.Runtime.InteropServices.ComTypes;

namespace EventPlannerServer.Services
{
    public class LoggerService :ILoggerService
    {
        private EventPlannerDbContext dbContext;
        public LoggerService(EventPlannerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Log(string login, string actionName, string? description, int? eventId)
        {
            var user = dbContext.Users.ToList().First((user) => user.Login == login);
            if (user is null)
                return;

            byte? action = dbContext.ActionTypes.ToList().First((action) => action.Name == actionName).Id;
            if (action is null) 
                return;
            var log = new Log() { TypeId = (byte)action, UserId = user.Login, DateTime = DateTime.UtcNow, Description = description, EventId = eventId };
            await dbContext.Logs.AddAsync(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
