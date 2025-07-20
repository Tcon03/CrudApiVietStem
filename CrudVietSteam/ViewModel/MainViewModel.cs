using CrudVietSteam.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChange(nameof(CurrentView));
            }
        }
        private string _currtenTitle;
        public string CurrentTitle
        {
            get { return _currtenTitle; }
            set
            {
                _currtenTitle = value;
                RaisePropertyChange(nameof(CurrentTitle));
            }
        }
        public ICommand ShowContestView { get; set; }
        public ICommand ShowCityView { get; set; }
        public MainViewModel()
        {
            ShowCityView = new VfxCommand(OnShowCityView, () => true);
            ShowContestView = new VfxCommand(OnContestView, () => true);
            // Default display contest view 
            OnContestView(null);
        }

        private void OnContestView(object obj)
        {
            // Tạo 1 instance của ContestsVM và gán nó cho CurrentView
            CurrentView = new ContestsVM();
            CurrentTitle = "Contest Information";
        }

        private void OnShowCityView(object obj)
        {
            MessageBox.Show("Chuyển đổi giao diện nè ");
            CurrentTitle = "City Information";
        }
    }
}
