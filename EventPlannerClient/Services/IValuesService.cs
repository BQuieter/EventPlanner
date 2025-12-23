using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Services
{
    public interface IValuesService
    {
        public bool TryGetImportanceString(byte importanceId, out string importanceString);
        public bool TryGetImportanceId(string inmportanceString, out byte importanceId);
    }
}
