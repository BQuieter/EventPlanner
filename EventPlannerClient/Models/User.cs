using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Models
{
    internal class User : INotifyPropertyChanged
    {
        private string _login = "Гость";
        private bool _isAuthorized = false;
        public string Login 
        {
            get => _login;
            set 
            {  
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public bool IsAuthorize
        {
            get => _isAuthorized;
            set
            {
                _isAuthorized = value;
                OnPropertyChanged(nameof(IsAuthorize));
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
