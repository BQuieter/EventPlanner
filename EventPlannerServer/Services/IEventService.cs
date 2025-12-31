using EventPlannerLibrary.SharedDTOs;

namespace EventPlannerServer.Services
{
    public interface IEventService
    {
        public (ErrorMessage?, List<EventDTO>?) GetEventsOfDay(int year, int month, int day);
        public (ErrorMessage?, EventDTO?) CreateEvent(EventDTO eventData, string login);
        public (ErrorMessage?, EventDTO?) EditEvent(EventDTO eventData, string login);
        public (ErrorMessage?, bool) DeleteEvent(EventDTO eventData, string login);
    }
}
