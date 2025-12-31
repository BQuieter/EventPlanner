using EventPlannerClient.Models;
using EventPlannerClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventPlannerClient.ViewModels
{
    public class EventViewViewModel : InitializedViewModel
    {
        private EventViewModel? _eventViewModel;
        private EventViewModel? _copyViewModel;
        private IAuthorizationService _authorizationService;
        private IValuesService _valuesService;
        private IEventsService _eventsService;
        private bool _dataChanged = false;
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public List<string> ImportancesList { get; private set; }
        public EventViewViewModel(IAuthorizationService authorizationService, IValuesService valuesService, IEventsService eventsService)
        {
            _authorizationService = authorizationService;
            _valuesService = valuesService;
            _eventsService = eventsService;
            ImportancesList = _valuesService.GetImportancesList();
            CancelCommand = new RelayCommand(CancelHandler);
            SaveCommand = new RelayCommand(SaveHandler, s => DataChanged);
        }
        public bool DataChanged
        {
            get => _dataChanged;
            set  
            {
                if (_dataChanged != value)
                {
                    _dataChanged = value;
                    OnPropertyChanged(nameof(DataChanged));
                    OnPropertyChanged(nameof(ActionString));
                }
            }
        }
        public string Login 
        {
            get => _copyViewModel?.OwnerLogin;
            set { }
        }
        public string TimeString
        {
            get => _copyViewModel?.TimeString;
            set
            {
                if (_copyViewModel.TimeString != value)
                {
                    _copyViewModel.TimeString = value;
                    DataChanged = true;
                    OnPropertyChanged(nameof(TimeString));
                }
            }
        }
        public string Importance
        {
            get => _copyViewModel?.ImportanceString;
            set
            {
                if (_copyViewModel.ImportanceString != value && _valuesService.TryGetImportanceId(value, out byte importanceId))
                {
                    _copyViewModel.Importance = importanceId;
                    DataChanged = true;
                    OnPropertyChanged(nameof(Importance));
                }
            }
        }
        public string Description
        {
            get => _copyViewModel?.Description;
            set
            {
                if (_copyViewModel.Description != value)
                {
                    _copyViewModel.Description = value;
                    DataChanged = true;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public bool IsOwner => _authorizationService.Login == Login;

        public string ActionString => _dataChanged ? "Редактирование" : "Просмотр";
        public override void Initialize(EventViewModel eventViewModel)
        {
            _eventViewModel = eventViewModel;
            _dataChanged = false;
            Update();
            _copyViewModel = _eventViewModel.GetCopy();
            Update();
        }
        private void Update()
        {
            OnPropertyChanged(nameof(IsOwner));
            OnPropertyChanged(nameof(ActionString));
            OnPropertyChanged(nameof(DataChanged));
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(TimeString));
            OnPropertyChanged(nameof(Importance));
            OnPropertyChanged(nameof(Description));
        }
        private void CancelHandler(object parameter)
        {
            Initialize(_eventViewModel);
        }
        private async void SaveHandler(object parameter)
        {
            var result = await _eventsService.EditEvent(_copyViewModel.GetEvent());
            if (!result.IsSuccessed)
            {
                Debug.WriteLine($"Ошибка {result.ErrorCode} - {result.ErrorMessage}");
                return;
            }
            _eventViewModel?.SetEvent(result.Result);
            Initialize(_eventViewModel);
        }
    }
}
