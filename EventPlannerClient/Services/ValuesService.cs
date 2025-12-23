using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace EventPlannerClient.Services
{
    public class ValuesService: IValuesService
    {
        //Для теста явно указал
        private Dictionary<byte, string> importancePairs = new() { { 1, "Незначительно"}, { 2, "Обычно" }, { 3, "Важно" }, { 4, "Критически важно" }, { 5, "Жизненно важно" } };
        public ValuesService() 
        {
            //Тут добавить сервис для получения данных с бд
        }

        public bool TryGetImportanceString(byte importanceId, out string importanceString)
        {
            importanceString = string.Empty;
            if (!importancePairs.TryGetValue(importanceId, out string str))
                return false;
            importanceString = str;
            return true;
        }
        public bool TryGetImportanceId(string importanceString, out byte importanceByte)
        {
            importanceByte = importancePairs.FirstOrDefault(i => i.Value == importanceString).Key;
            if (importanceByte != 0)
                return true;
            return false;
        }
    }
}
