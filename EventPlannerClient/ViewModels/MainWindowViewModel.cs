using EventPlannerClient.Views;
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
        public ICommand MinimizeCommand { get; }
        public ICommand CloseCommand { get; }

        public MainWindowViewModel()
        {
            MinimizeCommand = new RelayCommand(MinimizeWindow);
            CloseCommand = new RelayCommand(CloseWindow);
        }
        private void MinimizeWindow(object parameter)
        {
            _window.WindowState = WindowState.Minimized;
        }

        private void CloseWindow(object parameter)
        {
            _window?.Close();
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
