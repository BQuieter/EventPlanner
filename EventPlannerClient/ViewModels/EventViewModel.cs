using EventPlannerClient.Models;
using EventPlannerClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.ViewModels
{
    public class EventViewModel : INotifyPropertyChanged
    {
        private Event _event;
        private string _timeString;
        private string _importanceString;
        private IValuesService _valuesService;

        public EventViewModel(Event _event, IValuesService valuesService)
        {
            this._event = _event;
            this._valuesService = valuesService;
            SetImportanceString(_event.Importance);

        }
        public DateTime EventDateTime
        {
            get => _event.DateTime;
            set
            {
                if (_event.DateTime != value)
                {
                    _event.DateTime = value;
                    OnPropertyChanged(nameof(EventDateTime));
                    OnPropertyChanged(nameof(TimeString));
                }
            }

        }
        public string TimeString
        {
            get => _event.DateTime.ToString("t");
            set
            {
                if (_timeString != value && DateTime.TryParseExact(value, "t", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
                {
                    _timeString = value;
                    EventDateTime = new DateTime(EventDateTime.Year, EventDateTime.Month, EventDateTime.Day, parsedDateTime.Hour, parsedDateTime.Minute, 0);
                    OnPropertyChanged(nameof(TimeString));
                }
            }
        }
        public string OwnerLogin
        {
            get => _event.OwnerLogin;
            set
            {
                if (_event.OwnerLogin != value)
                {
                    _event.OwnerLogin = value;
                    OnPropertyChanged(nameof(OwnerLogin));
                }
            }
        }

        public string Description
        {
            get => _event.Description;
            set
            {
                if (_event.Description != value)
                {
                    _event.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public byte Importance
        {
            get => _event.Importance;
            set
            {
                if (_event.Importance != value)
                {
                    _event.Importance = value;
                    SetImportanceString(value);

                    OnPropertyChanged(nameof(Importance));
                }
            }
        }

        public string ImportanceString
        {
            get => _importanceString;
            set
            {
                if (_importanceString != value) 
                {
                    _importanceString = value;
                    OnPropertyChanged(nameof(ImportanceString));
                }
            }
        }
        public Event GetEvent()
        {
            return _event;
        }
        public void SetEvent(Event eventData)
        {
            _event = eventData;
            OnPropertyChanged(nameof(EventDateTime));
            OnPropertyChanged(nameof(TimeString));
            OnPropertyChanged(nameof(OwnerLogin));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(Importance));
            SetImportanceString(Importance);
        }
        public EventViewModel GetCopy()
        {
            var newEvent = new Event() { DateTime = this._event.DateTime, Description = this._event.Description, Id = this._event.Id, Importance = this._event.Importance, OwnerLogin = this._event.OwnerLogin };
            return new EventViewModel(newEvent, _valuesService) { Description = this.Description, Importance = this.Importance, EventDateTime = this.EventDateTime, ImportanceString = this.ImportanceString, TimeString = this.TimeString, OwnerLogin = this.OwnerLogin };
        }
        private void SetImportanceString(byte id)
        {
            if (_valuesService.TryGetImportanceString(id, out string importanceString))
            {
                ImportanceString = importanceString;
                OnPropertyChanged(nameof(ImportanceString));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
