using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using CrudVietSteam.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    /// <summary>
    /// Get data Api display contest data
    /// </summary>
    public class ContestsVM : ViewModelBase
    {
        private int _pageSize = 15; // Số lượng bản ghi trên mỗi trang 
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    //OnLoad(); // Tải lại dữ liệu khi thay đổi PageSize 
                    Debug.WriteLine("PageSize changed to: " + _pageSize);
                    RaisePropertyChange(nameof(PageSize)); // Thông báo thay đổi thuộc tính PageSize
                }
            }
        }
        private int _currentPage = 1; // Trang hiện tại 
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    Debug.WriteLine("CurrentPage changed to: " + _currentPage);
                    OnLoad(); // Tải lại dữ liệu khi thay đổi CurrentPage n 
                    RaisePropertyChange(nameof(CurrentPage));
                }
            }
        }
        private int _totalPage; // Tổng số trang, mặc định là 1 
        public int TotalPage
        {
            get { return _totalPage; }
            set
            {
                if (_totalPage != value)
                {
                    _totalPage = value;
                    Debug.WriteLine("TotalPage changed to: " + _totalPage);
                    RaisePropertyChange(nameof(TotalPage));
                }
            }
        }
        private int _totalRecords = 0; // Tổng số bản ghi, mặc định là 0 vì chưa có gửi api lên  
        public int TotalRecords
        {
            get
            {
                return _totalRecords;
            }
            set
            {
                if (_totalRecords != value)
                {
                    _totalRecords = value;
                    Debug.WriteLine("TotalRecords changed to: " + _totalRecords);
                    RaisePropertyChange(nameof(TotalRecords)); // Thông báo thay đổi thuộc tính TotalRecords
                }
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChange(nameof(Name));
                }
            }
        }


        private string _introduce;
        public string Introduce
        {
            get { return _introduce; }
            set
            {
                if (_introduce != value)
                {
                    _introduce = value;
                    RaisePropertyChange(nameof(Introduce));
                }
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChange(nameof(Status));
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                Debug.WriteLine("Value thay đổi nè " +value );
                RaisePropertyChange(nameof(Description));
            }
        }
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChange(nameof(Title));
            }
        }
        private string _keyword;
        public string Keywords
        { 
            get
            {
                return _keyword; 
            } 
            set
            {
                _keyword = value;
                RaisePropertyChange(nameof(Keywords));
            }
        }

        public ObservableCollection<ContestsDTO> Contests { get; set; }

        public ICommand NextPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand AddContestCommand { get; set; }
        public ContestsVM()
        {
            Contests = new ObservableCollection<ContestsDTO>();
            NextPageCommand = new VfxCommand(OnNextPage, CanNextPage);
            PrevPageCommand = new VfxCommand(OnPrevPage, CanPrevPage);
            AddContestCommand = new VfxCommand(o => AddContest(), o => true);
            //OnLoad();
        }


        public async void AddContest()
        {


            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Introduce) ||
               string.IsNullOrEmpty(Status) || string.IsNullOrEmpty(Description) ||
               string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Keywords))
            {
                MessageBox.Show("Thông tin cuộc thi không được để trống.");
                return;
            }
            var addContestInfor = new ContestsDTO
            {
                name = Name,
                introduce = Introduce,
                status = Status,
                description = Description,
                title = Title,
                keywords = Keywords
            };
            var result = await App.vietstemService.CreateContestAsync(addContestInfor);
            if (result != null)
            {
                Debug.WriteLine("Thêm cuộc thi thành công: " + result.name);
                // Cập nhật lại danh sách cuộc thi sau khi thêm mới
                OnLoad();
            }
            else
            {
                Debug.WriteLine("Thêm cuộc thi thất bại.");
            }


        }
        /// <summary>
        /// PrevPage Page Check if can go to pevious page
        /// </summary>
        private bool CanPrevPage()
        {
            return CurrentPage > 1;
        }

        /// <summary>
        /// PrevPage Page Go to previous page
        /// </summary>
        private void OnPrevPage(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
            else
            {
                Debug.WriteLine("Đã ở trang đầu tiên, không thể chuyển về trang trước.");
            }
        }


        /// <summary>
        /// Check if can go to nextPage 
        /// </summary>
        private bool CanNextPage()
        {

            return CurrentPage < TotalPage;
        }

        /// <summary>
        /// Nextpage default
        /// </summary>
        private void OnNextPage(object obj)
        {
            CurrentPage++;
        }

        private async void OnLoad()
        {
            try
            {
                //1. Get Total Records from Api
                var totalRecords = await App.vietstemService.GetCountAsync();

                if (totalRecords > 0)
                {
                    TotalRecords = totalRecords; // Cập nhật tổng số bản ghi
                    //  Amount TotalRecord / Amount PageSize (30/1)
                    TotalPage = (int)Math.Ceiling((double)TotalRecords / PageSize); // Tính tổng số trang dựa trên tổng số bản ghi và PageSize
                    Debug.WriteLine(" ====== Tổng số Page ==== :" + TotalPage);


                    Debug.WriteLine("Tổng số bản ghi Item: " + TotalRecords);

                    var dataApi = await App.vietstemService.GetContestAsync(PageSize, CurrentPage);
                    if (dataApi == null)
                    {
                        return;
                    }

                    Contests.Clear();
                    foreach (var item in dataApi)
                    {
                        Contests.Add(item);
                    }
                }
                else
                {
                    Debug.WriteLine("Không có bản ghi nào để hiển thị.");
                    TotalPage = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                // Cập nhật lại trạng thái của các nút Next/Prev sau mỗi lần tải
                (NextPageCommand as VfxCommand)?.RaiseCanExecuteChanged();
                (PrevPageCommand as VfxCommand)?.RaiseCanExecuteChanged();
            }


        }
    }
}
