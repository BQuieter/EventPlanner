using EventPlannerClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Services
{
    public interface IEventsService
    {
        public Task<ServiceResponse<List<Event>>> GetEvents(DateTime? date);
        public Task<ServiceResponse<Event>> CreateEvent(Event eventData);
        public Task<ServiceResponse<Event>> EditEvent(Event eventData);
        public Task<ServiceResponse<bool>> DeleteEvent(Event eventData);
    }
}
