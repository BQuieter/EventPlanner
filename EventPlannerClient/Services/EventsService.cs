using EventPlannerClient.Models;
using EventPlannerClient.ViewModels;
using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.SharedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Services
{
    public class EventsService : IEventsService
    {
        private IApiService _apiService;

        public EventsService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ServiceResponse<List<Event>>> GetEvents(DateTime? date) 
        {
            var parsedDate = date.Value;
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"Events/getevents/{parsedDate.Year }-{parsedDate.Month}-{parsedDate.Day}");
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());

            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<List<EventDTO>>>();
            if (!response.IsSuccessStatusCode && responseData is not null)
                return new ServiceResponse<List<Event>>() { IsSuccessed = false, ErrorMessage = responseData.Error, ErrorCode = responseData.ErrorCode };
            else if (!response.IsSuccessStatusCode || (response.IsSuccessStatusCode && responseData.Data is null))
                return new ServiceResponse<List<Event>>() { IsSuccessed = false, ErrorMessage = "Неизвестная ошибка", ErrorCode = (int)response.StatusCode };

            return new ServiceResponse<List<Event>>() { IsSuccessed = true, Result = responseData.Data.Select(r => Mapper.MapToEvent(r)).ToList()};
        }
        public async Task<ServiceResponse<Event>> CreateEvent(Event eventData)
        {
            var requestData = new EventDTO() { Id = eventData.Id, DateTime = eventData.DateTime, Event = eventData.Description, Importance = eventData.Importance, User = eventData.OwnerLogin };
            if (!Validator<EventDTO>.IsValid(requestData))
                return new ServiceResponse<Event?>() { IsSuccessed = false, ErrorMessage = "Некорректные данные" };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Events/createevent") { Content = JsonContent.Create(requestData) };
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());

            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<EventDTO>>();
            if (!response.IsSuccessStatusCode && responseData is not null)
                return new ServiceResponse<Event>() { IsSuccessed = false, ErrorMessage = responseData.Error, ErrorCode = responseData.ErrorCode };
            else if (!response.IsSuccessStatusCode || (response.IsSuccessStatusCode && responseData.Data is null))
                return new ServiceResponse<Event>() { IsSuccessed = false, ErrorMessage = "Неизвестная ошибка", ErrorCode = (int)response.StatusCode };

            return new ServiceResponse<Event>() { IsSuccessed = true, Result = Mapper.MapToEvent(responseData.Data) };
        }
        public async Task<ServiceResponse<Event>> EditEvent(Event eventData)
        {
            var requestData = new EventDTO() { Id = eventData.Id, DateTime = eventData.DateTime, Event = eventData.Description, Importance = eventData.Importance, User = eventData.OwnerLogin};
            if (!Validator<EventDTO>.IsValid(requestData))
                return new ServiceResponse<Event?>() { IsSuccessed = false, ErrorMessage = "Некорректные данные" };
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, "Events/editevent") { Content = JsonContent.Create(requestData) };
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());

            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<EventDTO>>();
            if (!response.IsSuccessStatusCode && responseData is not null)
                return new ServiceResponse<Event>() { IsSuccessed = false, ErrorMessage = responseData.Error, ErrorCode = responseData.ErrorCode };
            else if (!response.IsSuccessStatusCode || (response.IsSuccessStatusCode && responseData.Data is null))
                return new ServiceResponse<Event>() { IsSuccessed = false, ErrorMessage = "Неизвестная ошибка", ErrorCode = (int)response.StatusCode };

            return new ServiceResponse<Event>() { IsSuccessed = true, Result = Mapper.MapToEvent(responseData.Data) };
        }
        public async Task<ServiceResponse<bool>> DeleteEvent(Event eventData)
        {
            var requestData = new EventDTO() { Id = eventData.Id, DateTime = eventData.DateTime, Event = eventData.Description, Importance = eventData.Importance, User = eventData.OwnerLogin };
            if (!Validator<EventDTO>.IsValid(requestData))
                return new ServiceResponse<bool>() { IsSuccessed = false, ErrorMessage = "Некорректные данные" };

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "Events/deleteevent") { Content = JsonContent.Create(requestData) };
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());

            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
            if (!response.IsSuccessStatusCode && responseData is not null)
                return new ServiceResponse<bool>() { IsSuccessed = false, ErrorMessage = responseData.Error, ErrorCode = responseData.ErrorCode };
            else if (!response.IsSuccessStatusCode)
                return new ServiceResponse<bool>() { IsSuccessed = false, ErrorMessage = "Неизвестная ошибка", ErrorCode = (int)response.StatusCode };

            return new ServiceResponse<bool>() { IsSuccessed = true, Result = true };
        }
    }
}
