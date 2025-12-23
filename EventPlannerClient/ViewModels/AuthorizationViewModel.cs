using EventPlannerClient.Services;
using EventPlannerLibrary;
using EventPlannerLibrary.SharedDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventPlannerClient.ViewModels
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private IAuthorizationService _authorizationService;
        private string _login;
        private string _password;
        private string _errorMessage;
        public ICommand AuthorizationCommand { get; set; }

        public AuthorizationViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            AuthorizationCommand = new RelayCommand(Authorization);
        }
        public string Login
        {
            get => _login; set
            {
                if (_login != value)
                {
                    _login = value;
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(Login));
                    OnPropertyChanged(nameof(IsLoginFailed));
                }
            }
        }
        public string Password
        {
            get => _password; set
            {
                if (_password != value)
                {
                    _password = value;
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(Password));
                    OnPropertyChanged(nameof(IsLoginFailed));
                }
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                    OnPropertyChanged(nameof(IsLoginFailed));
                }
            }
        }
        public bool IsLoginFailed
        {
            get => ErrorMessage != string.Empty && ErrorMessage is not null;
            set => OnPropertyChanged(nameof(IsLoginFailed));
        }

        public async void Authorization(object parameter)
        {
            var serviceResponse = await _authorizationService.Authorize(Login, Password);
            if (!serviceResponse.IsSuccessed)
                ErrorMessage = serviceResponse.ErrorMessage;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
