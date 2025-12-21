using EventPlannerClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventPlannerClient.ViewModels
{
    class RegistrationViewModel
    {
        private IAuthorizationService _authorizationService;
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public ICommand RegistrationCommand { get; set; }

        public RegistrationViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            RegistrationCommand = new RelayCommand(Registration);
        }

        public async void Registration(object parameter)
        {
            bool success = await _authorizationService.Register(Login, Password, ConfirmPassword);
            if (!success)
                Console.WriteLine("Ошибка");
        }
    }
}
