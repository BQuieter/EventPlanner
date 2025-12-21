using EventPlannerClient.Models;
using EventPlannerClient.Services;
using EventPlannerClient.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EventPlannerClient.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MainWindow _window;
        private User _user;
        private IServiceProvider _serviceProvider;
        private IAuthorizationService _authorizationService;
        private object _currentMainViewModel;
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand AuthorizationCommand { get; }
        public ICommand RegistrationCommand { get; }

        public MainWindowViewModel(IServiceProvider serviceProvider, IAuthorizationService authorizationService)
        {
            _serviceProvider = serviceProvider;
            MinimizeCommand = new RelayCommand(MinimizeWindow);
            CloseCommand = new RelayCommand(CloseWindow);
            AuthorizationCommand = new RelayCommand(Authorization);
            RegistrationCommand = new RelayCommand(Registration);
            _authorizationService = authorizationService;
            _authorizationService.Authorized += AuthorizedHandler;
            _user = new();
        }
        public object CurrentMainViewModel
        {
            get => _currentMainViewModel;
            private set
            {
                if (_currentMainViewModel != value)
                {
                    _currentMainViewModel = value;
                    OnPropertyChanged(nameof(CurrentMainViewModel));
                }
            }
                
        }
        public string UserLogin
        {
            get => _user.Login;
            set
            {
                if (_user.Login != value)
                {
                    _user.Login = value;
                    OnPropertyChanged(nameof(UserLogin));
                }
            }
        }
        public bool IsAuthorized 
        
        { 
            get => _user.IsAuthorize;
            set
            {
                if (_user.IsAuthorize != value)
                {
                    _user.IsAuthorize = value;
                    OnPropertyChanged(nameof(IsAuthorized));
                    OnPropertyChanged(nameof(IsAuthorizedString));
                }
            }
        } 
        public string IsAuthorizedString => IsAuthorized ? "Авторизован:" : "Не авторизован:";
        
        private void MinimizeWindow(object parameter)
        {
            _window.WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object parameter)
        {
            _window?.Close();
        }
        private void Authorization(object parameter)
        {
            CurrentMainViewModel = _serviceProvider.GetRequiredService<AuthorizationViewModel>();
        }

        private void Registration(object parameter)
        {
            CurrentMainViewModel = _serviceProvider.GetRequiredService<RegistrationViewModel>();

        }
        private void AuthorizedHandler()
        {
            UserLogin = _authorizationService.Login;
            IsAuthorized = true;
            CurrentMainViewModel = _serviceProvider.GetRequiredService<CalendarEventsViewModel>();
        }

        public void SetWindow()
        {
            _window = MainWindow.CurrentWindow;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
