using EventPlannerClient.Models;
using EventPlannerLibrary.SharedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient
{
    public class Mapper
    {
        public static Event MapToEvent(EventDTO eventDTO)
        {
            return new() { Id = eventDTO.Id, DateTime = eventDTO.DateTime, Description = eventDTO.Event, Importance = eventDTO.Importance, OwnerLogin = eventDTO.User };
        }
        public static EventDTO MapToEventDTO(Event eventData)
        {
            return new() { Id = eventData.Id, DateTime = eventData.DateTime, Event = eventData.Description, Importance = eventData.Importance, User = eventData.OwnerLogin };
        }
    }
}
