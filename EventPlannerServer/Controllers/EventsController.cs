using Azure.Core;
using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.ResponseDTOs;
using EventPlannerLibrary.SharedDTOs;
using EventPlannerServer.Models;
using EventPlannerServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventPlannerServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController: ControllerBase
    {
        private IEventService eventService;
        private ILoggerService loggerService;
        public EventsController(IEventService eventService, ILoggerService loggerService)
        {
            this.eventService = eventService;
            this.loggerService = loggerService;
        }

        [HttpGet("getevents")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<List<EventDTO>>>> GetEventsOfMonth(DateTime dateRequest)
        {
            var result = eventService.GetEventsOfMonth(dateRequest);
            if (result.Item1 is not null)
                return BadRequest(ApiResponse<List<EventDTO>>.Fail(result.Item1.Message, result.Item1.ErrorCode));
            return Ok(ApiResponse<List<EventDTO>>.Ok(result.Item2!));
        }

        [HttpPost("createevent")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<EventDTO>>> CreateEvent(EventDTO eventData, HttpContext httpContext)
        {
            if (!Validator<EventDTO>.IsValid(eventData))
                return BadRequest(ApiResponse<EventDTO>.Fail("Неккоректные данные", "400"));
            string user = httpContext.User.Identity!.Name!;
            var result = eventService.CreateEvent(eventData, user);
            if (result.Item1 is not null)
                return BadRequest(ApiResponse<EventDTO>.Fail(result.Item1.Message, result.Item1.ErrorCode));
            await loggerService.Log(user, ActionTypes.EventCreated, null, result.Item2!.Id);
            return Ok(ApiResponse<EventDTO>.Ok(result.Item2!));
        }

        [HttpPut("editevent")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<EventDTO>>> EditEvent(List<EventDTO> eventDataList, HttpContext httpContext)
        {
            foreach(EventDTO eventDTO in eventDataList)
                if (!Validator<EventDTO>.IsValid(eventDTO))
                    return BadRequest(ApiResponse<EventDTO>.Fail("Неккоректные данные", "400"));
            if (eventDataList.Count != 2)
                return BadRequest(ApiResponse<EventDTO>.Fail("Неккоректные данные", "400"));

            string user = httpContext.User.Identity!.Name!;
            var result = eventService.EditEvent(eventDataList[0], eventDataList[1], user);
            if (result.Item1 is not null)
                return BadRequest(ApiResponse<EventDTO>.Fail(result.Item1.Message, result.Item1.ErrorCode));
            await loggerService.Log(user, ActionTypes.EventCreated, null, result.Item2!.Id);
            return Ok(ApiResponse<EventDTO>.Ok(result.Item2!));
        }
    }
}
