using EventPlannerClient.Services;
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
    class RegistrationViewModel : INotifyPropertyChanged
    {
        private IAuthorizationService _authorizationService;
        private string _login;
        private string _password;
        private string _confirmPassword;
        private string _errorMessage;
        public string Login
        {
            get => _login; set
            {
                if (_login != value)
                {
                    _login = value;
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(Login));
                    OnPropertyChanged(nameof(IsRegisterFailed));
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
                    OnPropertyChanged(nameof(IsRegisterFailed));
                }
            }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword; set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    ErrorMessage = string.Empty;
                    OnPropertyChanged(nameof(ConfirmPassword));
                    OnPropertyChanged(nameof(IsRegisterFailed));
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
                    OnPropertyChanged(nameof(IsRegisterFailed));
                }
            }
        }
        public bool IsRegisterFailed
        {
            get => ErrorMessage != string.Empty && ErrorMessage is not null;
            set => OnPropertyChanged(nameof(IsRegisterFailed));
        }
        public ICommand RegistrationCommand { get; set; }

        public RegistrationViewModel(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            RegistrationCommand = new RelayCommand(Registration);
        }

        public async void Registration(object parameter)
        {
            var serviceResponse = await _authorizationService.Register(Login, Password, ConfirmPassword);
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
