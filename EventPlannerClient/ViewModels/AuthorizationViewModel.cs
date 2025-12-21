using EventPlannerClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventPlannerClient.ViewModels
{
    public class AuthorizationViewModel
    {
        private IAuthorizationService _authorizationService;
        public string Login {  get; set; }
        public string Password {  get; set; }
        public ICommand AuthorizationCommand { get; set; }

        public AuthorizationViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            AuthorizationCommand = new RelayCommand(Authorization);
        }

        public async void Authorization(object parameter)
        {
            bool success = await _authorizationService.Authorize(Login, Password);
            if (!success)
                Console.WriteLine("Ошибка");
        }
    }
}
