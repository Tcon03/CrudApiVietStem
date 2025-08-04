using CrudVietSteam.Command;
using CrudVietSteam.View;
using CrudVietSteam.View.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static CrudVietSteam.ViewModel.MainViewModel;

namespace CrudVietSteam.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Properties
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
                Debug.WriteLine("CurrentView is value :" + value);

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
        #endregion

        public ContestsVM contestVM { get; private set; }
        public ICommand ShowContestView { get; set; }
        public ICommand ShowCityView { get; set; }
        public ICommand AddInfor { get; set; }

        public EventHandler CloseViet;

        public MainViewModel()
        {
            contestVM = new ContestsVM();
            ShowCityView = new VfxCommand(o => SwitchView(ViewType.CityView), o => true);
            ShowContestView = new VfxCommand(o => SwitchView(ViewType.ContestView), o => true);
            AddInfor = new VfxCommand(OnAdd, o => true);
            // Default display contest view 
            SwitchView(ViewType.ContestView);
        }

        private void OnAdd(object obj)
        {
            switch (CurrentViewType)
            {
                case ViewType.ContestView:
                    // Open Add Contest Window
                    var addContestWindow = new AddInformation();
                    addContestWindow.ShowDialog();
                    break;
                case ViewType.CityView:
                    var cityWindow = new CityInfor();
                    cityWindow.ShowDialog();
                    break;


            }
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
                    CurrentView = new CityVM();
                    CurrentTitle = "City Management";

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }


    }
}
