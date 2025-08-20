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
                Debug.WriteLine($"********** [Debug] Current ViewType có giá trị   **********:\n OlderValue : {oldValue} => {_currentViewType}");
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
                Debug.WriteLine($"********** [Debug] Current View có giá trị **********: {value}");
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
                    (SearchData as VfxCommand)?.RaiseCanExecuteChanged();
                    (ClearCommand as VfxCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        // có thể là null nếu không có giá trị nào được chọn
        private DateTime? _updatedAt;
        public DateTime? createTo
        {
            get { return _updatedAt; }
            set
            {
                if (_updatedAt != value)
                {
                    _updatedAt = value; //luôn là 00:00:00
                    Debug.WriteLine("====== UpdatedAt changed to: =======" + value);
                    RaisePropertyChange(nameof(createTo));
                    (SearchData as VfxCommand)?.RaiseCanExecuteChanged();
                    (ClearCommand as VfxCommand)?.RaiseCanExecuteChanged();
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
                    Debug.WriteLine(" ======= Keywords changed to ==========: " + value);
                    RaisePropertyChange(nameof(Keywords));
                    (SearchData as VfxCommand)?.RaiseCanExecuteChanged(); // cập nhật trạng thái của lệnh tìm kiếm khi Keywords thay đổi
                    (ClearCommand as VfxCommand)?.RaiseCanExecuteChanged();

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
        public ICommand ClearCommand { get; set; }


        public MainViewModel()
        {

            contestVM = new ContestsVM();
            cityVM = new CityVM();
            ShowCityView = new VfxCommand(o => SwitchView(ViewType.CityView), () => true);
            ShowContestView = new VfxCommand(o => SwitchView(ViewType.ContestView), () => true);
            AddInforCommand = new VfxCommand(OnAdd, () => true);
            SearchData = new VfxCommand(OnSearch, CanSearch);
            ClearCommand = new VfxCommand(OnClear, CanClear);

            // Default display contest view 
            SwitchView(ViewType.ContestView);


        }

        private bool CanClear()
        {
            bool isValid = !string.IsNullOrEmpty(Keywords);
            bool IsDate = CreatedAt.HasValue && createTo.HasValue;
            return isValid || IsDate;
        }

        private void ClearSearchFilters()
        {
            Keywords = string.Empty;
            CreatedAt = null;
            createTo = null;
            Debug.WriteLine("======= Clear search filters =======");
        }
        private async void OnClear(object obj)
        {
            switch (CurrentViewType)
            {
                case ViewType.ContestView:

                    ClearSearchFilters();
                    await contestVM.LoadData();
                    break;

                case ViewType.CityView:
                    ClearSearchFilters();
                    await cityVM.LoadData();
                    break;
            }
        }

        private bool CanSearch()
        {
            bool isValidKey = !string.IsNullOrWhiteSpace(Keywords);
            bool isValidCreatedAt = CreatedAt.HasValue && createTo.HasValue && CreatedAt.Value <= createTo.Value;
            return isValidKey || isValidCreatedAt;
        }
        private void OnSearch(object obj)
        {
            switch (CurrentViewType)
            {
                case ViewType.ContestView:

                    contestVM.SearchContest(new Service.DTO.ContestSearch
                    {
                        KeyWord = Keywords,
                        CreatedAtForm = CreatedAt,
                        CreatedAtTo = createTo
                    });
                    break;
                case ViewType.CityView:
                    cityVM.Searchity(new Service.DTO.CitySearch
                    {
                        Key = Keywords,
                        CreatedForm = CreatedAt,
                        CreatedTo = createTo
                    });
                    break;
            }
        }

        private void OnAdd(object obj)
        {
            switch (CurrentViewType)
            {
                case ViewType.ContestView:
                    var addContestWindow = new AddInformation();
                    addContestWindow.DataContext = contestVM; // gán DataContext cho cửa sổ AddInformation
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
            // gán CurrentViewType cho viewType để biết được view nào đang được hiển thị
            CurrentViewType = viewType;
            switch (viewType)
            {
                case ViewType.ContestView:
                    // gán dữ liệu cho object CurrentView
                    CurrentView = contestVM;
                    CurrentTitle = "Contest Management";

                    break;
                case ViewType.CityView:
                    //Gán dữ liệu cho object City -> CurrentView
                    CurrentView = cityVM;
                    CurrentTitle = "City Management";

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
