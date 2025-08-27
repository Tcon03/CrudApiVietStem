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
using AutoUpdaterDotNET;
using static CrudVietSteam.ViewModel.MainViewModel;
using Microsoft.Web.WebView2.Core;

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



        public ContestsVModel contestVM { get; private set; }
        public CityVModel cityVM { get; private set; }
        public ICommand ShowContestView { get; set; }
        public ICommand ShowCityView { get; set; }
        public ICommand AddInforCommand { get; set; }
        public ICommand SearchData { get; set; }
        public ICommand ClearCommand { get; set; }


        public MainViewModel()
        {

            contestVM = new ContestsVModel();
            cityVM = new CityVModel();
            ShowCityView = new VfxCommand(o => SwitchView(ViewType.CityView), () => true);
            ShowContestView = new VfxCommand(o => SwitchView(ViewType.ContestView), () => true);
            AddInforCommand = new VfxCommand(OnAdd, () => true);
            SearchData = new VfxCommand(OnSearch, CanSearch);
            ClearCommand = new VfxCommand(OnClear, CanClear);

            // Default display contest view 
            SwitchView(ViewType.ContestView);
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
            AutoUpdater.Start("https://raw.githubusercontent.com/Tcon03/Interface-car-/refs/heads/master/Update.xml");

        }


        /// <summary>
        /// Check For Update Version
        /// </summary>
        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            try
            {

                if (args.Error != null)
                {
                    MessageBox.Show(args.Error.Message, "Update error");
                    return;
                }

                // (2) có bản cập nhật?
                if (args.IsUpdateAvailable)
                {
                    var result = MessageBox.Show(
                        $"Có phiên bản mới {args.CurrentVersion}. Bạn đang dùng {args.InstalledVersion}.\nCập nhật ngay?",
                        "Thông báo cập nhật",
                        MessageBoxButton.YesNo, MessageBoxImage.Information);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate(args))
                                Application.Current.Shutdown();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Lỗi tải/cập nhật");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Bạn đang dùng phiên bản mới nhất ({args.InstalledVersion}).",
                                    "Không có cập nhật", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cập nhật");
            }
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

        /// <summary>
        /// Clear Command
        /// </summary>
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

        /// <summary>
        /// Command SearchData
        /// </summary>
        private void OnSearch(object obj)
        {
            // 1. Kiểm tra CurrentViewType để xác định đang ở view nào
            switch (CurrentViewType)
            {
                // 2. Nếu đang ở ContestView thì gọi phương thức SearchContest của contestVM
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


        /// <summary>
        ///  Command Add Data
        /// </summary>
        private void OnAdd(object obj)
        {
            // 1. ktra CurrentViewType để biết đang ở view nào
            switch (CurrentViewType)
            {
                // 2. nếu đang ở ContestView thì mở cửa sổ AddContestView
                case ViewType.ContestView:
                    var addContestWindow = new AddContestView();
                    // gán dữ liệu cho DataContext của cửa sổ AddContestView
                    addContestWindow.DataContext = contestVM;
                    addContestWindow.ShowDialog();
                    break;
                // 3. nếu đang ở CityView thì mở cửa sổ AddCityView
                case ViewType.CityView:
                    var cityWindow = new AddCityView();
                    cityWindow.DataContext = cityVM; // gán dữ liệu cho DataContext của cửa sổ AddCityView
                    cityWindow.ShowDialog();
                    break;
            }
        }

        public void SwitchView(ViewType viewType)
        {
            // Ghi đè CurrentViewType và CurrentView dựa trên ViewType được truyền vào
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
