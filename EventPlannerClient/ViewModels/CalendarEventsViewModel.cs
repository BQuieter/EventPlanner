using EventPlannerClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using EventPlannerClient.Models;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace EventPlannerClient.ViewModels
{
    public class CalendarEventsViewModel : INotifyPropertyChanged
    {
        private IEventsService _eventsService;
        private IValuesService _valuesService;
        private IServiceProvider _serviceProvider;
        private IAuthorizationService _authorizationService;
        private ObservableCollection<EventViewModel> _eventViewModels = new();
        private EventViewModel _selectedEvent;
        private EventViewModel _createdEvent; 
        private DateTime? _selectedDate = DateTime.Today;
        private InitializedViewModel _currentEventViewModel;
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public  CalendarEventsViewModel(IServiceProvider serviceProvider ,IEventsService eventsService, IValuesService valuesService, IAuthorizationService authorizationService) 
        {
            _eventsService = eventsService;
            _valuesService = valuesService;
            _serviceProvider = serviceProvider;
            _authorizationService = authorizationService;
            AddCommand = new RelayCommand(AddHandler);
            DeleteCommand = new RelayCommand(DeleteHandler, c => SelectedEvent is not null && authorizationService.Login == _selectedEvent?.OwnerLogin);
            UpdateCommand = new RelayCommand(UpdateHandler);
            GetEvents();
        }
        public InitializedViewModel CurrentEventViewModel
        {
            get => _currentEventViewModel;
            set
            {
                if (_currentEventViewModel != value)
                {
                    _currentEventViewModel = value;
                    OnPropertyChanged(nameof(CurrentEventViewModel));
                }
            }
        }
        public EventViewModel SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (_selectedEvent != value)
                {
                    _selectedEvent = value;
                    if (value is not null)
                    {
                        CurrentEventViewModel = _serviceProvider.GetRequiredService<EventViewViewModel>();
                        CurrentEventViewModel.Initialize(value);
                        OnPropertyChanged(nameof(CurrentEventViewModel));
                    }
                    OnPropertyChanged(nameof(SelectedEvent));
                }
            }
        }
        public DateTime? Date
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value && value is not null)
                {
                    _selectedDate = value;
                    SelectedEvent = null;
                    CurrentEventViewModel = null;
                    OnPropertyChanged(nameof(Date));
                    GetEvents();
                }

            }
        }
        public ObservableCollection<EventViewModel> Events 
        {
            get => _eventViewModels;
            set
            {
                if (_eventViewModels != value) 
                {
                    _eventViewModels = value;
                    OnPropertyChanged(nameof(Events));
                }
            }
        }
        private void AddHandler(object parameter)
        {
            if (CurrentEventViewModel is EventCreateViewModel)
                return;
            CurrentEventViewModel = null;
            SelectedEvent = null;
            CurrentEventViewModel = _serviceProvider.GetRequiredService<EventCreateViewModel>();
            _createdEvent = new EventViewModel(new() { OwnerLogin = _authorizationService.Login, Importance = 1, DateTime = Date.Value }, _valuesService);
            CurrentEventViewModel.Initialize(_createdEvent);
            CurrentEventViewModel.Done += WasCreated;
        }
        private void WasCreated()
        {
            _eventViewModels.Add(_createdEvent);
            CurrentEventViewModel = null;
        }
        private async void DeleteHandler(object parameter)
        {
            var result = await _eventsService.DeleteEvent(_selectedEvent.GetEvent());
            if (!result.IsSuccessed)
            {
                Debug.WriteLine($"Ошибка {result.ErrorCode} - {result.ErrorMessage}");
                return;
            }
            UpdateHandler(parameter);
        }
        private void UpdateHandler(object parameter)
        {
            GetEvents();
            CurrentEventViewModel = null;
        }
        private async void GetEvents() 
        {
            Events.Clear();
            var response = await _eventsService.GetEvents(Date);
            if (!response.IsSuccessed)
            {
                Debug.WriteLine($"{response.ErrorCode} - {response.ErrorMessage}");
                return;
            }
            foreach (Event evnt in response.Result)
                Events.Add(new EventViewModel(evnt, _valuesService));
            OnPropertyChanged(nameof(Events));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
