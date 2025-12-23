using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.ViewModels
{
    public class CalendarEventsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EventViewModel> _eventViewModels;
        private DateTime? _selectedDate = DateTime.Today;

        public  CalendarEventsViewModel() 
        {
            
        }
        public DateTime? Date
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value && value is not null)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(Date));
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
