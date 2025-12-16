using EventPlannerLibrary.SharedDTOs;

namespace EventPlannerServer.Services
{
    public interface IEventService
    {
        public (ErrorMessage?, List<EventDTO>?) GetEventsOfMonth(int year, int month);
        public (ErrorMessage?, EventDTO?) CreateEvent(EventDTO eventData, string login);
        public (ErrorMessage?, EventDTO?) EditEvent(EventDTO eventToFind, EventDTO eventData, string login);
        public (ErrorMessage?, bool) DeleteEvent(EventDTO eventData, string login);
    }
}
