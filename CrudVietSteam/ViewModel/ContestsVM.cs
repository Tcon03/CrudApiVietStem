using CrudVietSteam.Command;
using CrudVietSteam.Service.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CrudVietSteam.ViewModel
{
    /// <summary>
    /// Get data Api display contest data
    /// </summary>
    public class ContestsVM : ViewModelBase
    {
        private int _pageSize = 10; // Số lượng bản ghi trên mỗi trang 
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
                    //OnLoad(); // Tải lại dữ liệu khi thay đổi CurrentPage n 
                    RaisePropertyChange(nameof(CurrentPage)); // Thông báo thay đổi thuộc tính CurrentPage
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
                    RaisePropertyChange(nameof(TotalPage)); // Thông báo thay đổi thuộc tính TotalPage
                }
            }
        }
        private int _totalRecords = 0; // Tổng số bản ghi, mặc định là 0 
        public int TotalRecords
        {
            get { return _totalRecords; }
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



        public ObservableCollection<ContestsDTO> Contests { get; set; }

        public ICommand NextPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ContestsVM()
        {
            Contests = new ObservableCollection<ContestsDTO>();
            NextPageCommand = new VfxCommand(OnNextPage, CanNextPage);
            PrevPageCommand = new VfxCommand(OnPrevPage, CanPrevPage);
            OnLoad();
        }

        private bool CanPrevPage()
        {
            return CurrentPage > 1;
        }

        private void OnPrevPage(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                OnLoad();
            }
            else
            {
                Debug.WriteLine("Đã ở trang đầu tiên, không thể chuyển về trang trước.");
            }
        }



        private bool CanNextPage()
        {
            return CurrentPage < TotalPage;
        }

        private void OnNextPage(object obj)
        {
            CurrentPage++;
            OnLoad(); // Tải lại dữ liệu khi chuyển sang trang tiếp theo
        }

        private async void OnLoad()
        {
            try
            {
                var totalRecords = await App.vietstemService.GetCountAsync(); // Giả sử có phương thức để lấy tổng số bản ghi
                if (totalRecords > 0)
                {
                    //   tổng số lượng / số lượng item của 1 page (30/1)
                    TotalPage = (int)Math.Ceiling((double)totalRecords / PageSize); // Tính tổng số trang dựa trên tổng số bản ghi và PageSize
                    Debug.WriteLine(" ====== Tổng số Page ====: " + TotalPage);

                    TotalRecords = totalRecords; // Cập nhật tổng số bản ghi
                    Debug.WriteLine("Tổng số bản ghi Item: " + TotalRecords);

                    var data = await App.vietstemService.GetContestAsync(PageSize, CurrentPage);
                    if (data == null)
                    { 
                        return;
                    }
                    Contests.Clear();
                    foreach (var item in data)
                    {
                        Contests.Add(item);
                    }
                }
                else
                {
                    Debug.WriteLine("Không có bản ghi nào để hiển thị.");
                    TotalPage = 0; // Nếu không có bản ghi, đặt tổng số trang là 1
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Lỗi khi tải dữ liệu: " + ex.Message);
                // Xử lý lỗi nếu cần, ví dụ: hiển thị thông báo lỗi cho người dùng
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
