using Azure.Core;
using EventPlannerLibrary;
using EventPlannerLibrary.SharedDTOs;
using EventPlannerServer.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerServer.Services
{
    public class EventService: IEventService
    {
        private EventPlannerDbContext dbContext;
        public EventService(EventPlannerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public (ErrorMessage?, List<EventDTO>?) GetEventsOfMonth(int year, int month)
        {
            if (year < 1900 || year > 2100 || month < 1 || month > 12)
                return (new ErrorMessage() { Message = "Неккоректные данные", ErrorCode = 400 }, null);

            List<EventDTO> events = dbContext.Events
                .Where((evnt) => (evnt.DateTime.Year == year) && (evnt.DateTime.Month == month))
                .OrderBy((evnt) => evnt.DateTime)
                .Select((evnt) => new EventDTO() {
                                    Id = evnt.Id,
                                    Event = evnt.Description, 
                                    DateTime = evnt.DateTime, 
                                    Importance = evnt.ImportanceId, 
                                    User = evnt.User.Login 
                }).ToList();
            if (events is null)
                return (new ErrorMessage() { Message = "Произошла непредвиденная ошибка", ErrorCode = 500 }, null);
            return (null, events);
        }

        public (ErrorMessage?, EventDTO?) CreateEvent(EventDTO eventData, string login)
        {
            User user = dbContext.Users.FirstOrDefault(u => u.Login == login);
            if (user is null)
                return (new ErrorMessage() { Message = "Пользователь не найден", ErrorCode = 400 }, null);

            Event eventMapped  = new() { UserId = user.Id, DateTime = eventData.DateTime, Description = eventData.Event, ImportanceId = eventData.Importance};
            dbContext.Events.Add(eventMapped);
            dbContext.SaveChanges();
            return (null, new EventDTO() { 
                            Id = eventMapped.Id,
                            User = login, 
                            Importance = eventMapped.ImportanceId, 
                            Event = eventMapped.Description, 
                            DateTime = eventMapped.DateTime });

        }

        public (ErrorMessage?, EventDTO?) EditEvent(EventDTO eventToFind, EventDTO eventData, string login)
        {
            User user = dbContext.Users.FirstOrDefault(u => u.Login == login);
            if (user is null)
                return (new ErrorMessage() { Message = "Пользователь не найден", ErrorCode = 400 }, null);

            Event eventInDb = dbContext.Events.Include(e => e.User).FirstOrDefault(e => e.Description == eventToFind.Event && e.DateTime == eventToFind.DateTime && e.ImportanceId == eventToFind.Importance && e.UserId == eventToFind.Id);
            if (eventInDb is null)
                return (new ErrorMessage() { Message = "Событие не найдено", ErrorCode = 500 }, null);
            if (eventInDb.UserId != user.Id)
                return (new ErrorMessage() { Message = "Нет доступа для изменения", ErrorCode = 401 }, null);
            eventInDb.DateTime = eventData.DateTime;
            eventInDb.Description = eventData.Event;
            eventInDb.ImportanceId = eventData.Importance;
            dbContext.SaveChanges();
            return (null, new EventDTO() {
                            Id = eventInDb.Id,
                            User = eventInDb.User.Login,
                            Importance = eventInDb.ImportanceId,
                            Event = eventInDb.Description,
                            DateTime = eventInDb.DateTime });
        }

        public (ErrorMessage?, bool) DeleteEvent(EventDTO eventData, string login)
        {
            User user = dbContext.Users.FirstOrDefault(u => u.Login == login);
            if (user is null)
                return (new ErrorMessage() { Message = "Пользователь не найден", ErrorCode = 400 }, false);

            Event eventInDb = dbContext.Events.FirstOrDefault(e => e.Description == eventData.Event && e.DateTime == eventData.DateTime && e.ImportanceId == eventData.Importance && e.UserId == eventData.Id);
            if (eventInDb is null)
                return (new ErrorMessage() { Message = "Событие не найдено", ErrorCode = 500 }, false);
            if (eventInDb.UserId != user.Id)
                return (new ErrorMessage() { Message = "Нет доступа для удаления", ErrorCode = 401 }, false);
            dbContext.Events.Remove(eventInDb);
            dbContext.SaveChanges(true);
            return (null, true);
        }

    }
}
