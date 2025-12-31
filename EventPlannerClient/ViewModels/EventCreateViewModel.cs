using EventPlannerClient.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventPlannerClient.ViewModels
{
    public class EventCreateViewModel : InitializedViewModel
    {
        private EventViewModel? _eventViewModel;
        private bool _isCreated = false;
        private IAuthorizationService _authorizationService;
        private IValuesService _valuesService;
        private IEventsService _eventsService;
        public ICommand ResetCommand { get; set; }
        public ICommand CreateCommand { get; set; }
        public List<string> ImportancesList { get; private set; }
        public EventCreateViewModel(IAuthorizationService authorizationService, IValuesService valuesService, IEventsService eventsService)
        {
            _authorizationService = authorizationService;
            _valuesService = valuesService;
            _eventsService = eventsService;
            ImportancesList = _valuesService.GetImportancesList();
            ResetCommand = new RelayCommand(ResetHandler);
            CreateCommand = new RelayCommand(CreateHandler);
        }
        public string Login
        {
            get => _eventViewModel?.OwnerLogin;
            set { }
        }
        public string TimeString
        {
            get => _eventViewModel?.TimeString;
            set
            {
                if (_eventViewModel.TimeString != value)
                {
                    _eventViewModel.TimeString = value;
                    OnPropertyChanged(nameof(TimeString));
                }
            }
        }
        public bool IsCreated
        {
            get => _isCreated;
            set
            {
                if (_isCreated != value)
                {
                    _isCreated = value;
                    OnPropertyChanged(nameof(IsCreated));
                }
            }
        }
        public string Importance
        {
            get => _eventViewModel?.ImportanceString;
            set
            {
                if (_eventViewModel.ImportanceString != value && _valuesService.TryGetImportanceId(value, out byte importanceId))
                {
                    _eventViewModel.Importance = importanceId;
                    OnPropertyChanged(nameof(Importance));
                }
            }
        }
        public string Description
        {
            get => _eventViewModel?.Description;
            set
            {
                if (_eventViewModel.Description != value)
                {
                    _eventViewModel.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public override void Initialize(EventViewModel eventViewModel)
        {
            Update();
            _eventViewModel = eventViewModel;
            Update();
        }
        private void Update()
        {
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(TimeString));
            OnPropertyChanged(nameof(Importance));
            OnPropertyChanged(nameof(Description));
        }
        protected override void OnDone() =>
            base.OnDone();
        private void ResetHandler(object parameter)
        {
            Description = "";
            if (_valuesService.TryGetImportanceString(1, out string importanceString))
                Importance = importanceString;
            TimeString = "00:00";
        }
        private async void CreateHandler(object parameter)
        {
            var result = await _eventsService.CreateEvent(_eventViewModel.GetEvent());
            if (!result.IsSuccessed)
            {
                Debug.WriteLine($"Ошибка {result.ErrorCode} - {result.ErrorMessage}");
                return;
            }
            _eventViewModel?.SetEvent(result.Result);
            OnDone();
        }
    }
}
