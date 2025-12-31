using EventPlannerClient.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient
{
    public class InitializedViewModel : INotifyPropertyChanged
    {
        public event Action Done;
        public virtual void Initialize(EventViewModel eventViewModel)
        {
        }
        protected virtual void OnDone() 
        {
            Done?.Invoke();
        } 
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
