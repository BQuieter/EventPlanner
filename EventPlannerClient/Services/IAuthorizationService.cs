using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Services
{
    public interface IAuthorizationService
    {
        public string Login {get; }
        public event Action Authorized;
        public Task<bool> Authorize(string login, string password);
        public Task<bool> Register(string login, string password, string confirmPassword);
        public void Logout();


    }
}
