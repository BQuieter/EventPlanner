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
    internal class EventViewModel : INotifyPropertyChanged
    {
        private Event _event;
        private string _timeString;
        private string _importanceString;
        private IValuesService _valuesService;

        public EventViewModel(Event _event, IValuesService valuesService)
        {
            this._event = _event;
            this._valuesService = valuesService;
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
                if (_timeString != value && DateTime.TryParseExact(value, "d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
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
                    if (_valuesService.TryGetImportanceString(value, out string importanceString))
                    {
                        ImportanceString = importanceString;
                        OnPropertyChanged(nameof(ImportanceString));
                    }

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
