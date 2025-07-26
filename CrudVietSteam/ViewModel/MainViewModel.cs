using CrudVietSteam.Command;
using CrudVietSteam.View;
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
        public enum ViewType
        {
            ContestView,
            CityView
        }
        private ViewType _currentViewType;
        public ViewType CurrentViewType
        {
            get { return _currentViewType; }
            set
            {
                _currentViewType = value;
                RaisePropertyChange(nameof(CurrentViewType));
            }
        }
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
        public ContestsVM contestVM { get; private set; }
        public ICommand ShowContestView { get; set; }
        public ICommand ShowCityView { get; set; }
        public ICommand AddInfor { get; set; }

        public MainViewModel()
        {
            contestVM = new ContestsVM();
            ShowCityView = new VfxCommand(o => SwitchView(ViewType.ContestView), o => true);
            ShowContestView = new VfxCommand(o => SwitchView(ViewType.CityView), o => true);
            AddInfor = new VfxCommand(OnAdd, o => true);
            // Default display contest view 
            SwitchView(ViewType.ContestView);
        }

        private void OnAdd(object obj)
        {
            AddInformation addInformation = new AddInformation();
            addInformation.ShowDialog(); // Hiển thị cửa sổ thêm thông tin cuộc thi
            
        }

        public void SwitchView(ViewType viewType)
        {
            CurrentViewType = viewType;
            switch (viewType)
            {
                case ViewType.ContestView:
                    CurrentView = contestVM;
                    CurrentTitle = "Contest Management";

                    break;
                case ViewType.CityView:
                    //CurrentView = new CityVM(); // Assuming you have a CityVM similar to ContestsVM
                    CurrentTitle = "City Management";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }

        }


        private void OnContestView(object obj)
        {

        }

        private void OnShowCityView(object obj)
        {

        }
    }
}
