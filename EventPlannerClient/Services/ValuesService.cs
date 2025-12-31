using EventPlannerLibrary;
using EventPlannerLibrary.SharedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace EventPlannerClient.Services
{
    public class ValuesService: IValuesService
    {
        private IApiService _apiService;
        private Dictionary<byte, string> _importanceDictionary;
        private List<string> _importanceList;
        public ValuesService(IApiService apiService) 
        {
            _apiService = apiService;
            LoadAllValues();
        }
        public List<string> GetImportancesList()
        {
            return _importanceList;
        }
        public async void LoadAllValues()
        {
            var importance = GetImportances();
            await importance;
            _importanceList = _importanceDictionary.Select(i => i.Value).ToList();
        }
        public bool TryGetImportanceString(byte importanceId, out string importanceString)
        {
            if (!_importanceDictionary.TryGetValue(importanceId, out importanceString))
                return false;
            return true;
        }
        public bool TryGetImportanceId(string importanceString, out byte importanceByte)
        {
            importanceByte = _importanceDictionary.FirstOrDefault(i => i.Value == importanceString).Key;
            if (importanceByte != 0)
                return true;
            return false;
        }
        private async Task GetImportances()
        {
            if (_importanceDictionary is not null)
                return;

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "Values/importances");
            var response = await _apiService.SendAsync(requestMessage, new CancellationToken());

            var responseData = await response.Content.ReadFromJsonAsync<ApiResponse<Dictionary<byte, string>>>();

            if (responseData is not null && responseData.Success && responseData.Data is not null && responseData.Data.Count > 0)
                _importanceDictionary = responseData.Data;
        }
            
    }
}
