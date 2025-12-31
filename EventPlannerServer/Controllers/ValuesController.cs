using EventPlannerLibrary;
using EventPlannerLibrary.RequestDTOs;
using EventPlannerLibrary.SharedDTOs;
using EventPlannerServer.Models;
using EventPlannerServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EventPlannerServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public ValuesController(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("importances")]
        public async Task<ActionResult<ApiResponse<Dictionary<byte, string>>>> GetImportance(EventPlannerDbContext dbContext)
        {
            _memoryCache.TryGetValue("importance", out Dictionary<byte, string> importanceDictionary);
            if (importanceDictionary == null) 
            {
                importanceDictionary = dbContext.EventImportances.ToDictionary(i => i.Id, i => i.Name);
                _memoryCache.Set(importanceDictionary, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
            }
            return new ApiResponse<Dictionary<byte, string>>() { Success = true, Data = importanceDictionary};
        }
    }
}
