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
                var oldValue = _currentViewType;
                _currentViewType = value;
                Debug.WriteLine($"********** [Debug] Current ViewType  **********:\n {oldValue} => {_currentViewType}");
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

        private DateTime? _createdAt;
        public DateTime? CreatedAt
        {
            get { return _createdAt; }
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    Debug.WriteLine("======= CreatedAt changed to ======== : " + value);
                    RaisePropertyChange(nameof(CreatedAt));
                }
            }
        }
        private DateTime? _updatedAt;
        public DateTime? UpdatedAt
        {
            get { return _updatedAt; }
            set
            {
                if (_updatedAt != value)
                {
                    _updatedAt = value;
                    Debug.WriteLine("==== UpdatedAt changed to:  =====" + value);
                    RaisePropertyChange(nameof(UpdatedAt));
                }
            }
        }
        private string _keywords;
        public string Keywords
        {
            get { return _keywords; }
            set
            {
                if (_keywords != value)
                {
                    _keywords = value;
                    Debug.WriteLine("Keywords changed to: " + value);
                    RaisePropertyChange(nameof(Keywords));
                }
            }
        }
        #endregion



        public ContestsVM contestVM { get; private set; }
        public CityVM cityVM { get; private set; }
        public ICommand ShowContestView { get; set; }
        public ICommand ShowCityView { get; set; }
        public ICommand AddInforCommand { get; set; }
        public ICommand SearchData { get; set; }


        public MainViewModel()
        {
            contestVM = new ContestsVM();
            cityVM = new CityVM();
            ShowCityView = new VfxCommand(o => SwitchView(ViewType.CityView), o => true);
            ShowContestView = new VfxCommand(o => SwitchView(ViewType.ContestView), o => true);
            AddInforCommand = new VfxCommand(OnAdd, o => true);
            SearchData = new VfxCommand(OnSearch, CanSearch);
            // Default display contest view 
            SwitchView(ViewType.ContestView);

        }

        private bool CanSearch(object arg)
        {
            throw new NotImplementedException();
        }

        private void OnSearch(object obj)
        {
            switch (CurrentViewType)
            {
                case ViewType.ContestView:
                    contestVM.SearchContest(Keywords, CreatedAt, UpdatedAt);
                    break;
                case ViewType.CityView:
                    cityVM.Searchity(Keywords, CreatedAt, UpdatedAt);
                    break;
            }
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
                    CurrentView = cityVM;
                    CurrentTitle = "City Management";

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
